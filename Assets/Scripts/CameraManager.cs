using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class CameraManager : MonoBehaviour {
        public static CameraManager Instance;
        public GameObject Camera;
        public GameObject PlayerOne;
        public GameObject PlayerTwo;
        public GameObject PlayerOneGoal;
        public GameObject PlayerTwoGoal;
        private CameraState m_followPlayerOne;
        private CameraState m_followPlayerTwo;
        private bool m_isFollowingPlayerOne;
        private Transparent m_goalOneTransparent;
        private Transparent m_goalTwoTransparent;
        public bool IsFollowingPlayerOne { get { return m_isFollowingPlayerOne; } }

        public void SwapPlayer() {
            m_isFollowingPlayerOne = !m_isFollowingPlayerOne;
            RefollowCurrentPlayer();
        }

        public void FollowPlayerOne() {
            if (!m_isFollowingPlayerOne) {
                m_isFollowingPlayerOne = true;
                RefollowCurrentPlayer();
            }
        }

        public void FollowPlayerTwo() {
            if (m_isFollowingPlayerOne) {
                m_isFollowingPlayerOne = false;
                RefollowCurrentPlayer();
            }
        }

        private void RefollowCurrentPlayer() {
            var cameraState = m_isFollowingPlayerOne ? m_followPlayerOne : m_followPlayerTwo;
            m_goalOneTransparent.SetTransparent( m_isFollowingPlayerOne );
            m_goalTwoTransparent.SetTransparent( !m_isFollowingPlayerOne );
            Camera.transform.position = cameraState.Position;
            Camera.transform.rotation = cameraState.Rotation;
        }

        public void ResetCamera() {
            if ( Camera != null && m_followPlayerOne != null ) {
                Camera.transform.position = m_followPlayerOne.Position;
                Camera.transform.rotation = m_followPlayerOne.Rotation;
            }
            m_isFollowingPlayerOne = true;
        }

        public void CapturePositions() {
            var cameraOffset = Camera.transform.position - PlayerOne.transform.position;
            m_followPlayerOne = new CameraState( Camera.transform.position, Camera.transform.rotation );

            var cameraPositionPlayerTwo = new Vector3( -cameraOffset.x, cameraOffset.y, cameraOffset.z );
            var cameraRotationPlayerTwo = Camera.transform.rotation * Quaternion.Euler( 180, 0, 180 );
            m_followPlayerTwo = new CameraState( PlayerTwo.transform.position + cameraPositionPlayerTwo, cameraRotationPlayerTwo );

            m_goalOneTransparent = PlayerOneGoal.GetComponent<Transparent>();
            m_goalTwoTransparent = PlayerTwoGoal.GetComponent<Transparent>();
        }

        void Awake() {
            if ( Instance == null ) {
                Instance = this;
            }
            else {
                Destroy( gameObject );
            }
            m_isFollowingPlayerOne = true;
        }

        private class CameraState {
            private Vector3 _position;
            public Vector3 Position { get { return _position; } }

            private Quaternion _rotation;
            public Quaternion Rotation { get { return _rotation; } }

            public CameraState( Vector3 position, Quaternion rotation ) {
                _position = position;
                _rotation = rotation;
            }
        }
    }
}
