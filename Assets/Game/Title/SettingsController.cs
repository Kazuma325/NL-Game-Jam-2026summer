using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Title.Runtime
{
    public sealed class SettingsController : MonoBehaviour
    {
        [SerializeField]
        private Slider volumeSlider;

        [SerializeField]
        private Slider dialogueSpeedSlider;

        private SettingsState settingsState;

        public void Initialize(SettingsState settingsState)
        {
            this.settingsState =
                settingsState
                ?? throw new ArgumentNullException(
                    nameof(settingsState));

            volumeSlider.value = settingsState.Volume;
            dialogueSpeedSlider.value = settingsState.DialogueSpeed;

            volumeSlider.onValueChanged.AddListener(
                OnVolumeChanged);

            dialogueSpeedSlider.onValueChanged.AddListener(
                OnDialogueSpeedChanged);
        }

        private void OnVolumeChanged(float value)
        {
            settingsState.SetVolume(value);
        }

        private void OnDialogueSpeedChanged(float value)
        {
            settingsState.SetDialogueSpeed(value);
        }

        private void OnDestroy()
        {
            volumeSlider.onValueChanged.RemoveListener(
                OnVolumeChanged);

            dialogueSpeedSlider.onValueChanged.RemoveListener(
                OnDialogueSpeedChanged);
        }
    }
}