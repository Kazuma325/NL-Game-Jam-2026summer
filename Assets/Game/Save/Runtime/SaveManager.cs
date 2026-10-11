
using System;
using System.IO;
using ShopGame.Save.Data;
using UnityEngine;

namespace ShopGame.Save.Runtime
{
    public sealed class SaveManager
    {
        public const int MaxSaveSlots = 3;

        private const int CurrentSaveVersion = 1;

        private string GetSaveFilePath(int slot)
        {
            ValidateSlot(slot);

            string fileName = $"save_{slot:D2}.json";

            return Path.Combine(
                Application.persistentDataPath,
                fileName);
        }

        public bool HasSaveData(int slot)
        {
            return File.Exists(GetSaveFilePath(slot));
        }

        public void Save(int slot, SaveData saveData)
        {
            if (saveData == null)
            {
                throw new ArgumentNullException(
                    nameof(saveData));
            }

            saveData.saveVersion = CurrentSaveVersion;

            string json = JsonUtility.ToJson(
                saveData,
                true);

            File.WriteAllText(
                GetSaveFilePath(slot),
                json);
        }

        public SaveData Load(int slot)
        {
            string path = GetSaveFilePath(slot);

            if (!File.Exists(path))
            {
                return null;
            }

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            SaveData saveData;

            try
            {
                saveData = JsonUtility.FromJson<SaveData>(
                    json);
            }
            catch (ArgumentException)
            {
                return null;
            }

            if (saveData == null)
            {
                return null;
            }

            if (saveData.saveVersion != CurrentSaveVersion)
            {
                Debug.LogWarning(
                    $"Save slot {slot} has an unsupported save version.");

                return null;
            }

            return saveData;
        }

        public void DeleteSaveData(int slot)
        {
            string path = GetSaveFilePath(slot);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static void ValidateSlot(int slot)
        {
            if (slot < 1 || slot > MaxSaveSlots)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(slot),
                    $"Slot must be between 1 and {MaxSaveSlots}.");
            }
        }
    }
}
