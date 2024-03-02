using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowingCamera : MonoBehaviour
{

    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private float distance = 0;
    void Update()
    {
     

        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, transform.position.z - distance);
    }
}
