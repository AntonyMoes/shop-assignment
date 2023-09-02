using _Game.Scripts.Scheduling;
using UnityEngine;
using CharacterController = _Game.Scripts.Character.CharacterController;

namespace _Game.Scripts {
    public class App : MonoBehaviour {
        [SerializeField] private Scheduler _scheduler;
        
        [Header("Character")]
        [SerializeField] private CharacterController _characterPrefab;
        [SerializeField] private Transform _characterParent;
        [SerializeField] private Transform _spawnPoint;

        private void Start() {
            var gameRunner = new GameRunner(_characterPrefab, _characterParent, _spawnPoint, _scheduler);
            gameRunner.Start();
        }
    }
}