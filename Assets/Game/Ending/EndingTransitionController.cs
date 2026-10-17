using DG.Tweening;
using ShopGame.Core.EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopGame.Ending
{
    public sealed class EndingTransitionController : MonoBehaviour
    {
        //バグ画面演出
        [Header("Glitch Settings")]
        [SerializeField] private Material glitchMaterial;

        private const string endingSceneName = "Ending";

        private EventBus eventBus;
        private Sequence currentSequence;

        public void Initialize(EventBus eventBus)
        {
            this.eventBus = eventBus;

            eventBus.Subscribe<DialogueEndedEvent>(OnDialogueEnded);
        }

        private void OnDestroy()
        {
            eventBus.Unsubscribe<DialogueEndedEvent>(OnDialogueEnded);
        }

        private void OnDialogueEnded(DialogueEndedEvent eventData)
        {
            if (eventData.DialogueId == "blender_core")
            {
                EndingTransition();
            }
        }

        private void EndingTransition()
        {
            PlayLoopEffect();
        }

        public void PlayLoopEffect()
        {
            if (glitchMaterial == null)
            {
                Debug.LogWarning(
                    "Glitch material is missing.",
                    this);
                return;
            }

            currentSequence?.Kill();
            ResetGlitch();

            currentSequence = DOTween.Sequence()
                .SetUpdate(true);

            // 時刻ごとにグリッチの強さを切り替える
            currentSequence.InsertCallback(
                0.3f, () => SetGlitch(0.012f, 0.45f));

            currentSequence.InsertCallback(
                0.6f, () => SetGlitch(0f, 0f));

            currentSequence.InsertCallback(
                1.0f, () => SetGlitch(0.018f, 0.65f));

            currentSequence.InsertCallback(
                1.5f, () => SetGlitch(0.005f, 0.2f));

            currentSequence.InsertCallback(
                2.0f, () => SetGlitch(0.025f, 0.9f));

            currentSequence.InsertCallback(
                2.4f, () => SetGlitch(0.012f, 0.4f));

            currentSequence.InsertCallback(
                3.0f, () => SetGlitch(0.03f, 1f));

            currentSequence.InsertCallback(
                3.25f, () => SetGlitch(0.003f, 0.1f));

            currentSequence.InsertCallback(
                3.8f, () => SetGlitch(0.02f, 0.7f));

            currentSequence.InsertCallback(
                4.1f, () => SetGlitch(0.008f, 0.25f));

            currentSequence.InsertCallback(
                4.7f, () => SetGlitch(0.035f, 1f));

            currentSequence.InsertCallback(
                5.0f, () => SetGlitch(0.012f, 0.35f));

            currentSequence.InsertCallback(
                5.5f, () => SetGlitch(0.028f, 5f));

            currentSequence.InsertCallback(
                5.9f, () => SetGlitch(0.005f, 0.15f));

            currentSequence.InsertCallback(
                6.3f, () => SetGlitch(0.04f, 20f));

            currentSequence.InsertCallback(
                6.6f, () => SetGlitch(0.018f, 0.5f));

            currentSequence.InsertCallback(
                7.0f, () => SetGlitch(0.045f, 50f));

            currentSequence.InsertCallback(
                7.35f, () => SetGlitch(0.025f, 0.7f));

            currentSequence.InsertCallback(
                7.65f, () => SetGlitch(0.05f, 100f));

            // 全体の演出時間を8秒にする
            currentSequence.AppendInterval(8f);

            currentSequence.OnComplete(() =>
            {
                ResetGlitch();
                SceneManager.LoadScene(endingSceneName);
            });
        }

        private void SetGlitch(float rgbShift, float tearIntensity)
        {
            glitchMaterial.SetFloat("_RGBShift", rgbShift);
            glitchMaterial.SetFloat("_TearIntensity", tearIntensity);
        }

        private void ResetGlitch()
        {
            if (glitchMaterial == null)
            {
                return;
            }

            glitchMaterial.SetFloat("_RGBShift", 0f);
            glitchMaterial.SetFloat("_TearIntensity", 0f);
        }
    }
}
