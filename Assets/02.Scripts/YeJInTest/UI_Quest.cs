using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : MonoBehaviour
{
    public TextMeshProUGUI QuestTextUI;

    private void Start()
    {
        QuestTextUI.text = string.Empty;
    }
    private void Update()
    {
        QuestTextUI.text = "안녕";
    }
}
