using _Game.Scripts.Scheduling;
using _Game.Scripts.UI;
using UnityEngine;
using CharacterController = _Game.Scripts.Character.CharacterController;

namespace _Game.Scripts {
    public class App : MonoBehaviour {
        [SerializeField] private Scheduler _scheduler;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private Map _map;
        [SerializeField] private Camera _gameCamera;

        [Header("Character")]
        [SerializeField] private CharacterController _characterPrefab;
        [SerializeField] private Transform _characterParent;
        [SerializeField] private Transform _spawnPoint;

        private void Start() {
            var gameRunner = new GameRunner(_characterPrefab, _characterParent, _spawnPoint, _scheduler, _uiManager,_map, _gameCamera);
            gameRunner.Start();
        }
    }
}