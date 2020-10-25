using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Configuration Parameters
    [Header("Movement Parameters")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float jumpVelocity = 4f;


    public float flameDamage;
    public float flameCooldown;

    private bool onGround = true;
    private int gemsCollected = 0;
    private bool isFlaming;
    private float flameTimeLeft;
    private float lastFlame = -100f;
   

    //Cached references
    Animator playerAnim;
    Rigidbody2D playerRB;
    Collider2D playerCol;
    Collider2D Ground;
    GameManager gameManager;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
        Ground = FindObjectOfType<CompositeCollider2D>();
        gameManager = GameManager.Get();
    }

    void Update()
    {
        HorizontalMovement();
        PlayerJump();
        DetermineOnGround();
        Flame();

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
        gameManager.AddScore(20);
    }

    private void Flame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.time >= (lastFlame - flameCooldown)) // checks if flame is off cooldown
            {
                isFlaming = true;
                lastFlame = Time.time;
                checkFlame();
            }
            
        }

    }



    private void checkFlame()
    {
        if (isFlaming) 
        {
            if (flameTimeLeft > 0 && onGround == false)
            {
                // add a private bool that controls if u can jump
            
                flameTimeLeft -= Time.deltaTime;
                // instantiate the image
            }
            if (flameTimeLeft <0)
            {
                // the player can jump
                isFlaming = false;
            }

        }

    }

}
