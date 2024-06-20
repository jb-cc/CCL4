using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinJump : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0.8f;
    }


}