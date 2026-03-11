using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    Player player;
    public int damage = 1;
    public bool isFake = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
         if (player != null && (player.isDead || player.levelCompleted))
        return;
        
       Vector2 pos = transform.position;

       pos.x -= player.velocity.x * Time.fixedDeltaTime;
       if(pos.x < -100)
        {
            Destroy(gameObject);
        }
        transform.position = pos;
    }
}
