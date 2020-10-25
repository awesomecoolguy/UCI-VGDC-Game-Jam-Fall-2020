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
    private float flameCooldown = 5f;
    public float maxFlameAmmo;
    public float flameRate;
    

    private bool onGround = true;
    private int gemsCollected = 0;

    private bool isFlaming;
    private float currentFlameAmmo;
    private bool canFlame;
    private float nextTimeToFlame = 0f;
    private bool isReloading;
   

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
        currentFlameAmmo = maxFlameAmmo;
        gameManager.SetFlameGaugeMax((int)maxFlameAmmo);
        gameManager.SetFlameGauge((int)currentFlameAmmo);
        canFlame = true;
        isFlaming = false;
    }
    void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        HorizontalMovement();
        PlayerJump();
        DetermineOnGround();
        Flame();
        if (isReloading)
            return;
        if(currentFlameAmmo <= 0)
        {
            StartCoroutine(checkFlame());
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
                playerRB.velocity = new Vector2(movementSpeed, 0f);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                FlipPlayer(-1);
                playerRB.velocity = new Vector2(-movementSpeed, 0f);
            }
            else
            {
                playerRB.velocity = new Vector2(0f, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKey(KeyCode.P))
        {
            if (canFlame && currentFlameAmmo > 0 && onGround == false && Time.time >= nextTimeToFlame)
            {
                Debug.Log("Flame");
                isFlaming = true;
                nextTimeToFlame = Time.time + 1f / flameRate;
                currentFlameAmmo--;
                gameManager.SetFlameGauge((int)currentFlameAmmo);
            }

            if(canFlame == false)
            {
                Debug.Log("No Flame");

                
            }   
            
        }
        else
        {
            isFlaming = false;
        }


    }



    IEnumerator checkFlame()
    {
        isReloading = true;
        canFlame = false;
        isFlaming = false;
        Debug.Log("realoding");
        yield return new WaitForSeconds(flameCooldown);
        currentFlameAmmo = maxFlameAmmo;
        gameManager.SetFlameGauge((int)currentFlameAmmo);
        canFlame = true;
        isReloading = false;
 
          

    }

}
