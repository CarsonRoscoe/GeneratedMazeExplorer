using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    public static ScoreText Instance;
    public Text Text;

	// Use this for initialization
	void Awake () {
	   if (Instance == null ) {
            Instance = this;
        } else {
            Destroy( this );
        }
	}
	
	public void ScoreUpdated() {
        Text.text = string.Format( "Score: {0}", DataHandler.instance.data.hits );
    }
}
