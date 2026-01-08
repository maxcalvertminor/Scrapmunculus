using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickRoamPoint : Behavior
{
    GameObject subject;
    public PickRoamPoint(EntityBehavior s) : base(s) {
        script = s;
        subject = script.gameObject;
    }

    public override void Accumulate()
    {
        priority += Vector2.Distance(subject.transform.position, script.roamPoint) > script.basicMovement.tolerance ? 0 : 10;
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        script.roamPoint = new(Random.Range(-script.path.grid.gridWorldSize.x/2, script.path.grid.gridWorldSize.x/2), Random.Range(-script.path.grid.gridWorldSize.y/2, script.path.grid.gridWorldSize.y/2));
        //Debug.Log(script.roamPoint);
        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string CheckAction()
    {
        return "Picking a new roam point";
    }
}
