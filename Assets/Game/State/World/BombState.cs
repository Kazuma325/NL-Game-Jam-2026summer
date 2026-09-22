namespace ShopGame.State.World
{
    public sealed class BombState
    {
        public BombStatus Status { get; private set; } = BombStatus.Armed;

        public bool IsArmed => Status == BombStatus.Armed;

        public bool IsDisarmed => Status == BombStatus.Disarmed;

        public void Arm()
        {
            Status = BombStatus.Armed;
        }

        public void Disarm()
        {
            Status = BombStatus.Disarmed;
        }

        public void Reset()
        {
            Arm();
        }
    }
}
