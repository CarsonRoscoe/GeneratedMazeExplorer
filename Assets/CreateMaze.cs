using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreateMaze : MonoBehaviour {

    private Coordinate[,] Maze;
    private Coordinate MazeStart;
    private Coordinate MazeEnd;
    private enum MazeID {WALL, SPACE};
    public int MazeWidth = 20;
    public int MazeHeight = 10;
	// Use this for initialization
	void Start () {
        InitMaze();
        CreateMazeBranches();
        DrawMaze();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateMazeBranches() {
        MazeStart = RandomBorderTile();
        do {
            MazeEnd = RandomBorderTile();
        } while (MazeEnd.Equals(MazeStart) || MazeEnd.x == MazeStart.x || MazeEnd.y == MazeStart.y);

        Debug.Log("start " + MazeStart.ToString());
        Debug.Log("end " + MazeEnd.ToString());

        Maze[MazeStart.x, MazeStart.y].type = MazeID.SPACE;
        Maze[MazeEnd.x, MazeEnd.y].type = MazeID.SPACE;

        Stack<Coordinate> tree = new Stack<Coordinate>();
        int sX = 0;
        int sY = 0;
        if (MazeStart.x == 0)
            sX = 1;
        else if (MazeStart.x == MazeWidth - 1)
            sX = MazeWidth - 2;
        else
            sX = MazeStart.x;
        if (MazeStart.y == 0)
            sY = 1;
        else if (MazeStart.y == MazeHeight - 1)
            sY = MazeHeight - 2;
        else
            sY = MazeStart.y;
        Coordinate startTree = new Coordinate(sX, sY);
        tree.Push(startTree);
        Maze[sX, sY].visited = true;
        Maze[sX, sY].type = MazeID.SPACE;
        while (tree.Count > 0) {
            Coordinate visit = FindNeighboor(tree.Peek());
            if (visit != null) {
                visit.visited = true;
                visit.type = MazeID.SPACE;
                tree.Push(visit);
            } else {
                tree.Pop();
            }
        }

    }

    Coordinate FindNeighboor(Coordinate p) {
        bool canLeft = p.x - 2 > 0 && !Maze[p.x - 2, p.y].visited;
        bool canRight = p.x + 2 < MazeWidth - 1 && !Maze[p.x + 2, p.y].visited;
        bool canUp = p.y - 2 > 0 && !Maze[p.x, p.y - 2].visited;
        bool canDown = p.y + 2 < MazeHeight - 1 && !Maze[p.x, p.y + 2].visited;
        int[] around = { 0, 1, 2, 3 };
        around = RandomizeArray(around);
        for (int i = 0; i < 4; i++)
            switch (around[i]) {
                case 0:
                    if (canLeft) {
                        Maze[p.x - 1, p.y].type = MazeID.SPACE;
                        return Maze[p.x - 2, p.y];
                    }
                    break;
                case 1:
                    if (canRight) {
                        Maze[p.x + 1, p.y].type = MazeID.SPACE;
                        return Maze[p.x + 2, p.y];
                    }
                    break;
                case 2:
                    if (canUp) {
                        Maze[p.x, p.y - 1].type = MazeID.SPACE;
                        return Maze[p.x, p.y - 2];
                    }
                    break;
                case 3:
                    if (canDown) {
                        Maze[p.x, p.y + 1].type = MazeID.SPACE;
                        return Maze[p.x, p.y + 2];
                    }
                    break;
            }
        return null;
    }

    int[] RandomizeArray(int[] a) {
        for (int i = a.Length - 1; i > 0; i--) {
            int rnd = Random.Range(0, i + 1);
            int temp = a[i];
            a[i] = a[rnd];
            a[rnd] = temp;
        }
        return a;
    }

    Coordinate RandomBorderTile() {
        int pos = 0;
        Coordinate ret = new Coordinate(0, 0);
        if (Random.Range(0, 2) == 0) {//width
            pos = (Random.Range(0, (MazeWidth - 5) / 2) * 2) + 3;
            ret.x = pos;
            ret.y = Random.Range(0, 2) == 0 ? 0 : MazeHeight - 1;
            return ret;
        } else {//height
            pos = (Random.Range(0, (MazeHeight - 5) / 2) * 2) + 3;
            ret.x = Random.Range(0, 2) == 0 ? 0 : MazeWidth - 1;
            ret.y = pos;
            return ret;
        }
    }

    void InitMaze() {
        Maze = new Coordinate[MazeWidth, MazeHeight];
        for (int w = 0; w < MazeWidth; w++)
            for (int h = 0; h < MazeHeight; h++) {
                Maze[w, h] = new Coordinate(w, h);
            }
    }


    void DrawMaze() {
        for (int w = 0; w < MazeWidth; w++)
            for (int h = 0; h < MazeHeight; h++) {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(w, 0, h);
                cube.gameObject.GetComponent<Renderer>().material.color = getColor(Maze[w, h].type);
            }
    }

    Color getColor(MazeID id) {
        switch(id) {
            case MazeID.SPACE:
                return Color.white;
            case MazeID.WALL:
                return Color.black;
        }
        return Color.red;
    }

    private class Coordinate {
        public int x;
        public int y;
        public bool visited;
        public MazeID type = MazeID.WALL;

        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
            this.visited = false;
            this.type = MazeID.WALL;
        }

        public Coordinate(int x, int y, MazeID id) {
            this.x = x;
            this.y = y;
            this.visited = false;
            this.type = id;
        }

        public bool Equals(Coordinate c) {
            return (c.x == x && c.y == y);
        }
        
        public string ToString() {
            return "x: " + x + " y:" + y;
        }
    }

}
