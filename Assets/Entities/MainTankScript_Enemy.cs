using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTankScript_Enemy : MonoBehaviour
{
    public bool active;
    public GameObject main;
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
        
        if(active == true) {
            float xPos = mainCam.WorldToScreenPoint(target.transform.position).x - mainCam.WorldToScreenPoint(main.transform.position).x;
            float yPos = mainCam.WorldToScreenPoint(target.transform.position).y - mainCam.WorldToScreenPoint(main.transform.position).y;

            float newAngle = Mathf.Atan2(yPos, xPos) * Mathf.Rad2Deg;
        
            main.transform.rotation = Quaternion.RotateTowards(main.transform.rotation, Quaternion.Euler(0, 0, newAngle - 90), tiltSpeed * Time.deltaTime);
        }
    }
}
