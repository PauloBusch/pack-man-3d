using UnityEngine;
using UnityEngine.UI;

public class OverlayScreen : MonoBehaviour
{
    public Image[] Lifes;
    public Text LevelText;
    public Text PointsText;
    public Text ChasingText;
    public Text AmmunitionText;
    public GridLayoutGroup ChasingGrid;

    public void UpdateLevel(int level) => LevelText.text = $"Fase {level}";
    public void UpdateScore(int score) => PointsText.text = $"Placar: {score}";
    public void UpdateAmmunition(int count) => AmmunitionText.text = $"Minuição: {count}";

    public void ToggleHeart(int index, bool enable) => Lifes[index].enabled = enable;

    public void ToggleChase(bool enable)
    {
        ChasingText.enabled = enable;
        if (!enable) {
            for (var index = 0; index < ChasingGrid.transform.childCount; index++)
                Destroy(ChasingGrid.transform.GetChild(index).gameObject);
        }
    }

    public void AddChaseAvatar(Image avatarPrefab) => Instantiate(avatarPrefab, ChasingGrid.transform);
}
