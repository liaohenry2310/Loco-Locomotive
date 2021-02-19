using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    //Editor fields
    [SerializeField] private State CurrentState = State.Opened;

    [SerializeField] private float transitionDuration = 0.0f;
    [SerializeField] private float middlePosition = 0.0f;
    [SerializeField] private float topPosition = 0.0f;
    [SerializeField] private float bottomPosition = 0.0f;

    [SerializeField] RectTransform topImageTransform = null;
    [SerializeField] RectTransform bottomImageTransform = null;

    [SerializeField] AudioSource transitionSfx = null;

    //Public Members
    public float Duration { get => transitionDuration; }

    public void StartTransition()
    {
        if (_isTransitioning)
            return;

        if (!topImageTransform && !bottomImageTransform && !transitionSfx)
            Debug.LogError("Scene Transition: Missing references");

        switch (CurrentState)
        {
            case State.Opened:
                StartCoroutine(ClosingTransition(transitionDuration));
                transitionSfx.Play();
                break;
            case State.Closed:
                StartCoroutine(OpeningTransition(transitionDuration));
                transitionSfx.Play();
                break;
        }
    }

    //Private Members
    private enum State
    {
        Opened,
        Closed
    }

    private bool _isTransitioning = false;

    private IEnumerator OpeningTransition(float duration)
    {
        _isTransitioning = true;

        float startTime = Time.unscaledTime;
        float t = 0.0f;

        while(t <= 1.0f)
        {
            t = (Time.unscaledTime - startTime) / duration;

            topImageTransform.anchoredPosition = Vector3.Lerp(new Vector3(0.0f, middlePosition, 0.0f), new Vector3(0.0f, topPosition, 0.0f), t);
            bottomImageTransform.anchoredPosition = Vector3.Lerp(new Vector3(0.0f, middlePosition, 0.0f), new Vector3(0.0f, bottomPosition, 0.0f), t);

            yield return null;
        }

        CurrentState = State.Opened;
        _isTransitioning = false;
    }

    private IEnumerator ClosingTransition(float duration)
    {
        _isTransitioning = true;

        float startTime = Time.unscaledTime;
        float t = 0.0f;

        while (t <= 1.0f)
        {
            t = (Time.unscaledTime - startTime) / duration;

            topImageTransform.anchoredPosition = Vector3.Lerp(new Vector3(0.0f, topPosition, 0.0f), new Vector3(0.0f, middlePosition, 0.0f), t);
            bottomImageTransform.anchoredPosition = Vector3.Lerp(new Vector3(0.0f, bottomPosition, 0.0f), new Vector3(0.0f, middlePosition, 0.0f), t);

            yield return null;
        }

        CurrentState = State.Closed;
        _isTransitioning = false;
    }
}
