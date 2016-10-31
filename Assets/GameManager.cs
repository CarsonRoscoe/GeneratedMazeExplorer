using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    void Awake() {
        if ( Instance == null ) {
            Instance = this;
        }
        else {
            Destroy( this );
        }
    }

    public void ResetRound() {
        GameObject.Find( "Player" ).GetComponent<PlayerMovement>().ResetToStart();
    }

    public void ToggleWalkThroughWalls() {
        SettingsManager.Instance.WalkThroughWalls = !SettingsManager.Instance.WalkThroughWalls;
    }
}
