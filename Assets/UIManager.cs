﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;


    public Text EndGameText;

    public Button ResetGameButton;
    public Toggle ToggleFogButton;
    public Toggle ToggleDayNightButton;
    public Toggle ToggleWallClippingButton;
    public Toggle ToggleSoundButton;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        EndGameText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EndGame() {
        EndGameText.enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canMove = false;
        SettingsManager.Instance.GameOver = true;
    }

    public void ResetGame() {
        EndGameText.enabled = false;
        GameObject.FindGameObjectWithTag( "Enemy" ).GetComponent<EnemyMovement>().ClearPath();
        GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerMovement>().canMove = true;
        CreateMaze.Instance.ResetMaze();
        SettingsManager.Instance.GameOver = false;
    }

    public void ToggleFog(bool toggled) {
        GameManager.Instance.ToggleFog();
    }

    public void ToggleDayNight(bool toggled) {
        GameManager.Instance.ToggleDayNight();
    }

    public void ToggleClipping(bool toggled) {
        GameManager.Instance.ToggleWalkThroughWalls();
    }

    public void ToggleSound() {
        bool toggled = ToggleSoundButton.isOn;
        AudioManager.Instance.ToggleMusic(toggled);
    }

    public void ToggleFlashlight() {
        var light = GameObject.FindGameObjectWithTag( "Light" ).GetComponent<Light>();
        light.enabled = !light.enabled;
    }
}
