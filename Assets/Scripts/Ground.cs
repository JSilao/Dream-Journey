using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ground : MonoBehaviour
{
    Player player;
    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D boxCollider;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    public Obstacle airObstacleTemplate;
    public Obstacle dreamMonsterTemplate;
    public Obstacle spiritAnimalTemplate;
    

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
       
        screenRight = Camera.main.transform.position.x *2;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         groundHeight = transform.position.y + (boxCollider.size.y / 2);
    }

    private void FixedUpdate()
    {
         if (player != null && (player.isDead || player.levelCompleted))
        return;

        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (boxCollider.size.x /2);
        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }
        if (!didGenerateGround)
        {
            if(groundRight < screenRight)
        {
            didGenerateGround = true;
            generateGround();
        }   
        }
        transform.position = pos;
        
    }
    void generateGround()
{
    GameObject go = Instantiate(gameObject);
    BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
    Vector2 pos;

    float h1 = player.jumpVelocity * player.maxHoldJumpTime;
    float t = player.jumpVelocity / -player.gravity;
    float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
    float maxJumpHeight = h1 + h2;
    float maxY = maxJumpHeight * 0.5f;
    maxY += groundHeight;

    float minY = 1;
    float actualY = Random.Range(minY, maxY);

    pos.y = actualY - goCollider.size.y / 2;
    if (pos.y > 0.5f)
    {
        pos.y = 0.5f;
    }

    float t1 = t + player.maxHoldJumpTime;
    float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
    float totalTime = t1 + t2;

    float maxX = totalTime * player.velocity.x;
    maxX *= 0.7f;
    maxX += groundRight;

    float minX = screenRight + 5;
    float actualX = Random.Range(minX, maxX);

    pos.x = actualX + goCollider.size.x / 2;
    go.transform.position = pos;

    Ground goGround = go.GetComponent<Ground>();
    goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

    GroundFall fall = go.GetComponent<GroundFall>();
    if (fall != null)
    {
        Destroy(fall);
    }

    if (Random.value < GameManager.Instance.fallingPlatformChance)
    {
        fall = go.AddComponent<GroundFall>();
        fall.fallSpeed = Random.Range(1.0f, 3.0f);
    }

    // ======================
    // DEFINE PLATFORM BOUNDS (USED EVERYWHERE)
    // ======================
    float halfWidth = goCollider.size.x / 2 - 1;
    float left = go.transform.position.x - halfWidth;
    float right = go.transform.position.x + halfWidth;

    // ======================
    // GROUND OBSTACLES (Day + Afternoon)
    // ======================
    int obstacleNum = Random.Range(0, GameManager.Instance.maxObstaclesPerGround + 1);

    for (int i = 0; i < obstacleNum; i++)
    {
        GameObject box = Instantiate(boxTemplate.gameObject);

        float x = Random.Range(left, right);
        float y = goGround.groundHeight;

        box.transform.position = new Vector2(x, y);

        Obstacle o = box.GetComponent<Obstacle>();

        if (EnvironmentManager.Instance.currentMode == EnvironmentManager.Mode.Afternoon)
        {
            if (Random.value < 0.3f)
            {
                o.isFake = true;

                SpriteRenderer sr = box.GetComponent<SpriteRenderer>();
                sr.color = new Color(1f, 0f, 0f, 0.5f);
            }
        }

        if (fall != null)
        {
            fall.obstacles.Add(o);
        }
    }

    // ======================
    // NIGHT DREAM MODE
    // ======================
    if (EnvironmentManager.Instance.currentMode == EnvironmentManager.Mode.Night)
    {
        // AIR OBSTACLES
        int airObstacleCount = Random.Range(0, 2);
        for (int i = 0; i < airObstacleCount; i++)
        {
            GameObject air = Instantiate(airObstacleTemplate.gameObject);

            float x = right + Random.Range(1f, 3f);
            float y = goGround.groundHeight + Random.Range(5f, 10f);

            air.transform.position = new Vector2(x, y);

            Obstacle airObs = air.GetComponent<Obstacle>();
            airObs.obstacleType = Obstacle.ObstacleType.Air;
            airObs.isFloating = true;
            airObs.floatSpeed = Random.Range(0.5f, 1f);
            airObs.floatAmplitude = Random.Range(0.5f, 1f);
            airObs.Init();
        }

        // DREAM MONSTER
        if (Random.value < 0.3f)
        {
            GameObject monster = Instantiate(dreamMonsterTemplate.gameObject);

            float x = Random.Range(left, right);
            float y = goGround.groundHeight;

            monster.transform.position = new Vector2(x, y);

            Obstacle monsterObs = monster.GetComponent<Obstacle>();
            monsterObs.obstacleType = Obstacle.ObstacleType.DreamMonster;
            monsterObs.isChasing = true;
            monsterObs.damage = Random.Range(1, 3);
            monsterObs.isInstantKill = Random.value < 0.3f;
        }

        // SPIRIT ANIMAL
        if (Random.value < 0.25f)
        {
            bool spawnFloating = Random.value < 0.5f;
            if (!spawnFloating)
            {
                GameObject spirit = Instantiate(spiritAnimalTemplate.gameObject);

                float x = Random.Range(left, right);
                float y = goGround.groundHeight;
                

                spirit.transform.position = new Vector2(x, y);

                Obstacle o = spirit.GetComponent<Obstacle>();
                o.obstacleType = Obstacle.ObstacleType.SpiritAnimal;
                o.healAmount = 1;
            }else
            {
                GameObject spirit = Instantiate(spiritAnimalTemplate.gameObject);

                float x = Random.Range(left, right);
                float y = goGround.groundHeight + Random.Range(4f, 6f);

                spirit.transform.position = new Vector2(x, y);

                Obstacle o = spirit.GetComponent<Obstacle>();
                o.obstacleType = Obstacle.ObstacleType.SpiritAnimal;
                o.healAmount = 1;

                // o.isFloating = true;
                o.floatSpeed = Random.Range(0.5f, 0.5f);
                o.floatAmplitude = Random.Range(0.3f, 0.8f);
            }
            
        }
    }
}

}
