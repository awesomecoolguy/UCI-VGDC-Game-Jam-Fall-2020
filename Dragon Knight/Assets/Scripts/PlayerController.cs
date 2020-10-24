using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Configuration Parameters
    [Header("Movement Parameters")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float jumpVelocity = 4f;

    private bool onGround = true;
    private int gemsCollected = 0;

    //Cached references
    Animator playerAnim;
    Rigidbody2D playerRB;
    Collider2D playerCol;
    Collider2D Ground;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
        Ground = FindObjectOfType<CompositeCollider2D>();
    }

    void Update()
    {
        HorizontalMovement();
        PlayerJump();
        DetermineOnGround();
    }

    private void HorizontalMovement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            FlipPlayer(1);
            playerRB.velocity = new Vector2(movementSpeed, playerRB.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            FlipPlayer(-1);
            playerRB.velocity = new Vector2(-movementSpeed, playerRB.velocity.y);
        }

        else
        {
            playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
        }
    }

    private void FlipPlayer(int desiredScale)
    {
        if (transform.localScale.x != desiredScale)
        {
            transform.localScale = new Vector2(desiredScale, transform.localScale.y);
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            playerRB.velocity += new Vector2(0f, jumpVelocity);
        }
    }

    private void DetermineOnGround()
    {
        if(playerCol.IsTouching(Ground))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    public void AddGemCollected()
    {
        gemsCollected += 1;
    }
}

