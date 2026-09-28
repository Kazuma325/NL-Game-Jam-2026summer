using System;
using System.Collections.Generic;

namespace ShopGame.Event.Data
{
    public enum EffectType
    {
        StartDialogue,
        AdvanceProgress,
        RequestLoopEndAfterDialogue
    }

    public sealed class EffectData
    {
        public EffectType Type { get; }

        public string StringValue { get; }

        public int IntValue { get; }

        private EffectData(
            EffectType type,
            string stringValue,
            int intValue)
        {
            Type = type;
            StringValue = stringValue;
            IntValue = intValue;
        }

        public static EffectData StartDialogue(
            string dialogueId)
        {
            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new ArgumentException(
                    "Dialogue ID must not be null, empty, or whitespace.",
                    nameof(dialogueId));
            }

            return new EffectData(
                EffectType.StartDialogue,
                dialogueId,
                0);
        }

        public static EffectData AdvanceProgress(
            int targetProgress)
        {
            if (targetProgress < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(targetProgress),
                    "Target progress must not be negative.");
            }

            return new EffectData(
                EffectType.AdvanceProgress,
                null,
                targetProgress);
        }

        public static EffectData RequestLoopEndAfterDialogue()
        {
            return new EffectData(
                EffectType.RequestLoopEndAfterDialogue,
                null,
                0);
        }
    }
}