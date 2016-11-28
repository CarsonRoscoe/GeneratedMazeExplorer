using UnityEngine;
using System.Collections;

public class DistanceCalculator : MonoBehaviour {
    public static DistanceCalculator Instance;

    public GameObject player;
    public GameObject enemy;

    public float enemyPlayerDistance = 1;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    public float calculateDistance() {
        return enemyPlayerDistance = Vector3.Distance(player.transform.position, enemy.transform.position);
    }
}
