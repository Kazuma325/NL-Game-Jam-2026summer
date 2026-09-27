using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Graphics
{
    public class ThrowInputController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("投げる対象のパネル（シーン上のオブジェクト）")]
        private PanelThrowAnimator targetPanel;

        private InputAction clickAction;
        private InputAction positionAction;

        private void Awake()
        {
            clickAction = new InputAction(type: InputActionType.Button, binding: "<Pointer>/press");
            positionAction = new InputAction(type: InputActionType.Value, binding: "<Pointer>/position");
        }

        private void OnEnable()
        {
            clickAction.Enable();
            positionAction.Enable();
            clickAction.performed += OnClick;
        }

        private void OnDisable()
        {
            clickAction.Disable();
            positionAction.Disable();
            clickAction.performed -= OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (targetPanel == null) return;

            // マウスやタップの座標を取得
            Vector2 screenPosition = positionAction.ReadValue<Vector2>();

            // パネル側に投げる指示を出す（再装填中なら向こうで弾かれます）
            targetPanel.ThrowAt(screenPosition, () =>
            {
                Debug.Log("パネルを投げます");
            });
        }
    }
}