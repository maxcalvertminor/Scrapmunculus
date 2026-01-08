using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : Weapon
{
    public int bulletCount;
    public int bulletPoolSize;
    public float accuracy;
    
    // Start is called before the first frame update
    void Start()
    {
        equipped = false;
        ammo = maxAmmo;
        for(int i = 0; i < bulletPoolSize; i++) {
            bulletPool.Add(Instantiate(projectile));
            bulletPool[i].SetActive(false);
        }
        poolIterator = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override IEnumerator Fire() {
        firing = true;
        for(int i = 0; i < bulletCount; i++) {
            poolIterator %= bulletPoolSize;
            Vector2 direction = transform.up * speed + transform.up * Random.Range(accuracy - 1, 1 - accuracy);
            bulletPool[poolIterator].SetActive(true);
            bulletPool[poolIterator].GetComponent<Projectiles>().Setup((Vector2)transform.position + (Vector2)transform.up * firePoint.y + (Vector2)transform.right * firePoint.x, direction + (Vector2)transform.up * Random.Range(-spread, spread) + (Vector2)transform.right * Random.Range(-spread, spread), damage, range);
            poolIterator++;
        }
        Debug.Log("Wait start");
        yield return new WaitForSeconds(fireRate);
        Debug.Log("Wait end");
//        Debug.Log("Ammo: " + (ammo-1));
        ammo--;
        firing = false;
    }

    public void ChangeAccuracy(float addend) {
        accuracy += addend;
    }
}