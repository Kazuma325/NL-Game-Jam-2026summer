
using System;
using ShopGame.Sound.Runtime;
using ShopGame.Title.Runtime;
using UnityEngine;

namespace ShopGame.Sound.View
{
    public sealed class SoundSettingsController : MonoBehaviour
    {
        [SerializeField]
        private SettingsController settingsController;

        private SoundManager soundManager;
        private bool isInitialized;

        public void Initialize(SoundManager soundManager)
        {
            if (soundManager == null)
                throw new ArgumentNullException(nameof(soundManager));

            if (isInitialized)
                throw new InvalidOperationException(
                    "SoundSettingsController is already initialized.");

            if (settingsController == null)
                throw new InvalidOperationException(
                    "SettingsController is not assigned.");

            this.soundManager = soundManager;

            settingsController.VolumeChanged += OnBgmVolumeChanged;
            settingsController.SeVolumeChanged += OnSeVolumeChanged;

            isInitialized = true;
        }

        private void Start()
        {
            if (!isInitialized)
                throw new InvalidOperationException(
                    "SoundSettingsController is not initialized.");

            soundManager.SetBgmVolume(settingsController.Volume);
            soundManager.SetSeVolume(settingsController.SeVolume);
        }

        private void OnBgmVolumeChanged(float volume)
        {
            soundManager.SetBgmVolume(volume);
        }

        private void OnSeVolumeChanged(float volume)
        {
            soundManager.SetSeVolume(volume);
        }

        private void OnDestroy()
        {
            if (!isInitialized)
                return;

            settingsController.VolumeChanged -= OnBgmVolumeChanged;
            settingsController.SeVolumeChanged -= OnSeVolumeChanged;

            isInitialized = false;
        }
    }
}
