using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : Behavior
{
    public int raycastedBullets, bulletsInCollider;
    public GameObject target, subject;
    public int length;
    public Collider2D col;

    public Strafe(EnemyBehavior s) : base(s) {
        script = s;
        target = script.target;
        subject = script.gameObject;
        col = subject.transform.GetChild(0).GetComponent<Collider2D>();
    }

    public override void Accumulate()
    {
        priority += 1.5f + 0.2f * script.raycastsInCollider + 0.2f * bulletsInCollider + Random.Range(0f, 0.1f);
    }

    public override IEnumerator Queue()
    {
        script.queued = true;
        Vector2 point = new(target.transform.position.x - subject.transform.position.x, target.transform.position.y - subject.transform.position.y);
        if(Random.value > 0.5) {
            point = new Vector2(-point.y, point.x).normalized * script.distance;
        } else {
            point = new Vector2(point.y, -point.x).normalized * script.distance;
        }

        yield return script.FollowPath(script.transform.position, script.transform.position + (Vector3)point, (int)script.distance);

        script.queued = false;
        priority = 0;
        yield break;
    }

    public void IncrementRaycastedBullets() {

    }

    public void IncrementBulletsInCollider() {
        
    }

    public override string CheckAction()
    {
        return "Strafing";
    }

}
