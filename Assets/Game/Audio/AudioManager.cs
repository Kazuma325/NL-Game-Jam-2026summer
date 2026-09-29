using System;
using UnityEngine;

namespace ShopGame.Audio.Runtime
{
    public sealed class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource bgmSource;

        [SerializeField]
        private AudioClip bgmClip;

        [SerializeField]
        [Range(0f, 1f)]
        private float volume = 1f;

        private void Awake()
        {
            if (bgmSource == null)
                throw new InvalidOperationException(
                    "BGM AudioSource is not assigned.");

            if (bgmClip == null)
                throw new InvalidOperationException(
                    "BGM AudioClip is not assigned.");

            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.volume = volume;

            PlayBgm();
        }

        public void PlayBgm()
        {
            if (bgmSource.isPlaying)
                return;

            bgmSource.Play();
        }

        public void StopBgm()
        {
            bgmSource.Stop();
        }

        public void SetVolume(float volume)
        {
            if (volume < 0f || volume > 1f)
                throw new ArgumentOutOfRangeException(
                    nameof(volume));

            this.volume = volume;
            bgmSource.volume = volume;
        }
    }
}