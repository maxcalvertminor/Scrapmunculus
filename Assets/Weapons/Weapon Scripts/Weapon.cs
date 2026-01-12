using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject projectile;
    public List<GameObject> bulletPool;
    protected int poolIterator;

    public float range;
    public float damage;
    public float speed;
    public float spread;

    public float fireRate;
    public Vector2 firePoint;
    public float reloadRate;
    public int ammoAddedOnSingleReloadAmount;
    [SerializeField] protected int ammo;
    public int maxAmmo;
    public bool equipped;
    public float x;
    public float y;

    public bool firing;
    public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        firing = false;
        Debug.Log(ammo + " + " + maxAmmo);
        ammo = maxAmmo;
        Debug.Log(ammo);
        reloading = false;
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
        while (ammo != maxAmmo)
        {
            Debug.Log(ammo);
            ammo += ammoAddedOnSingleReloadAmount;
            for (float i = 0; i < reloadRate; i += Time.deltaTime)
            {
                if (firing)
                {
                    reloading = false;
                    Debug.Log("Interrupted reloading");
                    yield break;
                }
                yield return null;
            }
        }
        reloading = false;
        yield break;
    }

    public bool CanFire() {
        return !firing && ammo > 0;
    }

    public bool ShouldFire() {
        return !firing && ammo > 0 && !reloading;
    }

    public bool ShouldReload() {
        return !firing && ammo == 0 && !reloading;
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
        } else {
            transform.position = actuator.transform.position;
            transform.rotation = actuator.transform.rotation;
            transform.localPosition = new Vector3 (-x, y, 0);
            actuator.GetComponent<Arm_Actuator_Script>().attach(true);
        } 
        
    }

    public void detach(GameObject actuator) {
        actuator.GetComponent<Arm_Actuator_Script>().attach(false);
    }

}
