using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;

        public Transform PlayerOne;
        public Transform PlayerTwo;
        public Transform Ball;

        void Awake() {
            if ( Instance == null ) {
                Instance = this;
            }
            else {
                Destroy( gameObject );
            }
        }

        void Start() {
            if (Application.platform == RuntimePlatform.Android) {
                GameDataManager.Instance.PlayerOneMovementType = MovementType.Mouse;
            } else {
                GameDataManager.Instance.PlayerOneMovementType = MovementType.KeyboardOrController;
                GameObject.Find( "Pause" ).SetActive( false );
            }
            GameDataManager.Instance.PlayerTwoMovementType = MovementType.AI;
            StartGame();
        }

        void Update() {
            if ( Input.GetKeyDown( KeyCode.Escape ) || Input.GetKeyDown(KeyCode.Joystick1Button7)) {
                TogglePauseGame();
            }
        }

        public void ResetScores() {
            GameDataManager.Instance.PlayerOneScore = 0;
            GameDataManager.Instance.PlayerTwoScore = 0;
            HUDManager.Instance.UpdateScores();
        }

        public void PlayerScored(int player) {
            var gameData = GameDataManager.Instance;
            switch ( player ) {
                case 1:
                    gameData.PlayerOneScore++;
                    if ( gameData.PlayerOneScore == gameData.MaxScore ) {
                        EndGame( player );
                    }
                    break;
                case 2:
                    gameData.PlayerTwoScore++;
                    if ( gameData.PlayerTwoScore == gameData.MaxScore ) {
                        EndGame( player );
                    }
                    break;
                default:
                    break;
            }
            HUDManager.Instance.UpdateScores();
        }

        public void TogglePauseGame() {
            HUDManager.Instance.ToggleInGameMenu();
            ConsoleManager.Instance.ToggleConsoleVisibility();
            GameDataManager.Instance.IsPaused = !GameDataManager.Instance.IsPaused;
        }

        public MovementType GetMovementType(int controlType) {
            MovementType movementType = MovementType.Mouse;
            switch ( controlType ) {
                case 0:
                    movementType = MovementType.Mouse;
                    break;
                case 1:
                    movementType = MovementType.KeyboardOrController;
                    break;
                case 2:
                    movementType = MovementType.AI;
                    break;
            }
            return movementType;
        }

        public void ChangePlayerControls(int player) {
            if (player == 1) {
                var dropDown = GameObject.Find( "PlayerOneControls" ).GetComponent<Dropdown>();
                GameDataManager.Instance.PlayerOneMovementType = GetMovementType( dropDown.value );
                AddController( PlayerOne.gameObject, GameDataManager.Instance.PlayerOneMovementType, player );
            } else {
                var dropDown = GameObject.Find( "PlayerTwoControls" ).GetComponent<Dropdown>();
                GameDataManager.Instance.PlayerTwoMovementType = GetMovementType( dropDown.value );
                AddController( PlayerTwo.gameObject, GameDataManager.Instance.PlayerTwoMovementType, player );
            }
        }

        public void StartGame() {
            AddController( PlayerOne.gameObject, GameDataManager.Instance.PlayerOneMovementType, 1 );
            AddController( PlayerTwo.gameObject, GameDataManager.Instance.PlayerTwoMovementType, 2 );
            GameDataManager.Instance.GameInProgress = true;
            HUDManager.Instance.StartGame();
            CameraManager.Instance.CapturePositions();
        }

        public void ChangeTurns() {
            if (GameDataManager.Instance.IsPVP) {
                CameraManager.Instance.SwapPlayer();
            }
        }

        public void RestartGame() {
            CenterPlayer( PlayerOne.gameObject );
            CenterPlayer( PlayerTwo.gameObject );
            ResetCourt( Ball.gameObject );
            StartGame();
        }

        public void EndGame(int winningPlayer) {
            SceneManager.LoadScene( "GameScene" );
            //ResetScores();
            //GameDataManager.Instance.GameInProgress = false;
            //HUDManager.Instance.EndGame( winningPlayer );
        }

        public void ResetCourt(GameObject ball) {
            if (ball != null ) {
                var ballBehaviour = ball.GetComponent<BallBehaviour>();
                var rigidBody = ball.GetComponent<Rigidbody>();

                if ( ballBehaviour != null && rigidBody != null) {
                    var gameData = GameDataManager.Instance;

                    var newBallZ = gameData.MinimumCourtZ + gameData.CourtWidth / 2f;
                    var newBallY = gameData.MinimumCourtY + gameData.CourtHeight / 2f;
                    var newBallX = 0f;

                    ball.transform.position = new Vector3( newBallX, newBallY, newBallZ );
                    
                    ballBehaviour.RandomizeForce(gameData.WhichPlayerScoredLast);
                }
            }
        }

        private void AddController(GameObject player, MovementType movementType, int playerNumber) {
            var playerMovement = player.GetComponent<BaseMovement>();
            if ( playerMovement != null ) {
                Destroy( playerMovement );
            }
            switch ( movementType ) {
                case MovementType.KeyboardOrController:
                    player.AddComponent( typeof( KeyboardPlayerMovement ) );
                    player.GetComponent<KeyboardPlayerMovement>().PlayerNumber = playerNumber;
                    break;
                case MovementType.Mouse:
                    player.AddComponent( typeof( MousePlayerMovement ) );
                    player.GetComponent<MousePlayerMovement>().PlayerNumber = playerNumber;
                    break;
                case MovementType.AI:
                    var AI = player.AddComponent( typeof( AIMovement ) ) as AIMovement;
                    AI.Ball = Ball;
                    break;
            }
        }

        private void CenterPlayer(GameObject player) {
            var gameData = GameDataManager.Instance;
            var newZ = gameData.MinimumCourtZ + gameData.CourtWidth / 2f;
            var newY = gameData.MinimumCourtY + gameData.CourtHeight / 2f;
            player.transform.position = new Vector3( player.transform.position.x, newY, newZ );
        }
    }
}
