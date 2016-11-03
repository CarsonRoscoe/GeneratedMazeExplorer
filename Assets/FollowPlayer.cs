using UnityEngine;
using System.Collections;
using System;

public class FollowPlayer : MonoBehaviour {
    public Transform Player;
    public int LookRange = 90;
    private int m_restAngle;
    private float m_angleOffset;
    private float m_mouseY;
    private float m_rotateThreshold = 20;
    private float m_distance;

    private float m_joystickY;

    void Start() {
        CaptureCameraAngle();
        m_joystickY = Screen.height / 2;
    }

    // Update is called once per frame
    void Update() {
        transform.position = Player.position;

        //Look up/down on mobile
        if ( Application.isMobilePlatform ) {
            if ( Input.GetMouseButtonDown( 0 ) ) {
                m_mouseY = Input.mousePosition.y;
                m_distance = 0;
            }
            if ( Input.GetMouseButton( 0 ) ) {
                var newMouseY = Input.mousePosition.y;
                m_distance += (newMouseY - m_mouseY) / 40;
                if ( Mathf.Abs( m_distance ) > m_rotateThreshold ) {
                    var dist = 0f;
                    if ( m_distance > 0 ) {
                        dist = m_distance - m_rotateThreshold;
                    }
                    else {
                        dist = m_distance + m_rotateThreshold;
                    }
                    m_angleOffset = (m_angleOffset + dist).MinMax( -LookRange, LookRange );
                    transform.eulerAngles = Player.eulerAngles.WithX( m_restAngle - m_angleOffset );
                    m_mouseY = newMouseY;
                }
            } else {
                transform.eulerAngles = Player.eulerAngles.WithX( m_restAngle - m_angleOffset );
            }
        }
        //Look up/down on desktop
        else {
            if (Input.GetJoystickNames().Length == 0) {
                var height = Screen.height;
                var mouseY = Input.mousePosition.y;
                var ratio = mouseY / height;
                m_angleOffset = ratio * LookRange - LookRange / 2;
                transform.eulerAngles = Player.eulerAngles.WithX( m_restAngle - m_angleOffset );
            } else {
                var mouseChange = Input.GetAxis( "RightJoystickY" );

                m_joystickY += mouseChange * 2.5f;

                var ratio = m_joystickY / Screen.height;
                m_angleOffset = ratio * LookRange - LookRange / 2;
                transform.eulerAngles = Player.eulerAngles.WithX( m_restAngle - m_angleOffset );
            }
        }
    }

    public void CaptureCameraAngle() {
        m_restAngle = (int)transform.eulerAngles.x;
    }
}
