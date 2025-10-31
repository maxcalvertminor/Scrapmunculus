using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : Weapon
{
    public int bulletCount;
    public float bulletVariation;
    
    // Start is called before the first frame update
    void Start()
    {
        equipped = false;
        fireRate = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Fire() {
        firing = true;
        for(int i = 0; i < bulletCount; i++) {
            Instantiate(projectile, transform.TransformPoint(firePoint.x, firePoint.y, 0), transform.rotation).transform.Rotate(0, 0, Random.Range(-bulletVariation, bulletVariation));
        }
        yield return new WaitForSeconds(fireRate);
        Debug.Log("Waited " + fireRate + " seconds");
        ammo--;
        firing = false;
    }
}