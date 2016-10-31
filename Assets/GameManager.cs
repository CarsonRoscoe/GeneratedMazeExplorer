using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private Shader m_dayShader;
    private Shader m_nightShader;

    void Awake() {
        if ( Instance == null ) {
            Instance = this;
        }
        else {
            Destroy( this );
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            SetToDay();
        }
        if (Input.GetKeyDown(KeyCode.P )) {
            SetToNight();
        }
    }

    void Start() {
        m_dayShader = Shader.Find( "Custom/DayShader" );
        m_nightShader = Shader.Find( "Custom/NightShader" );
        SettingsManager.Instance.ActiveShader = m_dayShader;
    }

    public void ResetRound() {
        GameObject.Find( "Player" ).GetComponent<PlayerMovement>().ResetToStart();
    }

    public void ToggleWalkThroughWalls() {
        SettingsManager.Instance.WalkThroughWalls = !SettingsManager.Instance.WalkThroughWalls;
    }

    public void SetToDay() {
        SettingsManager.Instance.ActiveShader = m_dayShader;
        ReloadWallShaders();
    }

    public void SetToNight() {
        SettingsManager.Instance.ActiveShader = m_nightShader;
        ReloadWallShaders();
    }

    private void ReloadWallShaders() {
        foreach ( var wall in GameObject.FindGameObjectsWithTag( "Wall" ) ) {
            wall.GetComponent<Renderer>().material.shader = SettingsManager.Instance.ActiveShader;
        }
    }
}
