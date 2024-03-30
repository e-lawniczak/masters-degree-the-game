using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public bool IsAlive { get { return _isAlive; } }

    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private int _maxHp = 5;
    private Animator _animator;
    private Rigidbody2D rb;
    private bool _isAlive;
    private List<LayerMask> _hazardLayers;
    private int _hp;
    private bool _isInvincible;
    private float _iframeTime = 1.5f;
    private float _count = 0f;

    private void Start()
    {
        _isAlive = true;
        _hp = _maxHp;
        _isInvincible = false;
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isAlive)
        {
            CalculateDamage();
        }
        if (_isInvincible)
        {
            _count += Time.deltaTime;
            if (_count >= _iframeTime)
            {
                _count = 0;
                _isInvincible = false;
                _animator.SetTrigger(AnimationVariables.EndInvincible);

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
        DealDamage(1);
        GiveIframes();
        Recoil();
    }
    private void Die()
    {
        Debug.Log("Dead");
    }
    public void Recoil(Vector2? recoildVec = null)
    {
        Vector2 recoil = recoildVec.HasValue ? recoildVec.Value : new Vector2(0, 1800f);
        Debug.Log(recoil);
        rb.AddForce(recoil);

    }
    private void GiveIframes()
    {
        _isInvincible = true;
        _animator.SetTrigger(AnimationVariables.IsInvincible);
    }

    public void DealDamage(int damage)
    {
        _hp -= damage;
    }

    public int GetCurrentHp()
    {
        return _hp;
    }
}
