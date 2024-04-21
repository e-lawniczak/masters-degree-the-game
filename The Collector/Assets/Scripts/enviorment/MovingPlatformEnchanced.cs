using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.GraphicsBuffer;

public class MovingPlatformEnchanced : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 1.5f;


    private int dir = 1;
    private float maxDistance;

    PlayerController playerController;
    Rigidbody2D rb;
    Vector2 moveDirection;
    Vector2 targetPos;

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        targetPos = CurrentTarget();
        maxDistance = Vector2.Distance(startPoint.position, endPoint.position);
        GetDirection();
    }
    private void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        float dist = (targetPos - (Vector2)(transform.position)).magnitude;
        float distanceToTarget = Vector2.Distance(transform.position, targetPos);
        if (dist <= 0.2f || distanceToTarget > maxDistance + 0.5f)
        {
            ChangeDirection();
        }
    }
    public void ChangeDirection()
    {
        dir *= -1;
        targetPos = CurrentTarget();
        GetDirection();
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void GetDirection()
    {
        moveDirection = (targetPos - (Vector2)transform.position).normalized;
    }
    Vector2 CurrentTarget()
    {
        if (dir == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")))
        {
            playerController.isOnPlatform = true;
            playerController.platformRb = rb;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")))
        {
            playerController.isOnPlatform = false;
        }
    }

   


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(platform.position, startPoint.position);
        Gizmos.DrawLine(platform.position, endPoint.position);
    }
}
