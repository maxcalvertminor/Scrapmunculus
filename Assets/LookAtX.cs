using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtX : MonoBehaviour
{
    public bool active;
    public GameObject target;
    public Camera mainCam;
    public float tiltSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(active == true && target != null) {
            float xPos = target.transform.position.x - transform.position.x;
            float yPos = target.transform.position.y - transform.position.y;

            float newAngle = Mathf.Atan2(yPos, xPos) * Mathf.Rad2Deg;
        
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, newAngle - 90), tiltSpeed * Time.deltaTime);
        }
    }
}
