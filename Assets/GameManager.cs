using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public bool isDay;
    public bool hasFog;

    void Awake() {
        if ( Instance == null ) {
            Instance = this;
        }
        else {
            Destroy( this );
        }
        isDay = true;
        hasFog = false;
        SettingsManager.Instance.ActiveShader = Shader.Find( DetermineShader() );
        SettingsManager.Instance.GameOver = false;
    }

    void Start() {
        if ( Application.isMobilePlatform )
            Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update() {
        //Check if the user double tapped on mobile
        if ( Input.GetMouseButtonDown( 0 ) ) {
            for ( int i = 0; i < Input.touchCount; i++ ) {
                if ( Input.GetTouch( i ).phase == TouchPhase.Began ) {
                    if ( Input.GetTouch( i ).tapCount == 2 ) {
                        GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerMovement>().StartWalking( 1f );
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Home)) {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.E)) {
            ToggleWalkThroughWalls();
        }
    }

    public void ResetGame() {
        UIManager.Instance.ResetGame();
    }

    public void ToggleWalkThroughWalls( bool? toggled = null ) {
        if ( toggled.HasValue ) {
            SettingsManager.Instance.WalkThroughWalls = toggled.Value;
        }
        else {
            SettingsManager.Instance.WalkThroughWalls = !SettingsManager.Instance.WalkThroughWalls;
        }
    }

    public void ToggleDayNight( bool? toggled = null ) {
        if ( toggled.HasValue ) {
            isDay = toggled.Value;
        }
        else {
            isDay = !isDay;
        }
        AudioManager.Instance.ToggleMusic(AudioManager.Instance.playMusic);
        ReloadWallShaders();
    }

    public void ToggleFog( bool? toggled = null ) {
        if ( toggled.HasValue ) {
            hasFog = toggled.Value;
        }
        else {
            hasFog = !hasFog;
        }
        AudioManager.Instance.CalculateMusicVolume(DistanceCalculator.Instance.calculateDistance());
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
        if ( hasFog ) {
            if ( isDay ) {
                shaderName = "Custom/FogDayShader";
            }
            else {
                shaderName = "Custom/FogNightShader";
            }
        }
        else {
            if ( isDay ) {
                shaderName = "Custom/DayShader";
            }
            else {
                shaderName = "Custom/NightShader";
            }
        }
        return shaderName;
    }
}
