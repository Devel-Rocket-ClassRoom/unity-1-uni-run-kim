using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 연결")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;        // 게임 중 상단에 뜨는 점수
    public TextMeshProUGUI finalScoreText;   // 게임오버 창에 뜨는 최종 점수

    [Header("체력 수치 설정")]
    public float maxHp = 100f;
    public float hpDeclineSpeed = 5f;

    private float currentHp;
    private float score = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHp = maxHp;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 게임 시작 시 점수 UI 보이기
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R)) Restart();
            return;
        }

        currentHp -= hpDeclineSpeed * Time.deltaTime;

        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score);

        if (currentHp <= 0)
        {
            currentHp = 0;
            GameOver();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
    }

    public void RestoreHp(float amount)
    {
        if (isGameOver) return;
        currentHp = Mathf.Min(currentHp + amount, maxHp);
    }

    public void TakeDamage(float amount)
    {
        if (isGameOver) return;
        currentHp -= amount;
        if (currentHp <= 0) GameOver();
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 1. 플레이어 죽음 처리
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.Die();

        // 2. 현재 점수 UI 숨기기 (핵심 수정 부분)
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        // 3. 게임오버 패널 띄우기 및 최종 점수 입력
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score : " + Mathf.FloorToInt(score);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsGameOver() => isGameOver;
}