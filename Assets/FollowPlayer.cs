using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public Transform Player;
    public int LookRange = 45;
    private int m_restAngle;

    void Start() {
        CaptureCameraAngle();
    }

	// Update is called once per frame
	void Update () {
        transform.position = Player.position;
        var height = Screen.height;
        var mouseY = Input.mousePosition.y;
        var ratio = mouseY / height;
        var angleOffset = ratio * LookRange - LookRange/2;
        transform.eulerAngles = new Vector3( m_restAngle - angleOffset, Player.eulerAngles.y, Player.eulerAngles.z );
    }

    public void CaptureCameraAngle() {
        m_restAngle = (int)transform.eulerAngles.x;
    }
}
