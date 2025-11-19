using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Projectiles : MonoBehaviour
{

    public float range;
    public float speed;
    public float damage;

    public float timer;

    RaycastHit2D hit;

    public GameObject projectile;
    public Rigidbody2D body;
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        /* transform.Translate(Vector3.up * 0.6f);
        hit = Physics2D.Raycast(transform.position, transform.up);
        if(hit && hit.transform.tag == "Enemy") {
            hit.transform.GetComponent<EnemyBehavior>().raycastsInCollider++;
//            Debug.Log(hit.transform.GetComponent<EnemyBehavior>().raycastsInCollider + ", RiC");
        }
        direction = transform.up * speed; */
    }

    // Update is called once per frame
    void Update()
    {
        body.linearVelocity = transform.up * speed;
        timer += Time.deltaTime;
        if(timer >= range) {
            gameObject.SetActive(false);
        }
    }

    public void Setup(Vector2 pos, Vector2 dir, float dmg, float rang) {
        transform.position = pos;
        transform.up = dir.normalized;
        speed = dir.magnitude;
        range = rang;
        damage = dmg;
        timer = 0;
        GetComponent<TrailRenderer>().Clear();
    }

    public void OnEnable() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        switch(collision.gameObject.tag) {
            case "Walls":
                gameObject.SetActive(false);
                break;
            case "Enemy":
                collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage, direction);
                gameObject.SetActive(false);
                break;
        }
    }

    void OnDisable() {
        if(hit && hit.transform.tag == "EnemyProximityTrigger") {
            hit.transform.GetComponent<EnemyBehavior>().raycastsInCollider--;
//            Debug.Log(hit.transform.GetComponent<EnemyBehavior>().raycastsInCollider + ", RiC");
        }
    }
}
