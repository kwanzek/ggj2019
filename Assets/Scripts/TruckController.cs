using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public int truckSpeed;
    public int currentSpeed = 0;

    private bool alreadyStopped;
    private bool alreadyDroppedOff;

    public GameObject truckStop;
    public GameObject outsideHouse;
    public GameObject tempFurniture;

    private float dropOffTimer = 5f;
    private float removeTimer = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        currentSpeed = 0;
        alreadyStopped = false;
        alreadyDroppedOff = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (currentSpeed > 0)
        {
            Vector3 up = new Vector3(0, 1, 0);
            transform.Translate(up * truckSpeed * Time.deltaTime);
            if (collidedWithStop() && !alreadyStopped)
            {
                currentSpeed = 0;
                //TODO: Wait for loading animation
                alreadyStopped = true;
                destroyFurnitureOutsideHouse();
            }
        }
        if (alreadyStopped && dropOffTimer > 0)
        {
            dropOffTimer -= Time.fixedDeltaTime;
        }
        if(dropOffTimer <= 0 && !alreadyDroppedOff)
        {
            spawnNewFurniture();
            startMoving();
        }
        if (alreadyDroppedOff)
        {
            removeTimer -= Time.fixedDeltaTime;
        }
        if(removeTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void spawnNewFurniture()
    {
        Instantiate(tempFurniture, truckStop.transform.position, Quaternion.identity);
        alreadyDroppedOff = true;
    }

    private bool collidedWithStop()
    {
        BoxCollider2D thisCollider = this.GetComponent<BoxCollider2D>();
        BoxCollider2D stopCollider = truckStop.GetComponent<BoxCollider2D>();
        return thisCollider.IsTouching(stopCollider);
    }
    public void startMoving()
    {
        currentSpeed = truckSpeed;
    }

    public void stopMoving()
    {
        currentSpeed = 0;
    }

    public void setTargetStop(GameObject truckStop)
    {
        this.truckStop = truckStop;
    }

    public void setOutsideHouse(GameObject outsideHouse)
    {
        this.outsideHouse = outsideHouse;
    }

    private void destroyFurnitureOutsideHouse()
    {
        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");
        BoxCollider2D outsideHouseCollider = outsideHouse.GetComponent<BoxCollider2D>();
        foreach(GameObject furniture in furnitureObjects)
        {
            BoxCollider2D furnitureCollider = furniture.GetComponent<BoxCollider2D>();
            if (furnitureCollider.enabled && furnitureCollider.IsTouching(outsideHouseCollider))
            {
                Destroy(furniture);
            }
        }
    }
}
