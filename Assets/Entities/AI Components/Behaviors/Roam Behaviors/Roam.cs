using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roam : Behavior
{
    public Roam(EntityBehavior s) : base(s) {
        script = s;
    }

    public override void Accumulate()
    {
        priority += 1f;
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        yield return script.basicMovement.FollowPath(script.transform.position, script.roamPoint, (int)script.distance, true);
        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string CheckAction()
    {
        return "Roaming";
    }
}
