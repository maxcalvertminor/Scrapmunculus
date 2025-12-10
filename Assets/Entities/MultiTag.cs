using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTag : MonoBehaviour
{
    public List<string> tags;
    public int dangerLevel;

    public bool HasTag(string tag) {
        return tags.Contains(tag);
    }

    public void AddTag(string tag) {
        tags.Add(tag);
    }

    public void RemoveTag(string tag) {
        tags.Remove(tag);
    }
}
