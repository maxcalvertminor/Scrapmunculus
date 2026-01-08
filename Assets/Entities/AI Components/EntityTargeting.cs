using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EntityTargeting : MonoBehaviour
{
    public event Action<State> stateChange;
    public List<GameObject> visibleTargets;
    public float sightRange, sightConeAngle;
    public State state;
    public GameObject target;
    public Vector2 targetLastKnownPosition;
    private bool searchFlag;

    public BasicMovement basicMovement;

    public enum State {
        Pool,
        Inactive,
        Stationary,
        Roaming,
        Attacking,
        Searching,
        Dead
    }

    void Start()
    {
        InvokeRepeating("CanSeeTarget", 0f, 0.2f);
        target = null;
        SwitchState(State.Roaming);
        targetLastKnownPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(visibleTargets.Count >= 1) {
            foreach(GameObject possibleTarget in visibleTargets) {
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

        if(targetLastKnownPosition != null && Vector2.Distance(transform.position, targetLastKnownPosition) < basicMovement.tolerance && target == null) {
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

    public void CanSeeTarget() {
        visibleTargets.Clear();
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, sightRange);
        foreach(Collider2D collider in targetColliders) {
            Vector2 directionToTarget = collider.transform.position - transform.position;
            if(collider.GetComponent<MultiTag>() != null && collider.GetComponent<MultiTag>().HasTag("targetToGoliath") && Vector2.Angle(/*not supposed to be right*/transform.up, directionToTarget) <= sightConeAngle) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget.normalized, directionToTarget.magnitude, LayerMask.GetMask("Walls"));
                if(hit.collider == null) {
                    visibleTargets.Add(collider.transform.gameObject);
                }
            }
        }
    }

    public void SwitchState(State s) {
        if(state != s) {
            state = s;
            stateChange?.Invoke(state);
        }
    }
        

    public void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawRay(transform.position, transform.up * sightRange);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, sightConeAngle) * transform.up;
        Gizmos.DrawRay(transform.position, leftBoundary * sightRange);

        Vector3 rightBoundary = Quaternion.Euler(0, 0, -sightConeAngle) * transform.up;
        Gizmos.DrawRay(transform.position, rightBoundary * sightRange);
    }
}
