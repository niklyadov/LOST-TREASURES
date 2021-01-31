using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fading : MonoBehaviour
{
    public float DelayInSeconds = 1;
    public float FadingSpeed = 1;

    private float _time;
    private CanvasGroup _canvasGroup;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= DelayInSeconds)
            if (_canvasGroup.alpha > 0)
                _canvasGroup.alpha = _canvasGroup.alpha - Time.deltaTime * FadingSpeed;
            else
                gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _time = 0;
        if (_canvasGroup != null)
            _canvasGroup.alpha = 1;
    }
}
