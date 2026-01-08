using UnityEngine;
using System.Collections;
using System;

public class EquipPoint : MonoBehaviour
{
    public EntityWeapons weaponHolder;
    public bool equipped, rotating;
    public GameObject weapon;
    public Weapon weaponScript;
    public float distanceToTarget, minDistance, maxDistance, constantChance, waitTimeAfterFailedShot;
    float chanceTime /* YOWWW! */;
    void Start()
    {
        rotating = false;
        if(weapon != null) weaponScript = weapon.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        // Distance to target
        if(GetComponentInParent<EntityTargeting>().target != null) {
            distanceToTarget = Vector2.Distance(transform.position, GetComponentInParent<EntityTargeting>().target.transform.position);
        }

        // Firing
        if(weapon != null && transform.GetComponentInParent<EntityTargeting>().state == EntityTargeting.State.Attacking && weaponScript.ShouldFire() && chanceTime <= 0) {
            float range = weaponScript.range * weaponScript.speed;
            float chanceToShoot = Mathf.Clamp01(1 - (distanceToTarget - range*minDistance) / (range*maxDistance - range*minDistance)) + constantChance;
            Debug.Log(chanceToShoot);
            if(UnityEngine.Random.Range(0f, 1f) <= chanceToShoot) {
                StartCoroutine(weaponScript.Fire());
            } else {
                chanceTime = waitTimeAfterFailedShot;
            }
        }

        // Chance timer
        if(chanceTime > 0) {
            chanceTime -= Time.deltaTime;
        }
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
