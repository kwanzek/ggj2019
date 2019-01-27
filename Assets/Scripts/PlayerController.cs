using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Animations;

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

    public List<LocationGoal> locationGoalList;
    public int numLocationGoals = 2;
    public int locationGoalScore = 0;

    public FurnitureController.StyleEnum styleGoal;
    public int styleGoalScore = 0;

    public int playerNumber;
    public Color playerColor;

    Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        isCarrying = false;
        currentFacing = Facing.DOWN;

        locationGoalList = new List<LocationGoal>();
        for (int i = 0; i < numLocationGoals; i++)
        {
            LocationGoal currGoal = new LocationGoal();
            while (locationGoalList.Contains(currGoal))
            {
                currGoal = new LocationGoal();
            }
            locationGoalList.Add(currGoal);
        }

        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerNumber != 0)
        {
            // Reset triggers so player doesn't randomly start walking
            m_Animator.ResetTrigger("PlayerWalkingLeft");
            m_Animator.ResetTrigger("PlayerWalkingRight");
            m_Animator.ResetTrigger("PlayerWalkingUp");
            m_Animator.ResetTrigger("PlayerWalkingDown");

            horizontalMove = Mathf.Round(Input.GetAxisRaw("Player" + playerNumber + "Horizontal"));
            verticalMove = Mathf.Round(Input.GetAxisRaw("Player" + playerNumber + "Vertical"));

            SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();


            if (horizontalMove < 0)
            {
                currentFacing = Facing.LEFT;
                m_Animator.SetTrigger("PlayerWalkingRight");
                renderer.flipX = true;
            }
            else if (horizontalMove > 0)
            {
                currentFacing = Facing.RIGHT;
                m_Animator.SetTrigger("PlayerWalkingRight");
                renderer.flipX = false;
            }
            else if (verticalMove > 0)
            {
                currentFacing = Facing.UP;
                m_Animator.SetTrigger("PlayerWalkingUp");
                renderer.flipY = false;
            }
            else if (verticalMove < 0)
            {
                currentFacing = Facing.DOWN;
                m_Animator.SetTrigger("PlayerWalkingUp");
                renderer.flipY = true ;
            }

            if (isCarrying)
            {
                Vector3 facingOffset = getPickupOffset(transform.localPosition);
                m_carryObject.transform.position = facingOffset;
            }

            // If "pickup" is pressed, check facing direction
            if (Input.GetButtonDown("Player" + playerNumber + "Pickup"))
            {

                if (!isCarrying)
                {
                    Vector3 pickupOffset = getPickupOffset(transform.localPosition);
                    GameObject toPickupFurniture = getFacingFurniture(pickupOffset);
                    if (toPickupFurniture != null)
                    {
                        pickupFurniture(toPickupFurniture);
                    }
                }
                else
                {
                    dropFurniture();
                }
            }
            else if (Input.GetButtonDown("Player" + playerNumber + "Throw") && isCarrying)
            {
                GameObject furniture = m_carryObject;
                dropFurniture();
                throwFurniture(furniture, getForceVector());
            }
        }
    }

    public void setPlayerNumber(int num)
    {
        playerNumber = num;
    }

    public void setColor(Color color)
    {
        this.playerColor = color;
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

    void throwFurniture(GameObject furniture, Vector3 forceVector)
    {
        FurnitureController controller = furniture.GetComponent("FurnitureController") as FurnitureController;
        controller.throwObject(forceVector, this.gameObject);
        Debug.Log("throwing furniture");
    }

    GameObject getFacingFurniture(Vector3 pickupOffset)
    {
        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");
        foreach (GameObject furniture in furnitureObjects)
        {
            Transform furnitureTransform = furniture.GetComponent<Transform>() as Transform;
            Vector3 furniturePos = furnitureTransform.localPosition;

            if(Mathf.Abs(furniturePos.x - pickupOffset.x) <= 1 && Mathf.Abs(furniturePos.y - pickupOffset.y) <= 1)
            {
                return furniture;
            }
        }
        return null;
    }

    Vector3 getPickupOffset(Vector3 localPosition)
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
        return localPosition;
    }

    Vector3 getForceVector()
    {
        Vector3 forceVector = new Vector3(0, 0, 0);
        if (currentFacing == Facing.UP)
        {
            forceVector.y =  1;
        }
        else if (currentFacing == Facing.DOWN)
        {
            forceVector.y = - 1;
        }
        else if (currentFacing == Facing.LEFT)
        {
            forceVector.x = - 1;
        }
        else
        {
            forceVector.x = 1;
        }
        return forceVector;
    }


}
