using System;
using UnityEngine;

namespace ShopGame.Dialogue.View
{
    [Serializable]
    public sealed class DialoguePortraitEntry
    {
        [SerializeField] private string id;

        [SerializeField]
        private DialoguePortraitType type;

        [SerializeField] private Sprite sprite;

        [SerializeField] private GameObject prefab;

        public string Id => id;

        public DialoguePortraitType Type => type;

        public Sprite Sprite => sprite;

        public GameObject Prefab => prefab;
    }
}