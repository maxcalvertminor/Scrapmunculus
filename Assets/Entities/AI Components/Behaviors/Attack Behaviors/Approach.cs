using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approach : Behavior
{
    public bool inRange;
    public bool hiding;
    public Vector2 targetPoint;

    public Approach(EntityBehavior s) : base(s) {
        script = s;
        hiding = false;
    }

    public override void Accumulate()
    {
        priority += 0.5f * (inRange ? 0 : 1) + 1f * script.aggression + Random.Range(0f, 0.1f);
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        /*Vector2 dir = (target.transform.position - script.transform.position).normalized;
        targetPoint = (Vector2)script.transform.position + dir * distance;*/

        yield return script.basicMovement.FollowPath(script.transform.position, script.targetingSystem.target.transform.position, (int)script.distance);
        
        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string ToString() {
        return "Approach priority: " + priority;
    }

    public override Vector3 DebugHelper() {
        return targetPoint;
    }

    public override string CheckAction()
    {
        return "Approaching";
    }
}
