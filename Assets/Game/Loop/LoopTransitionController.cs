
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Loop
{
    public sealed class LoopTransitionController : MonoBehaviour
    {
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
        public void PlayLoopEffect(Action onWhiteReached)
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
        }

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
    }
}
