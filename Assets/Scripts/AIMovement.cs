using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

namespace Assets.Scripts {
    public class AIMovement : BaseMovement {
        public Transform Ball;
        public float Speed = .125f;
        private GameDataManager m_gameData;
        private bool m_isTracking;
        private Vector3 m_ballTrackingOffset;

        void Start() {
            m_gameData = GameDataManager.Instance;
            m_isTracking = false;
        }

        // Update is called once per frame
        protected override void OnUpdate() {
            if ( (transform.position.x < 0 && Ball.transform.position.x < 0) ||
                 (transform.position.x > 0 && Ball.transform.position.x > 0) ) {
                if (!m_isTracking) {
                    m_isTracking = true;
                    m_ballTrackingOffset = new Vector3( 0, (UnityEngine.Random.value - .5f)*2, (UnityEngine.Random.value - .5f )*1.2f);
                }
                var differenceInY = Ball.transform.position.y - transform.position.y + m_ballTrackingOffset.y;
                var differenceInZ = Ball.transform.position.z - transform.position.z + m_ballTrackingOffset.z;

                float yAmountMoved = differenceInY > 0 ? Mathf.Min( Speed, differenceInY ) : Mathf.Max( -Speed, differenceInY );
                float zAmountMoved = differenceInZ > 0 ? Mathf.Min( Speed, differenceInZ ) : Mathf.Max( -Speed, differenceInZ );

                var newY = transform.position.y + yAmountMoved;
                newY = Mathf.Min( newY, m_gameData.MaximumCourtY );
                newY = Mathf.Max( newY, m_gameData.MinimumCourtY );

                var newZ = transform.position.z + zAmountMoved;
                newZ = Mathf.Min( newZ, m_gameData.MaximumCourtZ );
                newZ = Mathf.Max( newZ, m_gameData.MinimumCourtZ );

                var newPosition = new Vector3( transform.position.x, newY, newZ );
                transform.position = newPosition;
            } else {
                m_isTracking = false;
            }
        }
    }
}