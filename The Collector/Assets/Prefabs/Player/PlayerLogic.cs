using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static HelperFunctions;

public class PlayerLogic : MonoBehaviour
{
    public bool IsAlive { get { return _isAlive; } }

    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private GameEngine engine;
    [SerializeField] private int _maxHp = 5;
    private Animator _animator;
    private Rigidbody2D rb;
    private bool _isAlive;
    private List<LayerMask> _hazardLayers;
    private int _hp;
    private bool _isInvincible;
    private float _iframeTime = 2.5f;
    private float _count = 0f;
    private int points;
    private float iframeTick = 0.25f;
    private float iframeCount = 0f;

    private void Start()
    {
        _isAlive = true;
        _hp = _maxHp;
        _isInvincible = false;
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        points = PlaytroughVariables.TotalPoints;
        if (RuntimeVariables.CurrentHp > -1 && _hp != RuntimeVariables.CurrentHp)
        {
            SetHp(RuntimeVariables.CurrentHp);
        }

    }

    void Update()
    {
        if (!_isAlive) { return; }
        if (_isAlive)
        {
            CalculateDamage();
        }
        if (_isInvincible)
        {
            _count += Time.deltaTime;
            iframeCount += Time.deltaTime;
            if (iframeCount >= iframeTick)
            {
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                iframeCount = 0;
            }

            if (_count >= _iframeTime)
            {
                _count = 0;
                _isInvincible = false;
                //_animator.SetTrigger(AnimationVariables.EndInvincible);
                GetComponent<SpriteRenderer>().enabled = true;

            }
        }
    }

    private void CalculateDamage()
    {
        if (!_isInvincible && _collider.IsTouchingLayers(LayerMask.GetMask(LayerVariables.Hazards)))
        {
            DealDamage(1);
            GiveIframes();
            Recoil();
        }
        if (_hp <= 0)
        {
            _isAlive = false;
            Die();
        }
    }
    public void GetHitByEnemy(GameObject enemy, Vector3 enemyPos)
    {
        if (!_isInvincible)
        {
            GiveIframes();
            DealDamage(1);
            Recoil();
        }
    }
    private void Die()
    {
        //Destroy(this, 0.0f);
        _animator.SetBool(AnimationVariables.IsDead, true);
        rb.velocity = Vector3.zero;
        PlaytroughVariables.Deaths += 1;
    }
    public void Recoil(Vector2? recoildVec = null)
    {
        Vector2 recoil = recoildVec.HasValue ? recoildVec.Value : new Vector2(0, 1800f);
        rb.AddForce(recoil);

    }
    private void GiveIframes()
    {
        if (_hp > 0)
            _isInvincible = true;
        //_animator.SetTrigger(AnimationVariables.IsInvincible);
    }

    public void DealDamage(int damage)
    {
        _hp -= damage;
    }

    public int GetCurrentHp()
    {
        return _hp;
    }
    public void SetHp(int hp)
    {
        _hp = hp;
    }
    public DashInfo GetCanDash()
    {
        return GetComponent<PlayerController>().DashInfo;
    }
    public void AddPoints(int p = 1)
    {
        PlaytroughVariables.TotalPoints += p;
        RuntimeVariables.CurrentLevelPoints += p;
    }
    public int GetPoints()
    {
        return points;
    }



}
