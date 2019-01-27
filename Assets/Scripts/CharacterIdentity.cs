using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdentity : MonoBehaviour
{
    public enum Character { Fancy, Cute, Cool, Goth };
    public Character thisCharacter;
    public bool available = true;
    public GameObject playerDisplay;

    public void setAvailable(bool status)
    {
        available = status;
    }

    public void enablePlayerDisplay(Color color)
    {
        SpriteRenderer playerDisplayRenderer = playerDisplay.GetComponent<SpriteRenderer>();
        playerDisplayRenderer.color = color;
        playerDisplayRenderer.enabled = true;
    }

    public void disablePlayerDisplay()
    {
        SpriteRenderer playerDisplayRenderer = playerDisplay.GetComponent<SpriteRenderer>();
        playerDisplayRenderer.enabled = false;
    }
}
