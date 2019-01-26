using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public LocationController[] allLocations;
    public FurnitureController[] allFurniture;

    public float testTimer = 0f;
    public float testThreshold = 5f;

    // Start is called before the first frame update
    void Start()
    {
        testTimer = 0f;
        allLocations = GameObject.FindObjectsOfType<LocationController>();
        allFurniture = GameObject.FindObjectsOfType<FurnitureController>();
    }

    // Update is called once per frame
    void Update()
    {
        checkAllFurnitureLocationOverlap();
        testTimer += Time.deltaTime;
        if(testTimer > testThreshold)
        {
            testTimer = 0f;
        }
    }

    // For each furniture, check if it overlapping a specific location.
    public void checkAllFurnitureLocationOverlap()
    {
        foreach (FurnitureController currFurniture in allFurniture) {
            foreach (LocationController currLocation in allLocations)
            {
                if (currLocation.hasFurniture(currFurniture))
                {
                    currFurniture.locationBoolArray[(int)currLocation.thisLocation] = true;
                } else
                {
                    currFurniture.locationBoolArray[(int)currLocation.thisLocation] = false;
                }
            }
        }
    }


}
