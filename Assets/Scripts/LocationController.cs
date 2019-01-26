using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationEnum { Red, Yellow, Green, Blue }

public class LocationController : MonoBehaviour
{

    public LocationEnum thisLocation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool hasFurniture(FurnitureController furnitureToCheck)
    {

        BoxCollider2D thisCollider = this.GetComponent<BoxCollider2D>();
        BoxCollider2D furnitureCollider = furnitureToCheck.GetComponent<BoxCollider2D>();
        Debug.Log(thisCollider.bounds);
        Debug.Log(furnitureCollider.bounds);
        if (thisCollider.bounds.Intersects(furnitureCollider.bounds))
        {
            Debug.Log("Furniture " + furnitureToCheck.thisFurnitureType + " with style " + furnitureToCheck.thisStyleType + " overlapping " + thisLocation);
            return true;
        }
        Debug.Log("Furniture " + furnitureToCheck.thisFurnitureType + " with style " + furnitureToCheck.thisStyleType + " NOT overlapping " + thisLocation);
        return false;

    }
}
