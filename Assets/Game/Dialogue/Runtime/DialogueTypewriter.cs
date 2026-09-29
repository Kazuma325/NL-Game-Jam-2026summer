using System;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueTypewriter
    {
        private float charactersPerSecond;
        private float elapsedTime;

        public DialogueSpeed Speed { get; private set; }

        public DialogueTypewriter(
            DialogueSpeed initialSpeed)
        {
            SetSpeed(initialSpeed);
        }

        public void SetSpeed(DialogueSpeed speed)
        {
            Speed = speed;

            charactersPerSecond =
    GetCharactersPerSecond(speed) * speedMultiplier;

            elapsedTime = 0f;
        }

        public bool Update(
            float deltaTime,
            DialogueManager dialogueManager)
        {
            if (dialogueManager == null)
            {
                throw new ArgumentNullException(
                    nameof(dialogueManager));
            }

            if (!dialogueManager.IsPlaying)
                return false;

            if (dialogueManager.CurrentLine == null)
                return false;

            if (Speed == DialogueSpeed.Instant)
            {
                if (!dialogueManager.IsLineCompleted)
                {
                    dialogueManager.CompleteCurrentLine();
                    return true;
                }

                return false;
            }

            if (dialogueManager.IsLineCompleted)
            {
                elapsedTime = 0f;
                return false;
            }

            elapsedTime += deltaTime;

            float secondsPerCharacter =
                1f / charactersPerSecond;

            bool advanced = false;

            while (elapsedTime >= secondsPerCharacter)
            {
                elapsedTime -= secondsPerCharacter;

                dialogueManager.AdvanceCharacter();

                advanced = true;

                if (dialogueManager.IsLineCompleted)
                {
                    elapsedTime = 0f;
                    break;
                }
            }

            return advanced;
        }

        public void Reset()
        {
            elapsedTime = 0f;
        }

        private static float GetCharactersPerSecond(
            DialogueSpeed speed)
        {
            switch (speed)
            {
                case DialogueSpeed.Slow:
                    return 5f;

                case DialogueSpeed.Normal:
                    return 10f;

                case DialogueSpeed.Fast:
                    return 20f;

                case DialogueSpeed.Instant:
                    return float.PositiveInfinity;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(speed),
                        speed,
                        "Unknown dialogue speed.");
            }
        }

        private float speedMultiplier = 1f;

        public void SetSpeedMultiplier(float multiplier)
        {
            if (multiplier <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(multiplier),
                    multiplier,
                    "Speed multiplier must be greater than zero.");
            }

            speedMultiplier = multiplier;

            charactersPerSecond =
                GetCharactersPerSecond(Speed);

            elapsedTime = 0f;
        }
    }
}