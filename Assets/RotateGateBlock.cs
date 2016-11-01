using UnityEngine;
using System.Collections;

public class RotateGateBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion rotate = gameObject.transform.rotation;
        gameObject.transform.Rotate(new Vector3(1, 1, -1));
	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Player") {
            UIManager.Instance.EndGame();
        }
    }
    
}
