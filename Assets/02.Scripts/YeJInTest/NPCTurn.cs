using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTurn : MonoBehaviour
{
    private Animator _animator;

    private Transform _targetPlayer;
    private float FindDistance = 5f;
  
    void Start()
    {
        _animator = GetComponent<Animator>();

        _targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(_targetPlayer.position, transform.position) <= FindDistance) 
        {
            
            _animator.SetTrigger("Turn");
        }
    }
}

