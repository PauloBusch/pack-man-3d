using Assets.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public void Begin() => SceneManager.LoadScene(Scenes.Game);

    public void Exit() => Application.Quit();
}
