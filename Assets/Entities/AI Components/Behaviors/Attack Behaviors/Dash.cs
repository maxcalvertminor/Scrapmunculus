using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : Behavior
{
    public Dash(EnemyBehavior s) : base(s) {
        script = s;
    }

    public override void Accumulate()
    {
        priority += 1f * script.raycastsInCollider + 1f * script.bulletsInCollider + Random.Range(0f, 0.1f);
    }

    public override IEnumerator Queue()
    {
        yield break;
    }

    public override string CheckAction()
    {
        return "Dashing";
    }
}
