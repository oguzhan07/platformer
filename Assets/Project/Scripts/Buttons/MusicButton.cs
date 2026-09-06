using System;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(Music);
    }

    void Music()
    {
        print("Müzik");
    }
}