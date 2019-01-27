using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum LocationEnum { Red, Yellow, Green, Blue }
// public enum FurnitureTypeEnum { Refrigerator, DiningTable, Sofa, TV, Bed, Desk, Dresser, Bathtub }

public class LocationGoal
{
    public LocationEnum location;
    public FurnitureController.FurnitureTypeEnum furnitureType;

    public LocationGoal()
    {
        this.location = (LocationEnum)Random.Range(0, System.Enum.GetValues(typeof(LocationEnum)).Length);
        this.furnitureType = (FurnitureController.FurnitureTypeEnum)Random.Range(0, System.Enum.GetValues(typeof(FurnitureController.FurnitureTypeEnum)).Length);
        Debug.Log(this);
    }

    public LocationGoal(LocationEnum location, FurnitureController.FurnitureTypeEnum furnitureType)
    {
        this.location = location;
        this.furnitureType = furnitureType;
    }

    public override string ToString()
    {
        return ("Location: " + location + ", Furniture: " + furnitureType);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            LocationGoal goal = (LocationGoal)obj;
            return this.location == goal.location || this.furnitureType == goal.furnitureType;
        }
    }

}
