using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType 
{
    QuestNPC,
    MerchantNPC,
    BlacksmithNPC,
}
public class NPCTurn : MonoBehaviour
{
    private Animator _animator;

    private Transform _targetPlayer;
    private float FindDistance = 5f;

    public NPCType _NPCType;

    
  
    void Start()
    {
        _animator = GetComponent<Animator>();

        _targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_NPCType == NPCType.QuestNPC && Vector3.Distance(_targetPlayer.position, transform.position) <= FindDistance)
        {
            _animator.SetTrigger("Turn");
        }
        else if (_NPCType == NPCType.MerchantNPC && Input.GetKeyDown(KeyCode.E)) 
        {
        
        }
    }
}

