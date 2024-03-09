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
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject blade;
    [SerializeField] private CharacterController characterController;

    private PolygonCollider2D hitTrigger;
    private PlayerLogic playerLogic;
    private float _vInput;
    private void Start()
    {
        hitTrigger = this.GetComponent<PolygonCollider2D>();
        playerLogic = this.GetComponentInParent<PlayerLogic>();
        hitTrigger.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerLogic.IsAlive) return;

        _vInput = Input.GetAxisRaw("Vertical");

        float isPlayerFlipped = playerTransform.localScale.x < 0 ? -1 : 1;


        Vector2 diretion = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        float angle = _vInput < 0 ? -90 * isPlayerFlipped : _vInput > 0 ? 90 * isPlayerFlipped : 0;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);


        if (Input.GetButtonDown(InputButtons.Swing))
        {
            Attack();
        }
    }

    private void Attack()
    {
        hitTrigger.enabled = true;
        blade.GetComponent<SpriteRenderer>().enabled = true;
        animator.SetTrigger(AnimationVariables.AttackTrigger);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == LayerVariables.Hazards)
        {
            characterController.BumpOnObstacle();
        }
    }
    public void DisableTrigger()
    {
        hitTrigger.enabled = false;
    }
}
