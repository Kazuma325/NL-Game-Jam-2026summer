
using ShopGame.Dialogue.Runtime;
using UnityEngine;

namespace ShopGame.Title.Runtime
{
    public sealed class SettingsState
    {
        // 既存キー。旧バージョンからの移行用にも残す。
        private const string VolumeKey = "Settings.Volume";

        private const string BgmVolumeKey = "Settings.BgmVolume";
        private const string SeVolumeKey = "Settings.SeVolume";
        private const string DialogueSpeedKey = "Settings.DialogueSpeed";

        // 既存のVolumeはBGM音量として扱う。
        public float Volume { get; private set; }

        public float SeVolume { get; private set; }

        public float DialogueSpeed { get; private set; }

        public SettingsState(
            float volume,
            float seVolume,
            float dialogueSpeed)
        {
            Volume = Mathf.Clamp01(volume);
            SeVolume = Mathf.Clamp01(seVolume);
            DialogueSpeed = dialogueSpeed;
        }

        public void SetVolume(float volume)
        {
            Volume = Mathf.Clamp01(volume);

            PlayerPrefs.SetFloat(BgmVolumeKey, Volume);

            // 旧キーも更新し、既存コードとの互換性を保つ。
            PlayerPrefs.SetFloat(VolumeKey, Volume);
        }

        public void SetSeVolume(float volume)
        {
            SeVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SeVolumeKey, SeVolume);
        }

        public void SetDialogueSpeed(float dialogueSpeed)
        {
            DialogueSpeed = dialogueSpeed;

            PlayerPrefs.SetFloat(
                DialogueSpeedKey,
                dialogueSpeed);
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
            float legacyVolume =
                PlayerPrefs.GetFloat(VolumeKey, 1f);

            float bgmVolume =
                PlayerPrefs.GetFloat(
                    BgmVolumeKey,
                    legacyVolume);

            float seVolume =
                PlayerPrefs.GetFloat(
                    SeVolumeKey,
                    bgmVolume);

            float dialogueSpeed =
                PlayerPrefs.GetFloat(
                    DialogueSpeedKey,
                    1f);

            return new SettingsState(
                bgmVolume,
                seVolume,
                dialogueSpeed);
        }
    }
}
