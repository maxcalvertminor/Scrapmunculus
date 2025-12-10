using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public event Action<GameObject> TargetEnteredVision;
    public event Action TargetLeftVision;
    public List<GameObject> visibleTargets;
    public float sightRange, sightConeAngle;

    void Start()
    {
        InvokeRepeating("CanSeeTarget", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawRay(transform.position, transform.up * sightRange);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, sightConeAngle) * transform.up;
        Gizmos.DrawRay(transform.position, leftBoundary * sightRange);

        Vector3 rightBoundary = Quaternion.Euler(0, 0, -sightConeAngle) * transform.up;
        Gizmos.DrawRay(transform.position, rightBoundary * sightRange);
    }
}
