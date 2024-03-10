using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 500f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject blade;
    [SerializeField] private int weaponDamage = 1;

    private PlayerLogic playerLogic;
    private bool _canAttackDown;
    private Vector3 initialPos;
    private float yAxis;
    private float xAxis;
    private int isPlayerFlipped;

    private void Start()
    {
        initialPos = transform.localPosition;
        playerLogic = this.GetComponentInParent<PlayerLogic>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerLogic.IsAlive) return;

        yAxis = Input.GetAxis("Vertical");
        xAxis = Input.GetAxis("Horizontal");

        isPlayerFlipped = playerTransform.localScale.x < 0 ? -1 : 1;
        float angle = yAxis < 0 ? -90 * isPlayerFlipped : yAxis > 0 ? 90 * isPlayerFlipped : 0;
        angle = yAxis < 0 && !_canAttackDown ? 0 : angle;
        float transformAnimation = angle == -90 ? -0.5f : 0;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        blade.GetComponent<SpriteRenderer>().enabled = true;
        if (_canAttackDown && yAxis < 0)
        {
            transform.localPosition += new Vector3(0.2f, 0.8f, 0);

        }
        animator.SetTrigger(AnimationVariables.AttackTrigger);
    }

    internal void PerformAnimation(bool attackOnX, bool attackUp, bool attackDown)
    {
        _canAttackDown = attackDown;
        Attack();
    }

    internal int GetDamage()
    {
        return weaponDamage;
    }

    internal void ResetTransform()
    {
        transform.localPosition = initialPos;
    }
}
