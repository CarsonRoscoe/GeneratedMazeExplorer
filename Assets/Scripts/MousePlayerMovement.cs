using UnityEngine;
using System.Collections;

namespace Assets.Scripts {
    public class MousePlayerMovement : BaseMovement {
        // Update is called once per frame
        protected override void OnUpdate() {
            var gameData = GameDataManager.Instance;
            var widthPercent = Input.mousePosition.x / Screen.width;
            var heightPerecent = Input.mousePosition.y / Screen.height;

            var newZPosition = gameData.CourtWidth * widthPercent + gameData.MinimumCourtZ;
            newZPosition = PlayerNumber == 1 ? newZPosition : -newZPosition;

            var newYPosition = gameData.CourtHeight * heightPerecent + gameData.MinimumCourtY;

            transform.position = new Vector3( transform.position.x, newYPosition, newZPosition );
        }
    }
}

