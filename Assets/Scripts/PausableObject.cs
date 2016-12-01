using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public abstract class PausableObject : MonoBehaviour {
        private bool m_isPaused;

        void Update() {
            if ( !GameDataManager.Instance.IsPaused ) {
                OnUpdate();
            }
            if (m_isPaused != GameDataManager.Instance.IsPaused) {
                if (m_isPaused) {
                    OnResume();
                } else {
                    OnPause();
                }
            }
            m_isPaused = GameDataManager.Instance.IsPaused;
        }

        protected virtual void OnPause() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnResume() { }
    }
}
