
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Behavior
{
    public EnemyBehavior script;
    public float priority;
    public Behavior(EnemyBehavior s) {
        script = s;
    }

    public virtual void Accumulate() {

    }
    public virtual IEnumerator Queue() {
        yield return null;
    }

    public virtual Vector3 DebugHelper() {
        return Vector3.one;
    }

    public virtual string CheckAction() {
        return "Behaving";
    }
}