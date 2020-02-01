using System.Collections;
using UnityEngine;

public class Match : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private GameObject _dragObjectsRoot;

    [Header("General Animation")]
    [SerializeField] private TMPro.TMP_Text _matchStateDescriptor;
    [SerializeField, Range(1f, 5f)] private float _fadeMaxTime = 1f;
    [SerializeField] private SpriteRenderer _fadeSprite;
    [SerializeField] private DeltaTimeType _fadeDeltaTimeType;

    [Header("Ready Animation")]
    [SerializeField] private string _readyText = "Ready!";
    [SerializeField, Range(1f, 5f)] private float _readyMaxTime = 1f;

    [Header("Set Animation")]
    [SerializeField] private string _setText = "Ready!";
    [SerializeField, Range(1f, 5f)] private float _setMaxTime = 1f;

    [Header("Go Animation")]
    [SerializeField] private string _goText = "Ready!";
    [SerializeField, Range(1f, 5f)] private float _goMaxTime = 0.5f;

    [Header("Finish Animation")]
    [SerializeField] private string _finishText = "Finish!";
    [SerializeField, Range(1f, 5f)] private float _finishMaxTime = 0.5f;

    private float _timeLimit; // time limit in seconds

    protected virtual void OnEnable()
    {
        Begin(3f);
    }

    public virtual void Begin(float timeLimit)
    {
        _timeLimit = timeLimit;
        StartCoroutine(ShowReadySetGo());
    }

    private IEnumerator ShowReadySetGo()
    {
        _matchStateDescriptor.text = string.Empty;

        // Fade Sprite
        yield return StartCoroutine(Fade(1f, 0f));

        _fadeSprite.enabled = false;
        _matchStateDescriptor.enabled = true;

        // Show Ready
        _matchStateDescriptor.text = _readyText;
        yield return new WaitForSeconds(_readyMaxTime);

        // Show Set
        _matchStateDescriptor.text = _setText;
        yield return new WaitForSeconds(_setMaxTime);

        // Show Go
        _matchStateDescriptor.text = _goText;
        yield return new WaitForSeconds(_goMaxTime);

        _matchStateDescriptor.text = string.Empty;
        _matchStateDescriptor.enabled = false;

        yield return StartCoroutine(RunCountdown());
    }

    private IEnumerator RunCountdown()
    {
        Drag[] dragComponents = _dragObjectsRoot.GetComponentsInChildren<Drag>();
        foreach (Drag drag in dragComponents)
        {
            _hand.AddDragListener(drag);
        }
        yield return new WaitForSeconds(_timeLimit);

        foreach (Drag drag in dragComponents)
        {
            drag.OnCursorEndDrag(_hand.gameObject);
            _hand.RemoveDragListener(drag);
        }

        yield return StartCoroutine(ShowFinish());
    }

    private IEnumerator ShowFinish()
    {
        _matchStateDescriptor.enabled = true;
        _matchStateDescriptor.text = _finishText;
        yield return new WaitForSeconds(_finishMaxTime);

        _matchStateDescriptor.enabled = false;
        _matchStateDescriptor.text = string.Empty;
        yield return StartCoroutine(Fade(0f, 1f));
    }

    private IEnumerator Fade(float alphaStart, float alphaEnd)
    {
        // Fade Sprite
        _fadeSprite.enabled = true;
        for (float time = 0; time < _fadeMaxTime; time += Utils.GetDeltaTime(_fadeDeltaTimeType))
        {
            float fadeAlpha = Mathf.Lerp(alphaStart, alphaEnd, time / _fadeMaxTime);
            Color fadeSpriteColor = _fadeSprite.color;
            fadeSpriteColor.a = fadeAlpha;
            _fadeSprite.color = fadeSpriteColor;
            yield return null;
        }
    }
}
