using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    public void Continue() => _gameController.Continue();

    public void Exit() => _gameController.Exit();
}
