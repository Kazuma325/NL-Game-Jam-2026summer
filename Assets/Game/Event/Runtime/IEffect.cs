using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public interface IEffect
    {
        bool Execute(GameContext context);
    }
}