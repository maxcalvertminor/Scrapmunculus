using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderbodyScriptMfoot : MonoBehaviour
{
    public GameObject main;
    public List<Foot> listOFeet; // !! TO USE: Place objects to use as feet inside feet[], then put them into groups starting with group 0. Feet in the same group will be moved together. !!
    public int numberOfGroups;
    [SerializeField] private List<List<Foot>> footGroups;
    public float randomMultiplier; // Higher randomMultiplier = more randomness in step location.
    public float seconds_between_steps;
    public float step_speed;
    private bool moving;
    private bool is_stepping;
    public float howFarToStepForward;
    private int groupIterator;

    // Start is called before the first frame update
    void Start()
    {
        footGroups = new List<List<Foot>>();
        moving = true;
        is_stepping = false;
        groupIterator = 0;

        for(int i = 0; i < numberOfGroups; i++) {
            footGroups.Add(new());
            foreach(Foot foot in listOFeet) {
                if(foot.group == i) footGroups[i].Add(foot); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(main.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0) {
            moving = true;
        } else {
            moving = false;
        }

        if(moving && !is_stepping) {
            StartCoroutine(Step(footGroups[groupIterator]));
        }
    }

    IEnumerator Step(List<Foot> feetToMove) {
        is_stepping = true;
        float fraction = 0;
        foreach(Foot foot in feetToMove) {
            foot.SetInitialPositionAndRandomness();
        }
        while(fraction < 1) {
            fraction += step_speed * Time.deltaTime;
            for(int i = 0; i < feetToMove.Count; i++) {
                feetToMove[i].transform.position = Vector3.Lerp(feetToMove[i].initialPosition, (Vector2)main.transform.TransformPoint(feetToMove[i].homePoint) + main.GetComponent<Rigidbody2D>().linearVelocity * howFarToStepForward + feetToMove[i].randomVariation * randomMultiplier, fraction);
            }
            yield return null;
        }

        yield return new WaitForSeconds(seconds_between_steps);
        groupIterator++;
        if(groupIterator == footGroups.Count) {
            groupIterator = 0;
        }
        is_stepping = false;



        /*Vector3 startPoint = foot.transform.position;
        is_stepping = true;
        float fraction = 0;
        while(fraction < 1) {
            fraction += step_speed * Time.deltaTime;
            foot.transform.position = Vector3.Lerp(startPoint, main.transform.TransformPoint(targetPoint + (main.GetComponent<Rigidbody>().linearVelocity * constant)), fraction);
            yield return null;
        }

        yield return new WaitForSeconds(seconds_between_steps);
        is_stepping = false;
        groupIterator++;
        if(groupIterator == feet.Length) {
            groupIterator = 0;
        }*/
    }
}

