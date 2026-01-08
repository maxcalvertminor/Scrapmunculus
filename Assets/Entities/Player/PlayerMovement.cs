using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D body;
    public float speed;
    private float dashTimer;
    private float dashCooldown;
    private float targetSpeed;
    private Vector2 direction;
    public float dashSpeed;
    public float walkSpeed;
    private float nSpeed;
    public float fraction;
    public float startDashTimer;
    public float cooldown_time;
    private bool dashing;
    public GameObject cooldown_obj;
    public Sprite[] cooldown_anim_list;
    public float anim_speed;

    // Start is called before the first frame update
    void Start() {
        direction = new Vector2(0, 0);
        dashing = false;
    }

    void OnEnable() {
        GetComponent<Health>().death += OnDeath;
    }

    // Update is called once per frame
    void Update() {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if(dashTimer <= 0) {
            if(new Vector2(xInput, yInput) == Vector2.zero) {
                if(targetSpeed != 0) {
                    targetSpeed = 0;
                    nSpeed = Mathf.Abs(targetSpeed - speed) / fraction * Time.deltaTime;
                }
            } else {
                direction = new Vector2(xInput, yInput).normalized;
                if(targetSpeed != walkSpeed) {
                    targetSpeed = walkSpeed;
                    nSpeed = Mathf.Abs(targetSpeed - speed) / fraction * Time.deltaTime;
                }
            }
        } else {
            speed = dashSpeed;
            if(targetSpeed != dashSpeed) {
                targetSpeed = dashSpeed;
                nSpeed = Mathf.Abs(targetSpeed - speed) / fraction * Time.deltaTime;
            }
            dashTimer -= Time.deltaTime;
        }

        if(speed <= 25 && dashing == true) {
            dashing = false;
        }
        
        if(targetSpeed > speed) {
            speed += nSpeed;
        } else if(targetSpeed < speed) {
            speed -= nSpeed;
        }
        if(Mathf.Abs(targetSpeed - speed) < nSpeed) {
            speed = targetSpeed;
        }

        //body.linearVelocity = direction * speed;

        if(Input.GetButtonDown("Dash") && dashCooldown <= 0) {
            //StartCoroutine(Cooldown_Animation(cooldown_obj.GetComponent<SpriteRenderer>()));
            dashing = true;
            dashTimer = startDashTimer;
            dashCooldown = cooldown_time;
        }

        if(dashCooldown > 0) {
            dashCooldown -= Time.deltaTime;
        }
    }

    /*IEnumerator Cooldown_Animation(SpriteRenderer renderer) {
        float timer = 0;
        int i = 0;
        while(i < cooldown_anim_list.Length) {
            timer += Time.deltaTime;
            if(timer > anim_speed) {
                renderer.sprite = cooldown_anim_list[i];
                timer = 0;
                i++;
            }
            yield return null;
        }
    }*/

    void FixedUpdate() {
        body.linearVelocity = direction * speed;
    }

    void OnDeath() {
        enabled = false;
    }
}
