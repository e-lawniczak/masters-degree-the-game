using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HelperFunctions;

public class PlayerController : MonoBehaviour
{
    // The script below is a modified verios of the script mentioned in the source

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
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private float fallSpeed = 500f;
    [SerializeField] private float jumpSteps = 5f;
    [SerializeField] private float jumpThreshold = 5f;
    [Space(5)]

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttack = 0.15f;
    [SerializeField] private Transform attackTransform; // this should be a transform childed to the player but to the right of them, where they attack from.
    private float attackRadius = RuntimeVariables.PlayerAttackRadius;
    [SerializeField] private Transform downAttackTransform;//This should be a transform childed below the player, for the down attack.
    private float downAttackRadius = RuntimeVariables.PlayerAttackRadiusDown;
    [SerializeField] private Transform upAttackTransform;//Same as above but for the up attack.
    private float upAttackRadius = RuntimeVariables.PlayerAttackRadiusUp;
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
    //[SerializeField] private GameEngine engine; 
    [Space(5)]


    private float timeSinceAttack;
    private float xAxis;
    private float yAxis;
    private float gravity;
    private int stepsXRecoiled;
    private int stepsYRecoiled;
    private int stepsJumped = 0;
    private int jumpMoveDivider = 1;
    private bool canDash = true;
    private float timeSinceDash = 0;
    private float dashCooldown = 1.5f;
    private bool isTurnedLeft = true;
    private Vector3 prevPos;
    private float checkPosTimer = 2f;
    private float counter = 0f;
    public DashInfo DashInfo { get { return new DashInfo { dashCd = dashCooldown, currentCd = timeSinceDash, canDash = canDash }; } }

    Rigidbody2D rb;
    [Header("Other")]
    [SerializeField] private float airMovementMod = 1.3f;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject weapon;
    private PlayerLogic playerLogic;
    private SoundHandler soundHandler;

    // Use this for initialization
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        playerLogic = GetComponent<PlayerLogic>();
        gravity = rb.gravityScale;
        prevPos = rb.position;
        soundHandler = GameObject.Find("SoundHandler").GetComponent<SoundHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerLogic.IsAlive) return;
        CheckEnemyInProximity();
        CheckIfStandingStill();
        GetInputs();
        Flip();
        Walk(xAxis);
        Recoil();
        Attack();
    }

    private void CheckIfStandingStill()
    {
        counter += Time.deltaTime;
        if (counter >= checkPosTimer)
        {
            if (rb.position.Equals(prevPos))
            {
                PlaytroughVariables.StandingStillTime += Time.deltaTime;
            }
            else
            {
                counter = 0;
            }
        }
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
        Dash();
    }

    void Jump()
    {
        if (pState.jumping)
        {
            anim.SetBool(AnimationVariables.IsJumping, true);
            float jSpeed = pState.jumpedOnSpikes ? jumpSpeed / 2 : jumpSpeed;
            if (stepsJumped < jumpSteps && !Roofed())
            {
                rb.velocity = new Vector2(rb.velocity.x / airMovementMod, jSpeed);
                //rb.AddForce(new Vector2(0, jSpeed));
                stepsJumped++;
            }
            else
            {
                StopJumpSlow();
                pState.jumpedOnSpikes = false;
            }
        }

        //This limits how fast the player can fall
        //Since platformers generally have increased gravity, you don't want them to fall so fast they clip trough all the floors.
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }

    }
    void Dash()
    {
        if (pState.dashing)
        {
            //rb.velocity = new Vector2(1000, 0);
            var force = 50000f;
            rb.AddForce(new Vector2(isTurnedLeft ? force : -force, 0));
            pState.dashing = false;
            canDash = false;
            soundHandler.Dash();
        }
        if (!canDash)
        {
            timeSinceDash += Time.deltaTime;
            if (timeSinceDash >= dashCooldown)
            {
                canDash = true;
                timeSinceDash = 0;
            }
        }
    }
    private float walkSoundTimer = 0.4f;
    private float walkSoundCounter;
    void Walk(float MoveDirection)
    {
        //Rigidbody2D rigidbody2D = rb;
        //float x = MoveDirection * walkSpeed;
        //Vector2 velocity = rb.velocity;
        //rigidbody2D.velocity = new Vector2(x, velocity.y);
        float x = MoveDirection * walkSpeed;
        //walkSoundCounter += Time.deltaTime;
        x = Grounded() ? x : x / airMovementMod;
        if (!pState.recoilingX)
        {
            rb.velocity = new Vector2(x, rb.velocity.y);

            if (Mathf.Abs(rb.velocity.x) > 0)
            {
                pState.walking = true;
                anim.SetBool(AnimationVariables.IsMoving, true);
                //if(walkSoundCounter > walkSoundTimer)
                //{
                //soundHandler.GrassStep();
                //    walkSoundCounter = 0f;
                //}
            }
            else
            {
                pState.walking = false;
                anim.SetBool(AnimationVariables.IsMoving, false);
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
            bool spikeHit = false;
            bool enemyHit = false;
            if (objectsToHit.Length > 0)
            {
                if (attackOnX)
                    pState.recoilingX = true;
                else
                    pState.recoilingY = true;

                if (objectsToHit.Select(o => o).Where(o => o.name == LayerVariables.HazardsTriggerObj || o.name == LayerVariables.Enemy).ToArray().Length > 0 && attackDown)
                {
                    pState.jumpedOnSpikes = true;
                    if (objectsToHit.Select(o => o).Where(o => o.name == LayerVariables.HazardsTriggerObj).ToArray().Length > 0)
                    {
                        spikeHit = true;
                    }
                }
            }
            for (int i = 0; i < objectsToHit.Length; i++)
            {
                if (objectsToHit[i].GetComponent<BasicEnemy>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    bool isDead = objectsToHit[i].GetComponent<BasicEnemy>().GetHit(weapon.GetComponent<WeaponScript>().GetDamage(), transform.position);
                    enemyHit = true;
                    if (isDead)
                    {
                        playerLogic.AddPoints(RuntimeVariables.BasicEnemyPoints);
                    }
                }
                if (objectsToHit[i].GetComponent<FlyingEnemy>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    bool isDead = objectsToHit[i].GetComponent<FlyingEnemy>().GetHit(weapon.GetComponent<WeaponScript>().GetDamage(), transform.position);
                    enemyHit = true;
                    if (isDead)
                    {
                        playerLogic.AddPoints(RuntimeVariables.FlyingEnemyPoints);
                    }
                }
                if (objectsToHit[i].GetComponent<CannonEnemy>() != null || objectsToHit[i].GetComponent<BulletScript>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    spikeHit = true;
                }

            }
            if (spikeHit)
            {
                soundHandler.SpikeHit();
            }
            else if (enemyHit)
            {
                soundHandler.EnemyHit();
            }
            weapon.GetComponent<WeaponScript>().PerformAnimation(attackOnX, attackUp, attackDown);
            soundHandler.SwingSword();
        }
    }
    void CheckEnemyInProximity()
    {
        float attackRradius = attackRadius;

        Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(rb.position, attackRradius * 2.5f, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            for (int i = 0; i < objectsToHit.Length; i++)
            {
                if (objectsToHit[i].GetComponent<BasicEnemy>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    PlaytroughVariables.TotalEnemyProxTime += Time.deltaTime;
                }
                else if (objectsToHit[i].GetComponent<FlyingEnemy>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    PlaytroughVariables.TotalEnemyProxTime += Time.deltaTime;
                }
                else if (objectsToHit[i].GetComponent<CannonEnemy>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    PlaytroughVariables.TotalEnemyProxTime += Time.deltaTime;
                }
                else if (objectsToHit[i].GetComponent<BulletScript>() != null && objectsToHit[i].tag == TagVariables.Enemy)
                {
                    PlaytroughVariables.TotalEnemyProxTime += Time.deltaTime;
                }
            }
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
                //rb.gravityScale = 0;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
                //rb.gravityScale = 0;
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
            isTurnedLeft = true;
        }
        else if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            isTurnedLeft = false;
        }
    }

    void StopJumpQuick()
    {
        //Stops The player jump immediately, causing them to start falling as soon as the button is released.
        stepsJumped = 0;
        pState.jumping = false;
        rb.velocity = new Vector2(rb.velocity.x / airMovementMod, 0);
    }

    void StopJumpSlow()
    {
        //stops the jump but lets the player hang in the air for a while.
        stepsJumped = 0;
        pState.jumping = false;
        rb.velocity = new Vector2(rb.velocity.x / airMovementMod, jumpSpeed);

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
            anim.SetBool(AnimationVariables.IsJumping, false);
            anim.SetTrigger(AnimationVariables.IsGrounded);
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
        float treshold = 0.1f;

        //This is essentially just sensitivity.
        if (yAxis > treshold)
        {
            yAxis = 1;
        }
        else if (yAxis < -treshold)
        {
            yAxis = -1;
        }
        else
        {
            yAxis = 0;
        }

        if (xAxis > treshold)
        {
            xAxis = 1;
        }
        else if (xAxis < -treshold)
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
        if (Input.GetButtonDown(InputButtons.Jump) && (Grounded() || pState.jumpedOnSpikes))
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

        if (Input.GetButton(InputButtons.Dash) && canDash)
        {
            pState.dashing = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, RuntimeVariables.PlayerAttackRadius);
        Gizmos.DrawWireSphere(downAttackTransform.position, RuntimeVariables.PlayerAttackRadiusDown);
        Gizmos.DrawWireSphere(upAttackTransform.position, RuntimeVariables.PlayerAttackRadiusUp);
        //Gizmos.DrawWireCube(groundTransform.position, new Vector2(groundCheckX, groundCheckY));

        Gizmos.DrawLine(groundTransform.position, groundTransform.position + new Vector3(0, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(-groundCheckX, 0), groundTransform.position + new Vector3(-groundCheckX, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(groundCheckX, 0), groundTransform.position + new Vector3(groundCheckX, -groundCheckY));

        Gizmos.DrawLine(roofTransform.position, roofTransform.position + new Vector3(0, roofCheckY));
        Gizmos.DrawLine(roofTransform.position + new Vector3(-roofCheckX, 0), roofTransform.position + new Vector3(-roofCheckX, roofCheckY));
        Gizmos.DrawLine(roofTransform.position + new Vector3(roofCheckX, 0), roofTransform.position + new Vector3(roofCheckX, roofCheckY));
    }
}
