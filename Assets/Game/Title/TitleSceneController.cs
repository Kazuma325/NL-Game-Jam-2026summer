using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopGame.Title.Runtime
{
    public sealed class TitleSceneController : MonoBehaviour
    {
        private SettingsState settingsState;
        [SerializeField]
        private SettingsController settingsController;
        private const string GameSceneName = "SampleScene";

        [SerializeField]
        private GameObject settingsPanel;

        private void Awake()
        {
            if (settingsPanel == null)
                throw new System.InvalidOperationException(
                    "Settings Panel is not assigned.");

            settingsPanel.SetActive(false);

            settingsState = SettingsState.Load();
            settingsController.Initialize(settingsState);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(GameSceneName);
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
    }
}