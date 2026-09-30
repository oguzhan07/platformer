using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private ReloadScene reloadScene;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(reloadScene.RestartScene);
    }
}
