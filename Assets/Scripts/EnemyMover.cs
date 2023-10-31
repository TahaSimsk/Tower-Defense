using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0, 100f)] float moveSpeed;

    List<Node> path = new List<Node>();

    Enemy enemy;

    GridManager gridManager;
    PathFinder pathFinder;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
        enemy = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void ReturnToStart() //snap the ram to the first waypoint's position
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates); 
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();

        path.Clear(); //clear the waypoints in the path list

        path = pathFinder.GetNewPath(coordinates);

        StartCoroutine(MoveToWaypoints());
    }


    IEnumerator MoveToWaypoints()
    {
        #region FirstMethod

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetPos = gridManager.GetPositionFromCoordinates(path[i].coordinates); //set the targetPos
            transform.LookAt(targetPos);

            while (transform.position != targetPos) //keep moving the ram if it hasn't reached the targetPos
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

        }

        enemy.StealGold();
        gameObject.SetActive(false); //deactivate gameobject after it has visited all the waypoints in the loop

        #endregion



        #region SecondMethod
        /*
        foreach (Waypoint waypoint in path)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = waypoint.transform.position;
            transform.LookAt(targetPos);

            float travelPercent = 0f;

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);

                yield return new WaitForEndOfFrame();
            }
        }
        */
        #endregion

    }
}



