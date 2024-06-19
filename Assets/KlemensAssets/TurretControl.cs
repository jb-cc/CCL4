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
    [SerializeField] ForceMode _forceMode;


    [SerializeField] private float xMaxRotation = 30f;
    [SerializeField] private float xMinRotation = -60f;
    [SerializeField] private float yMaxRotation = 200f;
    [SerializeField] private float yMinRotation = -50f;

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
            //LimitRotation();

            if(Time.time >= _nextShoot) {
                _nextShoot = Time.time + 1f/_fireRate;
                Shoot();
                
            }
            
        }

    }

    void LimitRotation()
    {
        Vector3 canonEulerAngles = _canonHead.rotation.eulerAngles;

        //canonEulerAngles.y = (canonEulerAngles.y > 180) ? canonEulerAngles.y - 360 : canonEulerAngles.y;
        //canonEulerAngles.x = (canonEulerAngles.x > 180) ? canonEulerAngles.x - 360 : canonEulerAngles.x;

        
        

        //canonEulerAngles.y = Mathf.Clamp(canonEulerAngles.y,yMinRotation,yMaxRotation);
        //canonEulerAngles.x = Mathf.Clamp(canonEulerAngles.x, xMinRotation, xMaxRotation);
        Debug.Log("" + canonEulerAngles.y);

        _canonHead.rotation = Quaternion.Euler(canonEulerAngles);

    }

    void Shoot()
    {
        GameObject clone = Instantiate(_projectile, _shootBarrel.position, _canonHead.rotation);

        clone.GetComponent<Rigidbody>().AddForce(_canonHead.forward * _projectileSpeed, _forceMode);
        Destroy(clone, 3);
    }
}
