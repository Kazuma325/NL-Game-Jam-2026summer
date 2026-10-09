using UnityEngine;

namespace ShopGame.Sound.View
{
    public sealed class AudioSourceView : MonoBehaviour
    {
        [SerializeField]
        private AudioSource bgmSource;

        [SerializeField]
        private AudioSource seSource;

        public void PlayBgm(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            if (bgmSource.clip == clip && bgmSource.isPlaying)
            {
                return;
            }

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void StopBgm()
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }

        public void PlaySe(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            seSource.PlayOneShot(clip);
        }
    }
}