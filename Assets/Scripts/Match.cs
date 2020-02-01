﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Match : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private GameObject _dragObjectsRoot;

    [Header("Match Settings")]
    [SerializeField] private int _maxScore = 100;
    [SerializeField] private int _timeLimit = 10; // time limit in seconds
    [SerializeField] private Color _countdownColorText = Color.white;
    [SerializeField] private int _finalCountdown = 3; // countdown in seconds
    [SerializeField, Range(0.5f, 3f)] private float _minDistanceToSlot = 3f;

    [Header("Emoji Builder")]
    [SerializeField] private EmojiData _emojiData;
    [SerializeField] private Transform _emojiRoot;
    [SerializeField] private Transform _emojiPiecesRoot;

    [Header("General Animation")]
    [SerializeField] private TMPro.TMP_Text _matchStateDescriptor;

    [Header("Ready Animation")]
    [SerializeField] private Color _readyColorText = Color.white;
    [SerializeField] private string _readyText = "Ready!";
    [SerializeField, Range(1f, 5f)] private float _readyMaxTime = 1f;

    [Header("Set Animation")]
    [SerializeField] private Color _setColorText = Color.white;
    [SerializeField] private string _setText = "Ready!";
    [SerializeField, Range(1f, 5f)] private float _setMaxTime = 1f;

    [Header("Go Animation")]
    [SerializeField] private Color _goColorText = Color.white;
    [SerializeField] private string _goText = "Ready!";
    [SerializeField, Range(1f, 5f)] private float _goMaxTime = 0.5f;

    [Header("Finish Animation")]
    [SerializeField] private Color _finishColorText = Color.white;
    [SerializeField] private string _finishText = "Finish!";
    [SerializeField, Range(1f, 5f)] private float _finishMaxTime = 0.5f;

    [Header("Score Animation")]
    [SerializeField, Range(0.05f, 0.15f)] private float _scoreIncrementTime = 0.05f;
    [SerializeField, Range(1f, 5f)] private float _scoreMaxAnimationTime = 1f;

    [Header("Player Events")]
    [SerializeField] private FinishMatchEvent _finishMatch;

    private List<GameObject> _slots = new List<GameObject>();
    private List<GameObject> _emojiPieces = new List<GameObject>();

    public virtual void Begin(int currentMatch)
    {
        BuildMatch();
        StartCoroutine(ShowReadySetGo());
    }

    public void Cleanup()
    {
        // Destroy all children
        for (int i = 0; i < _emojiRoot.childCount; i++)
        {
            Transform t = _emojiRoot.GetChild(i);
            Destroy(t.gameObject);
        }

        for (int i = 0; i < _emojiPiecesRoot.childCount; i++)
        {
            Transform t = _emojiPiecesRoot.GetChild(i);
            Destroy(t.gameObject);
        }
    }

    private void BuildMatch()
    {
        GameObject emoji = Instantiate(_emojiData.EmojiPrefab, _emojiRoot);
        BoxCollider2D[] slotColliders = emoji.GetComponentsInChildren<BoxCollider2D>();

        _slots.Clear();
        for (int i = 0; i < slotColliders.Length; i++)
        {
            _slots.Add(slotColliders[i].gameObject);
        }

        _emojiPieces.Clear();
        foreach (var emojiPiece in _emojiData.EmojiPiecePrefabs)
        {
            GameObject emojiPieceGO = Instantiate(emojiPiece.gameObject, _emojiPiecesRoot);
            _emojiPieces.Add(emojiPieceGO);
            emojiPieceGO.SetActive(false);
        }
    }

    private IEnumerator ShowReadySetGo()
    {
        _matchStateDescriptor.text = string.Empty;
        _matchStateDescriptor.enabled = true;

        // Show Ready
        _matchStateDescriptor.text = _readyText;
        _matchStateDescriptor.color = _readyColorText;
        yield return new WaitForSeconds(_readyMaxTime);

        // Show Set
        _matchStateDescriptor.text = _setText;
        _matchStateDescriptor.color = _setColorText;
        yield return new WaitForSeconds(_setMaxTime);

        // Hide/Show Emoji Pieces
        foreach (var slot in _slots)
        {
            SpriteRenderer slotRenderer = slot.GetComponentInChildren<SpriteRenderer>();
            slotRenderer.enabled = false;
        }

        foreach (var emojiPiece in _emojiPieces)
        {
            emojiPiece.SetActive(true);
        }

        // Show Go
        _matchStateDescriptor.text = _goText;
        _matchStateDescriptor.color = _goColorText;
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

        yield return new WaitForSeconds(_timeLimit - _finalCountdown);

        // show countdown
        _matchStateDescriptor.enabled = true;
        _matchStateDescriptor.color = _countdownColorText;
        for (int second = _finalCountdown; second > 0; second--)
        {
            _matchStateDescriptor.text = second.ToString();
            yield return new WaitForSeconds(1f);
        }

        foreach (Drag drag in dragComponents)
        {
            drag.OnCursorEndDrag(_hand);
            _hand.RemoveDragListener(drag);
        }

        yield return StartCoroutine(ShowFinish());
    }

    private IEnumerator ShowFinish()
    {
        _matchStateDescriptor.enabled = true;
        _matchStateDescriptor.text = _finishText;
        _matchStateDescriptor.color = _finishColorText;
        yield return new WaitForSeconds(_finishMaxTime);

        int finalScore = CalculateScore();

        for (int score = 0; score <= finalScore; score++)
        {
            _matchStateDescriptor.text = score.ToString();
            yield return new WaitForSeconds(_scoreIncrementTime);
        }

        yield return new WaitForSeconds(_scoreMaxAnimationTime);
        _matchStateDescriptor.text = string.Empty;
        _matchStateDescriptor.enabled = false;

        _finishMatch?.Invoke(this, finalScore);
    }

    private int CalculateScore()
    {
        List<GameObject> emojiPiecesCopy = new List<GameObject>();
        emojiPiecesCopy.AddRange(_emojiPieces);

        float totalScoreForAllSlots = 0f;
        foreach (var slot in _slots)
        {
            GameObject bestEmojiPlaceScore = null;
            float maxScoreForSlot = 0;
            foreach (var emojiPiece in emojiPiecesCopy)
            {
                // Get the emoji piece with the same name as the slot with the highest score
                if (emojiPiece.CompareTag(slot.tag))
                {                    
                    float distance = Vector2.Distance(emojiPiece.transform.position, slot.transform.position);
                    float scoreRatio = Mathf.InverseLerp(_minDistanceToSlot, 0f, distance);
                    float scoreForSlot = scoreRatio * _maxScore;

                    if (scoreForSlot > maxScoreForSlot)
                    {
                        bestEmojiPlaceScore = emojiPiece;
                        maxScoreForSlot = scoreForSlot;
                    }
                }
            }
            
            totalScoreForAllSlots += maxScoreForSlot;

            // Remove the best emoji from the list
            if (bestEmojiPlaceScore != null)
            {
                Debug.LogFormat("Best Emoji Place: {0} for Slot {1} Score: {2}", bestEmojiPlaceScore.name, slot.name, maxScoreForSlot);
                emojiPiecesCopy.Remove(bestEmojiPlaceScore);
            }
            else
            {
                Debug.LogFormat("Slot: {0} has no match", slot.name);
            }
        }

        int finalScore = Mathf.CeilToInt(totalScoreForAllSlots / _slots.Count);
        return finalScore;
    }

    public void AddGameListener(Game game)
    {
        _finishMatch.AddListener(game.OnEndMatch);
    }

    public void RemoveGameListener(Game game)
    {
        _finishMatch.RemoveListener(game.OnEndMatch);
    }
}
