using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public bool playMusic;

    public AudioSource nightMusic;
    public AudioSource dayMusic;
    public AudioSource footstepSXF;
    public AudioSource ballBounceSFX;
    public AudioSource wallHitSFX;

    public enum SFXType { STEP, BALL, WALL };

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        dayMusic.loop = true;
        nightMusic.loop = true;
        ToggleMusic(playMusic = true);
        CalculateMusicVolume(DistanceCalculator.Instance.calculateDistance());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playSFX(SFXType sound) {
        switch(sound) {
            case SFXType.BALL:
                if (!ballBounceSFX.isPlaying)
                    ballBounceSFX.Play();
                break;
            case SFXType.STEP:
                if (!footstepSXF.isPlaying)
                    footstepSXF.Play();
                break;
            case SFXType.WALL:
                if (!wallHitSFX.isPlaying)
                    wallHitSFX.Play();
                break;
        }
    }

    public void CalculateMusicVolume(float distance) {
        float volume = (float)(.1 + ((1 / Mathf.Sqrt(distance)) * .9)) * ((GameManager.Instance.hasFog) ? .5f : 1);
        dayMusic.volume = volume;
        nightMusic.volume = volume;
    }

    public void ToggleMusic(bool play) {
        if (play) {
            if (GameManager.Instance.isDay) {
                dayMusic.Play();
                nightMusic.Pause();
            } else {
                nightMusic.Play();
                dayMusic.Pause();
            }
        } else {
            if (dayMusic.isPlaying) {
                dayMusic.Pause();
            }
            if (nightMusic.isPlaying) {
                nightMusic.Pause();
            }
        }
        playMusic = play;
    }
}
