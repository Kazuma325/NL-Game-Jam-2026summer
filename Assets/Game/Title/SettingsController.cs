
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
        private Slider seVolumeSlider;

        [SerializeField]
        private Slider dialogueSpeedSlider;

        public event Action<DialogueSpeed> DialogueSpeedChanged;
        public event Action<float> VolumeChanged;
        public event Action<float> SeVolumeChanged;

        public float Volume => settingsState?.Volume ?? 1f;
        public float SeVolume => settingsState?.SeVolume ?? 1f;

        private void Awake()
        {
            if (settingsPanel == null)
                throw new InvalidOperationException(
                    "Settings Panel is not assigned.");

            if (volumeSlider == null)
                throw new InvalidOperationException(
                    "BGM Volume Slider is not assigned.");

            if (dialogueSpeedSlider == null)
                throw new InvalidOperationException(
                    "Dialogue Speed Slider is not assigned.");

            settingsPanel.SetActive(false);

            settingsState = SettingsState.Load();

            volumeSlider.value = settingsState.Volume;
            dialogueSpeedSlider.value =
                settingsState.DialogueSpeed;

            volumeSlider.onValueChanged.AddListener(
                OnVolumeChanged);

            dialogueSpeedSlider.onValueChanged.AddListener(
                OnDialogueSpeedChanged);

            if (seVolumeSlider != null)
            {
                seVolumeSlider.value = settingsState.SeVolume;

                seVolumeSlider.onValueChanged.AddListener(
                    OnSeVolumeChanged);
            }
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

        private void OnSeVolumeChanged(float value)
        {
            settingsState.SetSeVolume(value);
            SeVolumeChanged?.Invoke(value);
        }

        private void OnDialogueSpeedChanged(float value)
        {
            settingsState.SetDialogueSpeed(value);

            DialogueSpeedChanged?.Invoke(
                settingsState.GetDialogueSpeed());
        }

        private void OnDestroy()
        {
            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.RemoveListener(
                    OnVolumeChanged);
            }

            if (seVolumeSlider != null)
            {
                seVolumeSlider.onValueChanged.RemoveListener(
                    OnSeVolumeChanged);
            }

            if (dialogueSpeedSlider != null)
            {
                dialogueSpeedSlider.onValueChanged.RemoveListener(
                    OnDialogueSpeedChanged);
            }
        }
    }
}
