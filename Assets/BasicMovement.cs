using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Transform obj;
    public float speed;
    public float turnSpeed;
    public float tolerance;
    public Pathfinding pathFinding;
    public GameObject mainTankHead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pebug")) {
            //StartCoroutine(GoToPoint(pathFinding.target.position));
            StartCoroutine(FollowPath(pathFinding.path));
        }
    }

    public IEnumerator FollowPath(List<Node> path) {
        foreach(Node n in path) {
            Debug.Log(n.worldPosition);
            yield return StartCoroutine(GoToPoint(n.worldPosition));
            Debug.Log("loop finished");
        }
    }

    /*public IEnumerator FollowPath(Vector3 startPos, Vector3 targetPos) {
        pathFinding.Path(startPos, pathFinding.target.position);
        foreach(Node n in pathFinding.path) {
            Debug.Log(n.worldPosition);
            yield return StartCoroutine(GoToPoint(n.worldPosition));
            Debug.Log("loop finished");
        }
    }*/


    public IEnumerator GoToPoint(Vector3 point) {
        Debug.Log("started coroutine");
        float x = point.x - obj.position.x;
        float y = point.y - obj.position.y;

        /*float newAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        Quaternion temp = mainTankHead.transform.rotation;
        obj.rotation = Quaternion.Euler(0, 0, newAngle - 90);
        mainTankHead.transform.rotation = temp;*/

        while(Vector3.Distance(obj.position, point) > tolerance) {
            obj.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(x, y).normalized * speed;
            yield return null;
        }

        obj.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        Debug.Log("Coroutine finished");
    }

    public void PathfindingDebug() {
        
    }
}
