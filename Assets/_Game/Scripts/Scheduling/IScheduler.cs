namespace _Game.Scripts.Scheduling {
    public interface IScheduler {
        void RegisterFrameProcessor(IFrameProcessor frameProcessor);
        void UnregisterFrameProcessor(IFrameProcessor frameProcessor);
    }
}