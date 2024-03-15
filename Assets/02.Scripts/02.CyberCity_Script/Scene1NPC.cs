using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CyberNPC 
{
    NPC1,
    NPC2,
    Blacksmith,
}
public class Scene1NPC : MonoBehaviour
{
    public Transform Player;
    public CyberNPC CyberNPC;

    private bool _isOpenText;
    private bool _isTalking = false;
    private Animator _animator;



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
                if (CyberNPC == CyberNPC.NPC1 && !_isOpenText)
                {
                    Rotate();

                    if (!_isTalking) 
                    {
                        _animator.SetTrigger("Talking");
                        _isTalking = true;
                    }
                   
                    UI_Mission1.instance.NPC1MissionOpenText();
                    _isOpenText = true;
                    Invoke("Return", 10f);
                }
                else if (CyberNPC == CyberNPC.NPC2 && !_isOpenText)
                { 
                    Rotate();
                    if (!_isTalking) 
                    {
                        _animator.SetTrigger("Talking");
                        _isTalking = true;  
                    }
                    UI_Mission1.instance.NPC2MissionOpenText();
                    
                    _isOpenText = true;
                    Invoke("Return", 10f);
                }
                else if (CyberNPC == CyberNPC.Blacksmith) 
                {
                    
                }
            }
        }
    }
    private void Return()
    {
        _animator.SetTrigger("ExitTalking");
        _isTalking = false;
        Scene1GameManager.instance.StartBlinking();
    }
    private void Rotate()
    {
        transform.LookAt(Player);
    }
   
}
