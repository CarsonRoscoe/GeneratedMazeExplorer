using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class HUDManager : MonoBehaviour {
        public static HUDManager Instance;

        public GameObject PlayerOneScore;
        public GameObject PlayerTwoScore;
        public GameObject InGameHUD;
        public GameObject EndGameMenu;
        public GameObject InGameMenu;
        public GameObject EndGameWinnerText;
        private Text PlayerOneScoreText;
        private Text PlayerTwoScoreText;

        void Awake() {
            if ( Instance == null ) {
                Instance = this;
            }
            else {
                Destroy( gameObject );
            }
        }

        void Start() {
            PlayerOneScoreText = PlayerOneScore.GetComponent<Text>();
            PlayerTwoScoreText = PlayerTwoScore.GetComponent<Text>();
        }

        public void UpdateScores() {
            PlayerOneScoreText.text = GameDataManager.Instance.PlayerOneScore.ToString();
            PlayerTwoScoreText.text = GameDataManager.Instance.PlayerTwoScore.ToString();
        }

        public void StartGame() {
            InGameHUD.SetActive( true );
            InGameMenu.SetActive( false );
            EndGameMenu.SetActive( false );
        }

        public void ToggleInGameMenu() {
            InGameMenu.SetActive( !InGameMenu.activeSelf );
        }

        public void EndGame( int winningPlayer ) {
            InGameHUD.SetActive( false );
            InGameMenu.SetActive( false );
            EndGameMenu.SetActive( true );

            var winnerText = EndGameWinnerText.GetComponent<Text>();
            string winningPlayerName = string.Empty;

            if ( winnerText != null ) {
                switch ( winningPlayer ) {
                    case 1:
                        winningPlayerName = "One";
                        winnerText.color = Color.red;
                        break;
                    case 2:
                        winningPlayerName = "Two";
                        winnerText.color = Color.blue;
                        break;
                    default:
                        break;
                }
            }

            winnerText.text = string.Format( "Player {0} Wins!", winningPlayerName );
        }
    }
}
