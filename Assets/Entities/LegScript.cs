using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegScript : MonoBehaviour
{
    public Transform foot;
    public Transform pivot;
    public float sillyConstant;
    // Start is called before the first frame update
    void Start()
    {
        pivot = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float x = foot.position.x - pivot.position.x;
        float y = foot.position.y - pivot.position.y;
        float newAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        pivot.rotation = Quaternion.Euler(0, 0, newAngle - 90);

        pivot.localScale = new Vector3(pivot.localScale.x, Vector3.Distance(foot.position, pivot.position) * sillyConstant, pivot.localScale.z);
    }
}
