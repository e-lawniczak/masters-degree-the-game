using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private Transform _wallDetectTransform;
    [SerializeField] private float maxHp = 20;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float onHitRecoil = 3f;

    private Rigidbody2D rb;
    private CircleCollider2D bc;

    private float hp;
    private bool goingLeft, goingRight;
    private float xAxis;
    private float wallDetectRad = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CircleCollider2D>();
        
        hp = maxHp;
        xAxis = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) return;

    }
    private void FixedUpdate()
    {
        if (hp <= 0) return;
        //Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        //Debug.Log(collision.collider.tag);
    }

    void Move()
    {
        rb.velocity = new Vector2(xAxis * speed, rb.velocity.y);
    }
    void Die()
    {
        Destroy(gameObject, 0.0f);
    }

    void Recoil(Vector3 position)
    {
        Vector3 heading = position - transform.position;
        var dirNum = HelperFunctions.AngleDir(transform.forward, heading, transform.up);
        rb.velocity = new Vector2(-dirNum * onHitRecoil, 0);

    }

    public void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        xAxis = -1 * xAxis;
    }
    public void GetHit(int damage, Vector3 position)
    {
        Debug.Log("enemyHit");
        hp -= damage;
        if (hp > 0) Recoil(position);
        if (hp <= 0) Die();
    }

}
