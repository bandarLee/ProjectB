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
    private bool _isTurning = false;
    public NPCType _NPCType;

    public UI_Inventory InventoryUI;


    void Start()
    {
        _animator = GetComponent<Animator>();

        _targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {

        //Debug.Log(Vector3.Distance(_targetPlayer.position, transform.position));
        if (Vector3.Distance(_targetPlayer.position, transform.position) < FindDistance)
        {
            _isTurning = true;
            _animator.SetTrigger("Turn");
            StartCoroutine(Quest_Coroutine());
        }
        else if (_isTurning && (Vector3.Distance(_targetPlayer.position, transform.position) > FindDistance))
        {
            _animator.SetTrigger("LeftTurn");
            _isTurning = false;
        }
    }

    private IEnumerator Quest_Coroutine() 
    {
        yield return new WaitForSeconds(2f);
        InventoryUI.Open();
    }
}

