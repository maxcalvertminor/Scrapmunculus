using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    //public Transform obj;
    public float tolerance;
    public GameObject head;
    public Pathfinding path;
    public GameObject pathfindingTracking;
    public EntityTargeting targetingSystem;
    public EntityWeapons entityWeapons;

    public float targetSpeed, fraction, dashSpeed, walkSpeed, dashCooldownTime, dashStartTimer, turnSpeed, weaponPatrolTime, weaponRotateSpeed, headTiltSpeed;
    private float speed, nSpeed, dashCooldown, dashTimer;
    public Vector2 direction, targetDirection, targetHeadDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        GetComponentInChildren<EntityTargeting>().stateChange += OnStateChange;
    }

    public void OnStateChange(EntityTargeting.State state) {
        targetDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Current direction slides toward target direction
        if(direction != targetDirection) {
            direction = Vector2.MoveTowards(direction, targetDirection, Time.deltaTime * turnSpeed);
        }

        // Figuring out speed based on direction and if dashing
        if(targetDirection == Vector2.zero) {
            targetSpeed = 0;
        } else if(dashTimer > 0) {
            targetSpeed = dashSpeed;
            speed = dashSpeed;
            dashTimer -= Time.deltaTime;
        } else {
            targetSpeed = walkSpeed;
        }

        // Dash timer
        if(dashCooldown > 0) {
            dashCooldown -= Time.deltaTime;
        }

        // Speed is calculated
        nSpeed = Mathf.Abs(targetSpeed - speed) / fraction * Time.deltaTime;

        if(targetSpeed > speed) {
            speed += nSpeed;
        } else if(targetSpeed < speed) {
            speed -= nSpeed;
        }
        if(Mathf.Abs(targetSpeed - speed) < nSpeed) {
            speed = targetSpeed;
        }

        // Movement applied to velocity
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;

        // Moving head
        if(targetingSystem.state == EntityTargeting.State.Attacking) {
            targetHeadDirection = targetingSystem.target.transform.position - transform.position;
        } else if(targetingSystem.state == EntityTargeting.State.Roaming) {
            /*foreach(EquipPoint e in entityWeapons.equipPoints) {
                //if(e.rotating == false && e.equipped == true) StartCoroutine(e.EquipPointPassiveRotation(weaponRotateSpeed, weaponPatrolTime));
            }*/
        }
        head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, head.transform.rotation * Quaternion.FromToRotation(head.transform.up, targetHeadDirection), headTiltSpeed * Time.deltaTime);
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
        //point = targetPos;
        if(!path.grid.NodeFromWorldPoint(targetPos).walkable) {targetPos = path.grid.NearestNode(path.grid.NodeFromWorldPoint(targetPos)).worldPosition;}
        path.Find(startPos, targetPos);
        if(stepLimit > -1 && path.path.Count > stepLimit) {path.path.RemoveRange(stepLimit, path.path.Count - stepLimit);}
        //Debug.Log(path.path.Count);
        foreach(Node n in path.path) {
            if(lookInDirection) targetHeadDirection = /*Mathf.Atan2(n.worldPosition.y - transform.position.y, n.worldPosition.x - transform.position.x) * Mathf.Rad2Deg*/ n.worldPosition - transform.position;
            yield return StartCoroutine(GoToPoint(n.worldPosition));
        }
        //direction /= 2;
    }

    public IEnumerator GoToPoint(Vector3 point) {
        while(Vector3.Distance(transform.position, point) > tolerance) {
            targetDirection = new Vector2(point.x - transform.position.x, point.y - transform.position.y).normalized;
            //Debug.Log(direction);
            yield return null;
        }

        targetDirection = Vector2.zero;
    }

    public void PathfindingDebug() {
        
    }
}
