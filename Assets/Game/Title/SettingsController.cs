using ShopGame.Dialogue.Runtime;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Title.Runtime
{
    public sealed class SettingsController : MonoBehaviour
    {
        private SettingsState settingsState;

        [SerializeField]
        private GameObject settingsPanel;

        [SerializeField]
        private Slider volumeSlider;

        [SerializeField]
        private Slider dialogueSpeedSlider;

        public event Action<DialogueSpeed> DialogueSpeedChanged;
        public event Action<float> VolumeChanged;

        private void Awake()
        {
            if (settingsPanel == null)
                throw new InvalidOperationException(
                    "Settings Panel is not assigned.");

            if (volumeSlider == null)
                throw new InvalidOperationException(
                    "Volume Slider is not assigned.");

            if (dialogueSpeedSlider == null)
                throw new InvalidOperationException(
                    "Dialogue Speed Slider is not assigned.");

            settingsPanel.SetActive(false);

            settingsState = SettingsState.Load();

            volumeSlider.value = settingsState.Volume;
            dialogueSpeedSlider.value = settingsState.DialogueSpeed;

            volumeSlider.onValueChanged.AddListener(
                OnVolumeChanged);

            dialogueSpeedSlider.onValueChanged.AddListener(
                OnDialogueSpeedChanged);
        }

        public void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsState.Save();

            settingsPanel.SetActive(false);
        }

        private void OnVolumeChanged(float value)
        {
            settingsState.SetVolume(value);
            VolumeChanged?.Invoke(value);
        }

        private void OnDialogueSpeedChanged(float value)
        {
            settingsState.SetDialogueSpeed(value);

            DialogueSpeedChanged?.Invoke(
                settingsState.GetDialogueSpeed());
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