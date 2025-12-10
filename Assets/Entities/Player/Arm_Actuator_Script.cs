using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_Actuator_Script : MonoBehaviour
{

    public GameObject actuator;
    public float speed;
    public Camera mainCam;
    public float tiltSpeed;
    private bool attached;

    // Start is called before the first frame update
    void Start()
    {
        actuator = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetButton("Left")) {
            actuator.transform.Rotate(0, 0, speed * Time.deltaTime);
        }
        if(Input.GetButton("Right")) {
            actuator.transform.Rotate(0, 0, -speed * Time.deltaTime);
        }*/

        if(attached) {
            float xPos = Input.mousePosition.x - mainCam.WorldToScreenPoint(actuator.transform.position).x;
            float yPos = Input.mousePosition.y - mainCam.WorldToScreenPoint(actuator.transform.position).y;

            float newAngle = Mathf.Atan2(yPos, xPos) * Mathf.Rad2Deg;

            actuator.transform.rotation = Quaternion.RotateTowards(actuator.transform.rotation, Quaternion.Euler(0, 0, newAngle - 90), tiltSpeed * Time.deltaTime);
        } else {
            actuator.transform.localEulerAngles = new Vector3(0, 0, 0); 
        }

    }

    public void attach(bool helper) {
        attached = helper;
    }
}
