using UnityEngine;
using System.Collections;

namespace Assets.Scripts {
    public enum BallDirection { Forward, Backward }
    public class BallBehaviour : PausableObject {
        public int MinimumSpeed = 20;
        public int MaximumYZSpeed = 10;
        public int XForceSpeed = 500;
        public int YZForceSpeed = 200;
        private BallDirection m_currentDirection;
        public BallDirection CurrentDirection { get { return m_currentDirection; } }
        private Vector3 m_pausedVelocity;
        private Rigidbody m_rigidBody;

        void Start() {
            RandomizeForce( 1 );
            m_currentDirection = BallDirection.Forward;
        }

        void OnCollisionEnter( Collision collision ) {
            var collider = collision.collider;
            if ( collider.tag == "Paddle" ) {
                var paddlePosition = collision.collider.transform.position;
                var yDifference = transform.position.y - paddlePosition.y;
                var zDifference = transform.position.z - paddlePosition.z;
                var colliderScale = collider.transform.lossyScale;
                var forceSpeed = 300;

                m_rigidBody.AddForce( 0, yDifference * forceSpeed / colliderScale.y, zDifference * forceSpeed / colliderScale.z );
            }
        }

        protected override void OnUpdate() {
            RegulateVelocity();
        }

        void RegulateVelocity() {
            var addForceX = 0;
            var addForceY = 0;
            var addForceZ = 0;
            var forceYZModificationRate = 3;
            var forzeXModifacationRate = 5;

            //If we are going Forward on X axis
            if ( m_rigidBody.velocity.x > 0 ) {
                addForceX += (m_rigidBody.velocity.x < MinimumSpeed) ? forzeXModifacationRate : 0;
                if (GameDataManager.Instance.PlayerOneMovementType != MovementType.AI) {
                    CameraManager.Instance.FollowPlayerOne();
                }
            }
            //If we are going Backwards on X axis
            else if ( m_rigidBody.velocity.x < 0 ) {
                addForceX -= (m_rigidBody.velocity.x > -MinimumSpeed) ? forzeXModifacationRate : 0;
                if ( GameDataManager.Instance.PlayerTwoMovementType != MovementType.AI ) {
                    CameraManager.Instance.FollowPlayerTwo();
                }
            }

            //If we are going down at too fast of a rate, edge it back to our acceptable rate
            addForceY += (m_rigidBody.velocity.y < -MaximumYZSpeed) ? forceYZModificationRate : 0;

            //If we are going up at too fast of a rate, edge it back to our acceptable rate
            addForceY -= (m_rigidBody.velocity.y > MaximumYZSpeed) ? forceYZModificationRate : 0;

            //If we are going to the left too fast, edge it back to our acceptable rate
            addForceZ += (m_rigidBody.velocity.z < -MaximumYZSpeed) ? forceYZModificationRate : 0;

            //If we are going to the right too fast, edge it back to our acceptable rate
            addForceZ -= (m_rigidBody.velocity.z > MaximumYZSpeed) ? forceYZModificationRate : 0;

            m_rigidBody.AddForce( addForceX, addForceY, addForceZ );
        }

        public void RandomizeForce( int playerTurn ) {
            m_rigidBody = GetComponent<Rigidbody>();
            m_rigidBody.velocity = Vector3.zero;
            var xForce = playerTurn == 1 ? XForceSpeed : -XForceSpeed;
            var yForce = Random.value * YZForceSpeed - YZForceSpeed / 2;
            var zForce = Random.value * YZForceSpeed - YZForceSpeed / 2;
            m_rigidBody.AddForce( xForce, yForce, zForce );
        }

        protected override void OnPause() {
            m_pausedVelocity = m_rigidBody.velocity;
            m_rigidBody.velocity = new Vector3( 0, 0, 0 );
        }

        protected override void OnResume() {
            m_rigidBody.velocity = m_pausedVelocity;
        }
    }
}