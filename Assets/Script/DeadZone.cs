using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Transform player;        // 플레이어 트랜스폼 연결
    public float offset = -10f;     // 플레이어로부터의 거리 (화면 왼쪽 끝 정도)
    public float followSpeed = 5f;  // 따라오는 부드러움 (값이 클수록 즉각 반응)

    void Update()
    {
        // 플레이어가 없거나 게임오버면 멈춤
        if (player == null || (GameManager.Instance != null && GameManager.Instance.IsGameOver()))
            return;

        // 1. 목표 위치 계산 (플레이어의 X값 + 오프셋)
        float targetX = player.position.x + offset;

        // 2. 현재 위치에서 목표 위치로 부드럽게 이동 (Lerp 사용)
        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * followSpeed);

        // 3. 위치 적용 (Y값은 기존 데드존 위치 유지)
        transform.position = new Vector3(newX, transform.position.y, 0);
    }
}