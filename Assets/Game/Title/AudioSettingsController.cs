using ShopGame.Audio.Runtime;
using ShopGame.Title.Runtime;
using UnityEngine;

public class AudioSettingsController : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private SettingsController settingsController;

    private void Awake()
    {
        settingsController.VolumeChanged += OnVolumeChanged;
    }

    private void OnVolumeChanged(float value)
    {
        audioManager.SetVolume(value);
    }

    private void OnDestroy()
    {
        settingsController.VolumeChanged -= OnVolumeChanged;
    }
}
