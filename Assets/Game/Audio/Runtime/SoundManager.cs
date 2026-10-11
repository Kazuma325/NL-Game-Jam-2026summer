
using System;
using ShopGame.Sound.Data;
using ShopGame.Sound.View;

namespace ShopGame.Sound.Runtime
{
    public sealed class SoundManager
    {
        private readonly SoundDatabase soundDatabase;
        private readonly AudioSourceView audioSourceView;

        public SoundManager(
            SoundDatabase soundDatabase,
            AudioSourceView audioSourceView)
        {
            this.soundDatabase =
                soundDatabase
                ?? throw new ArgumentNullException(nameof(soundDatabase));

            this.audioSourceView =
                audioSourceView
                ?? throw new ArgumentNullException(nameof(audioSourceView));
        }

        public void PlayBgm(BgmId bgmId)
        {
            var clip = soundDatabase.GetBgmClip(bgmId);
            audioSourceView.PlayBgm(clip);
        }

        public void StopBgm()
        {
            audioSourceView.StopBgm();
        }

        public void PlaySe(SeId seId)
        {
            var clip = soundDatabase.GetSeClip(seId);
            audioSourceView.PlaySe(clip);
        }

        public void SetBgmVolume(float volume)
        {
            audioSourceView.SetBgmVolume(volume);
        }

        public void SetSeVolume(float volume)
        {
            audioSourceView.SetSeVolume(volume);
        }
    }
}
