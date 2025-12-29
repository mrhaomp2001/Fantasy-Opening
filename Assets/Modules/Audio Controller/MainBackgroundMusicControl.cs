using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBackgroundMusicControl : MonoBehaviour
{
    private static MainBackgroundMusicControl instance;

    [SerializeField] private List<AudioClip> audioClipBGMs;
    private Queue<AudioClip> audioClipBGMQueue = new Queue<AudioClip>();

    public static MainBackgroundMusicControl Instance { get => instance; set => instance = value; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        foreach (var clip in audioClipBGMs)
        {
            audioClipBGMQueue.Enqueue(clip);
        }
    }
    public void FadeOutAndStartNew()
    {
        var targetSound = AudioController.Instance.GetAudioSource("bgm");
        LeanTween.cancel(instance.gameObject);
        LeanTween.value(instance.gameObject, 1f, 0f, 2f)
            .setOnUpdate((float value) =>
            {
                targetSound.volume = value;
            })
            .setOnComplete(() =>
            {
                LeanTween.delayedCall(instance.gameObject, 0.5f, () =>
                {
                    targetSound.volume = 1f;
                    targetSound.Stop();

                    LeanTween.delayedCall(instance.gameObject, 0.5f, () =>
                    {
                        var nextBGM = audioClipBGMQueue.Dequeue();
                        audioClipBGMQueue.Enqueue(nextBGM);

                        targetSound.clip = nextBGM;
                        targetSound.Play();

                    });
                });
            });
    }
}
