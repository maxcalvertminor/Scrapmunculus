using System.Collections.Generic;
using UnityEngine;

public class EntityWeapons : MonoBehaviour
{
    public List<EquipPoint> equipPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipWeapon(GameObject weapon) {
        foreach(EquipPoint e in equipPoints) {
            if(e.equipped == true) {
                e.weapon = weapon;
            }
        }
    }

}