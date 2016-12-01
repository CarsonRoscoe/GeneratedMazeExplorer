using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class GoaleeNet : MonoBehaviour {
        public int PlayerNumber = 1;

        void OnCollisionEnter( Collision collision ) {
            var collider = collision.collider;
            if ( collider.tag == "Ball" ) {
                var ballObject = collider.gameObject;
                GameManager.Instance.PlayerScored( PlayerNumber == 1 ? 2 : 1 );
                if (GameDataManager.Instance.GameInProgress) {
                    GameManager.Instance.ResetCourt( ballObject );
                }
            }
        }
    }
}
