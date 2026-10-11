
namespace ShopGame.Save.Runtime
{
    public static class SaveSlotSelection
    {
        public const int MaxSaveSlots = 3;

        public static int SelectedSlot { get; private set; } = 1;

        public static void SelectSlot(int slot)
        {
            if (slot < 1 || slot > MaxSaveSlots)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(slot),
                    $"Slot must be between 1 and {MaxSaveSlots}.");
            }

            SelectedSlot = slot;
        }
    }
}
