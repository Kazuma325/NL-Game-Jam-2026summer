using ShopGame.Dialogue.Runtime;
using UnityEngine;

namespace ShopGame.Title.Runtime
{
    public sealed class SettingsState
    {
        private const string VolumeKey = "Settings.Volume";
        private const string DialogueSpeedKey = "Settings.DialogueSpeed";

        public float Volume { get; private set; }

        public float DialogueSpeed { get; private set; }

        public SettingsState(
            float volume,
            float dialogueSpeed)
        {
            Volume = volume;
            DialogueSpeed = dialogueSpeed;
        }

        public void SetVolume(float volume)
        {
            Volume = volume;
            PlayerPrefs.SetFloat(VolumeKey, volume);
        }

        public void SetDialogueSpeed(float dialogueSpeed)
        {
            DialogueSpeed = dialogueSpeed;
            PlayerPrefs.SetFloat(DialogueSpeedKey, dialogueSpeed);
        }

        public DialogueSpeed GetDialogueSpeed()
        {
            switch ((int)DialogueSpeed)
            {
                case 0:
                    return Dialogue.Runtime.DialogueSpeed.Slow;

                case 1:
                    return Dialogue.Runtime.DialogueSpeed.Normal;

                case 2:
                    return Dialogue.Runtime.DialogueSpeed.Fast;

                case 3:
                    return Dialogue.Runtime.DialogueSpeed.Instant;

                default:
                    return Dialogue.Runtime.DialogueSpeed.Normal;
            }
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public static SettingsState Load()
        {
            float volume =
                PlayerPrefs.GetFloat(
                    VolumeKey,
                    1f);

            float dialogueSpeed =
                PlayerPrefs.GetFloat(
                    DialogueSpeedKey,
                    1f);

            return new SettingsState(
                volume,
                dialogueSpeed);
        }
    }
}