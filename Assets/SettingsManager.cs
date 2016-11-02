using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance;

    public bool WalkThroughWalls { get; set; }
    public Vector3 PlayerStartPosition { get; set; }
    public Vector3 PlayerStartEuler { get; set; }
    public Shader ActiveShader { get; set; }
    public bool GameOver { get; set; }

    void Awake() {
      if (Instance == null ) {
            Instance = this;
        } else {
            Destroy( this );
        }
        ResetDefault();
    }

    void ResetDefault() {
        WalkThroughWalls = false;
    }
}
