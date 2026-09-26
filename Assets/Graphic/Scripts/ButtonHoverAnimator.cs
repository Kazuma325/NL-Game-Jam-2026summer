using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace UI.Graphics
{
    [RequireComponent(typeof(RectTransform))]
    public class ButtonHoverAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Scale Animation")]
        [SerializeField, Tooltip("マウスオーバー時にどれくらい大きくなるか")]
        private float scaleMultiplier = 1.1f;
        [SerializeField, Tooltip("大きさが変化するスピード")]
        private float scaleDuration = 0.15f;

        [Header("Opacity Animation")]
        [SerializeField, Tooltip("不透明度のアニメーションを有効にするか")]
        private bool useOpacity = true;
        [SerializeField, Tooltip("通常時の不透明度")]
        private float normalAlpha = 0.5f;
        [SerializeField, Tooltip("マウスオーバー時の不透明度 (Max)")]
        private float hoverAlpha = 1.0f;
        [SerializeField, Tooltip("マウスオーバー時、不透明度が上がる早さ")]
        private float fadeFastDuration = 0.1f;
        [SerializeField, Tooltip("マウスが離れた時、不透明度が下がる早さ")]
        private float fadeSlowDuration = 0.4f;

        private Vector3 originalScale;
        private CanvasGroup canvasGroup;
        private Sequence hoverSequence;

        private void Awake()
        {
            originalScale = transform.localScale;

            if (useOpacity)
            {
                // CanvasGroupがアタッチされていなければ自動追加
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = normalAlpha;
            }
        }

        private void OnEnable()
        {
            // 初期状態に戻す
            transform.localScale = originalScale;
            if (useOpacity && canvasGroup != null)
            {
                canvasGroup.alpha = normalAlpha;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 実行中のアニメーションが重ならないようにリセット
            hoverSequence?.Kill();
            transform.DOKill();

            // スケール：すこし大きくなって元の大きさに戻る
            hoverSequence = DOTween.Sequence()
                .Append(transform.DOScale(originalScale * scaleMultiplier, scaleDuration).SetEase(Ease.OutQuad))
                .Append(transform.DOScale(originalScale, scaleDuration).SetEase(Ease.InOutQuad));

            // 不透明度：かなり早くMaxになる
            if (useOpacity && canvasGroup != null)
            {
                canvasGroup.DOKill();
                canvasGroup.DOFade(hoverAlpha, fadeFastDuration).SetEase(Ease.OutQuad);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // スケールアニメーションを破棄して元の大きさに戻す
            hoverSequence?.Kill();
            transform.DOScale(originalScale, scaleDuration).SetEase(Ease.OutQuad);

            // 不透明度：ゆっくり目で下がる
            if (useOpacity && canvasGroup != null)
            {
                canvasGroup.DOKill();
                canvasGroup.DOFade(normalAlpha, fadeSlowDuration).SetEase(Ease.OutQuad);
            }
        }

        private void OnDestroy()
        {
            // オブジェクト破棄時にTweenをキル（メモリリーク防止）
            hoverSequence?.Kill();
            transform.DOKill();
            if (canvasGroup != null) canvasGroup.DOKill();
        }
    }
}