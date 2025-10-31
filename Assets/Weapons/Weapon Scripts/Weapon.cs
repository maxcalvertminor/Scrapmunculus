using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject projectile;
    public List<GameObject> bullets;

    public float fireRate;
    public Vector2 firePoint;
    public float reloadRate;
    public int ammoAddedOnSingleReloadAmount;
    private int ammo;
    public int maxAmmo;
    public bool equipped;
    public float x;
    public float y;
    public bool right_side;

    public bool firing;
    public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        firing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual IEnumerator Fire() {
        yield break;
    }

    public IEnumerator Reload() {
        reloading = true;
        Debug.Log("Reloading");
        while(ammo != maxAmmo) {
            ammo += ammoAddedOnSingleReloadAmount;
            for(float i = 0; i < reloadRate; i += Time.deltaTime) {
                if(firing) {
                    reloading = false;
                    Debug.Log("Interrupted reloading");
                    yield break;
                }
                yield return null;
            }
        }
        yield break;
    }

    public bool CanFire() {
        return !firing && ammo > 0;
    }

    public int getAmmo() {
        return ammo;
    }

    public void setEquipped(bool helper) {
        equipped = helper;
    } 

    public void attach(bool rightorleft, GameObject actuator) {
        if(rightorleft) {
            transform.position = actuator.transform.position;
            transform.rotation = actuator.transform.rotation;
            transform.localPosition = new Vector3 (x, y, 0);
            actuator.GetComponent<Arm_Actuator_Script>().attach(true);
            right_side = true;
        } else {
            transform.position = actuator.transform.position;
            transform.rotation = actuator.transform.rotation;
            transform.localPosition = new Vector3 (-x, y, 0);
            actuator.GetComponent<Arm_Actuator_Script>().attach(true);
            right_side = false;
        } 
        
    }

    public void detach(GameObject actuator) {
        actuator.GetComponent<Arm_Actuator_Script>().attach(false);
    }

}
