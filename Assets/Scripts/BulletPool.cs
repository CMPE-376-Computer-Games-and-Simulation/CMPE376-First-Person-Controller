using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _poolSize;
    Queue<GameObject> _bulletPool = new Queue<GameObject>();
    
    void Start()
    {
        FillThePool();
    }


    private void FillThePool()
    {
        _bulletPool.Clear();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity, transform);
            ReturnBullet(bullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        if (_bulletPool.Count <= 0)
            return null;
        GameObject bullet = _bulletPool.Dequeue();
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}
