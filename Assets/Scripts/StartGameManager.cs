using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    public GameObject startUI; // Start UI ��ü
    public GameObject player;  // ������ ������Ʈ

    void Start()
    {
        Time.timeScale = 0f; // ���� ���� (���� ������)
    }

    public void StartGame()
    {
        startUI.SetActive(false); // UI �����
        Time.timeScale = 1f; // ���� ����!
    }
}
