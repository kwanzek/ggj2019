using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TruckController : MonoBehaviour
{
    public int truckSpeed;
    public int currentSpeed = 0;

    private bool alreadyStopped;
    private bool alreadyDroppedOff;

    public int maxSpawnObjects;
    public int minSpawnObjects;
    public int maxSpawnObjectsPerType;
    public int maxSpawnObjectsPerStyle;

    public GameObject truckStop;
    public GameObject outsideHouse;
    public GameObject tempFurniture;
    public GameObject dropOffStart;

    public GameObject refrigerator;
    public GameObject diningTable;
    public GameObject sofa;
    public GameObject tv;
    public GameObject bed;
    public GameObject desk;
    public GameObject dresser;
    public GameObject bathtub;

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
            }
        }
        if (alreadyStopped && dropOffTimer > 0)
        {
            dropOffTimer -= Time.fixedDeltaTime;
        }
        if(dropOffTimer <= 0 && !alreadyDroppedOff)
        {
            destroyFurnitureOutsideHouse();
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

    public void setUpDropOffSpot(GameObject spot)
    {
        dropOffStart = spot;
    }

    private ArrayList setupSpawnLocs(Vector3 basePos)
    {
        ArrayList list = new ArrayList();
        for (int i = -2; i < 7; i+=3)
        {
            for (int j = -4; j < 4; j+=2)
            {
                list.Add(new Vector3(basePos.x + i + 0.01f, basePos.y + j + 0.01f, 0));
            }
        }
        return list;
    }

    private void spawnNewFurniture()
    {
        int itemCountToDrop = Random.Range(minSpawnObjects, maxSpawnObjects + 1);
        Transform itemDropBase = dropOffStart.transform;

        Vector3 itemDropPos = itemDropBase.position;
        ArrayList spawnLocs = setupSpawnLocs(itemDropPos);

        var enumTypes = System.Enum.GetValues(typeof(FurnitureController.FurnitureTypeEnum));
        var enumStyles = System.Enum.GetValues(typeof(FurnitureController.StyleEnum));
        Dictionary<FurnitureController.FurnitureTypeEnum, int> itemTypeDropOffs = new Dictionary<FurnitureController.FurnitureTypeEnum, int>();
        foreach(FurnitureController.FurnitureTypeEnum type in enumTypes)
        {
            itemTypeDropOffs.Add(type, 0);
        }
        Dictionary<FurnitureController.StyleEnum, int> itemStyleDropOffs = new Dictionary<FurnitureController.StyleEnum, int>();
        foreach (FurnitureController.StyleEnum style in enumStyles)
        {
            itemStyleDropOffs.Add(style, 0);
        }

        while (itemCountToDrop > 0)
        {
            int randomType = Random.Range(0, enumTypes.Length);
            int randomStyle = Random.Range(0, enumStyles.Length);
            FurnitureController.FurnitureTypeEnum chosenType = (FurnitureController.FurnitureTypeEnum)randomType;
            FurnitureController.StyleEnum chosenStyle = (FurnitureController.StyleEnum)randomStyle;

            int existingTypeCount = itemTypeDropOffs[chosenType];
            int existingStyleCount = itemStyleDropOffs[chosenStyle];

            if (existingTypeCount < maxSpawnObjectsPerType && existingStyleCount < maxSpawnObjectsPerStyle)
            {
                itemTypeDropOffs[chosenType] = existingTypeCount + 1;
                itemStyleDropOffs[chosenStyle] = existingStyleCount + 1;
                int spawnLocIndex = Random.Range(0, spawnLocs.Count);
                Vector3 spawnLoc = (Vector3) spawnLocs[spawnLocIndex];
                spawnLocs.RemoveAt(spawnLocIndex);
                spawnObject(chosenType, chosenStyle, spawnLoc);
                itemCountToDrop--;
            }
        }


        alreadyDroppedOff = true;
    }

    private void spawnObject(FurnitureController.FurnitureTypeEnum type, FurnitureController.StyleEnum style, Vector3 loc)
    {
        GameObject spawnObj = Instantiate(furniturePrefabFromType(type), loc, Quaternion.identity);
        FurnitureController controller = spawnObj.GetComponent<FurnitureController>();
        controller.setupFurniture(type, style);
    }

    private GameObject furniturePrefabFromType(FurnitureController.FurnitureTypeEnum type)
    {
        if (type == FurnitureController.FurnitureTypeEnum.Bathtub)
        {
            return bathtub;
        } else if(type == FurnitureController.FurnitureTypeEnum.Bed)
        {
            return bed;
        } else if(type == FurnitureController.FurnitureTypeEnum.Desk)
        {
            return desk;
        } else if(type == FurnitureController.FurnitureTypeEnum.DiningTable)
        {
            return diningTable;
        } else if (type == FurnitureController.FurnitureTypeEnum.Dresser)
        {
            return dresser;
        } else if (type == FurnitureController.FurnitureTypeEnum.Refrigerator)
        {
            return refrigerator;
        } else if (type == FurnitureController.FurnitureTypeEnum.Sofa)
        {
            return sofa;
        } else
        {
            return tv;
        }
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
