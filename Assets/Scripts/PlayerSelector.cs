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
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(horizontalMove * speed * Time.fixedDeltaTime, verticalMove * speed * Time.fixedDeltaTime, 0));

        if (Input.GetButtonDown("Player1Pickup")) {
            GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
            foreach(GameObject card in playerCards)
            {
                BoxCollider2D cardCollider = card.GetComponent<BoxCollider2D>();
                CircleCollider2D playerCollider = this.gameObject.GetComponent<CircleCollider2D>();
                if (playerCollider.IsTouching(cardCollider))
                {
                    CharacterIdentity charIdentity = card.GetComponent<CharacterIdentity>();
                    SpriteRenderer childDisplay = card.GetComponentInChildren<SpriteRenderer>();

                    if (charIdentity.available)
                    {
                        charIdentity.setAvailable(false);
                        charIdentity.enablePlayerDisplay(playerColor);

                        chosenCharacter = card;
                        Debug.Log("My character is : " + charIdentity.thisCharacter);
                    }
                }
            }
        }
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
