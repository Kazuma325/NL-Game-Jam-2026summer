using System;
using UnityEngine;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueLogInputView : MonoBehaviour
    {
        [SerializeField]
        private DialogueLogViewPresenter presenter;

        public void OnLogButtonClicked()
        {
            if (presenter == null)
            {
                throw new InvalidOperationException(
                    "Dialogue Log View Presenter is not assigned.");
            }

            presenter.Open();
        }

        public void OnCloseButtonClicked()
        {
            if (presenter == null)
            {
                throw new InvalidOperationException(
                    "Dialogue Log View Presenter is not assigned.");
            }

            presenter.Close();
        }
    }
}