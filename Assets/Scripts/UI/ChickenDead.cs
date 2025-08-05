using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChickenDead : MonoBehaviour
{
    public static ChickenDead Instance; // 单例模式
    [SerializeField] private TextMeshProUGUI deadText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDeadMessage()
    {
        deadText.text = "You are bad!";
    }   
}
