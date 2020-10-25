using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Ground_Enemy_AI : MonoBehaviour
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
    public ParticleSystem part;
    public List<ParticleCollisionEvent> events;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        events = new List<ParticleCollisionEvent>();
        health = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= Enemy_Awareness_Distance)
        {
            aware = true;
            //aware animation
            //aware sound
        }
        if (aware)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);
            anim.SetBool("isWalking", true);
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
            anim.SetBool("isWalking",true);
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
        while (i < numEvents)
        {
            if (rb)
            {
                if (other.gameObject.name == "Orc")
                {
                    StartCoroutine("Player_attacks_enemy");
                }
            }
            i++;
        }
    }
}
