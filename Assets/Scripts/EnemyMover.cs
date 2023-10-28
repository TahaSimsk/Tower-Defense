using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField][Range(0, 100f)] float moveSpeed;

    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        FindPath();
        transform.position = path[0].transform.position; //snap the ram to the first waypoint's position
        StartCoroutine(MoveToWaypoints());
    }

    void FindPath()
    {
        path.Clear(); //clear the waypoints in the path list

        GameObject parent = GameObject.FindGameObjectWithTag("Path");  //find the parent gameobject 


        foreach (Transform child in parent.transform) //loop and add the children of the parent gameobject to path list
        {
            Tile waypoint = child.GetComponent<Tile>();

            if (waypoint != null)
            {
                path.Add(waypoint);
            }


        }
    }


    IEnumerator MoveToWaypoints()
    {
        #region FirstMethod

        foreach (Tile waypoint in path)
        {
            Vector3 targetPos = waypoint.transform.position; //set the targetPos
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



