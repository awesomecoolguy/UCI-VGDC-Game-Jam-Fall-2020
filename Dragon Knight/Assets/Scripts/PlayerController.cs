using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Configuration Parameters
    [Header("Movement Parameters")]
    [SerializeField] private float movementAccel = .1f;
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpVelocity = 4f;
    [SerializeField] private float groundFriction = 0.99f;

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
        if (Input.GetKey(KeyCode.D))
        {
            float newVelX = playerRB.velocity.x + movementAccel;
            float newVelY = playerRB.velocity.y;
            if (newVelX >= movementSpeed)
            {
                newVelX = movementSpeed;
            }
            if (isFlaming)
            {
                newVelY = 0;
            }
            playerRB.velocity = new Vector2(newVelX, newVelY);
            FlipPlayer(1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            float newVelX = playerRB.velocity.x - movementAccel;
            float newVelY = playerRB.velocity.y;
            if (newVelX <= -movementSpeed)
            {
                newVelX = -movementSpeed;
            }
            if (isFlaming)
            {
                newVelY = 0;
            }
            playerRB.velocity = new Vector2(newVelX, newVelY);
            FlipPlayer(-1);
        }
        else
        {
            if (Mathf.Abs(playerRB.velocity.x) > 0.01)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x * groundFriction, playerRB.velocity.y);
            }
            else
            {
                playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
            }
        }
    }

<<<<<<< Updated upstream
    private void FlipPlayer(int flipScale)
=======

    private void FlipPlayer(int desiredScale)
>>>>>>> Stashed changes
    {
        float desiredScale = Mathf.Abs(transform.localScale.x) * flipScale;
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
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time >= nextTimeToFlame && canFlame)
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
