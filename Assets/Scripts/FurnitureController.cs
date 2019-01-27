using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{

    public Vector3 velocity;
    public float friction;
    public float maxSpeed;
    public float currentSpeed;
    private GameObject playerThrowing;

    public enum FurnitureTypeEnum { Refrigerator, DiningTable, Sofa, TV, Bed, Desk, Dresser, Bathtub }

    public enum StyleEnum { Cute, Goth, Fancy, Cool }

    public FurnitureTypeEnum thisFurnitureType;
    public StyleEnum thisStyleType;
    public bool[] locationBoolArray = new bool[System.Enum.GetValues(typeof(LocationEnum)).Length];

    public int pointValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (currentSpeed > 0)
        {
            currentSpeed -= friction * Time.fixedDeltaTime;
            Vector3 forceVector = velocity.normalized * currentSpeed * Time.fixedDeltaTime;
            transform.Translate(forceVector);
        }
    }

    public void throwObject(Vector3 newVelocity, GameObject player)
    {
        currentSpeed = maxSpeed;
        velocity = newVelocity;
        playerThrowing = player;
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        Debug.Log("Velocity: " + velocity);
        if (velocity.x != 0)
        {
            collider.size = new Vector3(1, 0.4f, 0);
        } else if (velocity.y != 0)
        {
            collider.size = new Vector3(0.4f, 1, 0);
        }

        Debug.Log("Setting collider size: " + collider.size);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        if (collision.gameObject.tag != "Player") {
            currentSpeed = 0;
            collider.size = new Vector3(1, 1, 0);
        } else if (playerThrowing != null)
        {
            if (playerThrowing.GetInstanceID() != collision.gameObject.GetInstanceID())
            {
                currentSpeed = 0;
                collider.size = new Vector3(1, 1, 0);
                playerThrowing = null;
            }
        }
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

    public bool isInLocation(LocationEnum locationToCheck)
    {
        return locationBoolArray[(int)locationToCheck];
    }
}
