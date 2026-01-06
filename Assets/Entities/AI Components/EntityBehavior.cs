using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    public BasicMovement basicMovement;
    public float health;
    public Weapon weapon1, weapon2;
    public GameObject head;    
    public LookAtX lookAtPlayerScript;
    public Pathfinding path;
    public EntityWeapons entityWeapons;

    //public GameObject pathfindingTracking;

    public bool queued;
    public List<Behavior> behaviors; 

    public float distance;

    public float aggression, nervousness, perception;
    //public float tolerance;

    public int bulletsInCollider;
    public int raycastsInCollider;

    public List<Behavior> attackBehaviors, roamBehaviors, searchBehaviors;

    //public float targetSpeed, fraction, dashSpeed, walkSpeed, dashCooldownTime, dashStartTimer, turnSpeed, weaponPatrolTime, weaponRotateSpeed;
    //private float speed, nSpeed, dashCooldown, dashTimer;
    //public Vector2 direction, targetDirection;

    public Vector2 point;

    public float dpsThreshold;
    public float dps, dashVariation;
    public Queue<DamageEvent> damageEvents;

    public Vector2 roamPoint, targetLastKnownPosition;

    //public Vector2 targetHeadDirection;
    public float tiltSpeed;
    public EntityTargeting targetingSystem;

    //public List<string> hostileTags;
    //public List<GameObject> hostileEntities;

    //public float searchActions, searchActionsNumber;
    //public bool searchFlag;

    // Start is called before the first frame update
    void Start()
    {
        attackBehaviors = new List<Behavior> { new Approach(this), new Pause(this, 2), new Retreat(this), new Strafe(this) };
        roamBehaviors = new List<Behavior> { new PickRoamPoint(this), new Roam(this), new LookAround(this), new Pause(this, 4) };
        searchBehaviors = new List<Behavior> { new SearchLastKnownPoint(this), new LookAround(this)};
        behaviors = new List<Behavior>();
        lookAtPlayerScript = gameObject.GetComponent<LookAtX>();
        queued = false;
        //SwitchState(State.Roaming);
        InvokeRepeating("SlowUpdate", 0f, 0.2f);
        //InvokeRepeating("WeaponPatrolling", 0f, 1f);
        //direction = new(0,0);
        damageEvents = new();
        targetingSystem = GetComponentInChildren<EntityTargeting>();
        //target = null;
    }

    void OnEnable() {
        targetingSystem.stateChange += OnStateChange;
        //targetingSystem.stateChange +=
    }

    // Update is called once per frame
    void Update()
    {
        //pathfindingTracking.transform.position = targetLastKnownPosition;
        // Behavior Queuing
        if(!queued) {
            Behavior priority = behaviors[0];
            foreach(Behavior behavior in behaviors) {
                if(behavior.priority > priority.priority) {
                    priority = behavior;
                }
            }
            queued = true;
            Debug.Log(priority.CheckAction() + ": " + priority.priority);
            StartCoroutine(priority.Queue()); 
        }

        // Movement and direction
       /* if(direction != targetDirection) {
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

        gameObject.GetComponent<Rigidbody2D>().linearVelocity = direction * speed; */

        /* //Head direction and weapon rotating
        if(targetingSystem.state == EntityTargeting.State.Attacking) {
            targetHeadDirection = targetingSystem.target.transform.position - transform.position;
        } else if(targetingSystem.state == EntityTargeting.State.Roaming) {
            foreach(EquipPoint e in entityWeapons.equipPoints) {
                //if(e.rotating == false && e.equipped == true) StartCoroutine(e.EquipPointPassiveRotation(weaponRotateSpeed, weaponPatrolTime));
            }
        } */
        //head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, head.transform.rotation * Quaternion.FromToRotation(head.transform.up, targetHeadDirection), tiltSpeed * Time.deltaTime);

        // Targeting enemies and state switching
        /*if(targetingSystem.visibleTargets.Count >= 1) {
            foreach(GameObject possibleTarget in targetingSystem.visibleTargets) {
                if(target == null) {
                    target = possibleTarget;
                    targetLastKnownPosition = target.transform.position;
                    searchFlag = true;
                } else if(target.GetComponent<MultiTag>().dangerLevel < possibleTarget.GetComponent<MultiTag>().dangerLevel) {
                    target = possibleTarget;
                    targetLastKnownPosition = target.transform.position;
                    searchFlag = true;
                } else {
                    targetLastKnownPosition = target.transform.position;
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
        } */
    }

    /*public void Dash() {
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
    }*/

    public void OnStateChange(EntityTargeting.State state) {
        StopAllCoroutines();
        queued = false;
        switch(state) {
            case EntityTargeting.State.Inactive:
                break;
            case EntityTargeting.State.Roaming:
                behaviors.Clear();
                behaviors.AddRange(roamBehaviors);
                StartCoroutine(behaviors[0].Queue());
                behaviors[0].CheckAction();
                break;
            case EntityTargeting.State.Attacking:
                behaviors.Clear();
                behaviors.AddRange(attackBehaviors);
                break;
            case EntityTargeting.State.Searching:
                behaviors.Clear();
                behaviors.AddRange(searchBehaviors);
                break;
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
        //if(dps / health * 100 >= dpsThreshold && dashCooldown <= 0) {
            //Dash();
        //}
    }

    /*public IEnumerator FollowPath(Vector3 startPos, Vector3 targetPos) {
        path.Find(startPos, targetPos);
        foreach(Node n in path.path) {
            //Debug.Log(n.worldPosition);
            yield return StartCoroutine(GoToPoint(n.worldPosition));
            //Debug.Log("loop finished");
        }
    } */

    /*public IEnumerator FollowPath(Vector3 startPos, Vector3 targetPos, int stepLimit, bool lookInDirection = false) {
        point = targetPos;
        if(!path.grid.NodeFromWorldPoint(targetPos).walkable) {targetPos = path.grid.NearestNode(path.grid.NodeFromWorldPoint(targetPos)).worldPosition;}
        path.Find(startPos, targetPos);
        if(stepLimit > -1 && path.path.Count > stepLimit) {path.path.RemoveRange(stepLimit, path.path.Count - stepLimit);}
        Debug.Log(path.path.Count);
        foreach(Node n in path.path) {
            if(lookInDirection) targetHeadDirection =  n.worldPosition - transform.position;
            yield return StartCoroutine(GoToPoint(n.worldPosition));
        }
        direction /= 2;
    } 

    public IEnumerator GoToPoint(Vector3 point) {
        while(Vector3.Distance(transform.position, point) > tolerance) {
            targetDirection = new Vector2(point.x - transform.position.x, point.y - transform.position.y).normalized;
            //Debug.Log(direction);
            yield return null;
        }

        targetDirection = Vector2.zero;
    } */

    void OnDrawGizmos() {
        Gizmos.DrawCube(point, Vector2.one);
        Gizmos.DrawCube(roamPoint, Vector2.one * 3);
    }
}