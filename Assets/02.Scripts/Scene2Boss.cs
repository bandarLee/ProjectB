using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scene2Boss : MonoBehaviour
{
    public float health = 200;

    public float attackDelay = 0f;
    private float lastAttackTime = 0f;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;
    GameObject player;
    private Animator animator;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
