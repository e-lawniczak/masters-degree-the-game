using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HelperFunctions;

public class PlayerController : MonoBehaviour
{

    private PlayerStateList pState;
    public PlayerStateList CurrentPlayerState { get { return pState; } }

    [Header("X Axis Movement")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float dashSpeed = 12f;

    [Space(5)]

    [Header("Y Axis Movement")]
    [SerializeField] private float jumpSpeed = 7f;
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

    //dashing
    private bool canDash = true;
    private float timeSinceDash = 0;
    private float dashCooldown = 1.5f;
    private float dashTime = 0.2f;


    private bool isTurnedLeft = true;
    private float timeSinceAttack;
    private float xAxis;
    private float yAxis;
    private float gravity;
    private int stepsXRecoiled;
    private int stepsYRecoiled;
    private int stepsJumped = 0;
    private int jumpMoveDivider = 1;
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
        Attack();
        CheckEnemyInProximity();
        CheckIfStandingStill();
        AnimationStates();
        DashUiInfo();

        if (pState.dashing) return;

        GetInputs();
        Jump();
        Flip();
    }
    private void FixedUpdate()
    {
        if (!playerLogic.IsAlive) return;
        if (pState.dashing) return;
        Move();
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

                if (objectsToHit.Select(o => o).Where(o => o.tag == LayerVariables.Hazards || o.tag == LayerVariables.EnemyTag).ToArray().Length > 0 && attackDown)
                {
                    pState.jumpedOnSpikes = true;
                    pState.canJumpAgain = true;
                    if (objectsToHit.Select(o => o).Where(o => o.name == LayerVariables.HazardsTriggerObj).ToArray().Length > 0)
                    {
                        spikeHit = true;
                    }
                }
            }
            for (int i = 0; i < objectsToHit.Length; i++)
            {
                if (objectsToHit[i].GetComponent<BasicEnemy>() != null && (objectsToHit[i].tag == TagVariables.Enemy))
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
                if ((objectsToHit[i].GetComponent<CannonEnemy>() != null) && objectsToHit[i].tag == TagVariables.Hazards)
                {
                    spikeHit = true;
                }
                if ((objectsToHit[i].GetComponent<BulletScript>() != null) && objectsToHit[i].tag == TagVariables.Hazards)
                {
                    objectsToHit[i].GetComponent<BulletScript>().GetHit();
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
    void AnimationStates()
    {
        if (pState.jumping)
        {
            anim.SetBool(AnimationVariables.IsJumping, true);
        }
        if (!pState.jumping)
        {
            anim.SetBool(AnimationVariables.IsJumping, false);
            anim.SetTrigger(AnimationVariables.IsGrounded);
        }
      
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
    void Jump()
    {
        if (Input.GetButtonDown(InputButtons.Jump) && (Grounded() || pState.canJumpAgain))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            pState.canJumpAgain = false;
        }

        if (Input.GetButtonUp(InputButtons.Jump) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (pState.jumpedOnSpikes)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed / 1.2f);
            pState.jumpedOnSpikes = false;
        }
        pState.jumping = !Grounded() || pState.canJumpAgain;
    }
    void Move()
    {
        if (xAxis != 0)
        {
            anim.SetBool(AnimationVariables.IsMoving, true);
        }
        else
        {
            anim.SetBool(AnimationVariables.IsMoving, false);
        }
        rb.velocity = new Vector2(xAxis * walkSpeed, rb.velocity.y);
    }



    void GetInputs()
    {
        yAxis = Input.GetAxisRaw("Vertical");
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown(InputButtons.Dash) && canDash)
        {
            StartCoroutine(Dash());
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
    private void DashUiInfo()
    {
        if(!canDash)
        {
            timeSinceDash += Time.deltaTime;
            if(timeSinceDash >= dashCooldown)
            {
                timeSinceDash = 0;
            }
        }
    }


    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetBool(AnimationVariables.IsDashing, true);
        float originalGrav = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        anim.SetBool(AnimationVariables.IsDashing, false);
        rb.gravityScale = originalGrav;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;



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


    }
}
