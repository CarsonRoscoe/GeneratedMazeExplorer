using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public Transform Player;

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3( Player.position.x, 40, Player.position.z );	
	}
}
