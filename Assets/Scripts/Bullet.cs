using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private float _maxDistance;
    [SerializeField]
    private float _bulletSpeed = 10f;
    private Rigidbody _rb;
    private Vector3 _startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         
        Vector3 dist = transform.position - _startPosition;
        if (dist.sqrMagnitude > _maxDistance * _maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.forward * _bulletSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Damagable")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
