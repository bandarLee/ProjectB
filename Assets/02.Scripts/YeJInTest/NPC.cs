using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Animator _animator;

    public UI_Inventory InventoryUI;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            InventoryUI.Open();
        }
    }
}
