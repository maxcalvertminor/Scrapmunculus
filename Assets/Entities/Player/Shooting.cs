using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    private GameObject player;
    public GameObject equipPointRight;
    public GameObject equipPointLeft;
    public GameObject actuatorRight;
    public GameObject actuatorLeft;
    private GameObject weaponRight;
    private GameObject weaponLeft;
    private Weapon scriptRight;
    private Weapon scriptLeft;
    [SerializeField] private bool hasWeaponRight;
    [SerializeField] private bool hasWeaponLeft;
    private int triggerNum;
    private Collider2D coll;
    private List<GameObject> triggerList;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        hasWeaponRight = false;
        hasWeaponLeft = false;
        triggerNum = 0;
        triggerList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && hasWeaponLeft && scriptLeft.CanFire() == true) {
            Debug.Log("Fired left");
            StartCoroutine(scriptLeft.Fire());
        //    ammoSprites[ammoLeft].GetComponent<Image>().sprite = emptyAmmo;
            //Debug.Log(ammoLeft);
        }
        if(Input.GetMouseButtonDown(1) && hasWeaponRight && scriptRight.CanFire() == true) {
            Debug.Log("Fired right");
            StartCoroutine(scriptRight.Fire());
        //    ammoSprites[ammoRight].GetComponent<Image>().sprite = emptyAmmo;
            //Debug.Log(ammoRight);
        }

        if(Input.GetButtonDown("Reload")) {

            if(hasWeaponRight) {
                StartCoroutine(scriptRight.Reload());
            }
            if(hasWeaponLeft) {
                StartCoroutine(scriptLeft.Reload());
            }
            
        }

        if (Input.GetButtonDown("Pickup Right"))
        {
            if (hasWeaponRight == false && triggerNum > 0)
            {
                // RIGHT PICKUP
                weaponRight = coll.gameObject;
                weaponRight.transform.SetParent(equipPointRight.transform);
                OnTriggerExit2D(weaponRight.GetComponent<Collider2D>());
                weaponRight.tag = "Player";
                scriptRight = weaponRight.GetComponent<Weapon>();
                Debug.Log(scriptRight + " + " + equipPointRight);
                Attach(scriptRight, equipPointRight);
                actuatorRight.GetComponent<SpriteRenderer>().enabled = true;

                /* ammoSprites.Capacity = scriptRight.getAmmo();
                 //Debug.Log(ammoSprites.Count);
                 int space = maxSpriteSpace;
                 //if(allowedSpriteSpace / ammo > maxSpriteSpace) {space = maxSpriteSpace;}else{space = allowedSpriteSpace / ammo;}
                 float first = Screen.width / 2 - (distanceBetweenSprites * (scriptRight.getAmmo() - 1) / 2);
                 for(int i = 0; i < scriptRight.getAmmo(); i++) {
                     //Debug.Log(i);
                     ammoSprites.Add(Instantiate(ammoPip, player.transform.position, player.transform.rotation));
                     ammoSprites[i].GetComponent<Image>().sprite = fullAmmo;
                     ammoSprites[i].transform.SetParent(canvas.transform);
                     ammoSprites[i].transform.position = new Vector3 (first + (i * distanceBetweenSprites), Screen.height / 2 + spritePos, 0);
                 } */
                hasWeaponRight = true;
            }
            else if (hasWeaponRight == true)
            {
                // RIGHT DROP
                weaponRight.transform.SetParent(null);
                weaponRight.tag = "Weapon";
                OnTriggerEnter2D(weaponRight.GetComponent<Collider2D>());
                scriptRight.setEquipped(false);
                hasWeaponRight = false;
                weaponRight = null;
                scriptRight = null;
                actuatorRight.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        

        if(Input.GetButtonDown("Pickup Left")) {
            if(hasWeaponLeft == false && triggerNum > 0) {
                // LEFT PICKUP
                weaponLeft = coll.gameObject;
                weaponLeft.transform.SetParent(equipPointLeft.transform);
                OnTriggerExit2D(weaponLeft.GetComponent<Collider2D>());
                weaponLeft.tag = "Player";
                scriptLeft = weaponLeft.GetComponent<Weapon>();
                Attach(scriptLeft, equipPointLeft);
                actuatorLeft.GetComponent<SpriteRenderer>().enabled = true;

              /*  ammoSprites.Capacity = scriptLeft.getAmmo();
                //Debug.Log(ammoSprites.Count);
                int space = maxSpriteSpace;
                //if(allowedSpriteSpace / ammo > maxSpriteSpace) {space = maxSpriteSpace;}else{space = allowedSpriteSpace / ammo;}
                float first = Screen.width / 2 - (distanceBetweenSprites * (scriptLeft.getAmmo() - 1) / 2);
                for(int i = 0; i < scriptLeft.getAmmo(); i++) {
                    //Debug.Log(i);
                    ammoSprites.Add(Instantiate(ammoPip, player.transform.position, player.transform.rotation));
                    ammoSprites[i].GetComponent<Image>().sprite = fullAmmo;
                    ammoSprites[i].transform.SetParent(canvas.transform);
                    ammoSprites[i].transform.position = new Vector3 (first + (i * distanceBetweenSprites), Screen.height / 2 + spritePos, 0);
                }  */
                hasWeaponLeft = true;
            } else if(hasWeaponLeft == true) {
                // LEFT DROP
                weaponLeft.transform.SetParent(null);
                weaponLeft.tag = "Weapon";
                OnTriggerEnter2D(weaponLeft.GetComponent<Collider2D>());
                scriptLeft.setEquipped(false);
                hasWeaponLeft = false;
                weaponLeft = null;
                scriptLeft = null;
                actuatorLeft.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public void Attach(Weapon weapon, GameObject equipPoint) {
        weapon.transform.position = equipPoint.transform.position;
        weapon.transform.position = equipPoint.transform.position;
        weapon.transform.rotation = equipPoint.transform.rotation;
        weapon.transform.localPosition = new Vector3 (0, weapon.y, 0);
        //equipPoint.GetComponent<Arm_Actuator_Script>().attach(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Weapon")) {
            triggerNum++;
            triggerList.Add(other.gameObject);
            coll = other;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Weapon")) {
            int min = 0;
            for(int i = 1; i < triggerList.Count; i++) {
                float a = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(triggerList[i].transform.position.x, triggerList[i].transform.position.y));
                float b = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(triggerList[i - 1].transform.position.x, triggerList[i - 1].transform.position.y));
                if(a < b) {
                    min = i;
                }
            }
            coll = triggerList[min].GetComponent<Collider2D>();
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Weapon")) {
            triggerNum--;
            triggerList.Remove(other.gameObject);
        }
    }

    // && !other.gameObject.GetComponent<RevolverScript>().isEquipped() && !hasWeapon
}
