using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootScript_Multiple : MonoBehaviour
{
    public GameObject main;
    public GameObject movement_obj;
    public GameObject[] feet;
    public Vector3[] footHomePoint;
    public float seconds_between_steps;
    public float step_speed;
    private bool moving;
    private bool is_stepping;
    public float deflection;
    public float constant;
    public float tolerance;
    private int farthest_foot_index;
    private int ix;

    // Start is called before the first frame update
    void Start()
    {
        moving = true;
        is_stepping = false;
        ix = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(main.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0) {
            moving = true;
        } else {
            moving = false;
        }

       /* farthest_foot_index = 0;
        for(int i = 1; i < feet.Length; i++) {
            if(Vector2.Distance(feet[i].transform.position, movement_obj.transform.position) > Vector2.Distance(feet[farthest_foot_index].transform.position, movement_obj.transform.position)) {
                farthest_foot_index = i;
            }
        } */

        if(moving && !is_stepping) {
            StartCoroutine(Step(feet[ix], footHomePoint[ix], main.GetComponent<Rigidbody2D>()));
        }


        /* if(Vector2.Distance(foot_right.transform.position, movement_obj.transform.position) > tolerance) {
            StartCoroutine(StepToPoint(foot_right, movement_obj.transform.TransformPoint(x, y, z)));
        }
        if(Vector2.Distance(foot_left.transform.position, movement_obj.transform.position) > tolerance) {
            StartCoroutine(StepToPoint(foot_left, movement_obj.transform.TransformPoint(-x, y, z)));
        } 

        if(Input.GetButtonDown("Left")) {
            for(int i = 0; i < feet.Length; i++) {
                Debug.Log("Numba " + i + ": " + Vector2.Distance(feet[i].transform.position, movement_obj.transform.position));
            }
        }
        if(Input.GetButtonDown("Right")) {
            Debug.Log("Farthest foot " + farthest_foot_index);
        } */
    }

    /*IEnumerator MyCoroutine(GameObject foot, Vector3 targetPoint, float deflection) {
        Vector3 startPoint = foot.transform.position;
        is_stepping = true;
        float fraction = 0;
        while(fraction < 1) {
            fraction += step_speed * Time.deltaTime;
            foot.transform.position = Vector3.Lerp(startPoint, movement_obj.transform.TransformPoint(targetPoint * deflection + added_constant), fraction);
            yield return null;
        }

        yield return new WaitForSeconds(seconds_between_steps);
        is_stepping = false;
        ix++;
        if(ix == feet.Length) {
            ix = 0;
        }
    }*/

    IEnumerator Step(GameObject foot, Vector2 targetPoint, Rigidbody2D mainBody) {
        Vector3 startPoint = foot.transform.position;
        is_stepping = true;
        float fraction = 0;
        while(fraction < 1) {
            fraction += step_speed * Time.deltaTime;
            foot.transform.position = Vector3.Lerp(startPoint, main.transform.TransformPoint(targetPoint + (mainBody.linearVelocity * constant)), fraction);
            //Debug.Log("targetPoint: " + targetPoint);
            //Debug.Log("velocity: " + mainBody.velocity);
            //Debug.Log("full thing: " + main.transform.TransformPoint(targetPoint + (mainBody.velocity * constant)));
            yield return null;
        }

        yield return new WaitForSeconds(seconds_between_steps);
        is_stepping = false;
        ix++;
        if(ix == feet.Length) {
            ix = 0;
        }
    }

    /*IEnumerator StepToPoint(GameObject foot, Vector3 axle, Vector3 point) {
        Vector3 startPoint = foot.transform.position;
        float fraction = 0;
        while(fraction < 1) {
            fraction += step_speed * Time.deltaTime;
            foot.transform.position = Vector3.Lerp(startPoint, movement_obj.transform.TransformPoint(axle * deflection + added_constant), fraction);
            yield return null;
        }
    }*/

}

