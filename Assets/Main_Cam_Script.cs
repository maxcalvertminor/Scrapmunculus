using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Cam_Script : MonoBehaviour
{
    public GameObject player;
    public Camera mainCam;
    public float camera_distance;
    public float smoothTime;
    private Rigidbody2D body;
    public float mouseEffectConstant, maxMouseDistanceAllowed;
    [SerializeField] private Vector2 acceleration = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        body = mainCam.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0);

        Vector2 camPos = mainCam.transform.position;
        Vector2 targetPos = (Vector2)player.transform.position;
        //Vector2.Distance(mousePos, new Vector3(Screen.width/2, Screen.height/2, 0));
        Debug.Log(mousePos.magnitude);
        if(mousePos.magnitude > maxMouseDistanceAllowed) {
            targetPos += (Vector2)(mousePos - mousePos.normalized * maxMouseDistanceAllowed) * mouseEffectConstant;
        }
        //Vector2 targetPos = new(xPos, yPos);
        Vector2 holder = Vector2.SmoothDamp(camPos, targetPos, ref acceleration, smoothTime);
        mainCam.transform.position = new Vector3(holder.x, holder.y, camera_distance);
    }
}
