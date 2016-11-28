using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DataHandler : MonoBehaviour {
    public static DataHandler instance = new DataHandler();
    [HideInInspector]
    public GameData data;

    private string filename = "gamedata14.data";

    void Awake() {
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    private DataHandler() {
        loadData();
    }

    public void saveData() {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename); //you can call it anything you want
        bf.Serialize(file, data);
        file.Close();
    }

    public void loadData() {
        if (File.Exists(Application.persistentDataPath + "/" + filename)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
            data = (GameData)bf.Deserialize(file);
            file.Close();
        } else {
            Debug.Log("No save data found, creating file now.");
            data = new GameData();
            saveData();
        }
    }

    [Serializable]
    public class GameData {

        public Coordinate player;
        public Coordinate enemy;
        public CreateMaze.MazeData maze;
        public int hits;

        public GameData() {
            maze = null;
            player = new Coordinate(0,-200,0);
            enemy = new Coordinate(0,-200,0);
        }

        [Serializable]
        public class Coordinate {
            public float x;
            public float y;
            public float z;

            public Coordinate(float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public void toCoordinate(Vector3 v) {
                x = v.x;
                y = v.y;
                z = v.z;
            }
        }

    }
}
