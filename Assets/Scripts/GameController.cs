﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public LocationController[] allLocations;
    public FurnitureController[] allFurniture;

    public int maxGoalItems;
    public int minGoalItems;
    public int maxPerItemType;
    private Dictionary<FurnitureController.FurnitureTypeEnum, int> globalItemGoals;
    private Dictionary<FurnitureController.FurnitureTypeEnum, int> globalGoalScores;

    public GameObject truck;
    public GameObject truckSpawn;
    public GameObject truckStop;
    public GameObject outsideHouse;
    public GameObject playerPrefab;
    public GameObject playerIcon;

    public GameObject player1Spawn;
    public GameObject player2Spawn;
    public GameObject player3Spawn;
    public GameObject player4Spawn;

    public GameObject dropOffStart;

    public PlayerController[] playerList;

    private int numPointsPerLocationGoal = 7;
    private int numPointsPerStyleGoal = 2;

    private float recalcTimer = 3f;
    private float recalcTimerReset = 3f;

    // Start is called before the first frame update
    void Start()
    {
        allLocations = GameObject.FindObjectsOfType<LocationController>();
        allFurniture = GameObject.FindObjectsOfType<FurnitureController>();
        setupGlobalGoals();

        spawnTruck();

        for (int i = 1; i < PlayerPrefs.GetInt("NumPlayers") + 1; i++)
        {
            Transform spawnLoc = getSpawnLocForPlayerNum(i);
            GameObject playerObj = Instantiate(playerPrefab, spawnLoc.position, Quaternion.identity);
            GameObject playerIconObj = Instantiate(playerIcon, new Vector3((float)-14.7, 7 - 3 * (i - 1), 1), Quaternion.identity);
            PlayerController playerController = playerObj.GetComponent<PlayerController>();
            playerController.setPlayerNumber(i);
            playerController.setColor(getPlayerColor(i));
        }
        playerList = GameObject.FindObjectsOfType<PlayerController>();
        foreach (PlayerController currPlayer in playerList)
        {
            //Debug.Log(currPlayer);
        }
        //Debug.Log("Passed through for player 1 : " + PlayerPrefs.GetString("Player1_Character"));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Timer: " + recalcTimer);
        if (recalcTimer < 0f)
        {
            checkAllFurnitureLocationOverlap();
            updatePlayerLocationGoalScores();
            updatePlayerStyleGoalScores();
            updateGlobalGoalScores();
            recalcTimer = recalcTimerReset;
        }
        recalcTimer -= Time.deltaTime;

    }

    // For each furniture, check if it overlapping a specific location.
    public void checkAllFurnitureLocationOverlap()
    {
        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");

        foreach (GameObject furniture in furnitureObjects) {
            FurnitureController currFurniture = furniture.GetComponent<FurnitureController>();
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

    Transform getSpawnLocForPlayerNum(int playerNum)
    {
        if (playerNum == 1)
        {
            return player1Spawn.transform;
        } else if(playerNum == 2)
        {
            return player2Spawn.transform;
        } else if(playerNum == 3)
        {
            return player3Spawn.transform;
        } else
        {
            return player4Spawn.transform;
        }
    }

    void setupGlobalGoals()
    {
        int goalItems = Random.Range(minGoalItems, maxGoalItems + 1);
        int remainingItems = goalItems;
        globalItemGoals = new Dictionary<FurnitureController.FurnitureTypeEnum, int>();

        var enumTypes = System.Enum.GetValues(typeof(FurnitureController.FurnitureTypeEnum));

        while (remainingItems > 0)
        {
            int randomPick = Random.Range(0, enumTypes.Length);
            FurnitureController.FurnitureTypeEnum chosenType = (FurnitureController.FurnitureTypeEnum)randomPick;
            if (globalItemGoals.ContainsKey(chosenType))
            {
                int existingCount = globalItemGoals[chosenType];
                if (existingCount < maxPerItemType)
                {
                    globalItemGoals[chosenType] = existingCount + 1;
                    remainingItems--;
                }
            }else
            {
                globalItemGoals.Add(chosenType, 1);
                remainingItems--;
            }
        }

        Debug.Log("Goal items: " + goalItems);
        foreach (FurnitureController.FurnitureTypeEnum type in enumTypes)
        {
            if (globalItemGoals.ContainsKey(type))
            {
                Debug.Log("Key: " + type + " value: " + globalItemGoals[type]);
            }
            else
            {
                Debug.Log("Key: " + type + "value: 0");
            }
        }

        int i = 0;
        foreach (KeyValuePair<FurnitureController.FurnitureTypeEnum, int> entry in globalItemGoals)
        {
            GameObject textObject = new GameObject(entry.Key.ToString());
            textObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            textObject.transform.localScale = new Vector3(1, 1, 1);
            textObject.transform.localPosition = new Vector3(300, 125 - (i * 20), 2);

            Text goalLabel = textObject.AddComponent<Text>();

            string furnitureName;
            if(entry.Key == FurnitureController.FurnitureTypeEnum.Refrigerator)
            {
                furnitureName = "Fridge";
            } else if (entry.Key == FurnitureController.FurnitureTypeEnum.DiningTable) {
                furnitureName = "Table";
            }
            else
            {
                furnitureName = entry.Key.ToString();
            }

            goalLabel.text =furnitureName + ": " + "0 / " + entry.Value;

            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            goalLabel.font = ArialFont;
            goalLabel.material = ArialFont.material;
            goalLabel.alignment = TextAnchor.MiddleRight;
            goalLabel.fontSize = 16;

            i++;
        }

    }

    void spawnTruck()
    {
        GameObject truckInstance = Object.Instantiate(truck, truckSpawn.transform.position, Quaternion.identity);
        TruckController truckController = truckInstance.GetComponent<TruckController>();
        truckController.setTargetStop(truckStop);
        truckController.setOutsideHouse(outsideHouse);
        truckController.setUpDropOffSpot(dropOffStart);
        truckController.startMoving();
    }

    public void updatePlayerLocationGoalScores()
    {
        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");
        foreach (PlayerController currPlayer in playerList)
        {
            currPlayer.locationGoalScore = 0;
            foreach (GameObject furniture in furnitureObjects)
            {
                FurnitureController currFurniture = furniture.GetComponent<FurnitureController>();
                foreach (LocationGoal currLocationGoal in currPlayer.locationGoalList)
                {
                    if (currFurniture.thisFurnitureType == currLocationGoal.furnitureType && currFurniture.isInLocation(currLocationGoal.location))
                    {
                        currPlayer.locationGoalScore += numPointsPerLocationGoal;
                    }
                    //Debug.Log("Player " + currPlayer.ToString() + " has goal " + currLocationGoal);
                }
                //Debug.Log("Player " + currPlayer.ToString() + " has " + currPlayer.locationGoalScore + " location points.");

            }
        }
    }

    public void updatePlayerStyleGoalScores()
    {
        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");
        foreach (PlayerController currPlayer in playerList)
        {
            currPlayer.styleGoalScore = 0;
            foreach (GameObject furniture in furnitureObjects)
            {
                FurnitureController currFurniture = furniture.GetComponent<FurnitureController>();
                if (currFurniture.thisStyleType == currPlayer.styleGoal && currFurniture.isInHouse())
                {
                    currPlayer.styleGoalScore += numPointsPerStyleGoal;
                }
               // Debug.Log("Player " + currPlayer.ToString() + " has " + currPlayer.styleGoalScore + " style points AND THEIR STYLE IS " + currPlayer.styleGoal + ".");

            }
        }
    }

    public void updateGlobalGoalScores()
    {
        globalGoalScores = new Dictionary<FurnitureController.FurnitureTypeEnum, int>();

        GameObject[] furnitureObjects = GameObject.FindGameObjectsWithTag("Furniture");
        foreach (GameObject furniture in furnitureObjects)
        {
            FurnitureController currFurniture = furniture.GetComponent<FurnitureController>();
            if (currFurniture.isInHouse())
            {
                FurnitureController.FurnitureTypeEnum currType = currFurniture.thisFurnitureType;
                if (globalItemGoals.ContainsKey(currType)) {
                    if (globalGoalScores.ContainsKey(currType))
                    {
                        globalGoalScores[currType]++;
                    }
                    else
                    {
                        globalGoalScores.Add(currType, 1);
                    }

                    GameObject textObject = GameObject.Find("Canvas/" + currType.ToString());
                    Text goalLabel = textObject.GetComponent<Text>();

                    string furnitureName;
                    if (currType == FurnitureController.FurnitureTypeEnum.Refrigerator)
                    {
                        furnitureName = "Fridge";
                    }
                    else if (currType == FurnitureController.FurnitureTypeEnum.DiningTable)
                    {
                        furnitureName = "Table";
                    }
                    else
                    {
                        furnitureName = currType.ToString();
                    }
                    goalLabel.text = furnitureName + ": " + globalGoalScores[currType] + " / " + globalItemGoals[currType];
                 }
            }
        }
    }

    Color getPlayerColor(int playerNum)
    {
        if (playerNum == 1)
        {
            return Color.red;
        }
        else if (playerNum == 2)
        {
            return Color.blue;
        }
        else if (playerNum == 3)
        {
            return Color.yellow;
        }
        else
        {
            return Color.green;
        }
    }
}
