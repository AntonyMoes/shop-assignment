using _Game.Scripts.Scheduling;
using _Game.Scripts.UI;
using UnityEngine;
using CharacterController = _Game.Scripts.Character.CharacterController;

namespace _Game.Scripts {
    public class GameRunner {
        private readonly CharacterController _characterPrefab;
        private readonly Transform _characterParent;
        private readonly Transform _spawnPoint;
        private readonly IScheduler _scheduler;
        private readonly UIManager _uiManager;
        private readonly Map _map;
        private readonly Camera _gameCamera;

        private CharacterController _character;
        private Player.Player _player;

        public GameRunner(CharacterController characterPrefab, Transform characterParent, Transform spawnPoint,
            IScheduler scheduler, UIManager uiManager, Map map, Camera gameCamera) {
            _characterPrefab = characterPrefab;
            _characterParent = characterParent;
            _spawnPoint = spawnPoint;
            _scheduler = scheduler;
            _uiManager = uiManager;
            _map = map;
            _gameCamera = gameCamera;
        }

        public void Start() {
            InitUI();
            InitMap();
            CreateCharacter();
            InitPlayer();
        }

        private void InitUI() {
            _uiManager.Init(_gameCamera);
        }

        private void InitMap() {
            _map.Init(_uiManager);
        }

        private void CreateCharacter() {
            _character = Object.Instantiate(_characterPrefab, _characterParent);
            _character.transform.position = _spawnPoint.position;
        }

        private void InitPlayer() {
            _player = new Player.Player(_scheduler, _uiManager);
            _player.SetController(_character);
        }
    }
}