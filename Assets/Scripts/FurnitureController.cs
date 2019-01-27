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

    private Color fancyColor = new Color(118/255f, 190 / 255f, 21 / 255f);
    private Color cuteColor = new Color(202/255f, 236 / 255f, 118 / 255f);
    private Color coolColor = new Color(250 / 255f, 203 / 255f, 114 / 255f);
    private Color gothColor = new Color(99 / 255f, 57 / 255f, 132 / 255f);

    public Sprite fancySprite;
    public Sprite cuteSprite;
    public Sprite coolSprite;
    public Sprite gothSprite;

    private float originalColliderSizeX;
    private float originalColliderSizeY;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        originalColliderSizeX = collider.size.x;
        originalColliderSizeY = collider.size.y;
        Debug.Log("Original X: " + originalColliderSizeX);
        Debug.Log("original Y: " + originalColliderSizeY);
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
        else
        {
            BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
            collider.size = new Vector3(originalColliderSizeX, originalColliderSizeY, 0);
        }
    }

    public void setupFurniture(FurnitureTypeEnum type, StyleEnum style)
    {
        thisFurnitureType = type;
        thisStyleType = style;
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();

        GameObject childIcon = null;
        foreach (Transform child in transform)
        {
            childIcon = child.gameObject;
        }

        SpriteRenderer childRenderer = childIcon.GetComponent<SpriteRenderer>();

        if (style == StyleEnum.Cool)
        {
            renderer.color = coolColor;
            childRenderer.sprite = coolSprite;
        } else if(style == StyleEnum.Cute)
        {
            renderer.color = cuteColor;
            childRenderer.sprite = cuteSprite;
        } else if(style == StyleEnum.Fancy)
        {
            renderer.color = fancyColor;
            childRenderer.sprite = fancySprite;
        } else if (style == StyleEnum.Goth)
        {
            renderer.color = gothColor;
            childRenderer.sprite = gothSprite;
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
            collider.size = new Vector3(originalColliderSizeX, originalColliderSizeY * 0.4f, 0);
        } else if (velocity.y != 0)
        {
            collider.size = new Vector3(originalColliderSizeX * 0.4f, originalColliderSizeY, 0);
        }

        Debug.Log("Setting collider size: " + collider.size);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        if (collision.gameObject.tag != "Player") {
            currentSpeed = 0;
            collider.size = new Vector3(originalColliderSizeX, originalColliderSizeY, 0);
        } else if (playerThrowing != null)
        {
            if (playerThrowing.GetInstanceID() != collision.gameObject.GetInstanceID())
            {
                currentSpeed = 0;
                collider.size = new Vector3(originalColliderSizeX, originalColliderSizeY, 0);
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
