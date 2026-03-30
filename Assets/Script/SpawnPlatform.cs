using UnityEngine;
using System.Collections.Generic;

public class SpawnPlatform : MonoBehaviour
{
    [Header("프리팹 설정")]
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public GameObject[] groundObstaclePrefabs;
    public GameObject[] airObstaclePrefabs;

<<<<<<< HEAD
    [Header("배치 설정")]
    public int poolCount = 8;
    public float moveSpeed = 7f; // 속도를 조금 올렸습니다.
    public float platformWidth = 10f; // 플랫폼 길이를 조금 더 여유있게 설정 권장
    public float yPos = -3f;
    public float spawnX = 15f;
    public float despawnX = -15f;
    public float minGap = 1.0f;
    public float maxGap = 3.0f;

    private GameObject[] platformPool;
    private float lastRightEdge;

    // 코인/장애물 재사용을 위한 리스트 (간이 풀링)
    private List<GameObject> subObjectPool = new List<GameObject>();

    private void Start()
    {
        platformPool = new GameObject[poolCount];
        lastRightEdge = 0f;

        for (int i = 0; i < poolCount; i++)
        {
            platformPool[i] = Instantiate(platformPrefab, transform);
            platformPool[i].SetActive(false);
        }

        // 초기 배치
        for (int i = 0; i < poolCount; i++)
            PlacePlatform(i);
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver()) return;

        float rightmost = despawnX;

        for (int i = 0; i < poolCount; i++)
        {
            if (!platformPool[i].activeSelf) continue;

            // 이동
            platformPool[i].transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

            float right = platformPool[i].transform.position.x + platformWidth / 2f;

            if (right < despawnX)
                DeactivatePlatform(platformPool[i]);
            else if (right > rightmost)
                rightmost = right;
        }

        if (rightmost < spawnX)
        {
            lastRightEdge = rightmost;
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        for (int i = 0; i < poolCount; i++)
        {
            if (!platformPool[i].activeSelf)
            {
                PlacePlatform(i);
                return;
            }
        }
    }

    private void DeactivatePlatform(GameObject platform)
    {
        // 자식들(코인, 장애물)을 삭제하지 않고 비활성화 후 리스트에 보관
        foreach (Transform child in platform.transform)
        {
            child.gameObject.SetActive(false);
            subObjectPool.Add(child.gameObject);
        }
        platform.SetActive(false);
    }

    private void PlacePlatform(int index)
    {
        float gap = Random.Range(minGap, maxGap);
        float newX = lastRightEdge + gap + platformWidth / 2f;

        platformPool[index].transform.position = new Vector2(newX, yPos);
        platformPool[index].SetActive(true);
        lastRightEdge = newX + platformWidth / 2f;

        // 패턴 결정
        float rand = Random.value;
        if (rand < 0.15f) { /* 공백 구간 */ }
        else if (rand < 0.5f) SpawnCoinPattern(platformPool[index].transform);
        else SpawnObstacleWithCoins(platformPool[index].transform);
    }

    // 오브젝트 풀에서 꺼내오거나 생성하는 헬퍼 함수
    private GameObject GetSubObject(GameObject prefab, Transform parent)
    {
        foreach (GameObject obj in subObjectPool)
        {
            if (!obj.activeSelf && obj.name.Contains(prefab.name))
            {
                obj.transform.SetParent(parent);
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject newObj = Instantiate(prefab, parent);
        return newObj;
    }

    private void SpawnCoinPattern(Transform parent)
    {
        int count = 7;
        float spacing = platformWidth / (count + 1);
        bool isWave = Random.value > 0.5f;

        for (int i = 0; i < count; i++)
        {
            float x = -platformWidth / 2f + spacing * (i + 1);
            // Sin 함수를 이용한 물결 배치
            float y = isWave ? Mathf.Sin(i * 0.7f) + 1.5f : 1.2f;
            GetSubObject(coinPrefab, parent).transform.localPosition = new Vector3(x, y, 0);
        }
    }

    private void SpawnObstacleWithCoins(Transform parent)
    {
        // 0: 바닥 장애물, 1: 공중 장애물 (슬라이드 제외)
        int type = Random.Range(0, 2);
        GameObject[] prefabs;

        // type 0이면 ground, 아니면 air (슬라이딩은 사용하지 않음)
        prefabs = (type == 0) ? groundObstaclePrefabs : airObstaclePrefabs;

        if (prefabs == null || prefabs.Length == 0) return;

        // 장애물 높이 설정 (0: 바닥, 1: 공중)
        float obsY = (type == 0) ? 0.7f : 2f;
        GameObject obs = GetSubObject(prefabs[Random.Range(0, prefabs.Length)], parent);
        obs.transform.localPosition = new Vector2(0, obsY);

        // --- 점프 가이드 코인 배치 ---
        if (type == 0)
        {
            // 바닥 장애물: 플레이어가 넘어가도록 포물선(무지개) 모양 배치
            for (int i = -3; i <= 3; i++)
            {
                float x = i * 0.8f;
                // y = -ax^2 + h (포물선 공식)
                float y = -0.3f * Mathf.Pow(x, 2) + 3.2f;
                GetSubObject(coinPrefab, parent).transform.localPosition = new Vector2(x, y);
            }
        }
        else
        {
            // 공중 장애물: 플레이어가 아래로 지나가도록 낮은 위치에 일직선 배치
            int coinCount = 4;
            float coinSpacing = 1.5f;
            for (int i = 0; i < coinCount; i++)
            {
                float x = (i - (coinCount - 1) / 2f) * coinSpacing;
                GetSubObject(coinPrefab, parent).transform.localPosition = new Vector2(x, 0.5f);
=======
    private int count;
    public float width = 8;
    public float maxWidth = 15;

    public float ypos = -1f;
    public float startX = 0f;

    private GameObject[] platforms;
    private float lastPlatformX;

    private void Awake()
    {
        count = platformPrfaps.Length;
        platforms = new GameObject[count];

        lastPlatformX = startX;

        for (int i = 0; i < count; i++)
        {
            platforms[i] = Instantiate(platformPrfaps[i % platformPrfaps.Length], transform);

            float halfWidth = width / 2;

            Vector2 pos = new Vector2(lastPlatformX + halfWidth, ypos);
            platforms[i].transform.position = pos;

            lastPlatformX = pos.x + halfWidth;

            platforms[i].SetActive(true);
        }
    }

    void Update()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            // ⭐ 자식/단일 프리팹 모두 대응
            SpriteRenderer sr = platforms[i].GetComponentInChildren<SpriteRenderer>();

            // ⭐ 왼쪽 끝 기준
            float leftEdge = sr.bounds.min.x;

            if (leftEdge < -maxWidth)
            {
                // ⭐ 실제 크기 사용
                float halfWidth = sr.bounds.extents.x;

                Vector2 pos = new Vector2(lastPlatformX + halfWidth, ypos);

                // 👉 부모 이동
                platforms[i].transform.position = pos;

                lastPlatformX = pos.x + halfWidth;
>>>>>>> fe4fb878346be5dff825df8019e46c8e60b5ee38
            }
        }
    }
}