using UnityEngine;

public class ParallaxScrollingBG : MonoBehaviour
{
    [Range(0f, 40)]
    public float scrollSpeed = 1f;
    public float scrollOffset;
    private Vector3 _startPos;
    private float _newPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        _newPos = Mathf.Repeat(Time.time * -scrollSpeed, scrollOffset);
        transform.localPosition = _startPos + (Vector3.right * _newPos);
    }
}
