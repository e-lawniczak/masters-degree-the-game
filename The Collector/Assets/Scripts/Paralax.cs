using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Paralax : MonoBehaviour
{
    private float _length;
    private float _startPos;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private float _paralaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float movedDist = _camera.transform.position.x * (1 - _paralaxEffect);
        float dist = _camera.transform.position.x * _paralaxEffect;
        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        if (movedDist > _startPos + _length) _startPos += _length;
        else if (movedDist < _startPos - _length) _startPos -= _length;
    }
}
