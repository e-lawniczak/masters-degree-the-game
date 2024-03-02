using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private float _runSpeed;

    private float _hInput;
    private float _vInput;
    private bool _jump;
    private bool _crouch;

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal") * _runSpeed;
        _vInput = Input.GetAxisRaw("Vertical");

        if (_vInput > 0)
        {
            _jump = true;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            _crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            _crouch = false;
        }
    }
    void FixedUpdate()
    {
        _characterController.Move(_hInput * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;
    }
}
