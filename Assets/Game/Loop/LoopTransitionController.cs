
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Loop
{
    public sealed class LoopTransitionController : MonoBehaviour
    {
        //白画面演出
        [Header("UI References")]
        [SerializeField] private Image blackOverlay;
        [SerializeField] private Image lightImage;
        [SerializeField] private Image whiteOverlay;

        [Header("Animation Settings")]
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float lightDelay = 0.3f;
        [SerializeField] private float lightDuration = 3f;
        [SerializeField] private float lightStartScale = 0.1f;
        [SerializeField] private float lightEndScale = 30f;
        [SerializeField] private float fadeFromWhiteDuration = 0.8f;

        private Sequence currentSequence;

        //バグ画面演出
        [Header("Glitch Settings")]
        [SerializeField] private Material glitchMaterial;


        // テストボタンからも呼び出せる入口
        public void PlayFadeToBlack()
        {
            PlayLoopEffect();
        }

        // ループ演出を再生する
        public void PlayLoopEffect()
        {
            PlayLoopEffect(null);
        }

        // 白い画面になった時点で callback を呼び出す
        /*public void PlayLoopEffect(Action onWhiteReached)
        {
            if (blackOverlay == null ||
                lightImage == null ||
                whiteOverlay == null)
            {
                Debug.LogWarning(
                    "Loop transition UI references are missing.",
                    this);
                return;
            }

            currentSequence?.Kill();

            blackOverlay.gameObject.SetActive(true);
            lightImage.gameObject.SetActive(true);
            whiteOverlay.gameObject.SetActive(true);

            SetAlpha(blackOverlay, 0f);
            SetAlpha(lightImage, 0f);
            SetAlpha(whiteOverlay, 0f);

            lightImage.rectTransform.localScale =
                Vector3.one * lightStartScale;

            currentSequence = DOTween.Sequence()
                .SetUpdate(true);

            // 1. 暗転
            currentSequence.Append(
                blackOverlay.DOFade(1f, fadeDuration));

            // 2. 暗闇で待機
            currentSequence.AppendInterval(lightDelay);

            // 3. 白い光を出現させながら拡大
            currentSequence.Append(
                lightImage.DOFade(1f, 0.5f));

            currentSequence.Join(
                lightImage.rectTransform
                    .DOScale(lightEndScale, lightDuration));

            // 4. 画面全体を白く覆う
            currentSequence.Append(
                whiteOverlay.DOFade(1f, 0.5f));

            // 白くなった後に次の処理を実行する
            currentSequence.OnComplete(() =>
            {
                onWhiteReached?.Invoke();

                blackOverlay.gameObject.SetActive(false);
                lightImage.gameObject.SetActive(false);
                whiteOverlay.gameObject.SetActive(false);
            });
        }*/

        // 新しいループの画面を見せる
        public void PlayFadeFromWhite()
        {
            if (blackOverlay == null ||
                lightImage == null ||
                whiteOverlay == null)
            {
                Debug.LogWarning(
                    "Loop transition UI references are missing.",
                    this);
                return;
            }

            currentSequence?.Kill();

            currentSequence = DOTween.Sequence()
                .SetUpdate(true);

            // 白・黒・光を同時に消して画面を見せる
            currentSequence.Append(
                whiteOverlay.DOFade(
                    0f,
                    fadeFromWhiteDuration));

            currentSequence.Join(
                blackOverlay.DOFade(
                    0f,
                    fadeFromWhiteDuration));

            currentSequence.Join(
                lightImage.DOFade(
                    0f,
                    fadeFromWhiteDuration));

            currentSequence.OnComplete(() =>
            {
                lightImage.rectTransform.localScale =
                    Vector3.one * lightStartScale;
            });
        }

        private static void SetAlpha(Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private void OnDestroy()
        {
            currentSequence?.Kill();
        }



        public void PlayLoopEffect(Action onWhiteReached)
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
                1.7f, () => SetGlitch(0.03f, 1f));

            currentSequence.InsertCallback(
                2.0f, () => SetGlitch(0.02f, 0.7f));

            currentSequence.InsertCallback(
                2.2f, () => SetGlitch(0.008f, 0.25f));

            currentSequence.InsertCallback(
                2.5f, () => SetGlitch(0.035f, 1f));

            currentSequence.InsertCallback(
                2.7f, () => SetGlitch(0.018f, 0.5f));

            currentSequence.InsertCallback(
                3.0f, () => SetGlitch(0.045f, 50f));

            // 全体の演出時間を8秒にする
            currentSequence.AppendInterval(3f);

            currentSequence.OnComplete(() =>
            {
                ResetGlitch();
                onWhiteReached?.Invoke();
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
