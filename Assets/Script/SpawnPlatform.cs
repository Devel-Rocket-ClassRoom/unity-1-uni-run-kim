using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public GameObject[] platformPrfaps;

    public int count = 4;
    public float width = 8;

    private float lastPlatformX;
    public float maxWidth = 15;

    public float ypos = -1f;
    public float xPos = 8f;

    private GameObject[] platforms;
    private int currentIndex = 0;


    private void Awake()
    {
        platforms = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            platforms[i] = Instantiate(platformPrfaps[i], transform);
            platforms[i].SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (platforms[currentIndex].transform.position.x > Mathf.Abs(maxWidth))
        {

            Vector2 pos;
            pos.x = xPos;
            pos.y = ypos;
            platforms[currentIndex].transform.position = pos;

            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            lastPlatformX = pos.x;

            currentIndex = (int)Mathf.Repeat(currentIndex + 1, platforms.Length);
        }


        //if (Time.time >= LastSpawnTime + timeSpawn)
        //{
        //    LastSpawnTime = Time.time;
        //    timeSpawn = Random.Range(spawnTimeRange.x, spawnTimeRange.y);

        //    Vector2 pos;
        //    pos.x = lastPlatformX + platformWidth/2;
        //    pos.y = ypos;
        //    platforms[currentIndex].transform.position = pos;

        //    platforms[currentIndex].SetActive(false);
        //    platforms[currentIndex].SetActive(true);

        //    lastPlatformX = pos.x;

        //    currentIndex = (int)Mathf.Repeat(currentIndex + 1, platforms.Length);
        //}
    }
}
