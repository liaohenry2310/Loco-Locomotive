using UnityEngine;

public class ParallaxScrollingBG : MonoBehaviour
{
    [Range(0f, 40)]
    public float scrollSpeed = 1f;
    public float scrollOffset;
    Vector3 startPos;
    float newPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * -scrollSpeed, scrollOffset);
        transform.position = startPos + Vector3.right * newPos;
    }
}
