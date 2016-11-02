using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyMovement : HumanMovement {
    Animation m_anim;
    Stack<CreateMaze.Coordinate> m_coordinates;
    CreateMaze.Coordinate m_currentCoordinate;

    enum Direction { North, East, South, West }

    // Use this for initialization
    void Start() {
        TurnManager.Instance.Subscribe( GetInstanceID(), Move );
        m_anim = GetComponent<Animation>();
        m_anim.enabled = true;
        m_coordinates = CalculatePath();
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

    private Direction DetermineFacing(Vector2 from, Vector2 to) {
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
        foreach(var turn in DetermineIfTurn(coordinate))
            yield return StartTurning( turn );
        yield return StartWalking( 1f );
    }

    public Stack<CreateMaze.Coordinate> CalculatePath() {
        return new Stack<CreateMaze.Coordinate>(); //Return Jaegar's method
    }

    public void Move() {
        if ( m_coordinates == null || m_coordinates.Count == 0 ) {
            m_coordinates = CalculatePath();
        }
        var coordinate = m_coordinates.Pop();

        StartCoroutine( MoveOne( coordinate ) );
    }

    private List<float> FindTurningDifference(Direction from, Direction to) {
        var result = new List<float>();

        switch ( from ) {
            case Direction.North:
                switch ( to ) {
                    case Direction.East:
                        return result.With( 1f );
                    case Direction.South:
                        return result.With( 1f, 1f );
                    case Direction.West:
                        return result.With( -1f );
                    case Direction.North:
                    default:
                        return result;
                }
            case Direction.East:
                switch ( to ) {
                    case Direction.North:
                        return result.With( -1f );
                    case Direction.South:
                        return result.With( 1f );
                    case Direction.West:
                        return result.With( 2f );
                    case Direction.East:
                    default:
                        return result;
                }
            case Direction.South:
                switch ( to ) {
                    case Direction.North:
                        return result.With( 1f, 1f );
                    case Direction.East:
                        return result.With( -1f );
                    case Direction.West:
                        return result.With( 1f );
                    case Direction.South:
                    default:
                        return result;
                }
            case Direction.West:
                switch ( to ) {
                    case Direction.North:
                        return result.With( 1f );
                    case Direction.East:
                        return result.With( 1f, 1f );
                    case Direction.South:
                        return result.With( -1f );
                    case Direction.West:
                    default:
                        return result;
                }
            default:
                return result;
        }
    }
}
