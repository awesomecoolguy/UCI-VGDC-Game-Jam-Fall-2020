using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Sky_Enemy_AI : MonoBehaviour
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
    public int Audio_Distance;
    public AudioClip wings;
    public AudioClip fireball;
    private AudioSource audio;
    private bool Reloaded = true;
    public float projectile_speed;
    public GameObject projectile;
    public int Reloading_time;
    private bool first_shot = true;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= Audio_Distance)
        {
            audio.clip = wings;
            audio.loop = true;
            audio.Play();
        }
        else
        {
            audio.loop = false;
        }
        if (Vector3.Distance(player.transform.position, transform.position) <= Enemy_Awareness_Distance)
        {
            aware = true;
            //aware animation
            //aware sound
        }
        if (aware)
        {
            if (first_shot)
            {
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                projectile.SetActive(true);
                projectile.transform.position = transform.position;
                //attack sound
                projectile.GetComponent<Rigidbody2D>().velocity = transform.forward * projectile_speed;
                Reloaded = false;
                StartCoroutine("Reloading");
                first_shot = false;
            }
            if (Reloaded && !first_shot)
            {
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                GameObject projectile_clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
                projectile_clone.GetComponent<Rigidbody2D>().velocity = transform.forward * projectile_speed;
                //attack sound
                Reloaded = false;
                StartCoroutine("Reloading");
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                {
                    if (hit.point == player.transform.position)
                    {
                        player.GetComponent<Health>().Damage();
                    }
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);
            //move attack animation
            var direction = player.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        //if(hit.transform.gameObject.name == "flame" || hit.transform.gameObject.name == "projectile")
        //{
        //StartCoroutine("Player_attacks_enemy");
        //}
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(Reloading_time);
        Reloaded = true;
    }

    //IEnumerator Player_attacks_enemy()
    //{
    //health.currentHealth -= 1;
    //yield return new WaitForSeconds(1);
    //taking damage animation
    //taking damage sound
    //}
}

