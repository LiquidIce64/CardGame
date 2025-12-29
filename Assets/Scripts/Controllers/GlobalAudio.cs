using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Utils
{
    public enum GlobalAudioClip
    {
        Ignite,
        CardDeckDraw,
    }

    [RequireComponent(typeof(AudioSource))]
    public class GlobalAudio : MonoBehaviour
    {
        [SerializeField] private AudioResource ignite;
        [SerializeField] private AudioResource cardDeckDraw;
        private static GlobalAudio instance;
        private AudioSource audioSource;

        public static GlobalAudio Instance => instance;

        private void Awake()
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        private void PlayClip(GlobalAudioClip clip)
        {
            audioSource.resource = clip switch
            {
                GlobalAudioClip.Ignite => ignite,
                GlobalAudioClip.CardDeckDraw => cardDeckDraw,
                _ => throw new Exception("Invalid clip"),
            };
            audioSource.Play();
        }

        public static void Play(GlobalAudioClip clip) => Instance.PlayClip(clip);
    }
}