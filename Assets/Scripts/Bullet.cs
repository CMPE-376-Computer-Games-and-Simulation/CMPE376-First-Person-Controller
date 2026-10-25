using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private float _maxDistance;
    [SerializeField]
    private float _bulletSpeed = 10f;
   
   private BulletPool _bulletPool;
    
    private Rigidbody _rb;
    private Vector3 _startPosition;
    
    private void Awake()
    {
        _bulletPool = transform.parent.GetComponent<BulletPool>();
        _rb = GetComponent<Rigidbody>();
    }
    
    void OnEnable()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = transform.position - _startPosition;
        if (dist.sqrMagnitude > _maxDistance * _maxDistance)
        {
            ReturnBullet(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.forward * _bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Damagable")
        {
            Destroy(other.gameObject);
            ReturnBullet(gameObject);
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReturnBullet(gameObject);
    }

    private void ReturnBullet(GameObject bullet)
    {
        _bulletPool.ReturnBullet(gameObject);
    }
}
