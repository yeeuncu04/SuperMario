using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    public GameObject gameOverUI;

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // 게임 정지
    }
}
