using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class KeyboardPlayerMovement : BaseMovement {
        public float MovementSpeed = .5f;

        // Update is called once per frame
        protected override void OnUpdate() {
            var gameData = GameDataManager.Instance;

            var newYPosition = transform.position.y + Input.GetAxis( "Vertical" ) * MovementSpeed;
            newYPosition = Mathf.Min( newYPosition, gameData.MaximumCourtY );
            newYPosition = Mathf.Max( newYPosition, gameData.MinimumCourtY );

            var horizontalDirection = Input.GetAxis( "Horizontal" ) * (PlayerNumber == 1 ? 1 : -1);
            var newZPosition = transform.position.z + horizontalDirection * MovementSpeed;
            newZPosition = Mathf.Min( newZPosition, gameData.MaximumCourtZ );
            newZPosition = Mathf.Max( newZPosition, gameData.MinimumCourtZ );

            transform.position = new Vector3( transform.position.x, newYPosition, newZPosition );
        }
    }
}
