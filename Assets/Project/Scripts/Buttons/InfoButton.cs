using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    private Button button;

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
        // zamanın akış hızı: (1 de normal oluyor) 
        Time.timeScale = 0;
        print("İnfo");
    }
}
