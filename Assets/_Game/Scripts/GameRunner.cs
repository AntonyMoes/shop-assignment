using _Game.Scripts.Scheduling;
using UnityEngine;
using CharacterController = _Game.Scripts.Character.CharacterController;

namespace _Game.Scripts {
    public class GameRunner {
        private readonly CharacterController _characterPrefab;
        private readonly Transform _characterParent;
        private readonly Transform _spawnPoint;
        private readonly IScheduler _scheduler;

        private CharacterController _character;
        private PlayerInput _playerInput;

        public GameRunner(CharacterController characterPrefab, Transform characterParent, Transform spawnPoint, IScheduler scheduler) {
            _characterPrefab = characterPrefab;
            _characterParent = characterParent;
            _spawnPoint = spawnPoint;
            _scheduler = scheduler;
        }

        public void Start() {
            CreateCharacter();
            InitInput();
        }

        private void CreateCharacter() {
            _character = Object.Instantiate(_characterPrefab, _characterParent);
            _character.transform.position = _spawnPoint.position;
        }

        private void InitInput() {
            _playerInput = new PlayerInput();
            _scheduler.RegisterFrameProcessor(_playerInput);
            _playerInput.DirectionalInput.Subscribe(_character.SetMovementDirection);
        }
    }
}