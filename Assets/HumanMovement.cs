using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HumanMovement : MonoBehaviour {
    public float RotationSpeed = 5f;
    public float TurnTime = .5f;
    protected bool m_blockInput = false;
    [HideInInspector]
    public bool canMove = true;

    protected bool StartTurning( float direction ) {
        if ( direction < 0 ) {
            m_blockInput = true;
            StartCoroutine( RotateMe( Vector3.down * 90, TurnTime ) );
        }
        else if ( direction > 0 ) {
            m_blockInput = true;
            StartCoroutine( RotateMe( Vector3.up * 90, TurnTime ) );
        }
        else {
            return false;
        }
        return true;
    }

    public virtual bool StartWalking( float direction ) {
        if ( direction > 0 ) {
            var moveTo = transform.position + transform.forward * 2;
            if ( !CreateMaze.Instance.IsWorldCoordinateOccupied( moveTo ) || SettingsManager.Instance.WalkThroughWalls ) {
                m_blockInput = true;
                StartCoroutine( WalkMe( moveTo, TurnTime ) );
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
        return true;
    }

    protected IEnumerator RotateMe( Vector3 byAngles, float inTime ) {
        SnapToGrid();
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler( transform.eulerAngles + byAngles );
        for ( var t = 0f; t < 1; t += Time.deltaTime / inTime ) {
            transform.rotation = Quaternion.Lerp( fromAngle, toAngle, t );
            yield return null;
        }
        SnapToGrid();
        m_blockInput = false;
    }

    protected virtual IEnumerator WalkMe( Vector3 point, float inTime ) {
        SnapToGrid();
        var offset = (point - transform.position);
        for ( var t = 0f; t < 1; t += Time.deltaTime / inTime ) {
            transform.position += offset * (Time.deltaTime / inTime);
            yield return null;
        }
        m_blockInput = false;
        SnapToGrid();
    }

    protected void SnapToGrid() {
        var x = (int)Math.Round( transform.position.x );
        var z = (int)Math.Round( transform.position.z );
        transform.position = new Vector3( x, transform.position.y, z );

        var yRotation = Mathf.Round( transform.rotation.eulerAngles.y / 90 ) * 90;
        transform.eulerAngles = new Vector3( 0, yRotation, 0 );
    }
}
