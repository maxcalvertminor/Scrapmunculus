using System.Collections.Generic;
using UnityEngine;

public class EntityWeapons : MonoBehaviour
{
    public List<EquipPoint> equipPoints;
    public float maxEquipPointRotation, weaponRotateSpeed, weaponPatrolTime;
    public bool passiveRotationOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(passiveRotationOn) {
            PassiveRotation();
        }
    }

    public void EquipWeapon(GameObject weapon) {
        foreach(EquipPoint e in equipPoints) {
            if(e.equipped == true) {
                e.weapon = weapon;
            }
        }
    }

    public void PassiveRotation() {
        foreach(EquipPoint e in equipPoints) {
            if(e.rotating == false && e.equipped == true) StartCoroutine(e.EquipPointPassiveRotation(weaponRotateSpeed, weaponPatrolTime));
        }
    }

}