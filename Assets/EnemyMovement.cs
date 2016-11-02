using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyMovement : HumanMovement {
    Animation m_anim;
    Stack<CreateMaze.Coordinate> m_coordinates;

    enum Direction { North, East, South, West }

    // Use this for initialization
    void Start() {
        TurnManager.Instance.Subscribe( GetInstanceID(), Move );
        m_anim = GetComponent<Animation>();
        m_anim.enabled = true;
    }

    ~EnemyMovement() {
        TurnManager.Instance.Unsubscribe( GetInstanceID() );
    }

    // Update is called once per frame
    void Update() {

    }

    public void StopWalkingAnimation() {
        m_anim.Stop();
    }

    public void StartWalkingAnimation() {
        m_anim.Play();
    }

    private Direction DetermineFacing( Vector2 from, Vector2 to ) {
        if ( from.x == to.x )
            if ( from.y < to.y )
                return Direction.South;
            else
                return Direction.North;
        else
    if ( from.x < to.x )
            return Direction.East;
        else
            return Direction.West;
    }

    private List<float> DetermineIfTurn( CreateMaze.Coordinate coordinateTo ) {
        Vector2 coordinateFrom = CreateMaze.Instance.GetWorldPoint( transform.position - transform.forward );
        Vector2 coordinateAt = CreateMaze.Instance.GetWorldPoint( transform.position );

        Direction facing;
        Direction needToFace;

        facing = DetermineFacing( coordinateFrom, coordinateAt );
        needToFace = DetermineFacing( coordinateAt, new Vector2( coordinateTo.x, coordinateTo.y ) );

        return FindTurningDifference( facing, needToFace );
    }

    private IEnumerator MoveOne( CreateMaze.Coordinate coordinate ) {
        foreach ( var turn in DetermineIfTurn( coordinate ) ) {
            if ( turn < 0 ) {
                SnapToGrid();
                var fromAngle = transform.rotation;
                var toAngle = Quaternion.Euler( transform.eulerAngles + Vector3.down * 90 );
                for ( var t = 0f; t < 1; t += Time.deltaTime / TurnTime ) {
                    transform.rotation = Quaternion.Lerp( fromAngle, toAngle, t );
                    yield return null;
                }
                SnapToGrid();
                m_blockInput = false;
            }
            else if ( turn > 0 ) {
                SnapToGrid();
                var fromAngle = transform.rotation;
                var toAngle = Quaternion.Euler( transform.eulerAngles + Vector3.up * 90 );
                for ( var t = 0f; t < 1; t += Time.deltaTime / TurnTime ) {
                    transform.rotation = Quaternion.Lerp( fromAngle, toAngle, t );
                    yield return null;
                }
                SnapToGrid();
                m_blockInput = false;
            }
        }
        var moveTo = transform.position + transform.forward * 2;
        SnapToGrid();
        var offset = (moveTo - transform.position);
        for ( var t = 0f; t < 1; t += Time.deltaTime / TurnTime ) {
            transform.position += offset * (Time.deltaTime / TurnTime);
            yield return null;
        }
        m_blockInput = false;
        SnapToGrid();
    }

    public Stack<CreateMaze.Coordinate> CalculatePath() {
        return CreateMaze.Instance.FindRandomPath( transform.position );
    }

    public void Move() {
        int tries = 5;
        while ( m_coordinates == null || m_coordinates.Count == 0 ) {
            m_coordinates = CalculatePath();
            m_coordinates.Pop();
            if (tries-- == 0) {
                print( "Move Callback hanging. CalculatePath is returning a path of size" + m_coordinates.Count + " post-pop" );
                return;
            }
        }
        var coordinate = m_coordinates.Pop();

        StartCoroutine( MoveOne( coordinate ) );
    }

    private List<float> FindTurningDifference( Direction from, Direction to ) {
        var result = new List<float>();

        switch ( from ) {
            case Direction.North:
                switch ( to ) {
                    case Direction.East:
                        return result.With( -1f );
                    case Direction.South:
                        return result.With( -1f, -1f );
                    case Direction.West:
                        return result.With( 1f );
                    case Direction.North:
                    default:
                        return result;
                }
            case Direction.East:
                switch ( to ) {
                    case Direction.North:
                        return result.With( 1f );
                    case Direction.South:
                        return result.With( -1f );
                    case Direction.West:
                        return result.With( -1f, -1f );
                    case Direction.East:
                    default:
                        return result;
                }
            case Direction.South:
                switch ( to ) {
                    case Direction.North:
                        return result.With( -1f, -1f );
                    case Direction.East:
                        return result.With( 1f );
                    case Direction.West:
                        return result.With( -1f );
                    case Direction.South:
                    default:
                        return result;
                }
            case Direction.West:
                switch ( to ) {
                    case Direction.North:
                        return result.With( -1f );
                    case Direction.East:
                        return result.With( -1f, -1f );
                    case Direction.South:
                        return result.With( 1f );
                    case Direction.West:
                    default:
                        return result;
                }
            default:
                return result;
        }
    }
}
