using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLastKnownPoint : Behavior
{
    public SearchLastKnownPoint(EntityBehavior s) : base(s) {
        script = s;
        priority = 10;
    }

    public override void Accumulate()
    {
        priority = 10;
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        yield return script.basicMovement.FollowPath(script.transform.position, script.targetingSystem.targetLastKnownPosition, (int)script.distance, true);
        Vector2 dir = script.targetingSystem.targetLastKnownPosition - (Vector2)script.transform.position;
        Debug.Log(dir);
        script.basicMovement.targetHeadDirection = script.targetingSystem.targetLastKnownPosition - (Vector2)script.transform.position;
        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string CheckAction()
    {
        return "Searching target's last known position";
    }
}
