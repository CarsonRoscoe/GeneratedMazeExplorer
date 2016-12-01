using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class Transparent : MonoBehaviour {
        public bool IsTransparent = true;

        void Start() {
            SetTransparent( IsTransparent );
        }

        public void SetTransparent(bool isObjectTransparent) {
            foreach ( var renderer in gameObject.GetComponentsInChildren<Renderer>() ) {
                renderer.enabled = !isObjectTransparent;
            }
            IsTransparent = isObjectTransparent;
        }
    }
}
