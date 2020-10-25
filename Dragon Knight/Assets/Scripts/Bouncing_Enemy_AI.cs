using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Bouncing_Enemy_AI : MonoBehaviour
{
    public GameObject player;
    public int Enemy_Awareness_Distance;
    public int speed;
    public int speed_after_collision;
    private bool aware = false;
    public Vector3 starting_position;
    public int patroling_distance;
    private Vector3 direction;
    private bool patrol = false;
    private EnemyHealth health;
    private bool bounce = true;
    public float jump_speed;
    public GameObject ground;
    public ParticleSystem part;
    public List<ParticleCollisionEvent> events;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        events = new List<ParticleCollisionEvent>();
        health = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bounce)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0f, jump_speed);
        }
        if (GetComponent<Collider2D>().IsTouching(ground.GetComponentInChildren<Collider2D>()))
        {
            bounce = true;
        }
        else
        {
            bounce = false;
        }
        if (Vector3.Distance(player.transform.position, transform.position) <= Enemy_Awareness_Distance)
        {
            aware = true;
            //aware animation
            //aware sound
        }
        if (aware)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            //move attack animation
            //move attack sound
        }
        if (Vector3.Distance(player.transform.position, transform.position) >= 2)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            speed = speed_after_collision;
        }
        if (!aware)
        {
            if (transform.position.x <= starting_position.x - patroling_distance / 2)
            {
                patrol = true;
            }
            if (transform.position.x >= starting_position.x + patroling_distance / 2)
            {
                patrol = false;
            }
            if (!patrol)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.right;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            //patroling animation
            //patroling sound
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.transform.gameObject.name == player.name)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            speed = 0;
            //attack animation
            //attack sound
            StartCoroutine("Enemy_attacks_player");
        }
    }

    IEnumerator Enemy_attacks_player()
    {
        player.GetComponent<Health>().Damage();
        yield return new WaitForSeconds(1);
    }

    IEnumerator Player_attacks_enemy()
    {
            health.currentHealth -= 1;
            yield return new WaitForSeconds(1);
        //taking damage animation
        //taking damage sound
    }
    
    void OnParticleCollision(GameObject other)
    {
        int numEvents = part.GetCollisionEvents(other, events);
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        int i = 0;
        while(i < numEvents)
        {
            if (rb)
            {
                if (other.gameObject.name == "Flying Orc")
                {
                    StartCoroutine("Player_attacks_enemy");
                }
            }
            i++;
        }
    }
}
