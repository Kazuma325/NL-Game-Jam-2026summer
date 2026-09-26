using System;
using ShopGame.Core.EventBus;
using ShopGame.Core.Context;
using ShopGame.Interaction.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Interaction.View
{
    public sealed class CraftingStationViewController : MonoBehaviour
    {
        [SerializeField]
        private Transform materialsContent;

        [SerializeField]
        private CraftingMaterialEntryView materialEntryPrefab;

        [SerializeField]
        private Button resetButton;

        [SerializeField]
        private Button createButton;

        private CraftingStation craftingStation;
        private EventBus eventBus;

        public void Initialize(GameContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            craftingStation =
                context.CraftingStation;

            eventBus =
                context.EventBus;

            eventBus.Subscribe<CraftingMaterialsChangedEvent>(
                HandleMaterialsChanged);

            resetButton.onClick.AddListener(
                HandleResetClicked);

            createButton.onClick.AddListener(
                HandleCreateClicked);

            Refresh();
        }

        private void HandleMaterialsChanged(
            CraftingMaterialsChangedEvent eventData)
        {
            Refresh();
        }

        private void HandleResetClicked()
        {
            craftingStation.ResetMaterials();
        }

        private void HandleCreateClicked()
        {
            craftingStation.Create();
        }

        private void Refresh()
        {
            ClearMaterialEntries();

            foreach (string materialId in
         craftingStation.GetInsertedMaterials())
            {
                CraftingMaterialEntryView entry =
                    Instantiate(
                        materialEntryPrefab,
                        materialsContent);

                string displayName =
                    craftingStation.GetMaterialDisplayName(
                        materialId);

                entry.Initialize(displayName);
            }
        }

        private void ClearMaterialEntries()
        {
            for (int i = materialsContent.childCount - 1;
                 i >= 0;
                 i--)
            {
                Destroy(
                    materialsContent.GetChild(i).gameObject);
            }
        }

        private void OnDestroy()
        {
            if (eventBus == null)
            {
                return;
            }

            eventBus.Unsubscribe<CraftingMaterialsChangedEvent>(
                HandleMaterialsChanged);

            resetButton.onClick.RemoveListener(
                HandleResetClicked);

            createButton.onClick.RemoveListener(
                HandleCreateClicked);
        }
    }
}