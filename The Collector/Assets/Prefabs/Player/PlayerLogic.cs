using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public bool IsAlive { get { return _isAlive; } }

    [SerializeField] private CapsuleCollider2D _collider;
    private bool _isAlive;
    private List<LayerMask> _hazardLayers;

    private void Start()
    {
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
        {
            if (_collider.IsTouchingLayers(LayerMask.GetMask(LayerVariables.Hazards, LayerVariables.Enemy)))
            {
                Debug.Log("dead");
                //_isAlive = false; break;
            }
        }
    }
    private void Die()
    {

    }
}
