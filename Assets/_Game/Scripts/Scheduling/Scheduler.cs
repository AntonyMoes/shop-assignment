using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Scheduling {
    public class Scheduler : MonoBehaviour, IScheduler {
        private readonly List<IFrameProcessor> _frameProcessors = new();

        public void RegisterFrameProcessor(IFrameProcessor frameProcessor) {
            if (!_frameProcessors.Contains(frameProcessor)) {
                _frameProcessors.Add(frameProcessor);
            }
        }

        public void UnregisterFrameProcessor(IFrameProcessor frameProcessor) {
            _frameProcessors.Remove(frameProcessor);
        }

        private void Update() {
            var deltaTime = Time.deltaTime;
            foreach (var frameProcessor in _frameProcessors) {
                frameProcessor.ProcessFrame(deltaTime);
            }
        }
    }
}