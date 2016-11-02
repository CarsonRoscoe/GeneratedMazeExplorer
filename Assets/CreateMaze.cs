using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CreateMaze : MonoBehaviour {
    public static CreateMaze Instance;

    private Coordinate[,] Maze;
    private Coordinate MazeStart;
    private Coordinate MazeEnd;
    private enum MazeID { WALL, SPACE };
    public int MazeWidth;
    public int MazeHeight;
    private int mazeScale = 2;

    void Awake() {
        if ( Instance == null ) {
            Instance = this;
        }
        else {
            Destroy( this );
        }
    }

    // Use this for initialization
    void Start() {
        InitAll();
    }

    void InitAll() {
        InitMaze();
        CreateMazeBranches();
        DrawMaze();
        InitPlayer();
        AStarPath pathfinder = new AStarPath(Maze, MazeStart, MazeEnd, MazeWidth, MazeHeight);
        pathfinder.FindPath();
    }

    // Update is called once per frame
    void Update() {

    }

    void CreateMazeBranches() {
        MazeStart = RandomBorderTile();
        do {
            MazeEnd = RandomBorderTile();
        } while ( MazeEnd.Equals( MazeStart ) || MazeEnd.x == MazeStart.x || MazeEnd.y == MazeStart.y );

        Debug.Log( "start " + MazeStart.ToString() );
        Debug.Log( "end " + MazeEnd.ToString() );

        Maze[MazeStart.x, MazeStart.y].type = MazeID.SPACE;
        Maze[MazeEnd.x, MazeEnd.y].type = MazeID.SPACE;

        Stack<Coordinate> tree = new Stack<Coordinate>();
        int sX = 0;
        int sY = 0;
        if ( MazeStart.x == 0 )
            sX = 1;
        else if ( MazeStart.x == MazeWidth - 1 )
            sX = MazeWidth - 2;
        else
            sX = MazeStart.x;
        if ( MazeStart.y == 0 )
            sY = 1;
        else if ( MazeStart.y == MazeHeight - 1 )
            sY = MazeHeight - 2;
        else
            sY = MazeStart.y;
        Coordinate startTree = new Coordinate( sX, sY );
        tree.Push( startTree );
        Maze[sX, sY].visited = true;
        Maze[sX, sY].type = MazeID.SPACE;
        while ( tree.Count > 0 ) {
            Coordinate visit = FindNeighboor( tree.Peek() );
            if ( visit != null ) {
                visit.visited = true;
                visit.type = MazeID.SPACE;
                tree.Push( visit );
            }
            else {
                tree.Pop();
            }
        }
    }

    Coordinate FindNeighboor( Coordinate p ) {
        bool canLeft = p.x - 2 > 0 && !Maze[p.x - 2, p.y].visited;
        bool canRight = p.x + 2 < MazeWidth - 1 && !Maze[p.x + 2, p.y].visited;
        bool canUp = p.y - 2 > 0 && !Maze[p.x, p.y - 2].visited;
        bool canDown = p.y + 2 < MazeHeight - 1 && !Maze[p.x, p.y + 2].visited;
        int[] around = { 0, 1, 2, 3 };

        around = RandomizeArray( around );
        for ( int i = 0; i < 4; i++ )
            switch ( around[i] ) {
                case 0:
                    if ( canLeft ) {
                        Maze[p.x - 1, p.y].type = MazeID.SPACE;
                        return Maze[p.x - 2, p.y];
                    }
                    break;
                case 1:
                    if ( canRight ) {
                        Maze[p.x + 1, p.y].type = MazeID.SPACE;
                        return Maze[p.x + 2, p.y];
                    }
                    break;
                case 2:
                    if ( canUp ) {
                        Maze[p.x, p.y - 1].type = MazeID.SPACE;
                        return Maze[p.x, p.y - 2];
                    }
                    break;
                case 3:
                    if ( canDown ) {
                        Maze[p.x, p.y + 1].type = MazeID.SPACE;
                        return Maze[p.x, p.y + 2];
                    }
                    break;
            }
        return null;
    }

    int[] RandomizeArray( int[] a ) {
        for ( int i = a.Length - 1; i > 0; i-- ) {
            int rnd = UnityEngine.Random.Range( 0, i + 1 );
            int temp = a[i];
            a[i] = a[rnd];
            a[rnd] = temp;
        }
        return a;
    }

    Coordinate RandomBorderTile() {
        int pos = 0;
        Coordinate ret = new Coordinate( 0, 0 );
        if ( UnityEngine.Random.Range( 0, 2 ) == 0 ) {//width
            pos = (UnityEngine.Random.Range( 0, (MazeWidth - 5) / 2 ) * 2) + 3;
            ret.x = pos;
            ret.y = UnityEngine.Random.Range( 0, 2 ) == 0 ? 1 : MazeHeight - 2;
            return ret;
        } else {//height
            pos = (UnityEngine.Random.Range( 0, (MazeHeight - 5) / 2 ) * 2) + 3;
            ret.x = UnityEngine.Random.Range( 0, 2 ) == 0 ? 1 : MazeWidth - 2;
            ret.y = pos;
            return ret;
        }
    }

    void InitMaze() {
        Maze = new Coordinate[MazeWidth, MazeHeight];
        for ( int w = 0; w < MazeWidth; w++ )
            for ( int h = 0; h < MazeHeight; h++ ) {
                Maze[w, h] = new Coordinate( w, h );
            }
    }


    void DrawMaze() {
        //Load the floor
        var floor = (GameObject)Instantiate( Resources.Load( "Floor" ) );
        floor.transform.position = new Vector3( MazeWidth - 1, -3f, MazeHeight - 1 );
        floor.transform.localScale = new Vector3( MazeWidth * 2, 1, MazeHeight * 2 );

        for ( int w = 0; w < MazeWidth; w++ )
            for ( int h = 0; h < MazeHeight; h++ ) {
                string resource = string.Empty;
                if ( Maze[w, h].type == MazeID.WALL ) {
                    if ( w == 0 )
                        resource = "WestWall";
                    else if ( w == MazeWidth - 1 )
                        resource = "EastWall";
                    else if ( h == 0 )
                        resource = "NorthWall";
                    else if ( h == MazeHeight - 1 )
                        resource = "SouthWall";
                    else
                        resource = "StandardWall";
                    GameObject wall = (GameObject)Instantiate( Resources.Load( resource ) );
                    wall.transform.position = new Vector3( w * mazeScale, 0, h * mazeScale );
                    Maze[w, h].positionObject = wall;
                    Maze[w, h].Activate(false);
                }
            }

        GameObject endGate = (GameObject)Instantiate(Resources.Load("Gate"));
        endGate.transform.position = new Vector3(MazeEnd.x * mazeScale, 0, MazeEnd.y * mazeScale);
    }

    public void ResetMaze() {
        for (int w = 0; w < MazeWidth; w++)
            for (int h = 0; h < MazeHeight; h++) {
                Destroy(Maze[w, h].positionObject);
            }
        Destroy(GameObject.FindWithTag("Gate"));
        Destroy(GameObject.FindWithTag("Floor"));
        InitAll();
    }

    private void InitPlayer() {
        var player = GameObject.Find( "Player" );
        var camera = GameObject.Find( "Main Camera" );
        var startOffset = Vector3.zero;
        var startFacing = Vector3.zero;

        if ( MazeStart.x == 0 ) {
            startOffset = Vector3.left;
            startFacing.y = 90;
        }
        else if ( MazeStart.x == MazeWidth - 1 ) {
            startOffset = Vector3.right;
            startFacing.y = -90;
        }
        else if ( MazeStart.y == 0 ) {
            startOffset = Vector3.back;
        }
        else if ( MazeStart.y == MazeHeight - 1 ) {
            startOffset = Vector3.forward;
            startFacing.y = 180;
        }
        else
            print( "Error in CreateMaze.cs - InitPlayer() method" );

        startOffset.y = -.1f;
        player.transform.position = (MazeStart.position * mazeScale);
        player.transform.eulerAngles = startFacing;
        SettingsManager.Instance.PlayerStartPosition = player.transform.position;
        SettingsManager.Instance.PlayerStartEuler = startFacing;
        CalculatePooling(player.transform.position);
    }

    private Coordinate WorldToCoordinate(Vector3 worldCoordinate) {
        var w = (int)Math.Round(worldCoordinate.x / mazeScale);
        var h = (int)Math.Round(worldCoordinate.z / mazeScale);
        return new Coordinate(w, h);
    }

    public bool IsWorldCoordinateOccupied( Vector3 worldCoordinate ) {
        Coordinate pos = WorldToCoordinate(worldCoordinate);
        bool result;
        try {
            result = (Maze[pos.x, pos.y].type == MazeID.WALL);
        }
        catch ( Exception ) {
            result = false;
        }
        return result;
    }

    public void CalculatePooling(Vector3 p) {
        Coordinate pos = WorldToCoordinate(p);

        for (int xx = 0; xx < MazeWidth; xx++)
            for (int yy = 0; yy < MazeHeight; yy++)
                Maze[xx, yy].Activate(false);

        int x = 0, y = 0;
        x = Math.Max(Math.Min(MazeWidth - 1, pos.x), 0);
        do {
            Maze[Math.Max(Math.Min(MazeWidth - 1, x), 0), Math.Max(0, Math.Min(MazeHeight - 1, pos.y - 1))].Activate(true);
            Maze[Math.Max(Math.Min(MazeWidth - 1, x), 0), Math.Max(0, Math.Min(MazeHeight - 1, pos.y))].Activate(true);
            Maze[Math.Max(Math.Min(MazeWidth - 1, x), 0), Math.Max(0, Math.Min(MazeHeight - 1, pos.y + 1))].Activate(true);
        } while (x < MazeWidth - 1 && Maze[x++, Math.Max(0, Math.Min(MazeHeight - 1, pos.y))].type == MazeID.SPACE);

        x = Math.Max(Math.Min(MazeWidth - 1, pos.x), 0);
        do {
            Maze[Math.Max(Math.Min(MazeWidth - 1, x), 0), Math.Max(0, Math.Min(MazeHeight - 1, pos.y - 1))].Activate(true);
            Maze[Math.Max(Math.Min(MazeWidth - 1, x), 0), Math.Max(0, Math.Min(MazeHeight - 1, pos.y))].Activate(true);
            Maze[Math.Max(Math.Min(MazeWidth - 1, x), 0), Math.Max(0, Math.Min(MazeHeight - 1, pos.y + 1))].Activate(true);
        } while (x > 0 && Maze[x--, Math.Max(0, Math.Min(MazeHeight - 1, pos.y))].type == MazeID.SPACE);

        y = Math.Max(Math.Min(MazeHeight - 1, pos.y), 0);
        do {
            Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x - 1)), Math.Max(0, Math.Min(MazeHeight - 1, y))].Activate(true);
            Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x)), Math.Max(0, Math.Min(MazeHeight - 1, y))].Activate(true);
            Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x + 1)), Math.Max(0, Math.Min(MazeHeight - 1, y))].Activate(true);
        } while (y < MazeHeight - 1 && Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x)), y++].type == MazeID.SPACE);

        y = Math.Max(Math.Min(MazeHeight - 1, pos.y), 0);
        do {
            Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x - 1)), Math.Max(0, Math.Min(MazeHeight - 1, y))].Activate(true);
            Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x)), Math.Max(0, Math.Min(MazeHeight - 1, y))].Activate(true);
            Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x + 1)), Math.Max(0, Math.Min(MazeHeight - 1, y))].Activate(true);
        } while (y > 0 && Maze[Math.Max(0, Math.Min(MazeWidth - 1, pos.x)), --y].type == MazeID.SPACE);

        for (var xx = Math.Max(pos.x - 3, 0); xx <= Math.Min(pos.x + 3, MazeWidth - 1); xx++)
            for (var yy = Math.Max(pos.y - 3, 0); yy <= Math.Min(pos.y + 3, MazeHeight - 1); yy++)
                Maze[xx, yy].Activate(true);
    }

    private class Coordinate {
        public int x;
        public int y;
        public bool visited;
        public MazeID type = MazeID.WALL;
        public Vector3 position { get { return new Vector3( x, 0, y ); } }
        public GameObject positionObject;
        public int Heuristic;
        public const int cost = 10;
        public int costToGetHere = Int32.MaxValue;
        public Coordinate parent = null;


        public Coordinate( int x, int y ) {
            this.x = x;
            this.y = y;
            this.visited = false;
            this.type = MazeID.WALL;
            this.positionObject = null;
            this.Heuristic = 0;
        }

        public Coordinate( int x, int y, MazeID id ) {
            this.x = x;
            this.y = y;
            this.visited = false;
            this.type = id;
            this.positionObject = null;
            this.Heuristic = 0;
        }

        public bool Equals( Coordinate c ) {
            return (c.x == x && c.y == y);
        }

        public override string ToString() {
            return "x: " + x + " y:" + y;
        }

        public void Activate(bool active) {
            if (positionObject != null)
                positionObject.SetActive(active);
        }

    }

    private class AStarPath {
        Coordinate[,] maze;
        Coordinate start;
        Coordinate end;
        int mazeWidth;
        int mazeHeight;

        private List<Coordinate> openSet = new List<Coordinate>();
        private List<Coordinate> closedSet = new List<Coordinate>();

        public AStarPath(Coordinate [,] maze, Coordinate start, Coordinate end, int mazeWidth, int mazeHeight) {
            this.maze = maze;
            this.start = start;
            this.end = end;
            this.mazeWidth = mazeWidth;
            this.mazeHeight = mazeHeight;
        }

        public Stack<Coordinate> FindPath() {
            openSet.Clear();
            closedSet.Clear();
            start.Heuristic = calculateHeuristic(start);
            start.costToGetHere = 0;
            closedSet.Add(start);
            AddSurroundingCoordinates(closedSet[closedSet.Count - 1]);
            do {
                int index = 0;
                int lowestValue = int.MaxValue;
                for (int i = 0; i < openSet.Count; i++) {
                    int thisVal = openSet[i].Heuristic + openSet[i].costToGetHere;
                    if (lowestValue > thisVal) {
                        lowestValue = thisVal;
                        index = i;
                    }
                }
                closedSet.Add(openSet[index]);
                if (openSet[index].Equals(end))
                    break;
                else
                    openSet.RemoveAt(index);
                AddSurroundingCoordinates(closedSet[closedSet.Count - 1]);
            } while (openSet.Count > 0);
            
            Stack<Coordinate> pathList = new Stack<Coordinate>();
            pathList.Push(closedSet[closedSet.Count - 1]);
            while(!pathList.Peek().Equals(start) && pathList.Peek().parent != null) {
                pathList.Push(pathList.Peek().parent);
            }
            return pathList;
        }

        private void AddSurroundingCoordinates(Coordinate c) {
            if (c.x - 1 >= 0 && maze[c.x - 1, c.y].type != MazeID.WALL && maze[c.x - 1, c.y].parent == null) {
                maze[c.x - 1, c.y].Heuristic = calculateHeuristic(maze[c.x - 1, c.y]);
                maze[c.x - 1, c.y].costToGetHere = c.costToGetHere + Coordinate.cost;
                maze[c.x - 1, c.y].parent = c;
                openSet.Add(maze[c.x - 1, c.y]);
            }
            if (c.x + 1 <= mazeWidth - 1 && maze[c.x + 1, c.y].type != MazeID.WALL && maze[c.x + 1, c.y].parent == null) {
                maze[c.x + 1, c.y].Heuristic = calculateHeuristic(maze[c.x + 1, c.y]);
                maze[c.x + 1, c.y].costToGetHere = c.costToGetHere + Coordinate.cost;
                maze[c.x + 1, c.y].parent = c;
                openSet.Add(maze[c.x + 1, c.y]);
            }
            if (c.y - 1 >= 0 && maze[c.x, c.y - 1].type != MazeID.WALL && maze[c.x, c.y - 1].parent == null) {
                maze[c.x, c.y - 1].Heuristic = calculateHeuristic(maze[c.x, c.y - 1]);
                maze[c.x, c.y - 1].costToGetHere = c.costToGetHere + Coordinate.cost;
                maze[c.x, c.y - 1].parent = c;
                openSet.Add(maze[c.x, c.y - 1]);
            }
            if (c.y + 1 <= mazeHeight - 1 && maze[c.x, c.y + 1].type != MazeID.WALL && maze[c.x, c.y + 1].parent == null) {
                maze[c.x, c.y + 1].Heuristic = calculateHeuristic(maze[c.x, c.y + 1]);
                maze[c.x, c.y + 1].costToGetHere = c.costToGetHere + Coordinate.cost;
                maze[c.x, c.y + 1].parent = c;
                openSet.Add(maze[c.x, c.y + 1]);
            }

        }

        private int calculateHeuristic(Coordinate c) {
            return Math.Abs(c.x - end.x) + Math.Abs(c.y - end.y);
        }

    }

}
