using UnityEngine;
using UnityEngine.UI;

public class NextPhaseScreen : MonoBehaviour
{
    public Text LevelText;

    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        Setup(_gameController.Level);
    }

    public void Setup(int level)
    {
        LevelText.text = $"VOC� AVAN�OU PARA A FASE {level}";
    }

    public void Begin() => _gameController.BeginNextPhase();

    public void Exit() => _gameController.Exit();
}
