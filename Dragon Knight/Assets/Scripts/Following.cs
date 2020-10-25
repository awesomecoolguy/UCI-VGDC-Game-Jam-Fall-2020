using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Following : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;
    private PlayerController playerController; 
    private float distance = 0.24f;
    [SerializeField] float distanceMultiplier = 2f;
    private Vector3 v3Pos;
    private float angle;
    private bool temp;

    private void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
    }
    void Update()
    {

        v3Pos = Input.mousePosition;
        v3Pos.z = (playerTransform.position.z - Camera.main.transform.position.z);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        v3Pos = v3Pos - playerTransform.position;
        angle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) angle += 360.0f;
        transform.localEulerAngles = new Vector3(0, 0, angle);


        float xPos = Mathf.Cos(Mathf.Deg2Rad * angle) + distance;
        float yPos = Mathf.Sin(Mathf.Deg2Rad * angle) + distance;
        transform.localPosition = new Vector3(playerTransform.position.x + xPos*distanceMultiplier, playerTransform.position.y + yPos*distanceMultiplier, 0);
     
    }
}
