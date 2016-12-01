using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : HumanMovement {
    public Transform Projectile;
    private float m_mouseX;
    private int m_turnThreshold = 150;
    private bool m_canThrow;

    void Start() {
        m_canThrow = true;
        TurnManager.Instance.Subscribe( GetInstanceID(), TurnOnCanThrow );
    }

    // Update is called once per frame
    void Update() {
        if ( canMove && !m_blockInput ) {
            if ( Application.isMobilePlatform ) {
                //Move on mobile
                if ( Input.GetMouseButtonDown( 0 ) ) {
                    m_mouseX = Input.mousePosition.x;
                }
                if ( Input.GetMouseButton( 0 ) ) {
                    var newMouseX = Input.mousePosition.x;
                    var dif = (newMouseX - m_mouseX);
                    if ( dif > m_turnThreshold ) {
                        StartTurning( .5f );
                    }
                    else if ( dif < -m_turnThreshold ) {
                        StartTurning( -.5f );
                    }
                }
            }
            else {
                //Move on desktop
                if ( !StartTurning( Input.GetAxis( "Horizontal" ) ) ) {
                    StartWalking( Input.GetAxis( "Vertical" ) );
                }

            }
        }
        if ( Input.GetAxis( "Fire" ) > 0 ) {
            ThrowBall();
        }
    }

    void TurnOnCanThrow() {
        m_canThrow = true;
    }

    void ThrowBall() {
        if ( m_canThrow ) {
            var ball = Instantiate( Projectile );
            ball.transform.position = transform.position + transform.forward/2 - Vector3.up/2;
            ball.GetComponent<Rigidbody>().AddForce( transform.forward * 200 );
            m_canThrow = false;
        }
    }


    protected override IEnumerator WalkMe( Vector3 point, float inTime ) {
        yield return base.WalkMe( point, inTime );
        TakeTurn();
    }

    void TakeTurn() {
        TurnManager.Instance.TakeTurn();
    }


    public override bool StartWalking( float direction ) {
        if ( direction > 0 ) {
            var moveTo = transform.position + transform.forward * 2;
            if ( !CreateMaze.Instance.IsWorldCoordinateOccupied( moveTo ) || SettingsManager.Instance.WalkThroughWalls ) {
                CreateMaze.Instance.CalculatePooling( moveTo );
                AudioManager.Instance.playSFX( AudioManager.SFXType.STEP );
                AudioManager.Instance.CalculateMusicVolume( DistanceCalculator.Instance.calculateDistance() );
                var point = CreateMaze.Instance.GetWorldPoint( moveTo );
                var obj = CreateMaze.Instance.Maze[(int)point.x, (int)point.y].positionObject;
                if (obj.HasComponent<RenderWall>() && !obj.GetComponent<RenderWall>().IsDoor ) {
                    DataHandler.instance.data.player.toCoordinate( moveTo );
                    DataHandler.instance.saveData();
                }
            }
            else {
                AudioManager.Instance.playSFX( AudioManager.SFXType.WALL );
            }
            //print( string.Format( "Player Position: {0}", CreateMaze.Instance.GetWorldPoint( transform.position ).ToString() ) );
        }
        return base.StartWalking( direction );
    }

    public void ResetToStart() {
        transform.position = SettingsManager.Instance.PlayerStartPosition;
        transform.eulerAngles = SettingsManager.Instance.PlayerStartEuler;
    }
}
