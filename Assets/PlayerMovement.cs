using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour {
    public float RotationSpeed = 5f;
    public float TurnTime = .25f;
    private float m_turningStartDirection;
    private float m_turning = 0;
    private float m_walking = 0;
    private bool m_blockInput = false;

    // Update is called once per frame
    void Update() {
        if ( !m_blockInput ) {
            if ( !StartTurning( Input.GetAxis( "Horizontal" ) ) ) {
                StartWalking( Input.GetAxis( "Vertical" ) );
            }
        }
    }


    bool StartTurning( float direction ) {
        if ( direction < 0 ) {
            m_blockInput = true;
            StartCoroutine( RotateMe( Vector3.down * 90, TurnTime ) );
        }
        else if (direction > 0) {
            m_blockInput = true;
            StartCoroutine( RotateMe( Vector3.up * 90, TurnTime ) );
        } else {
            return false;
        }
        return true;
    }

    bool StartWalking( float direction ) {
        if ( direction > 0 ) {
            var moveTo = transform.position + transform.forward * 2;
            if (!CreateMaze.Instance.IsWorldCoordinateOccupied(moveTo) || SettingsManager.Instance.WalkThroughWalls) {
                m_blockInput = true;
                StartCoroutine( WalkMe( moveTo, TurnTime ) );
            } else {
                return false;
            }
        } else {
            return false;
        }
        return true;
    }

    IEnumerator RotateMe( Vector3 byAngles, float inTime ) {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler( transform.eulerAngles + byAngles );
        for ( var t = 0f; t < 1; t += Time.deltaTime / inTime ) {
            transform.rotation = Quaternion.Lerp( fromAngle, toAngle, t );
            yield return null;
        }
        SnapToGrid();
        m_blockInput = false;
        TakeTurn();
    }

    IEnumerator WalkMe( Vector3 point, float inTime ) {
        var offset = (point - transform.position);
        for ( var t = 0f; t < 1; t += Time.deltaTime / inTime ) {
            transform.position += offset * (Time.deltaTime / inTime);
            yield return null;
        }
        m_blockInput = false;
        SnapToGrid();
        TakeTurn();
    }

    void SnapToGrid() {
        var x = (int)Math.Round( transform.position.x );
        var z = (int)Math.Round( transform.position.z );
        transform.position = new Vector3( x, transform.position.y, z );

        var yRotation = Mathf.Round( transform.rotation.eulerAngles.y / 90 ) * 90;
        transform.eulerAngles = new Vector3( 0, yRotation, 0 );
    }

    void TakeTurn() {
        TurnManager.Instance.TakeTurn();
    }

    public void ResetToStart() {
        transform.position = SettingsManager.Instance.PlayerStartPosition;
        transform.eulerAngles = SettingsManager.Instance.PlayerStartEuler;
    }
}
