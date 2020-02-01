using UnityEngine;

[CreateAssetMenu]
public class EmojiData : ScriptableObject
{
    [SerializeField] private GameObject _emojiPrefab;
    [SerializeField] private Drag[] _emojiPiecePrefabs;

    public GameObject EmojiPrefab { get => _emojiPrefab; set => _emojiPrefab = value; }

    public Drag[] EmojiPiecePrefabs { get => _emojiPiecePrefabs; set => _emojiPiecePrefabs = value; }
}
