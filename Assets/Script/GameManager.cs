using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 연결")]
    public Image hpFillImage;        // HP_Fill 이미지를 여기에 드래그
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    [Header("체력 설정")]
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
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R)) Restart();
            return;
        }

        // 1. 체력 자연 감소 및 UI 반영
        currentHp -= hpDeclineSpeed * Time.deltaTime;
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = currentHp / maxHp; // 비율 계산 (0~1)
        }

        // 2. 점수 업데이트
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score);

        // 3. 사망 체크
        if (currentHp <= 0)
        {
            currentHp = 0;
            GameOver();
        }
    }

    public void AddScore(int amount) { if (!isGameOver) score += amount; }
    public void RestoreHp(float amount) { if (!isGameOver) currentHp = Mathf.Min(currentHp + amount, maxHp); }
    public void TakeDamage(float amount) { if (!isGameOver) currentHp -= amount; }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.Die();

        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = "Final Score : " + Mathf.FloorToInt(score);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsGameOver() => isGameOver;
}