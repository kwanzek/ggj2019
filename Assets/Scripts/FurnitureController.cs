using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 velocity;
    public float friction;
    public float maxSpeed;
    public float currentSpeed;
    private GameObject playerThrowing;

    void Start()
    {

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
    }

    public void throwObject(Vector3 newVelocity, GameObject player)
    {
        currentSpeed = maxSpeed;
        velocity = newVelocity;
        playerThrowing = player;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") {
            currentSpeed = 0;
        } else if (playerThrowing != null)
        {
            if (playerThrowing.GetInstanceID() != collision.gameObject.GetInstanceID())
            {
                currentSpeed = 0;
                playerThrowing = null;
            }
        }
    }
}
