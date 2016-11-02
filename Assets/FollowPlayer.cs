using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public Transform Player;
    public int LookRange = 90;
    private int m_restAngle;
    private float m_angleOffset;
    private float m_mouseY;
    private float m_rotateThreshold = 20;
    private float m_distance;

    void Start() {
        CaptureCameraAngle();
    }

    // Update is called once per frame
    void Update() {
        transform.position = Player.position;

        //Look up/down on mobile
        if ( !Application.isMobilePlatform ) {
            if ( Input.GetMouseButtonDown( 0 ) ) {
                m_mouseY = Input.mousePosition.y;
                m_distance = 0;
            }
            if ( Input.GetMouseButton( 0 ) ) {
                var newMouseY = Input.mousePosition.y;
                m_distance += (newMouseY - m_mouseY) / 12;
                if (Mathf.Abs(m_distance) > m_rotateThreshold) {
                    var dist = 0f;
                    if (m_distance > 0) {
                        dist = m_distance - m_rotateThreshold;
                    } else {
                        dist = m_distance + m_rotateThreshold;
                    }
                    m_angleOffset = (m_angleOffset + dist).MinMax( -LookRange, LookRange );
                    transform.eulerAngles = Player.eulerAngles.WithX( m_restAngle - m_angleOffset );
                    m_mouseY = newMouseY;
                }
            }
        }
        //Look up/down on desktop
        else {
            var height = Screen.height;
            var mouseY = Input.mousePosition.y;
            var ratio = mouseY / height;
            m_angleOffset = ratio * LookRange - LookRange / 2;
            transform.eulerAngles = Player.eulerAngles.WithX( m_restAngle - m_angleOffset );
        }
    }

    public void CaptureCameraAngle() {
        m_restAngle = (int)transform.eulerAngles.x;
    }
}
