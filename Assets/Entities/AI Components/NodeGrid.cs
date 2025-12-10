using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{

    //public RectTransform rect_transform;
    public Transform startObj;
    public Vector2 gridWorldSize;
    private int gridSizeX;
    private int gridSizeY;
    private Node[,] grid;
    public Node start;
    public Node goal;
    public float nodeRadius;
    public float seekerRadius;
    private float nodeDiameter;
    public LayerMask unwalkable;
    public Vector3 worldBottomLeft;

    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = 2 * nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //rect_transform = GetComponent<RectTransform>();
        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.up * gridWorldSize.y / 2);

        for(int x = 0; x < gridSizeX; x++) {
            for(int y = 0; y < gridSizeY; y++) {
                Vector3 pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(pos, seekerRadius, unwalkable);
                grid[x,y] = new Node(pos, walkable, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y ==0) {continue;}

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node NearestNode(Node node) {
        List<Node> closedSet = new();
        Node nearest = null;

        int i = 1;
        while(true) {
            for(int x = -i; x <= i; x++) {
                for(int y = -i; y <= i; y++) {
                    if(x == 0 && y ==0) {continue;}

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if(closedSet.Contains(grid[checkX, checkY])) {continue;}

                    if(!grid[checkX, checkY].walkable) {
                        closedSet.Add(grid[checkX, checkY]);
                    } else {
                        nearest = grid[checkX, checkY];
                    }
                }
            }
            if(nearest != null) {
                return nearest;
            }
            i++;
        }
    }
    
    public List<Node> path;
    /*void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if(grid != null) {
            foreach(Node n in grid) {
                Gizmos.color = (n.walkable)?Color.white:Color.red;
                if(path != null) 
                    if(path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.3f));
            }
        }
        
    }*/

    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x - worldBottomLeft.x) / gridWorldSize.x;
        float percentY = (worldPosition.y - worldBottomLeft.y) / gridWorldSize.y;

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);
        return grid[x, y];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
