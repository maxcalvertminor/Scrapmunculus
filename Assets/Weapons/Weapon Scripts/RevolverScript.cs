using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverScript : Weapon
{
    private Quaternion accuracy;

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

