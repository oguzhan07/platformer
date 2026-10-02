using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    private string currentScene;
    [SerializeField] private Image blackScreen;

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ChangeScene(string scene)
    {
        if (currentScene != scene)
        {
            currentScene = scene;
            blackScreen.DOFade(1f, 1f).OnComplete(() =>
            {
                SceneManager.LoadScene(currentScene);
            });
        }
    }
}
