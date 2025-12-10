using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadMovement : MonoBehaviour
{

    private Quaternion to;
    public GameObject tread;
    public int speed;
    public int lowspeed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GO");
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        float newAngle;

       /* if(xInput > 0) {
            newAngle = Mathf.Atan(yInput / xInput) * Mathf.Rad2Deg;
        } else {
            newAngle = Mathf.Atan(yInput / xInput) * Mathf.Rad2Deg + 180;
        } */

        
        newAngle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
        to = Quaternion.Euler(0, 0, newAngle);

        if(xInput != 0 || yInput != 0) {
          //  Debug.Log(newAngle);
            tread.transform.rotation = Quaternion.RotateTowards(tread.transform.rotation, to, speed);  
        }


       // if(xInput != 0 || yInput != 0) {
       //    tread.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(yInput / xInput) * Mathf.Rad2Deg + 90); 
       // }
    }
}
