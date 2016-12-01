using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public enum MovementType { Mouse, KeyboardOrController, AI }

    public class GameDataManager : MonoBehaviour {
        public static GameDataManager Instance;

        public Transform LeftWall;
        public Transform RightWall;
        public Transform Ceiling;
        public Transform Floor;
        public int MaxScore = 3;

        [HideInInspector]
        public float MinimumCourtZ;
        [HideInInspector]
        public float MaximumCourtZ;
        [HideInInspector]
        public float MinimumCourtY;
        [HideInInspector]
        public float MaximumCourtY;
        [HideInInspector]
        public float CourtWidth;
        [HideInInspector]
        public float CourtHeight;
        [HideInInspector]
        public bool GameInProgress;
        [HideInInspector]
        public bool IsPVP;
        [HideInInspector]
        public int WhichPlayerScoredLast;
        [HideInInspector]
        public bool IsPaused;

        private MovementType _playerOneMovementType;
        public MovementType PlayerOneMovementType {
            get {
                return _playerOneMovementType;
            }
            set {
                _playerOneMovementType = value;
                IsPVP = DetermineIfPVP( _playerOneMovementType, PlayerTwoMovementType );
            }
        }

        private MovementType _playerTwoMovementType;
        public MovementType PlayerTwoMovementType {
            get {
                return _playerTwoMovementType;
            }
            set {
                _playerTwoMovementType = value;
                IsPVP = DetermineIfPVP( PlayerOneMovementType, _playerTwoMovementType );
            }
        }

        private int _playerOneScore;
        public int PlayerOneScore {
            get {
                return _playerOneScore;
            }
            set {
                if ( value >= 0 ) {
                    _playerOneScore = value;
                }
                WhichPlayerScoredLast = 1;
            }
        }

        private int _playerTwoScore;
        public int PlayerTwoScore {
            get {
                return _playerTwoScore;
            }
            set {
                if (value >= 0) {
                    _playerTwoScore = value;
                }
                WhichPlayerScoredLast = 2;
            }
        }


        void Awake() {
            if ( Instance == null ) {
                Instance = this;
            }
            else {
                Destroy( gameObject );
            }
        }

        // Use this for initialization
        void Start() {
            MinimumCourtZ = LeftWall.position.z + ((transform.lossyScale.z) / 2 + .5f);
            MaximumCourtZ = RightWall.position.z - ((transform.lossyScale.z) / 2 + .5f);
            MinimumCourtY = Floor.position.y + ((transform.lossyScale.y) / 2 + .5f);
            MaximumCourtY = Ceiling.position.y - ((transform.lossyScale.y) / 2 + .5f);
            CourtWidth = MaximumCourtZ - MinimumCourtZ;
            CourtHeight = MaximumCourtY - MinimumCourtY;
            IsPaused = false;
        }

        bool DetermineIfPVP(MovementType playerOne, MovementType playerTwo) {
            return !(playerOne == MovementType.AI || playerTwo == MovementType.AI);
        }
    }
}
