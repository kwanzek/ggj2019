using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public int playerNumber;
    private Color playerColor;
    public int speed;
    public GameObject chosenCharacter;
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
        float horizontalMove = Input.GetAxisRaw("Player" + playerNumber + "Horizontal");
        float verticalMove = Input.GetAxisRaw("Player" + playerNumber + "Vertical");

        transform.Translate(new Vector3(horizontalMove * speed * Time.fixedDeltaTime, verticalMove * speed * Time.fixedDeltaTime, 0));

        if (Input.GetButtonDown("Player" + playerNumber + "Pickup") && chosenCharacter == null) {
            GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
            foreach(GameObject card in playerCards)
            {
                BoxCollider2D cardCollider = card.GetComponent<BoxCollider2D>();
                CircleCollider2D playerCollider = this.gameObject.GetComponent<CircleCollider2D>();
                if (playerCollider.IsTouching(cardCollider))
                {
                    CharacterIdentity charIdentity = card.GetComponent<CharacterIdentity>();

                    if (charIdentity.available)
                    {
                        charIdentity.setAvailable(false);
                        charIdentity.enablePlayerDisplay(playerColor);

                        chosenCharacter = card;
                    }
                }
            }
        } else if (Input.GetButton("Player" + playerNumber + "Back") && chosenCharacter != null)
        {
            unpickCharacter();
        }
    }

    void unpickCharacter()
    {
        CharacterIdentity charIdentity = chosenCharacter.GetComponent<CharacterIdentity>();
        charIdentity.unpickCharacter();
        chosenCharacter = null;
    }

    public void setPlayerNumber(int num)
    {
        playerNumber = num;
    }

    public void setPlayerColor(Color color)
    {
        playerColor = color;
    }

}
