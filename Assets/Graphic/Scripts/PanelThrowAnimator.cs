using UnityEngine;
using DG.Tweening;

namespace UI.Graphics
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))] // 不透明度を操作するため必須にします
    public class PanelThrowAnimator : MonoBehaviour
    {
        [Header("Throw Settings")]
        [SerializeField, Tooltip("飛んでいく時間")] private float throwDuration = 0.6f;
        [SerializeField, Tooltip("放物線の高さ")] private float jumpPower = 150f;
        [SerializeField, Tooltip("奥に行ったときの小ささ")] private float targetScale = 0.15f;
        [SerializeField, Tooltip("回転数")] private int rotateCount = 2;

        [Header("Respawn Settings")]
        [SerializeField, Tooltip("当たって消えてから、再び生えてくるまでの秒数")]
        private float respawnDelay = 1.0f;
        [SerializeField, Tooltip("にょきっと生えるアニメーションの秒数")]
        private float respawnDuration = 0.4f;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Canvas parentCanvas;

        // 元の位置や大きさを記憶しておく変数
        private Vector3 defaultPosition;
        private Vector3 defaultScale;
        private Quaternion defaultRotation;

        // 投げている最中かどうか（連射防止用フラグ）
        private bool isThrowing = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            parentCanvas = GetComponentInParent<Canvas>();

            // ゲーム開始時の位置・大きさ・角度を「デフォルト」として記憶
            defaultPosition = rectTransform.localPosition;
            defaultScale = rectTransform.localScale;
            defaultRotation = rectTransform.localRotation;
        }

        public void ThrowAt(Vector2 screenTarget, System.Action onImpact = null)
        {
            // 投げている最中、またはリスポーン待ちの場合は何もしない（撃てない）
            if (isThrowing || parentCanvas == null) return;

            isThrowing = true; // ロックをかける

            rectTransform.DOKill();
            canvasGroup.DOKill();

            // ターゲット座標の計算
            Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTransform.parent as RectTransform,
                screenTarget,
                cam,
                out Vector3 targetWorldPos
            );

            Sequence sequence = DOTween.Sequence();

            // 1. 投げるアニメーション
            sequence.Append(rectTransform.DOJump(targetWorldPos, jumpPower, 1, throwDuration).SetEase(Ease.OutQuad))
                    .Join(rectTransform.DOScale(defaultScale.x * targetScale, throwDuration).SetEase(Ease.InCubic))
                    .Join(rectTransform.DORotate(new Vector3(0, 0, 360 * rotateCount), throwDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            // 2. 当たった瞬間（シュッとフェードアウトしてコールバックを呼ぶ）
            sequence.Append(canvasGroup.DOFade(0f, 0.1f))
                    .AppendCallback(() => onImpact?.Invoke());

            // 3. 復活までの待機時間（ここで少し待つ）
            sequence.AppendInterval(respawnDelay);

            // 4. 再出現の準備（元の位置に戻し、大きさをゼロにして見えない状態にする）
            sequence.AppendCallback(() =>
            {
                rectTransform.localPosition = defaultPosition;
                rectTransform.localRotation = defaultRotation;
                rectTransform.localScale = Vector3.zero; // ここから大きくする
                canvasGroup.alpha = 1f; // 不透明度は元に戻しておく
            });

            // 5. にょきっと生えるアニメーション（Ease.OutBackを使うと少しポヨンと飛び出ます）
            sequence.Append(rectTransform.DOScale(defaultScale, respawnDuration).SetEase(Ease.OutBack));

            // 6. すべてのアニメーションが終わったらロック解除
            sequence.OnComplete(() =>
            {
                isThrowing = false;
            });
        }
    }
}