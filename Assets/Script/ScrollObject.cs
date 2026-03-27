using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.left * speed * Time.deltaTime;

        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

    }
}
