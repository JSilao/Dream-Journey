using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parallax : MonoBehaviour
{
    public float depth = 1;
    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("Player not found in scene!");
    }

    void FixedUpdate()
    {
       float realVelocity = player.velocity.x / depth;
       Vector2 pos = transform.position;

       pos.x -= realVelocity * Time.fixedDeltaTime;

       if(pos.x <= -25)
        pos.x = 80;

        transform.position = pos;
    }
}
