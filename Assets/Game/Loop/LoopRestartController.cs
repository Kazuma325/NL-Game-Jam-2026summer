
using ShopGame.Core.EventBus;
using ShopGame.Sound.Runtime;
using System;

namespace ShopGame.Loop
{
    public sealed class LoopRestartController
    {
        private readonly LoopManager loopManager;
        private readonly EventBus eventBus;
        private readonly LoopTransitionController transitionController;
        private readonly SeController seController;

        private bool restartPending;

        public LoopRestartController(
    LoopManager loopManager,
    EventBus eventBus,
    LoopTransitionController transitionController,
    SeController seController)
        {
            this.loopManager =
                loopManager
                ?? throw new ArgumentNullException(
                    nameof(loopManager));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            this.transitionController =
                transitionController;

            this.seController =
                seController
                ?? throw new ArgumentNullException(
                    nameof(seController));

            eventBus.Subscribe<LoopEndedEvent>(
                OnLoopEnded);
        }

        public void Update()
        {
            if (!restartPending)
                return;

            restartPending = false;

            // 演出が設定されていない場合の予備処理
            if (transitionController == null)
            {
                loopManager.RestartLoop();
                return;
            }

            seController.PlayLoopRestartSe();

            // 演出が白く覆い終わってからループを再開する
            transitionController.PlayLoopEffect(() =>
            {
                loopManager.RestartLoop();

                // 新しいループが始まった後に画面を戻す
                transitionController.PlayFadeFromWhite();
            });
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<LoopEndedEvent>(
                OnLoopEnded);
        }

        private void OnLoopEnded(LoopEndedEvent eventData)
        {
            restartPending = true;
        }
    }
}
