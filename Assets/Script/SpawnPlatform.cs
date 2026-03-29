using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public GameObject[] platformPrfaps;

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
            }
        }
    }
}