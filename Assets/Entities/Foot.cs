//using System;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public Vector2 homePoint;
    public int group;
    public Vector3 initialPosition;
    public Vector2 randomVariation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInitialPositionAndRandomness() {
        initialPosition = transform.position;
        randomVariation = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
}
