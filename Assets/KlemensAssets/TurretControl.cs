using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform _player;

    [SerializeField] private GameObject _canonierEnemy;

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
            _canonierEnemy.transform.LookAt(_player);
            LimitRotation();

            if(Time.time >= _nextShoot) {
                _nextShoot = Time.time + 1f/_fireRate;
                Shoot();
                
            }
            
        }

    }

    void LimitRotation()
    {
        Vector3 canonEulerAngles = _canonHead.rotation.eulerAngles;
        //Debug.Log("test" + canonEulerAngles.y);
        //240 300
        

        if (canonEulerAngles.y > 240 && canonEulerAngles.y < 300)
        {
            //Debug.Log("between" + canonEulerAngles.y);

            if (canonEulerAngles.y <= 270)
            {
                canonEulerAngles.y = 239.9f;
                //Debug.Log("" + canonEulerAngles.y);
            }
            else
            {
                canonEulerAngles.y = 300.1f;
                //Debug.Log("" + canonEulerAngles.y);
            }
        }
        

        _canonHead.rotation = Quaternion.Euler(canonEulerAngles);

    }

    void Shoot()
    {
        AkSoundEngine.PostEvent("Play_CanonShot", gameObject);
        GameObject clone = Instantiate(_projectile, _shootBarrel.position, _canonHead.rotation);
        
        clone.GetComponent<Rigidbody>().AddForce(_canonHead.forward * _projectileSpeed, _forceMode);
        Destroy(clone, 3);
    }
}
