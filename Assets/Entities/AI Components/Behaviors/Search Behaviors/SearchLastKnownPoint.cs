using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLastKnownPoint : Behavior
{
    public SearchLastKnownPoint(EnemyBehavior s) : base(s) {
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
        yield return script.FollowPath(script.transform.position, script.targetLastKnownPosition, (int)script.distance, true);
        Vector2 dir = script.targetLastKnownPosition - (Vector2)script.transform.position;
        Debug.Log(dir);
        script.targetHeadDirection = script.targetLastKnownPosition - (Vector2)script.transform.position;
        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string CheckAction()
    {
        return "Searching target's last known position";
    }
}
