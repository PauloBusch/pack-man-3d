using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text PointsText;

    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        Setup(_gameController.Score);
    }

    public void Setup(int score)
    {
        PointsText.text = $"{score} PONTOS";
    }

    public void Restart() => _gameController.RestartPhase();

    public void Exit() => _gameController.Exit();
}
