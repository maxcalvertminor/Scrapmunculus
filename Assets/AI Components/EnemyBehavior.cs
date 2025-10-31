using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float health;
    public Weapon weapon1, weapon2;
    public GameObject target, head;    
    public MainTankScript_Enemy lookAtPlayerScript;
    public Pathfinding path;

    public bool queued;
    public List<Behavior> behaviors; 
    public State state;

    public float distance;

    public float aggression, nervousness, perception;
    public float tolerance;

    public int bulletsInCollider;
    public int raycastsInCollider;

    public List<Behavior> attackBehaviors, roamBehaviors, searchBehaviors;

    public float targetSpeed, fraction, dashSpeed, walkSpeed, dashCooldownTime, dashStartTimer, turnSpeed;
    private float speed, nSpeed, dashCooldown, dashTimer;
    public Vector2 direction, targetDirection;

    public Vector2 point;

    public float dpsThreshold;
    public float dps, dashVariation;
    public Queue<DamageEvent> damageEvents;

    public Vector2 roamPoint, targetLastKnownPosition;

    public float targetHeadDirection, tiltSpeed;
    private VisionCone visionCone;

    public List<string> hostileTags;
    public List<GameObject> hostileEntities;

    public float searchActions, searchActionsNumber;
    public bool searchFlag;

    // Start is called before the first frame update
    void Start()
    {
        attackBehaviors = new List<Behavior> { new Approach(this), new Pause(this, 2), new Retreat(this), new Strafe(this) };
        roamBehaviors = new List<Behavior> { new PickRoamPoint(this), new Roam(this), new LookAround(this), new Pause(this, 4) };
        searchBehaviors = new List<Behavior> { new SearchLastKnownPoint(this), new LookAround(this)};
        behaviors = new List<Behavior>();
        lookAtPlayerScript = this.gameObject.GetComponent<MainTankScript_Enemy>();
        queued = false;
        SwitchState(State.Roaming);
        InvokeRepeating("SlowUpdate", 0f, 0.2f);
        direction = new(0,0);
        damageEvents = new();
        visionCone = GetComponentInChildren<VisionCone>();
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Behavior Queuing
        if(!queued) {
            Behavior priority = behaviors[0];
            foreach(Behavior behavior in behaviors) {
                if(behavior.priority > priority.priority) {
                    priority = behavior;
                }
            }
            queued = true;
            Debug.Log(priority.CheckAction());
            StartCoroutine(priority.Queue());
        }

        // Movement and direction
        if(direction != targetDirection) {
            direction = Vector2.MoveTowards(direction, targetDirection, Time.deltaTime * turnSpeed);
        }

        if(targetDirection == Vector2.zero) {
            targetSpeed = 0;
        } else if(dashTimer > 0) {
            targetSpeed = dashSpeed;
            speed = dashSpeed;
            dashTimer -= Time.deltaTime;
        } else {
            targetSpeed = walkSpeed;
        }

        if(dashCooldown > 0) {
            dashCooldown -= Time.deltaTime;
        }

        nSpeed = Mathf.Abs(targetSpeed - speed) / fraction * Time.deltaTime;

        if(targetSpeed > speed) {
            speed += nSpeed;
        } else if(targetSpeed < speed) {
            speed -= nSpeed;
        }
        if(Mathf.Abs(targetSpeed - speed) < nSpeed) {
            speed = targetSpeed;
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = direction * speed;

        // Head direction
        if(state == State.Attacking) {
            Vector2 dir = target.transform.position - transform.position;
            targetHeadDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, Quaternion.Euler(0, 0, targetHeadDirection - 90), tiltSpeed * Time.deltaTime);

        // Targeting enemies and state switching
        if(visionCone.visibleTargets.Count >= 1) {
            foreach(GameObject possibleTarget in visionCone.visibleTargets) {
                if(target == null) {
                    target = possibleTarget;
                    targetLastKnownPosition = target.transform.position;
                    searchFlag = true;
                } else if(target.GetComponent<MultiTag>().dangerLevel < possibleTarget.GetComponent<MultiTag>().dangerLevel) {
                    target = possibleTarget;
                    targetLastKnownPosition = target.transform.position;
                    searchFlag = true;
                }
            }
        } else {
            target = null;
        }

        if(Vector2.Distance(transform.position, targetLastKnownPosition) < tolerance && target == null) {
            searchFlag = false;
        }

        if(target != null) {
            SwitchState(State.Attacking);
        } else if(searchFlag == true) {
            SwitchState(State.Searching);
        } else {
            SwitchState(State.Roaming);
        }
    }

    public void Dash() {
        Vector2 optDirection = Vector2.zero;
        foreach(DamageEvent eve in damageEvents) {
            optDirection += eve.direction;
        }
        
        if(optDirection != Vector2.zero) {
            StopAllCoroutines();
            queued = false;
            dashTimer = dashStartTimer;
            dashCooldown = dashCooldownTime;

            float randAngle = Random.Range(-dashVariation, dashVariation);
            Vector2 newDirection;
            newDirection.x = optDirection.x * Mathf.Cos(randAngle * Mathf.Deg2Rad) - optDirection.y * Mathf.Sin(randAngle * Mathf.Deg2Rad);
            newDirection.y = optDirection.x * Mathf.Sin(randAngle * Mathf.Deg2Rad) + optDirection.y * Mathf.Cos(randAngle * Mathf.Deg2Rad);
            direction = newDirection.normalized;
            targetDirection = newDirection.normalized;
        }
    }

    public void SwitchState(State s) {
        if(state != s) {
            state = s;
            StopAllCoroutines();
            queued = false;
            switch(state) {
                case State.Inactive:
                    break;
                case State.Roaming:
                    behaviors.Clear();
                    behaviors.AddRange(roamBehaviors);
                    StartCoroutine(behaviors[0].Queue());
                    behaviors[0].CheckAction();
                    break;
                case State.Attacking:
                    behaviors.Clear();
                    behaviors.AddRange(attackBehaviors);
                    break;
                case State.Searching:
                    behaviors.Clear();
                    behaviors.AddRange(searchBehaviors);
                    break;
            }
        }
        
    }

    public void SlowUpdate() {
        foreach(Behavior behavior in behaviors) {
            behavior.Accumulate();
        }
        while(damageEvents.TryPeek(out DamageEvent eve) && eve.time < Time.time - ZaWarudo.dpsWindow) {
            damageEvents.Dequeue();
        }
        dps = 0;
        foreach(DamageEvent dEvent in damageEvents) {
            dps += dEvent.damage;
        }
        if(dps / health * 100 >= dpsThreshold && dashCooldown <= 0) {
            Dash();
        }
        //Debug.Log(dps);
    }

    public enum State {
        Pool,
        Inactive,
        Stationary,
        Roaming,
        Attacking,
        Searching,
        Dead
    }

    public IEnumerator FollowPath(Vector3 startPos, Vector3 targetPos) {
        path.Find(startPos, targetPos);
        foreach(Node n in path.path) {
            //Debug.Log(n.worldPosition);
            yield return StartCoroutine(GoToPoint(n.worldPosition));
            //Debug.Log("loop finished");
        }
    }

    public IEnumerator FollowPath(Vector3 startPos, Vector3 targetPos, int stepLimit, bool lookInDirection = false) {
        point = targetPos;
        if(!path.grid.NodeFromWorldPoint(targetPos).walkable) {targetPos = path.grid.NearestNode(path.grid.NodeFromWorldPoint(targetPos)).worldPosition;}
        path.Find(startPos, targetPos);
        if(stepLimit > -1 && path.path.Count > stepLimit) {path.path.RemoveRange(stepLimit, path.path.Count - stepLimit);}
        //Debug.Log(path.path.Count);
        foreach(Node n in path.path) {
            if(lookInDirection) targetHeadDirection = Mathf.Atan2(n.worldPosition.y - transform.position.y, n.worldPosition.x - transform.position.x) * Mathf.Rad2Deg;
            yield return StartCoroutine(GoToPoint(n.worldPosition));
        }
    }

    public IEnumerator GoToPoint(Vector3 point) {
        while(Vector3.Distance(transform.position, point) > tolerance) {
            targetDirection = new Vector2(point.x - transform.position.x, point.y - transform.position.y).normalized;
            //Debug.Log(direction);
            yield return null;
        }

        targetDirection = Vector2.zero;
    }

    public void TakeDamage(float damage, Vector2 direction){
        health -= damage;
        damageEvents.Enqueue(new DamageEvent(damage, Time.time, direction));
        //Debug.Log("" + damage + ", " + direction + ", " + Time.time);
        if(health <= 0) {
            state = State.Dead;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawCube(point, Vector2.one);
        Gizmos.DrawCube(roamPoint, Vector2.one * 3);
    }
}
