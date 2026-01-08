using System.Collections.Generic;
using UnityEngine;

public class EntityWeapons : MonoBehaviour
{
    public List<EquipPoint> equipPoints;
    public float maxEquipPointRotation, weaponRotateSpeed, weaponPatrolTime;
    public bool passiveRotationOn;
    public EntityTargeting targetingSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //targetingSystem = GetComponentInChildren<EntityTargeting>();
    }

    void OnEnable() {
        targetingSystem.stateChange += OnStateChange;
    }

    // Update is called once per frame
    void Update()
    {
        // Passive Rotation
        if(passiveRotationOn) {
            PassiveRotation();
        }

        // Shooting
        if(targetingSystem.state == EntityTargeting.State.Attacking) {
            
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

    void OnStateChange(EntityTargeting.State state) {
        if(state == EntityTargeting.State.Attacking) {
            foreach(EquipPoint e in equipPoints) {
                e.GetComponent<LookAtX>().target = targetingSystem.target;
            }
        } else {
            foreach(EquipPoint e in equipPoints) {
                e.GetComponent<LookAtX>().target = null;
            }
        }
    } 
}