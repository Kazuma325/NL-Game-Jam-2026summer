using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace UI.Graphics
{
    [RequireComponent(typeof(Image))]
    public class AlphaHitButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Hit Test Settings")]
        [SerializeField, Tooltip("このアルファ値以上の部分だけクリックに反応する（0.1〜1.0）")]
        [Range(0f, 1f)]
        private float alphaThreshold = 0.5f;

        [SerializeField, Tooltip("子オブジェクトが操作された時は親が反応しないようにする")]
        private bool ignoreChildObjects = true;

        [Header("Scale Animation")]
        [SerializeField, Tooltip("マウスオーバー時にどれくらい大きくなるか")]
        private float hoverScaleMultiplier = 1.05f;
        [SerializeField, Tooltip("クリック時にどれくらい縮むか（1.0以下でへこむ）")]
        private float clickScaleMultiplier = 1f;
        [SerializeField, Tooltip("大きさが変化するスピード")]
        private float scaleDuration = 0.15f;

        [Header("Sprite & Fade Settings")]
        [SerializeField, Tooltip("通常時の画像")]
        private Sprite normalSprite;
        [SerializeField, Tooltip("マウスオーバー時（アウトライン等）の画像")]
        private Sprite hoverSprite;
        [SerializeField, Tooltip("アウトラインがフェードする秒数")]
        private float fadeDuration = 0.2f;

        private Vector3 originalScale;
        private Image targetImage;
        private Image hoverOverlayImage;
        private bool isHovering = false;
        private bool isInitialized = false; // 初期化が完了しているかのフラグ

        private void Awake()
        {
            originalScale = transform.localScale;
            targetImage = GetComponent<Image>();
            targetImage.alphaHitTestMinimumThreshold = alphaThreshold;

            if (normalSprite != null)
            {
                targetImage.sprite = normalSprite;
            }

            if (hoverSprite != null)
            {
                CreateHoverOverlay();
            }

            isInitialized = true;
        }

        private void CreateHoverOverlay()
        {
            GameObject overlayObj = new GameObject("HoverOverlay_Auto");
            overlayObj.transform.SetParent(transform, false);

            RectTransform rect = overlayObj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            hoverOverlayImage = overlayObj.AddComponent<Image>();
            hoverOverlayImage.sprite = hoverSprite;
            hoverOverlayImage.raycastTarget = false;

            Color color = hoverOverlayImage.color;
            color.a = 0f;
            hoverOverlayImage.color = color;
        }

        // ▼ 追加：UIが非表示になった瞬間に強制リセット ▼
        private void OnDisable()
        {
            if (!isInitialized) return;
            ResetState();
        }

        // ▼ 追加：状態をデフォルトに戻す処理 ▼
        private void ResetState()
        {
            isHovering = false;

            // アニメーションを停止してスケールを元に戻す
            transform.DOKill();
            transform.localScale = originalScale;

            // オーバーレイ画像の透明度を完全に0に戻す
            if (hoverOverlayImage != null)
            {
                hoverOverlayImage.DOKill();
                Color color = hoverOverlayImage.color;
                color.a = 0f;
                hoverOverlayImage.color = color;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ignoreChildObjects && eventData.pointerCurrentRaycast.gameObject != gameObject) return;
            isHovering = true;

            transform.DOKill();
            transform.DOScale(originalScale * hoverScaleMultiplier, scaleDuration).SetEase(Ease.OutQuad);

            if (hoverOverlayImage != null)
            {
                hoverOverlayImage.DOKill();
                hoverOverlayImage.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;

            transform.DOKill();
            transform.DOScale(originalScale, scaleDuration).SetEase(Ease.OutQuad);

            if (hoverOverlayImage != null)
            {
                hoverOverlayImage.DOKill();
                hoverOverlayImage.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (ignoreChildObjects && eventData.pointerCurrentRaycast.gameObject != gameObject) return;

            transform.DOKill();
            transform.DOScale(originalScale * clickScaleMultiplier, scaleDuration).SetEase(Ease.OutQuad);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            EventSystem.current?.SetSelectedGameObject(null);

            transform.DOKill();
            float targetMultiplier = isHovering ? hoverScaleMultiplier : 1.0f;
            transform.DOScale(originalScale * targetMultiplier, scaleDuration).SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            transform.DOKill();
            if (hoverOverlayImage != null) hoverOverlayImage.DOKill();
        }
    }
}