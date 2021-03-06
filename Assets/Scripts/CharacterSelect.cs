﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPickerObject;
    bool[] created;
    void Start()
    {
        created = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
        for (int i = 1; i < 5; i++)
        {
            bool joinButtonPressed = Input.GetButtonDown("Player" + i + "Pickup");
            if (joinButtonPressed && !created[i-1])
            {
                GameObject playerPicker = Instantiate(playerPickerObject, transform);
                SpriteRenderer playerRenderer = playerPicker.GetComponent<SpriteRenderer>();
                PlayerSelector playerSelector = playerPicker.GetComponent<PlayerSelector>();
                playerSelector.setPlayerNumber(i);
                playerSelector.setPlayerColor(getPlayerColor(i));
                playerRenderer.color = getPlayerColor(i);
                created[i - 1] = true;
            }
        }

        if (Input.GetButtonDown("Player1Throw"))
        {
            int countTrues = 0;
            foreach (bool create in created)
            {
                if (create)
                {
                    countTrues++;
                }
            }

            int countSelected = 0;
            GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
            foreach (GameObject card in playerCards)
            {
                CharacterIdentity charIdentity = card.GetComponent<CharacterIdentity>();

                if (!charIdentity.available)
                {
                    countSelected++;
                }
            }

            if (countSelected == countTrues && countSelected > 1)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject player in players)
                {
                    PlayerSelector playerSelector = player.GetComponent<PlayerSelector>();
                    CharacterIdentity charIdentity = playerSelector.chosenCharacter.GetComponent<CharacterIdentity>();
                    PlayerPrefs.SetString("Player" + playerSelector.playerNumber + "_Character", charIdentity.thisCharacter.ToString());
                }
                PlayerPrefs.SetInt("NumPlayers", players.Length);
                SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            }
        }
    }

    Color getPlayerColor(int playerNum)
    {
        if (playerNum == 1)
        {
            return Color.red;
        } else if (playerNum == 2)
        {
            return Color.blue;
        } else if (playerNum == 3)
        {
            return Color.yellow;
        } else
        {
            return Color.green;
        }
    }
}
