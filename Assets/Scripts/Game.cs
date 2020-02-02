using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Match[] _matches = new Match[0];

    [Header("Game Settings")]
    [SerializeField] private int _bestOf = 10;
    [SerializeField] private float _matchInterval = 1f;
    [SerializeField] private float _showResultsMaxTime = 3f;
    [SerializeField] private int _maxScore = 100;
    [SerializeField] private int _timeLimit = 10; // time limit in seconds
    [SerializeField] private int _finalCountdown = 3; // countdown in seconds
    [SerializeField] private EmojiData[] _emojiDatas;

    [Header("Result Settings")]
    [SerializeField] private TMPro.TMP_Text _bestPlayerName;
    [SerializeField] private TMPro.TMP_Text _bestPlayerScore;

    [Header("Fade Settings")]
    [SerializeField, Range(1f, 5f)] private float _fadeMaxTime = 1f;
    [SerializeField] private SpriteRenderer _fadeSprite;
    [SerializeField] private DeltaTimeType _fadeDeltaTimeType;

    [Header("HUD Settings")]
    [SerializeField] private TMPro.TMP_Text _matchCounter;
    [SerializeField] private TMPro.TMP_Text _countdown;

    private int _currentMatch = 1;
    private int _finishedMatches = 0;
    private Dictionary<string, int> _resultsDict = new Dictionary<string, int>();

    protected virtual void OnEnable()
    {
        _finishedMatches = 0;
        _resultsDict.Clear();
            
        StartCoroutine(StartMatches());
    }

    private IEnumerator StartMatches()
    {
        _matchCounter.text = string.Empty;

        int randomIndex = UnityEngine.Random.Range(0, _emojiDatas.Length);
        EmojiData randomEmojiData = _emojiDatas[randomIndex];
        foreach (var match in _matches)
        {
            match.Cleanup();
            match.BuildMatch(randomEmojiData, _maxScore, _timeLimit, _finalCountdown);
        }

        yield return StartCoroutine(Fade(1f, 0f));

        _fadeSprite.enabled = false;

        _matchCounter.text = string.Format("Match {0} of {1}", _currentMatch, _bestOf);
        
        foreach (var match in _matches)
        {
            match.Begin();
            match.AddGameListeners(this);
        }
    }

    public void OnEndMatch(Match match, string playerName, int matchScore)
    {
        match.RemoveGameListeners(this);

        if (!_resultsDict.ContainsKey(playerName))
        {
            _resultsDict.Add(playerName, matchScore);
        }
        else
        {
            _resultsDict[playerName] += matchScore;
        }

        _finishedMatches += 1;

        if (_finishedMatches >= _matches.Length)
        {
            StartCoroutine(LoopMatches());
        }
    }

    public void CleanCountdown()
    {
        _countdown.text = string.Empty;
    }

    public void UpdateCountdown(int countdown)
    {
        _countdown.text = countdown.ToString();
    }

    private IEnumerator LoopMatches()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(_matchInterval);

        _currentMatch += 1;

        if (_currentMatch <= _bestOf)
        {            
            yield return StartCoroutine(StartMatches());
        }
        else
        {
            _matchCounter.text = string.Empty;

            foreach (var match in _matches)
            {
                match.Cleanup();
            }

            yield return StartCoroutine(Fade(1f, 0f));

            _fadeSprite.enabled = false;

            yield return StartCoroutine(ShowResults());
        }
    }

    private IEnumerator ShowResults()
    {
        // get best player
        string bestPlayer = string.Empty;
        int bestScore = int.MinValue;
        foreach (var item in _resultsDict)
        {
            if (item.Value > bestScore)
            {
                bestPlayer = item.Key;
                bestScore = item.Value;
            }
        }

        _bestPlayerName.gameObject.SetActive(true);
        _bestPlayerName.text = string.Concat("WINNER", System.Environment.NewLine, bestPlayer);
        _bestPlayerScore.gameObject.SetActive(true);
        _bestPlayerScore.text = bestScore.ToString();

        yield return new WaitForSeconds(_showResultsMaxTime);

        SceneManager.LoadScene(Utils.MainMenuScene);
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
