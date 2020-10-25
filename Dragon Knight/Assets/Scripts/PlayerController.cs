using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public bool onGround = true;
    private int gemsCollected = 0;
    
    public bool canFlame;
    private float maxFlameAmmo = 6f;
    private float m_CurrentFlameAmmo;
    private float currentFlameAmmo
    {
        get
        {
            return m_CurrentFlameAmmo;
        }
        set
        {
            m_CurrentFlameAmmo = value;
            if (gameManager != null)
                gameManager.SetFlameGauge((int)m_CurrentFlameAmmo);
        }
    }
    private float nextTimeToFlame = 0f;
    private float flameRate = 2f;
    private int flameCooldown = 3;
    public bool isRealoading = false;

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
        Ground = FindObjectOfType<TilemapCollider2D>();
        gameManager = GameManager.Get();
        gameManager.SetFlameGaugeMax((int)maxFlameAmmo);
        flameBreathPS = flameBreath.GetComponent<ParticleSystem>();
        currentFlameAmmo = maxFlameAmmo;
        canFlame = true;
    }

    private void OnEnable()
    {
        isRealoading = false;

    }

    void FixedUpdate()
    {
        HorizontalMovement();
        PlayerJump();
        DetermineOnGround();
        if (currentFlameAmmo <= 0)
        {
            Debug.Log("hi");
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

    private void TriggerFire() // add bigger if statement to track the cooldown so it stops triggering twice
    {
        if(Input.GetKey(KeyCode.E))
        { 
            if (Time.time >= nextTimeToFlame && canFlame )
            {
                Debug.Log(currentFlameAmmo);
                flameBreathPS.Play();
                isFlaming = true;
                currentFlameAmmo--;
                nextTimeToFlame = Time.time + 1f / flameRate;
            }  
        }
        else
        {
            isFlaming = false;
            flameBreathPS.Stop();
        }
    }

    IEnumerator reloadFlame()
    {
        isRealoading = true;
        isFlaming = false;
        canFlame = false;
       
        yield return new WaitForSeconds(flameCooldown);
  
        canFlame = true;
        isRealoading = false;
        currentFlameAmmo = maxFlameAmmo;
    }



}
