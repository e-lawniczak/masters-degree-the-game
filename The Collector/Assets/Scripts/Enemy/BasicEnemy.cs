using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private int enemyId;
    [SerializeField] private float maxHp = 4;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float onHitRecoil = 50f;

    private Rigidbody2D rb;
    private CircleCollider2D bc;

    private float hp;
    private bool goingLeft, goingRight;
    private float xAxis;
    private bool recoiling;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        if (RuntimeVariables.defeatedEnemies.IndexOf(enemyId) > -1)
        {
            Destroy(gameObject, 0.0f);
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CircleCollider2D>();

        hp = maxHp;
        xAxis = 1;
        recoiling = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (RuntimeVariables.defeatedEnemies.IndexOf(enemyId) > -1)
        {
            Destroy(gameObject, 0.0f);
            return;
        }
        if (hp <= 0) return;

    }
    private void FixedUpdate()
    {
        if (hp <= 0) return;
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody && collision.rigidbody.name == LayerVariables.Player)
        {
            var pl = collision.collider.GetComponent<PlayerLogic>();
           
            pl.GetHitByEnemy(this.gameObject, this.transform.position);
        }
    }

    void Move()
    {
        if (!recoiling)
        {
            rb.velocity = new Vector2(xAxis * speed, rb.velocity.y);
        }
        else
        {
            //rb.velocity = new Vector2(xAxis * speed, rb.velocity.y);
            Recoil();
        }
    }
    void Die()
    {
        RuntimeVariables.defeatedEnemies.Add(enemyId);
        PlaytroughVariables.EnemiesDefeated += 1;
        RuntimeVariables.CurrentLevelEnemiesDefeated += 1;
        Destroy(gameObject, 0.0f);
    }

    void Recoil()
    {
        Vector3 heading = playerPos - transform.position;
        var dirNum = HelperFunctions.AngleDir(transform.forward, heading, transform.up);
        rb.velocity = new Vector2(-dirNum * onHitRecoil, 0);
        recoiling = false;

    }

    public void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        xAxis = -1 * xAxis;
    }
    public bool GetHit(int damage, Vector3 position)
    {
        hp -= damage;
        if (hp > 0)
        {
            recoiling = true;
            playerPos = position;
        }
        if (hp <= 0)
        {
            Die();
            return true;
        }
        
        return false;
    }
    public int GetId()
    {
        return enemyId;
    }

}
