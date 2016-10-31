using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour {
    public float RotationSpeed = 5f;
    private float m_turningStartDirection;
    private float m_turning = 0;
    private float m_walking = 0;
    private bool m_blockInput = false;

    // Update is called once per frame
    void Update() {
        if ( m_blockInput ) {
            bool actionComplete = false;

            if ( m_turning < 0 ) { //Turn left
                //transform.Rotate( Vector3.down * (RotationSpeed * Time.deltaTime) );
                //print( transform.rotation );
            }
            else if ( m_turning > 0 ) { //Turn right
            }
            else if ( m_walking > 0 ) { //Walk forward
            }
            else if ( m_walking < 0 ) { //Walk backwards
            }

            if ( actionComplete ) {
                m_walking = 0;
                m_turning = 0;
                m_blockInput = false;
            }
        }
        else {
            if ( !StartTurning( Input.GetAxis( "Horizontal" ) ) ) {
                StartWalking( Input.GetAxis( "Vertical" ) );
            }
        }
    }


    bool StartTurning( float direction ) {
        if ( direction < 0 ) {
            m_blockInput = true;
            StartCoroutine( RotateMe( Vector3.down * 90, 1 ) );
        }
        else if (direction > 0) {
            m_blockInput = true;
            StartCoroutine( RotateMe( Vector3.up * 90, 1 ) );
        } else {
            return false;
        }
        return true;
    }

    bool StartWalking( float direction ) {
        if ( direction > 0 ) {
            m_blockInput = true;
            StartCoroutine( WalkMe( transform.position + transform.forward * 2, 1 ) );
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
        m_blockInput = false;
        TakeTurn();
    }

    IEnumerator WalkMe( Vector3 point, float inTime ) {
        var offset = (point - transform.position);
        var ticks = inTime / Time.deltaTime;
        var movementPerTick = offset / ticks;
        var moved = Vector3.zero;
        for ( var i = 0; i < ticks; i++ ) {
            transform.position += movementPerTick;
            moved += movementPerTick;
            yield return null;
        }
        print( "MOVED " + moved.ToString() );
        m_blockInput = false;
        TakeTurn();
    }

    void TakeTurn() {
        TurnManager.Instance.TakeTurn();
    }
}
