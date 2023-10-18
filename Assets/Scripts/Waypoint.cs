using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Tower ballista;

    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }


    private void OnMouseDown()
    {
        if (isPlaceable)
        {

            bool isPlaced = ballista.PlaceTower(ballista, transform.position);
            isPlaceable = !isPlaced;
        }
    }
}
