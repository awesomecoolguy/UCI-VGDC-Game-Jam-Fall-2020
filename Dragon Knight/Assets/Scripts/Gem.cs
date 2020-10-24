using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.gameObject.GetComponent<PlayerController>();
        if(other != null)
        {
            other.AddGemCollected();
            Destroy(gameObject);
        }
    }
}
