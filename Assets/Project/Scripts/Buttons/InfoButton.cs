using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    private Button button;
    private bool isPressedButton;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(Settings);
    }

    void Settings()
    {
        if (!isPressedButton)
        {
            isPressedButton = true;
            Time.timeScale = 0;
        }
        else if (isPressedButton)
        {
            isPressedButton = false;
            Time.timeScale = 1;
        }
        
        
    }
}
