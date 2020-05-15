using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollower : MonoBehaviour
{
    private Collider _colliderFollower;
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _sensitivity = 100f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    public void SetFollowTarget(Collider collider) 
    {
        _colliderFollower = collider;
    }

    private void FixedUpdate()
    {
        Vector3 destination = _colliderFollower.transform.position;
        _rigidbody.transform.rotation = transform.rotation;
        _rigidbody.velocity = (destination - _rigidbody.transform.position) * _sensitivity;
    }
}
