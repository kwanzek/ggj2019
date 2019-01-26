using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FurnitureTypeEnum { Refrigerator, DiningTable, Sofa, TV, Bed, Desk, Dresser, Bathtub }

public enum StyleEnum { Cute, Goth, IDK1, IDK2 }

public class FurnitureController : MonoBehaviour
{

    public FurnitureTypeEnum thisFurnitureType;
    public StyleEnum thisStyleType;
    public bool[] locationBoolArray = new bool[System.Enum.GetValues(typeof(LocationEnum)).Length];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns true if the furniture is overlapping with any square.
    public bool isInHouse()
    {
        foreach (bool curr_bool in locationBoolArray)
        {
            if (curr_bool)
            {
                return true;
            }
        }
        return false;
    }

}
