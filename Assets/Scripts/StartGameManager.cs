using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    public GameObject startUI; // Start UI 전체
    public GameObject player;  // 마리오 오브젝트

    void Start()
    {
        Time.timeScale = 0f; // 게임 정지 (시작 전까지)
    }

    public void StartGame()
    {
        startUI.SetActive(false); // UI 숨기기
        Time.timeScale = 1f; // 게임 시작!
    }
}
