using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public bool walkable;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;

    public Node(Vector3 pos, bool w, int _gridX, int _gridY) {
        worldPosition = pos;
        walkable = w;
        gridX = _gridX;
        gridY = _gridY;
    }
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public bool checkAvailibility() {
        return walkable;
    }
    
}
