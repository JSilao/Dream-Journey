using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    Player player;
    public int damage = 1;
    public bool isFake = false;
    public ObstacleType obstacleType = ObstacleType.Ground;
    public bool isInstantKill = false;
    public int healAmount = 0;

    public bool isMoving = false;
    public bool isFloating = false;
    public bool isChasing = false;

    public float moveSpeed = 2f;
    public float floatSpeed = 0.5f;
    public float floatAmplitude = 0.5f;

    private Vector2 startPos;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public enum ObstacleType
    {
        Ground,
        Air,
        DreamMonster,
        SpiritAnimal
    }

    void Start()
    {
    }
    public void Init()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
     Vector2 pos = transform.position;

    // AIR OBSTACLE (bird)
    if (isFloating)
    {
        pos.y = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        if(obstacleType == ObstacleType.Air)
        {
            pos.x -= 30f * Time.deltaTime;
        }
    }

    // DREAM MONSTER (NOW FIXED)
    if (isChasing)
    {
        float direction = Mathf.Sign(player.transform.position.y - pos.y);
        pos.y += direction * 1.5f * Time.deltaTime;
    }

    transform.position = pos;
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
