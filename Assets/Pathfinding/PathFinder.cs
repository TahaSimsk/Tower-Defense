using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int targetCoordinates;

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
        }


    }

    void Start()
    {
        startNode = gridManager.Grid[startCoordinates];
        targetNode = gridManager.Grid[targetCoordinates];
        BreadthFirstSearch();
        BuildPath();

    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + directions[i];

            if (grid.ContainsKey(neighborCoordinates))
            {
                neighbors.Add(grid[neighborCoordinates]);

            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch()
    {
        bool isRunning = true;

        frontier.Enqueue(startNode); //add the startnode to the queue
        reached.Add(startCoordinates, startNode); //when added it u already reached it so add it to the reached dictionary

        while (frontier.Count > 0 && isRunning) //if there is still nodes that aren't explored, continue the search
        {
            currentSearchNode = frontier.Dequeue(); //retuns the first node in queue and sets the currentSearchNode to that
            currentSearchNode.isExplored = true;

            ExploreNeighbors();

            if (currentSearchNode.coordinates == targetCoordinates) //if reached to targetcoords stop looking at nodes
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

}
