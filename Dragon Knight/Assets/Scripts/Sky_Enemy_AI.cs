﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
    //private bool first_shot = true;
    private GameObject projectile_clone;
    public GameObject ground;
    public ParticleSystem part;
    public List<ParticleCollisionEvent> events;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        events = new List<ParticleCollisionEvent>();
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
            var direction = player.transform.position - transform.position;
            direction.Normalize();
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(-(angle + 60),-60,60), Vector3.forward);
            if (Reloaded)
            {
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), collider);
                }
                projectile.SetActive(true);
                projectile.transform.position = transform.position;
                audio.PlayOneShot(fireball, 0.7f);
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * projectile_speed;
                Reloaded = false;
                StartCoroutine("Reloading");
                //first_shot = false;
                //attack animation
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction.x, direction.y), Mathf.Infinity);
                foreach(Collider2D collider in player.GetComponents<Collider2D>())
                {
                    if (hit.collider == collider)
                    {
                        projectile.SetActive(false);
                        player.GetComponent<Health>().Damage();
                    }
                }
                foreach (Collider2D collider_ground in ground.GetComponents<Collider2D>())
                {
                    if (hit.collider == collider_ground)
                    {
                        projectile.SetActive(false);
                        //first_shot = true;

                    }
                }
            }
            //if (Reloaded && !first_shot)
            //{
            //    foreach (Collider2D collider in GetComponents<Collider2D>())
            //    {
            //        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), collider);
            //    }
            //    projectile_clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            //    projectile_clone.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * projectile_speed;
            //    audio.PlayOneShot(fireball, 0.7f);
            //    Reloaded = false;
            //    StartCoroutine("Reloading");
            //    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction.x, direction.y), Mathf.Infinity);
            //    foreach (Collider2D collider in player.GetComponents<Collider2D>())
            //    {
            //        if(hit.collider != null)
            //        {
            //            Destroy(projectile_clone);
            //        }
            //        if (hit.collider == collider)
            //        {
            //            player.GetComponent<Health>().Damage();
            //        }
            //    }
            //}
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);
            //move attack animation
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
                if (other.gameObject.name == "Raven")
                {
                    StartCoroutine("Player_attacks_enemy");
                }
            }
            i++;
        }
    }
}

