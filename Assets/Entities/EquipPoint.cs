using UnityEngine;
using System.Collections;
using System;

public class EquipPoint : MonoBehaviour
{
    public bool equipped, rotating;
    public GameObject weapon;
    void Start()
    {
        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator EquipPointPassiveRotation(float speed, float totalTime) {
        rotating = true;
        Debug.Log("Started coroutine");
        float waitTime = totalTime - UnityEngine.Random.Range(0, totalTime);
        float rotateTime = totalTime - waitTime;

        float sign = (float)Math.Pow(-1, UnityEngine.Random.Range(1, 3));

        yield return new WaitForSeconds(waitTime);
        Debug.Log("Finished waiting");
        for(float i = 0; i < rotateTime; i += Time.deltaTime) {
            transform.RotateAround(transform.position, new(0,0,1), speed * sign * Time.deltaTime);
            Debug.Log(sign);
            yield return null;
        }
        Debug.Log("Finished rotating");
        rotating = false;
        yield break;
    }
}
