using System;
using System.Collections.Generic;
using ShopGame.Core.EventBus;
using ShopGame.Interaction.Runtime;

namespace ShopGame.Interaction.Runtime
{
    public sealed class CraftingStation : IKnowledgeUsable
    {
        private readonly EventBus eventBus;

        private readonly HashSet<string> insertedMaterialIds =
            new();

        private readonly Dictionary<string, string>
    materialDisplayNames = new();

        public IReadOnlyCollection<string> InsertedMaterialIds =>
            insertedMaterialIds;

        public CraftingStation(EventBus eventBus)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));
        }

        public KnowledgeUseResult TryUseKnowledge(
    string knowledgeId)
        {
            if (string.IsNullOrWhiteSpace(knowledgeId))
            {
                throw new ArgumentException(
                    "Knowledge ID must not be null, empty, or whitespace.",
                    nameof(knowledgeId));
            }

            bool added =
                AddMaterial(knowledgeId);

            if (!added)
            {
                return KnowledgeUseResult.Failed;
            }

            return KnowledgeUseResult.Success;
        }

        public bool AddMaterial(string materialId)
        {
            ValidateMaterialId(materialId);

            bool added =
                insertedMaterialIds.Add(materialId);

            if (!added)
            {
                return false;
            }

            eventBus.Publish(
                new CraftingMaterialsChangedEvent());

            return true;
        }

        public bool AddMaterial(
    string materialId,
    string displayName)
        {
            ValidateMaterialId(materialId);

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Display name must not be null, empty, or whitespace.",
                    nameof(displayName));
            }

            bool added =
                insertedMaterialIds.Add(materialId);

            if (!added)
            {
                return false;
            }

            materialDisplayNames[materialId] = displayName;

            eventBus.Publish(
                new CraftingMaterialsChangedEvent());

            return true;
        }

        public bool HasMaterial(string materialId)
        {
            ValidateMaterialId(materialId);

            return insertedMaterialIds.Contains(materialId);
        }

        public IReadOnlyCollection<string> GetInsertedMaterials()
        {
            return insertedMaterialIds;
        }

        public void ResetMaterials()
        {
            if (insertedMaterialIds.Count == 0)
            {
                return;
            }

            insertedMaterialIds.Clear();
            materialDisplayNames.Clear();

            eventBus.Publish(
                new CraftingMaterialsChangedEvent());
        }

        public bool Create()
        {
            if (insertedMaterialIds.Count == 0)
            {
                return false;
            }

            var materialSnapshot =
                new HashSet<string>(
                    insertedMaterialIds);

            eventBus.Publish(
                new ItemCraftedEvent(
                    materialSnapshot));

            return true;
        }

        private static void ValidateMaterialId(
            string materialId)
        {
            if (string.IsNullOrWhiteSpace(materialId))
            {
                throw new ArgumentException(
                    "Material ID must not be null, empty, or whitespace.",
                    nameof(materialId));
            }
        }

        public void SetMaterialDisplayName(
    string materialId,
    string displayName)
        {
            if (string.IsNullOrWhiteSpace(materialId))
            {
                throw new ArgumentException(
                    "Material ID must not be null, empty, or whitespace.",
                    nameof(materialId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Display name must not be null, empty, or whitespace.",
                    nameof(displayName));
            }

            if (!insertedMaterialIds.Contains(materialId))
            {
                throw new InvalidOperationException(
                    $"Material '{materialId}' has not been inserted.");
            }

            materialDisplayNames[materialId] = displayName;
        }

        public string GetMaterialDisplayName(
    string materialId)
        {
            if (materialDisplayNames.TryGetValue(
                    materialId,
                    out string displayName))
            {
                return displayName;
            }

            return materialId;
        }
    }
}