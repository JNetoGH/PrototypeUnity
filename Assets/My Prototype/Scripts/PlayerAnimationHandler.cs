using System;
using UnityEngine;


public class PlayerAnimationHandler: MonoBehaviour {
    
    private Animator _animator;
    private PlayerController _controllerSingleton;
    
    private void Start() {
        _animator = GetComponent<Animator>();
        _controllerSingleton = PlayerController.instance;
    }

    private void Update() {

        if (_controllerSingleton.IsMovingInX) {
            _animator.SetBool("IsRunning", true);
        }
        else {
            _animator.SetBool("IsRunning", false);
        }
    }
}
