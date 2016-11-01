using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private bool m_isDay;
    private bool m_hasFog;

    void Awake() {
        if ( Instance == null ) {
            Instance = this;
        }
        else {
            Destroy( this );
        }
        m_isDay = true;
        m_hasFog = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            ToggleDayNight();
        }
        if (Input.GetKeyDown(KeyCode.P )) {
            ToggleFog();
        }
    }

    public void ResetRound() {
        GameObject.Find( "Player" ).GetComponent<PlayerMovement>().ResetToStart();
    }

    public void ToggleWalkThroughWalls() {
        SettingsManager.Instance.WalkThroughWalls = !SettingsManager.Instance.WalkThroughWalls;
    }

    public void ToggleDayNight() {
        m_isDay = !m_isDay;
        ReloadWallShaders();
    }

    public void ToggleFog() {
        m_hasFog = !m_hasFog;
        ReloadWallShaders();
    }

    private void ReloadWallShaders() {
        string shaderName = string.Empty;
        if (m_hasFog) {
            if (m_isDay) {
                shaderName = "Custom/FogDayShader";
            } else {
                shaderName = "Custom/FogNightShader";
            }
        } else {
            if (m_isDay) {
                shaderName = "Custom/DayShader";
            } else {
                shaderName = "Custom/NightShader";
            }
        }
        Shader shader = Shader.Find( shaderName );

        foreach ( var wall in GameObject.FindGameObjectsWithTag( "Wall" ) ) {
            wall.GetComponent<Renderer>().material.shader = shader;
        }
    }
}
