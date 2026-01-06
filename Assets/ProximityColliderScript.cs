using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Bullet") {
            transform.parent.gameObject.GetComponent<EntityBehavior>().bulletsInCollider++;
            transform.parent.gameObject.GetComponent<EntityBehavior>().damageEvents.Enqueue(new DamageEvent(col.GetComponent<Projectiles>().damage, Time.time, col.GetComponent<Projectiles>().direction));
            //Debug.Log(transform.parent.gameObject.GetComponent<EnemyBehavior>().bulletsInCollider + ", BiC");
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.tag == "Bullet") {
            transform.parent.gameObject.GetComponent<EntityBehavior>().bulletsInCollider--;
            //Debug.Log(transform.parent.gameObject.GetComponent<EnemyBehavior>().bulletsInCollider + ", BiC");
        }
    }
}
