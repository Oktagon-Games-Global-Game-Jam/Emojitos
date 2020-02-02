using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _introClip;
    [SerializeField] private AudioClip _loopClip;
    [SerializeField] private AudioSource _audioSource;

    // Start is called before the first frame update
    protected IEnumerator Start()
    {
        _audioSource.loop = false;
        _audioSource.clip = _introClip;
        _audioSource.Play();

        yield return new WaitUntil(() => _audioSource.isPlaying);
        yield return new WaitForSecondsRealtime(_introClip.length);

        _audioSource.loop = true;
        _audioSource.clip = _loopClip;
        _audioSource.Play();
    }
}
