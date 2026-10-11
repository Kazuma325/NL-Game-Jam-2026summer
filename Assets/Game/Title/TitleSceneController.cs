
using ShopGame.Save.Runtime;
using ShopGame.Save.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopGame.Title.Runtime
{
    public sealed class TitleSceneController : MonoBehaviour
    {
        private const string GameSceneName = "SampleScene";

        [SerializeField]
        private GameObject saveSlotPanel;

        [SerializeField] private TMP_Text saveSlot1Label;
        [SerializeField] private TMP_Text saveSlot2Label;
        [SerializeField] private TMP_Text saveSlot3Label;

        private SaveManager saveManager;

        private void Awake()
        {
            saveManager = new SaveManager();
        }

        private void OnEnable()
        {
            RefreshSaveSlotLabels();
        }

        public void RefreshSaveSlotLabels()
        {
            if (saveManager == null)
            {
                saveManager = new SaveManager();
            }

            UpdateSlotLabel(1, saveSlot1Label);
            UpdateSlotLabel(2, saveSlot2Label);
            UpdateSlotLabel(3, saveSlot3Label);
        }


        public void DeleteSaveSlot(int slot)
        {
            if (!saveManager.HasSaveData(slot))
            {
                Debug.Log($"Save slot {slot} is already empty.");
                RefreshSaveSlotLabels();
                return;
            }

            saveManager.DeleteSaveData(slot);

            Debug.Log($"Deleted save slot {slot}.");

            RefreshSaveSlotLabels();
        }



        private void UpdateSlotLabel(int slot, TMP_Text label)
        {
            if (label == null)
            {
                return;
            }

            if (!saveManager.HasSaveData(slot))
            {
                label.text = $"スロット {slot} - 空き";
                return;
            }

            SaveData saveData = saveManager.Load(slot);

            if (saveData == null)
            {
                label.text = $"スロット {slot} - 読み込み不可";
                return;
            }

            label.text =
                $"スロット {slot}\n" +
                $"ループ回数: {saveData.loopCount}";
        }


        public void SelectSaveSlotAndStart(int slot)
        {
            SaveSlotSelection.SelectSlot(slot);

            Debug.Log($"Selected save slot: {slot}");

            SceneManager.LoadScene(GameSceneName);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(GameSceneName);
        }

        public void OpenSaveSlotPanel()
        {
            saveSlotPanel.SetActive(true);
        }

        public void CloseSaveSlotPanel()
        {
            saveSlotPanel.SetActive(false);
        }
    }
}
