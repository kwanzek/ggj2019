using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float horizontalMove = 0f;
    float verticalMove = 0f;
    public int speed;

    enum Facing { LEFT, RIGHT, UP, DOWN };

    private Rigidbody2D m_Rigidbody2D;
    private GameObject m_carryObject;
    private bool isCarrying;
    private Facing currentFacing;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        isCarrying = false;
        currentFacing = Facing.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        if (horizontalMove < 0)
        {
            currentFacing = Facing.LEFT;
        } else if(horizontalMove > 0)
        {
            currentFacing = Facing.RIGHT;
        } else if(verticalMove > 0)
        {
            currentFacing = Facing.UP;
        } else if (verticalMove < 0)
        {
            currentFacing = Facing.DOWN;
        }

        // If "pickup" is pressed, check facing direction
        // TODO: Change names of input buttons, for now Jump because jump == space
        Debug.Log(isCarrying);
        Debug.Log(m_carryObject);
        if (Input.GetButtonDown("Jump"))
        {

            if (! isCarrying ) {
                Vector3Int pickupOffset = getPickupOffset(transform.localPosition);
                GameObject toPickupFurniture = getFacingFurniture(pickupOffset);
                if (toPickupFurniture != null)
                {
                    pickupFurniture(toPickupFurniture);
                }
            } else
            {
                dropFurniture();
            }
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector3(horizontalMove * speed * Time.fixedDeltaTime, verticalMove * speed * Time.fixedDeltaTime, 0);
    }

    void pickupFurniture(GameObject furniture)
    {
        m_carryObject = furniture;
        m_carryObject.transform.SetParent(this.transform);
        BoxCollider2D furnitureCollider = m_carryObject.GetComponent<BoxCollider2D>();
        furnitureCollider.enabled = false;
        isCarrying = true;
    }

    void dropFurniture()
    {
        m_carryObject.transform.SetParent(null);
        BoxCollider2D furnitureCollider = m_carryObject.GetComponent<BoxCollider2D>();
        furnitureCollider.enabled = true;
        m_carryObject = null;
        isCarrying = false;
    }

    GameObject getFacingFurniture(Vector3Int pickupOffset)
    {
        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");
        foreach (GameObject furniture in furnitureObjects)
        {
            Transform furnitureTransform = furniture.GetComponent<Transform>() as Transform;
            Vector3Int furniturePos = Vector3Int.FloorToInt(furnitureTransform.localPosition);
            //Debug.Log("Player vecto3: " + pickupOffset);
            //Debug.Log("Transform local: " + transform.localPosition);
            //Debug.Log("Transform psoition: " + transform.position);
            //Debug.Log("Furniture Pos: " + furniturePos);
            if (pickupOffset.x == furniturePos.x && pickupOffset.y == furniturePos.y)
            {
                Debug.Log("MATCH!");
                return furniture;
            }
        }
        return null;
    }

    Vector3Int getPickupOffset(Vector3 localPosition)
    {
        if (currentFacing == Facing.UP)
        {
            localPosition.y = localPosition.y + 1;
        }
        else if (currentFacing == Facing.DOWN)
        {
            localPosition.y = localPosition.y - 1;
        }
        else if (currentFacing == Facing.LEFT)
        {
            localPosition.x = localPosition.x - 1;
        }
        else
        {
            localPosition.x = localPosition.x + 1;
        }
        return Vector3Int.FloorToInt(localPosition);
    }


}
