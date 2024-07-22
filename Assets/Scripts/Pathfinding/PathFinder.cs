using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int targetCoordinates;
    public Vector2Int TargetCoordinates { get { return targetCoordinates; } }

    Node startNode;
    Node targetNode;
    Node currentSearchNode;

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Queue<Node> frontier = new Queue<Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();



    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            targetNode = grid[targetCoordinates];
        }
    }


    void Start()
    {
        GetNewPath();
    }


    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }


    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>(); //Initialize an empty list to hold neighbor nodes

        for (int i = 0; i < directions.Length; i++) //Loop through all possible directions
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + directions[i];  //Calculate neighbor coordinates

            if (grid.ContainsKey(neighborCoordinates)) //Add the neighbor node to the list if coordinates exist in the grid
            {
                neighbors.Add(grid[neighborCoordinates]);

            }
        }

        foreach (Node neighbor in neighbors)
        {
            // Check if the neighbor is not reached and is walkable
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable) 
            {
                neighbor.connectedTo = currentSearchNode; //Set the neighbor's connectedTo property to the current search node
                reached.Add(neighbor.coordinates, neighbor); //Add the neighbor to the reached dictionary
                frontier.Enqueue(neighbor); //Add the neighbor to the frontier for exploration
            }
        }
    }


    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true; 
        targetNode.isWalkable = true; 

        frontier.Clear(); 
        reached.Clear(); 

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]); //Add the grid to the queue to be explored
        reached.Add(coordinates, grid[coordinates]); //Add the grid to the reached dictionary

        while (frontier.Count > 0 && isRunning) //If there are still nodes that haven't been explored, continue the search
        {
            currentSearchNode = frontier.Dequeue(); //Get the next node to explore
            currentSearchNode.isExplored = true; //Mark it as explored

            ExploreNeighbors(); //Add valid neighbors to the frontier for exploration

            if (currentSearchNode.coordinates == targetCoordinates) //Stop the search if the target node is reached
            {
                isRunning = false;
            }
        }
    }


    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>(); //Initialize an empty list to store the path
        Node currentNode = targetNode; //Start from the target node

        path.Add(currentNode); //Add the target node to the path
        currentNode.isPath = true; //Mark the target node as part of the path

        while (currentNode.connectedTo != null) //Trace back from the target node to the start node
        {
            currentNode = currentNode.connectedTo; //Move to the connected node
            path.Add(currentNode); //Add the current node to the path
            currentNode.isPath = true; //Mark the current node as part of the path
        }

        path.Reverse(); //Reverse the list to get the path from start to target

        return path;
    }


    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable; //Store the grid's walkable status
            grid[coordinates].isWalkable = false; //Temporarily block the node
            List<Node> newPath = GetNewPath(); //Recalculate the path when the node is unwalkable
            grid[coordinates].isWalkable = previousState; //Restore the node's walkable state

            if (newPath.Count <= 1) //Check if the recalculated path is invalid (indicating a block).
            {
                GetNewPath(); //Restore the original path
                return true;
            }
        }
        return false;
    }


    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
