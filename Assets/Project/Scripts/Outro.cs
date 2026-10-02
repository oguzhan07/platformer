using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Outro : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button reloadButton;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.image.DOFade(1, 0.5f);
        text.DOFade(1, 0.5f);
    }

    private void Update()
    {
        button.onClick.AddListener(Quit);
        reloadButton.onClick.AddListener(Reload);
    }

    private void Quit()
    {
        Application.Quit();
        print("kapatıldı");
    }

    private void Reload()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }
}
