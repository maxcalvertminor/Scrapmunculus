using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : Behavior
{
    int divider;
    public Pause(EntityBehavior s, int d) : base(s) {
        script = s;
        divider = d;
    }
    public override void Accumulate()
    {
        priority += 0.05f + Random.Range(0f, 0.1f);
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        //Debug.Log(script.targetDirection);
        script.basicMovement.targetDirection = Vector2.zero;
        script.basicMovement.targetSpeed = 0;
        yield return new WaitForSeconds(script.distance / divider * Random.Range(0f, 2f));
        script.queued = false;
        priority = 0;
    }

    public override string CheckAction()
    {
        return "Pausing";
    }
}
