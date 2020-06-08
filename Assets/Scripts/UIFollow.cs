using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour
{
    public RectTransform rectBloodPos;
    public Vector3 offset;

    void Update()
    {
        Vector2 vec2 = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + offset);
        rectBloodPos.anchoredPosition = new Vector2(vec2.x - Screen.width / 2 + 0, vec2.y - Screen.height / 2 + 60);
    }

}
