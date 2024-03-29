using Assets.Constants;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    public GameController GameController;

    void Start()
    {
        GameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag(Tags.Ammunition)) return;

        if (other.gameObject.CompareTag(Tags.Wall)) 
        {
            Destroy(gameObject);
        } 
        else if (!other.gameObject.CompareTag(Tags.Player))
        {
            if (other.gameObject.CompareTag(Tags.Candy))
                GameController.DestroyScorePoint();
            Destroy(other.gameObject);
        }
    }
}
