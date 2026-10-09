using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Sound.Data
{
    [CreateAssetMenu(
        fileName = "SoundDatabase",
        menuName = "ShopGame/Sound/Sound Database")]
    public sealed class SoundDatabase : ScriptableObject
    {
        [SerializeField]
        private List<BgmEntry> bgms = new();

        [SerializeField]
        private List<SeEntry> ses = new();

        public AudioClip GetBgmClip(Runtime.BgmId bgmId)
        {
            foreach (BgmEntry entry in bgms)
            {
                if (entry.Id == bgmId)
                {
                    return entry.Clip;
                }
            }

            throw new InvalidOperationException(
                $"BGM is not registered: {bgmId}");
        }

        public AudioClip GetSeClip(Runtime.SeId seId)
        {
            foreach (SeEntry entry in ses)
            {
                if (entry.Id == seId)
                {
                    return entry.Clip;
                }
            }

            throw new InvalidOperationException(
                $"SE is not registered: {seId}");
        }

        [Serializable]
        private sealed class BgmEntry
        {
            public Runtime.BgmId Id;
            public AudioClip Clip;
        }

        [Serializable]
        private sealed class SeEntry
        {
            public Runtime.SeId Id;
            public AudioClip Clip;
        }
    }
}