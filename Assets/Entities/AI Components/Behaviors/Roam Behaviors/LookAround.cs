using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : Behavior
{
    public LookAround(EnemyBehavior s) : base(s) {
        script = s;
    }

    public override void Accumulate()
    {
        priority += 0.112f;
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        int loop = Random.Range(1, 4);
        for(int i = 0; i < loop; i++) {
            script.targetHeadDirection += new Vector2(Random.Range(-script.targetHeadDirection.magnitude, script.targetHeadDirection.magnitude), Random.Range(-script.targetHeadDirection.magnitude, script.targetHeadDirection.magnitude));
            while(script.head.transform.rotation != script.head.transform.rotation * Quaternion.FromToRotation(script.head.transform.up, script.targetHeadDirection)) {
                yield return null;
            }
            yield return new WaitForSeconds(script.distance / 20 * Random.Range(1f, 3f));
        }
        script.queued = false;
        priority = 0;
        yield break;
    }

    public override string CheckAction()
    {
        return "Looking around";
    }
}
