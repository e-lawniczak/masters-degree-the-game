using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float distanceToTravel = 5f;
    [SerializeField] private bool goingUp = false;
    [SerializeField] private bool oneTimeTravel = false;

    private Rigidbody2D rb;
    private Vector2 startingPos;
    private Vector2 destPos;
    private bool reachedPosition;
    // Start is called before the first frame update
    void Start()
    {
        reachedPosition = false;
        startingPos = transform.position;
        destPos = goingUp ? new Vector2(startingPos.x, startingPos.y + distanceToTravel) : new Vector2(startingPos.x + distanceToTravel, startingPos.y);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        MoveBlock();
    }

    private void MoveBlock()
    {
        Vector2 pos = transform.position;
        Vector2 destvector = reachedPosition ? startingPos : destPos;
        transform.position = Vector2.MoveTowards(transform.position, destvector, Time.deltaTime * movingSpeed);
        if (pos == destvector && !oneTimeTravel)
        {
            reachedPosition = !reachedPosition;
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startingPos, destPos);
    }

}
