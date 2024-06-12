using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Transform _canonHead, _shootBarrel;

    [SerializeField]
    private GameObject _projectile;

    private float distance;

    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private float _activeDistance;

    [SerializeField]
    private float _nextShoot;
    [SerializeField]
    private float _fireRate;
    void Start()
    {
        //_nextShoot = Time.time + 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(_player.position, transform.position);
        if(distance <= _activeDistance) 
        {
            _canonHead.LookAt(_player);

            if(Time.time >= _nextShoot) {
                _nextShoot = Time.time + 1f/_fireRate;
                Shoot();
                
            }
            
        }

    }

    void Shoot()
    {
        GameObject clone = Instantiate(_projectile, _shootBarrel.position, _canonHead.rotation);

        clone.GetComponent<Rigidbody>().AddForce(_canonHead.forward * _projectileSpeed);
        Destroy(clone, 3);
    }
}
