using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Match[] _matches = new Match[0];

    [Header("Game Settings")]
    [SerializeField] private int _bestOf = 10;

    [Header("Fade Settings")]
    [SerializeField, Range(1f, 5f)] private float _fadeMaxTime = 1f;
    [SerializeField] private SpriteRenderer _fadeSprite;
    [SerializeField] private DeltaTimeType _fadeDeltaTimeType;

    private int _currentMatch = 1;
    private int _finishedMatches = 0;

    protected virtual void OnEnable()
    {
        StartCoroutine(StartMatches());
    }

    private IEnumerator StartMatches()
    {
        _finishedMatches = 0;

        foreach (var match in _matches)
        {
            match.Cleanup();
        }

        yield return StartCoroutine(Fade(1f, 0f));

        _fadeSprite.enabled = false;

        foreach (var match in _matches)
        {
            match.Begin(_currentMatch);
            match.AddGameListener(this);
        }
    }

    public void OnEndMatch(Match match, int matchScore)
    {
        match.RemoveGameListener(this);

        _finishedMatches += 1;

        if (_finishedMatches >= _matches.Length)
        {
            StartCoroutine(LoopMatches());
        }        
    }

    private IEnumerator LoopMatches()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(3f);

        _currentMatch += 1;

        if (_currentMatch < _bestOf)
        {            
            yield return StartCoroutine(StartMatches());            
        }
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
