using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public bool IsAlive { get { return _isAlive; } }

    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private CircleCollider2D _circleCollider;
    private bool _isAlive;
    private List<Collider2D> _colliders;
    private List<LayerMask> _hazardLayers;

    private void Start()
    {
        _colliders = new List<Collider2D> { _boxCollider, _circleCollider };
        _isAlive = true;
    }

    void Update()
    {
        if (_isAlive)
        {
            CalculateDamage();
        }
    }

    private void CalculateDamage()
    {
        foreach (var collider in _colliders)
        {
            if (collider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy")))
            {
                Debug.Log("dead");
                _isAlive = false; break;
            }
        }
    }
    private void Die()
    {

    }
}
