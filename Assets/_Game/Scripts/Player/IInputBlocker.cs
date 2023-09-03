using GeneralUtils;

namespace _Game.Scripts.Player {
    public interface IInputBlocker {
        public IUpdatedValue<bool> InputBlocked { get; }
    }
}