using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AncientNPC 
{
    NPC1,
    NPC2,
    Blacksmith,
}
public class Scene2NPC : MonoBehaviour
{
    public Transform Player;
    public AncientNPC AncientNPC;

    private bool _openText = false;
    private bool _isTalking = false;

    private Animator _animator;

    public UI_Enforce EnforceUI;

    private bool _isEnforceUIOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.E))
            {
                if (AncientNPC == AncientNPC.NPC1 && !_openText)
                {
                    UI_Mission2.instance.NPC1MissionOpenText();
                    _openText = true;
                }
                else if (AncientNPC == AncientNPC.NPC2 && !_openText)
                {
                    Rotate();
                    if (!_isTalking)
                    {
                        _animator.SetTrigger("PlayerTalk");
                        _isTalking = true;
                    }
                    Scene2GameManager.instance.StopBlinking();
                    UI_Mission2.instance.NPC2MissionOpenText();
                    _openText = true;

                    Invoke("Return", 10f);
                }
                else if (AncientNPC == AncientNPC.Blacksmith) 
                {
                    if (_isEnforceUIOpen == true)
                    {
                        EnforceUI.EnforceOpen();
                        _isEnforceUIOpen = false;
                    }
                    else 
                    {
                        EnforceUI.EnforceClose();
                        _isEnforceUIOpen = true;
                    }
                    
                }
            }
        }
    }
    private void Return() 
    {
        _animator.SetTrigger("ExitTalk");
        _isTalking = false;
        Scene2GameManager.instance.StartBlinking2();
    }
    private void Rotate() 
    {
        transform.LookAt(Player);
    }
}
