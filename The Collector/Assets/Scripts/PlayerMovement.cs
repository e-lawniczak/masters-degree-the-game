using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerLogic _logic;
    [SerializeField] private float _runSpeed;

    private float _hInput;
    private float _vInput;
    private bool _jump;
    private bool _crouch;

    private void Update()
    {
        if(!_logic.IsAlive ) return;
        _hInput = Input.GetAxisRaw("Horizontal") * _runSpeed;
        _vInput = Input.GetAxisRaw("Vertical");

        //_animator.SetFloat("Speed", Mathf.Abs(_hInput));

        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            //_animator.SetBool("Jump", _jump);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            _crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            _crouch = false;
        }
        //_animator.SetBool("Crouch", _crouch);

    }
    void FixedUpdate()
    {
      
        _characterController.Move(_hInput * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;
    }

    
    public void OnLanding()
    {
        //_animator.SetBool("Jump", false);
    }
}
