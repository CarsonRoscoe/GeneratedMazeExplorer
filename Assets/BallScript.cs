using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine( KillAfterDelay() );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter( Collision collision ) {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy") {
            AudioManager.Instance.playSFX( AudioManager.SFXType.BALL );
        }
        if ( collision.gameObject.tag == "Enemy" ) {
            DataHandler.instance.data.hits += 1;
            DataHandler.instance.saveData();
            ScoreText.Instance.ScoreUpdated();
            Destroy( gameObject );
        }
    }

    IEnumerator KillAfterDelay() {
        yield return new WaitForSeconds( 5 );
        Destroy( gameObject );
    }
}
