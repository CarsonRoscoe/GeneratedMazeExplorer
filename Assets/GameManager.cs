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
        SettingsManager.Instance.ActiveShader = Shader.Find( DetermineShader() );
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

    public void ToggleWalkThroughWalls(bool? toggled = null) {
        if (toggled.HasValue) {
            SettingsManager.Instance.WalkThroughWalls = toggled.Value;
        } else {
            SettingsManager.Instance.WalkThroughWalls = !SettingsManager.Instance.WalkThroughWalls;
        }
    }

    public void ToggleDayNight(bool? toggled = null) {
        if ( toggled.HasValue ) {
            m_isDay = toggled.Value;
        }
        else {
            m_isDay = !m_isDay;
        }
        ReloadWallShaders();
    }

    public void ToggleFog(bool? toggled = null) {
        if (toggled.HasValue) {
            m_hasFog = toggled.Value;
        } else {
            m_hasFog = !m_hasFog;
        }
        ReloadWallShaders();
    }

    private void ReloadWallShaders() {
        SettingsManager.Instance.ActiveShader = Shader.Find( DetermineShader() );

        foreach ( var wall in GameObject.FindGameObjectsWithTag( "Wall" ) ) {
            wall.GetComponent<RenderSurface>().Redraw();
        }
    }

    private string DetermineShader() {
      string shaderName = string.Empty;
      if ( m_hasFog ) {
        if ( m_isDay ) {
          shaderName = "Custom/FogDayShader";
        }
        else {
          shaderName = "Custom/FogNightShader";
        }
      }
      else {
        if ( m_isDay ) {
          shaderName = "Custom/DayShader";
        }
        else {
          shaderName = "Custom/NightShader";
        }
      }
      return shaderName;
    }
}
