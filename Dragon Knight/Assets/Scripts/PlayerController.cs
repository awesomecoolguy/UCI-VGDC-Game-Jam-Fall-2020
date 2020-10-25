using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Configuration Parameters
    [Header("Movement Parameters")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float jumpVelocity = 4f;

    [Header("FlameBreathParameters")]
    [SerializeField] GameObject flameBreath;
    [SerializeField] float flameEmmisionRate;

    private ParticleSystem flameBreathPS;
    public float flameDamage;
    
    public bool isFlaming = false;

    private bool onGround = true;
    private int gemsCollected = 0;
    
    private bool canFlame;
    private float maxFlameAmmo = 10f;
    private float currentFlameAmmo;
    private float nextTimeToFlame = 0f;
    private float flameRate = 2f;
    private float flameCooldown = 10f;
    private bool isRealoading = false;

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
        flameBreathPS = flameBreath.GetComponent<ParticleSystem>();
        currentFlameAmmo = maxFlameAmmo;
        canFlame = true;
    }

    void Update()
    {
        HorizontalMovement();
        PlayerJump();
        DetermineOnGround();
        if (currentFlameAmmo <= 0)
        {
            StartCoroutine(reloadFlame());
            return;
        }
        TriggerFire();
        if (isRealoading)
        {
            return;
        }
        
    }

    private void HorizontalMovement()
    {
        if (isFlaming)
        {
            if (Input.GetKey(KeyCode.D))
            {
                FlipPlayer(1);
                playerRB.velocity = new Vector2(movementSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                FlipPlayer(-1);
                playerRB.velocity = new Vector2(-movementSpeed, 0);
            }

            else
            {
                playerRB.velocity = new Vector2(0f, 0);
            }
        }
        else
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

    private void TriggerFire()
    {
        if(Input.GetKey(KeyCode.E))
        { 
            if (onGround == false && Time.time >= nextTimeToFlame && canFlame )
            {
                flameBreathPS.Play();
                isFlaming = true;
                currentFlameAmmo--;
                nextTimeToFlame = Time.time + 1f / flameRate;
            }  
        }
        else
        {
            isFlaming = false;
        
            flameBreathPS.Pause();
            flameBreathPS.Clear();
        }
    }

    IEnumerator reloadFlame()
    {
        isRealoading = true;
        isFlaming = false;
        canFlame = false;
        yield return new WaitForSeconds(flameCooldown);
        currentFlameAmmo = maxFlameAmmo;
        canFlame = true;
        isRealoading = false;
    }



}