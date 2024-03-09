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

    private PlayerLogic playerLogic;
    private void Start()
    {
        playerLogic = this.GetComponentInParent<PlayerLogic>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerLogic.IsAlive) return;

        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        float isPlayerFlipped = playerTransform.localScale.x < 0 ? -1 : 1;
        float angle = yAxis < 0 ? -90 * isPlayerFlipped : yAxis > 0 ? 90 * isPlayerFlipped : 0;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        //if (Input.GetButtonDown(InputButtons.Swing))
        //{
        //    Attack();
        //}
    }

    private void Attack()
    {
        blade.GetComponent<SpriteRenderer>().enabled = true;
        animator.SetTrigger(AnimationVariables.AttackTrigger);
    }

    internal void PerformAnimation(bool attackOnX, bool attackUp, bool attackDown)
    {
        Attack();
    }
}
