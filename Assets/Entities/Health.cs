using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHitPoints;
    public float flatResistance;
    public float percentResistance;
    public float hitPoints;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage) {
        hitPoints -= damage * (1 - percentResistance) - flatResistance;
    }

    public void TakeTrueDamage(float damage) {
        hitPoints -= damage;
    }

    public void Heal(float hpRestored) {
        hitPoints += hpRestored;
    }

    public float GetHP() {
        return hitPoints;
    }

    public float GetMaxHP() {
        return maxHitPoints;
    }
}
