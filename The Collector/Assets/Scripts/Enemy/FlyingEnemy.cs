using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private int enemyId;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lockOnSpeed = 15f;
    [SerializeField] private float activationRadius = 25f;
    [SerializeField] private float lockOnRadius = 15f;
    [SerializeField] private float lockOnWait = 0.3f;
    [SerializeField] private float timeToDie = 3f;
    [SerializeField] private float lockOnOffset = 1f;
    [SerializeField] GameObject player;


    private Rigidbody2D rb;
    private PolygonCollider2D bc;

    [SerializeField] private bool goingLeft, goingRight;
    private float xAxis;
    private bool isActive;
    private bool isDying;
    private Vector2 playerPos;
    private Vector2? lockOnPos;
    private float lockOnWaitOriginal;


    // Start is called before the first frame update
    void Start()
    {
        if(RuntimeVariables.defeatedEnemies.IndexOf(enemyId) > -1)
        {
            Destroy(this, 0.0f);
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<PolygonCollider2D>();
        playerPos = player.GetComponent<Transform>().position;
        xAxis = 1;
        isActive = false;
        isDying = false;
        lockOnPos = null;
        lockOnWaitOriginal = lockOnWait;
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        if (RuntimeVariables.defeatedEnemies.IndexOf(enemyId) > -1)
        {
            Destroy(this, 0.0f);
            return;
        }
        if (!isActive)
        {
            CheckForPlayer();
            return;
        }
        else
        {
            TimeToDie();
        }


    }
    void TimeToDie()
    {
        if (timeToDie <= 0)
        {
            Die();
        }
        if (isDying)
        {
            timeToDie -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if (!isActive) return;
        LockOn();
        Move();
    }
    void CheckForPlayer()
    {
        playerPos = player.GetComponent<SpriteRenderer>().bounds.min;
        float dist = Vector2.Distance(transform.position, playerPos);
        if (dist < activationRadius)
        {
            isActive = true;
        }
    }
    void LockOn()
    {
        playerPos = player.GetComponent<Transform>().position;
        var targetPos = new Vector2(playerPos.x + (goingRight ? lockOnOffset : -lockOnOffset), playerPos.y - lockOnOffset);
        float dist = Vector2.Distance(transform.position, playerPos);
        if (dist < lockOnRadius && !lockOnPos.HasValue)
        {
            Vector2 direction = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y);
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, rotation);
            lockOnPos = targetPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody && collision.rigidbody.name == LayerVariables.Player)
        {
            var pl = collision.collider.GetComponent<PlayerLogic>();
            pl.GetHitByEnemy(this.gameObject, this.transform.position);
            Die();
        }
        else
        {
            isDying = true;
        }
    }


    void Move()
    {
        if (lockOnPos.HasValue && lockOnWait > 0)
        {
            lockOnWait -= Time.deltaTime;
        }
        if (lockOnPos.HasValue && lockOnWait <= 0)
        {
            rb.MovePosition(Vector2.MoveTowards(transform.position, lockOnPos.Value, Time.deltaTime * lockOnSpeed));
        }
        if (!lockOnPos.HasValue)
        {
            int sign = goingRight ? 1 : -1;
            rb.velocity = new Vector2(sign * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (lockOnPos.HasValue && (Vector2)transform.position == lockOnPos.Value)
        {
            lockOnPos = null;
            lockOnWait = lockOnWaitOriginal;
        }
    }
    void Die()
    {
        RuntimeVariables.defeatedEnemies.Add(enemyId);
        PlaytroughVariables.EnemiesDefeated += 1;
        RuntimeVariables.CurrentLevelEnemiesDefeated += 1;
        Destroy(gameObject, 0.0f);
    }

    public void Flip()
    {
        if (goingLeft)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0);

        }
    }
    public bool GetHit(int damage, Vector3 position)
    {
        Die();
        return true;
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, lockOnRadius);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, activationRadius);
    //}
    public int GetId()
    {
        return enemyId;
    }
}
