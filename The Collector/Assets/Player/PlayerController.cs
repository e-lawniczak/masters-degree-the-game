using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // script source: https://www.reddit.com/r/Unity2D/comments/arb0tp/hollowknight_style_movement/
    // https://pastebin.com/X0AytNFR


    //Hollow knight movement script for u/bigjew222
    //Contains jumping, attacking, moving, and recoil mechanics.
    //everything regarding the animator has been commented out so you can use it if you need to.

    //GAMEOBJECT SETUP
    //A gameobject with a rigidBody2D, with rotation locked, and a Box collider.
    //Five Gameobjects childed to it. One just above the collider, one just below the collider, one a distance above, one a distance below,
    //and one a distance in front.
    //If this is confusing just read the other comments and create them as they go.

    private PlayerStateList pState;

    [Header("X Axis Movement")]
    [SerializeField] private float walkSpeed = 6f;

    [Space(5)]

    [Header("Y Axis Movement")]
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float jumpSteps = 10f;
    [SerializeField] private float jumpThreshold = 4f;
    [Space(5)]

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttack = 0.2f;
    [SerializeField] private Transform attackTransform; // this should be a transform childed to the player but to the right of them, where they attack from.
    [SerializeField] private float attackRadius = 0.7f;
    [SerializeField] private Transform downAttackTransform;//This should be a transform childed below the player, for the down attack.
    [SerializeField] private float downAttackRadius = 0.7f;
    [SerializeField] private Transform upAttackTransform;//Same as above but for the up attack.
    [SerializeField] private float upAttackRadius = 0.7f;
    [SerializeField] private LayerMask attackableLayer;
    [Space(5)]

    [Header("Recoil")]
    [SerializeField] private int recoilXSteps = 3;
    [SerializeField] private int recoilYSteps = 5;
    [SerializeField] private float recoilXSpeed = 5;
    [SerializeField] private float recoilYSpeed = 5;
    [Space(5)]

    [Header("Ground Checking")]
    [SerializeField] private Transform groundTransform; //This is supposed to be a transform childed to the player just under their collider.
    [SerializeField] private float groundCheckY = 0.2f; //How far on the Y axis the groundcheck Raycast goes.
    [SerializeField] private float groundCheckX = 1;//Same as above but for X.
    [SerializeField] private LayerMask groundLayer;
    [Space(5)]

    [Header("Roof Checking")]
    [SerializeField] private Transform roofTransform; //This is supposed to be a transform childed to the player just above their collider.
    [SerializeField] private float roofCheckY = 0.2f;
    [SerializeField] private float roofCheckX = 1; // You probably want this to be the same as groundCheckX
    [Space(5)]


    private float timeSinceAttack;
    private float xAxis;
    private float yAxis;
    private float gravity;
    private int stepsXRecoiled;
    private int stepsYRecoiled;
    private int stepsJumped = 0;

    Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject weapon;
    private PlayerLogic playerLogic;

    // Use this for initialization
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        playerLogic = GetComponent<PlayerLogic>();
        gravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerLogic.IsAlive) return;
        GetInputs();
        Flip();
        Walk(xAxis);
        Recoil();
        Attack();
    }

    void FixedUpdate()
    {
        if (!playerLogic.IsAlive) return;

        if (pState.recoilingX == true && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY == true && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }
        if (Grounded())
        {
            StopRecoilY();
        }

        Jump();
    }

    void Jump()
    {
        if (pState.jumping)
        {

            if (stepsJumped < jumpSteps && !Roofed())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                stepsJumped++;
            }
            else
            {
                StopJumpSlow();
            }
        }

        //This limits how fast the player can fall
        //Since platformers generally have increased gravity, you don't want them to fall so fast they clip trough all the floors.
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }
    }

    void Walk(float MoveDirection)
    {
        //Rigidbody2D rigidbody2D = rb;
        //float x = MoveDirection * walkSpeed;
        //Vector2 velocity = rb.velocity;
        //rigidbody2D.velocity = new Vector2(x, velocity.y);
        if (!pState.recoilingX)
        {
            rb.velocity = new Vector2(MoveDirection * walkSpeed, rb.velocity.y);

            if (Mathf.Abs(rb.velocity.x) > 0)
            {
                pState.walking = true;
            }
            else
            {
                pState.walking = false;
            }
            if (xAxis > 0)
            {
                pState.lookingRight = true;
            }
            else if (xAxis < 0)
            {
                pState.lookingRight = false;
            }

            //anim.SetBool("Walking", pState.walking);
        }

    }

    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (Input.GetButtonDown(InputButtons.Swing) && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            //Attack Side
            bool attackOnX = yAxis == 0 || yAxis < 0 && Grounded();
            bool attackUp = yAxis > 0;
            bool attackDown = yAxis < 0 && !Grounded();
            Vector3 attackPos = attackOnX ? attackTransform.position : attackUp ? upAttackTransform.position : downAttackTransform.position;
            float attackRradius = attackOnX ? attackRadius : attackUp ? upAttackRadius : downAttackRadius;

            Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(attackPos, attackRradius, attackableLayer);
            if (objectsToHit.Length > 0)
            {
                if (attackOnX)
                    pState.recoilingX = true;
                else
                    pState.recoilingY = true;
            }
            for (int i = 0; i < objectsToHit.Length; i++)
            {
               // onhit events
            }
            
            weapon.GetComponent<WeaponScript>().PerformAnimation(attackOnX, attackUp, attackDown);
        }
    }

    void Recoil()
    {
        //since this is run after Walk, it takes priority, and effects momentum properly.
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }
        if (pState.recoilingY)
        {
            if (yAxis < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);
                rb.gravityScale = 0;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
                rb.gravityScale = 0;
            }

        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    void Flip()
    {
        if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    void StopJumpQuick()
    {
        //Stops The player jump immediately, causing them to start falling as soon as the button is released.
        stepsJumped = 0;
        pState.jumping = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    void StopJumpSlow()
    {
        //stops the jump but lets the player hang in the air for awhile.
        stepsJumped = 0;
        pState.jumping = false;
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    public bool Grounded()
    {
        //this does three small raycasts at the specified positions to see if the player is grounded.
        if (Physics2D.Raycast(groundTransform.position, Vector2.down, groundCheckY, groundLayer) || Physics2D.Raycast(groundTransform.position + new Vector3(-groundCheckX, 0), Vector2.down, groundCheckY, groundLayer) || Physics2D.Raycast(groundTransform.position + new Vector3(groundCheckX, 0), Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Roofed()
    {
        //This does the same thing as grounded but checks if the players head is hitting the roof instead.
        //Used for canceling the jump.
        if (Physics2D.Raycast(roofTransform.position, Vector2.up, roofCheckY, groundLayer) || Physics2D.Raycast(roofTransform.position + new Vector3(roofCheckX, 0), Vector2.up, roofCheckY, groundLayer) || Physics2D.Raycast(roofTransform.position + new Vector3(roofCheckX, 0), Vector2.up, roofCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GetInputs()
    {
        //WASD/Joystick
        yAxis = Input.GetAxis("Vertical");
        xAxis = Input.GetAxis("Horizontal");

        //This is essentially just sensitivity.
        if (yAxis > 0.25)
        {
            yAxis = 1;
        }
        else if (yAxis < -0.25)
        {
            yAxis = -1;
        }
        else
        {
            yAxis = 0;
        }

        if (xAxis > 0.25)
        {
            xAxis = 1;
        }
        else if (xAxis < -0.25)
        {
            xAxis = -1;
        }
        else
        {
            xAxis = 0;
        }

        //anim.SetBool("Grounded", Grounded());
        //anim.SetFloat("YVelocity", rb.velocity.y);

        //Jumping
        if (Input.GetButtonDown(InputButtons.Jump) && Grounded())
        {
            pState.jumping = true;
        }

        if (!Input.GetButton(InputButtons.Jump) && stepsJumped < jumpSteps && stepsJumped > jumpThreshold && pState.jumping)
        {
            StopJumpQuick();
        }
        else if (!Input.GetButton(InputButtons.Jump) && stepsJumped < jumpThreshold && pState.jumping)
        {
            StopJumpSlow();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
        Gizmos.DrawWireSphere(downAttackTransform.position, downAttackRadius);
        Gizmos.DrawWireSphere(upAttackTransform.position, upAttackRadius);
        //Gizmos.DrawWireCube(groundTransform.position, new Vector2(groundCheckX, groundCheckY));

        Gizmos.DrawLine(groundTransform.position, groundTransform.position + new Vector3(0, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(-groundCheckX, 0), groundTransform.position + new Vector3(-groundCheckX, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(groundCheckX, 0), groundTransform.position + new Vector3(groundCheckX, -groundCheckY));

        Gizmos.DrawLine(roofTransform.position, roofTransform.position + new Vector3(0, roofCheckY));
        Gizmos.DrawLine(roofTransform.position + new Vector3(-roofCheckX, 0), roofTransform.position + new Vector3(-roofCheckX, roofCheckY));
        Gizmos.DrawLine(roofTransform.position + new Vector3(roofCheckX, 0), roofTransform.position + new Vector3(roofCheckX, roofCheckY));
    }
}