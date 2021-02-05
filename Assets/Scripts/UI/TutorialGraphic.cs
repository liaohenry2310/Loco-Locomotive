using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TutorialGraphic : MonoBehaviour
{
    public float animationDuration = 0.05f;
    public UnityEvent onTutorialFinished;
    public UnityEvent onGraphicShow;
    public InputAction confirm;

    List<RectTransform> images;
    int currentImage = 0;
    bool imageLock = true;

    public void StartTutorial()
    {
        images = new List<RectTransform>(GetComponentsInChildren<RectTransform>(true));
        images.RemoveAt(0);

        confirm.performed += (InputAction.CallbackContext ctx) => { NextGraphic(); };
        confirm.Enable();

        if (images.Count > 0)
        {
            StartCoroutine(ShowGraphic());
        }
    }

    private void NextGraphic()
    {
        if (!imageLock)
        {
            onGraphicShow.Invoke();
            if (currentImage < images.Count - 1)
            {
                images[currentImage++].gameObject.SetActive(false);
                StartCoroutine(ShowGraphic());
            }
            else
            {
                images[currentImage].gameObject.SetActive(false);
                onTutorialFinished.Invoke();
                confirm.Disable();
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ShowGraphic()
    {
        if (currentImage == 0)
        {
            yield return new WaitForSeconds(1.0f);
        }

        imageLock = true;
        images[currentImage].gameObject.SetActive(true);
        RectTransform image = images[currentImage];
        image.localScale = new Vector2(0.0f, 0.0f);

        Vector2 begin = new Vector2(0.0f, 0.0f);
        Vector2 end = new Vector2(1.0f, 1.0f);

        float startTime = Time.unscaledTime;
        float t = 0.0f;

        while (t <= 1.0f)
        {
            t = (Time.unscaledTime - startTime) / (animationDuration);
            image.localScale = Vector2.Lerp(begin, end, t);
            yield return null;
        }

        imageLock = false;
    }
}
