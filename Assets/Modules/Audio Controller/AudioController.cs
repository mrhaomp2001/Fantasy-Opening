using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audioClip;
        public AudioMixerGroup audioMixer;
        [Range(0f, 1f)]
        public float volume;
        [Range(0.3f, 3f)]
        public float pitch;

        public bool isLoop;

        [Min(1)]
        public int poolSize = 1;

        [HideInInspector]
        public Queue<AudioSource> audioSource = new Queue<AudioSource>();
    }

    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioMixer audioMixerMaster;
    [SerializeField] public static AudioController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        else
        {
            Instance = this;
        }

        foreach (var sound in sounds)
        {
            for (int i = 0; i < (sound.poolSize <= 0 ? 1 : sound.poolSize); i++)
            {
                var targetSound = gameObject.AddComponent<AudioSource>();
                targetSound.clip = sound.audioClip;
                targetSound.volume = sound.volume;
                targetSound.pitch = sound.pitch;
                targetSound.loop = sound.isLoop;
                targetSound.outputAudioMixerGroup = sound.audioMixer;

                sound.audioSource.Enqueue(targetSound);
            }
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;

        var targetSound = sound.audioSource.Dequeue();
        sound.audioSource.Enqueue(targetSound);

        targetSound.Play();
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;

        var targetSound = sound.audioSource.Dequeue();
        sound.audioSource.Enqueue(targetSound);

        targetSound.Stop();
    }

    public void MuteVolume()
    {
        audioMixerMaster.SetFloat("volume_effects", -80f);
    }

    public void SetVolume(float volume)
    {

        audioMixerMaster.SetFloat("volume_effects", volume);
        if (volume <= -19f)
        {
            audioMixerMaster.SetFloat("volume_effects", -80f);

        }
    }

    public void Pause(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;

        var targetSound = sound.audioSource.Dequeue();
        sound.audioSource.Enqueue(targetSound);

        targetSound.Pause();
    }

    public void Resume(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;

        var targetSound = sound.audioSource.Dequeue();
        sound.audioSource.Enqueue(targetSound);

        targetSound.UnPause();
    }

    public void SetTime(string name, float time)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;

        var targetSound = sound.audioSource.Dequeue();
        sound.audioSource.Enqueue(targetSound);

        targetSound.time = time;
    }

    public AudioSource GetAudioSource(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return null;

        var targetSound = sound.audioSource.Dequeue();
        sound.audioSource.Enqueue(targetSound);

        return targetSound;
    }
}

