using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retreat : Behavior
{
    public int numEnemyWeaponInRange;
    public bool inRange;
    public float length;
    public GameObject target, subject;
    
    public Retreat(EntityBehavior s) : base(s) {
        script = s;
        length = 10;
        subject = script.gameObject;
        //target = script.targetingSystem.target;
    }

    public override void Accumulate()
    {
        priority += 0.2f * (inRange ? 0 : 1) + 1f * script.nervousness + Random.Range(0f, 0.1f);
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        Vector2 point = subject.transform.position - (script.targetingSystem.target.transform.position - subject.transform.position).normalized * length;

        yield return script.basicMovement.FollowPath(script.transform.position, point, (int)script.distance);

        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string CheckAction()
    {
        return "Retreating";
    }
}
