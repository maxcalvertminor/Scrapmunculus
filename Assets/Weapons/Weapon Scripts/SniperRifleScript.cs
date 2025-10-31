using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleScript : Weapon
{
    private Quaternion accuracy;
    public float firepoint_x;
    public float firepoint_y;
    public float firepoint_z;
    
    // Start is called before the first frame update
    void Start()
    {
        equipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Fire() {
        yield break;
    }
}