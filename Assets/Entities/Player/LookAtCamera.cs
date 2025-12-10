using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    public Vector3 mousePos;
    public float tiltSpeed;
    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        float xPos = mousePos.x - /*(Screen.width/2)*/ mainCam.WorldToScreenPoint(transform.position).x;
        float yPos = mousePos.y - /*(Screen.height/2)*/ mainCam.WorldToScreenPoint(transform.position).y;

        float newAngle = Mathf.Atan2(yPos, xPos) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, newAngle - 90), tiltSpeed * Time.deltaTime);

       // main.transform.eulerAngles = new Vector3(0, 0, newAngle-90);
    }
}

