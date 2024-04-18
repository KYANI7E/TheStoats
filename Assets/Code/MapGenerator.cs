using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public enum TileType { Floor, Wall}
public class MapGenerator : MonoBehaviour
{

    public static MapGenerator instance;

    [SerializeField]
    private Tilemap floorTileMap;
    [SerializeField]
    private Tilemap fogTileMap;
    [SerializeField]
    private Tilemap waterTileMap;

    [SerializeField]
    private TileBase floor;
    [SerializeField]
    private TileBase fog;
    [SerializeField]
    private TileBase water;

    private Dictionary<Vector2, ConnectionInfo> map = new Dictionary<Vector2, ConnectionInfo>();
    private Dictionary<Vector2, ConnectionInfo> fogMap = new Dictionary<Vector2, ConnectionInfo>();


    [SerializeField]
    private Vector2 topRightCorner;
    [SerializeField]
    private Vector2 bottomLeftCorner;

    [SerializeField]
    private int maxSteps;
    [SerializeField]
    private int fogSteps;
        
    [SerializeField]
    private GameObject[] bases;//used to spawn the four bases
    [SerializeField]
    private float minBaseSeperationDistance;


    private List<GameObject> basesDown = new List<GameObject>();


    [SerializeField]
    private int seed;
    public void NewSeed()
    { seed = Random.Range(0, 10000); }

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GenEverything();
    }

    public void ClearEverything()
    {
        ClearMap(floorTileMap, waterTileMap);
        ClearMap(fogTileMap, null);
    }

    public void ClearMap(Tilemap tilemap, Tilemap complement)
    {
        foreach(GameObject g in basesDown) {
            DestroyImmediate(g);
        }
        basesDown.Clear();

        for (int x = (int)bottomLeftCorner.x; x <= topRightCorner.x; x++) {
            for (int y = (int)bottomLeftCorner.y; y <= topRightCorner.y; y++) {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
                if(complement != null)
                    complement.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }


    public void GenEverything()
    {
        GenerateMap(floorTileMap, floor, waterTileMap, water,ref map, maxSteps);
        GenerateMap(fogTileMap, fog, null, null, ref fogMap, fogSteps);
    }

    public void GenerateMap(Tilemap tilemap, TileBase mainTile, Tilemap complement, TileBase compTile, ref Dictionary<Vector2, ConnectionInfo> curMap, int steps)
    {
        ClearMap(tilemap, complement);

        if (Seed.instance != null)
            seed = Seed.instance.GetSeed();

        Random.InitState(seed);

        curMap = new Dictionary<Vector2, ConnectionInfo>();
        List<Vector2> spots = GenerateBaseLocations();
        List<Walker> walkers = new List<Walker>();
        for (int i = 0; i < spots.Count; i++) {
            //basesDown.Add(Instantiate(bases[i], spots[i], Quaternion.identity));
            walkers.Add(new Walker(spots[i], curMap, topRightCorner, bottomLeftCorner));


        }

        int walkSteps = 0;

        int timeOut = 0;
        bool notConnected = true;
        while (notConnected) {
            foreach(Walker w in walkers) {
                notConnected = !w.Walk();
            }
            timeOut++;
            if (timeOut > 10000) {
                Debug.LogError("Walkers timeout");
                break;
            }
            if(walkSteps < steps) {
                notConnected = true;
                walkSteps++;
            } else {

                notConnected = false;
            }

        }


        for (int x = (int)bottomLeftCorner.x; x <= topRightCorner.x; x++) {
            for (int y = (int)bottomLeftCorner.y; y <= topRightCorner.y; y++) {
                if (curMap.ContainsKey(new Vector2(x, y))) {
                    tilemap.SetTile(new Vector3Int(x, y, 0), mainTile);
                } else {
                    if(complement != null)
                        complement.SetTile(new Vector3Int(x, y, 0), compTile);

                }
            }
        }

    }

    public Dictionary<Vector2, ConnectionInfo> GetMap()
    {
        return map;
    }
    public Dictionary<Vector2, ConnectionInfo> GetFog()
    {
        return fogMap;
    }

    //open position out of range of any other starting location for another base
    private List<Vector2> allPositons = new List<Vector2>();
    private List<Vector2> GenerateBaseLocations()
    {
        Vector2[] ls = { new Vector2(0, 0), 
                        new Vector2(5, 5), new Vector2(5, 0), new Vector2(0, 5), new Vector2(-5, 0), new Vector2(-5, -5), new Vector2(0, -5),
                        new Vector2(15, -15), new Vector2(-15, 15), new Vector2(-5, 5), new Vector2(5, -5),
                        new Vector2(15, 0), new Vector2(15, 15), new Vector2(0, 15), new Vector2(-15, 0), new Vector2(-15, -15), new Vector2(0, -15)};
        return new List<Vector2>( ls);

        for (int x = (int)bottomLeftCorner.x + 1; x < topRightCorner.x; x++) {
            for (int y = (int)bottomLeftCorner.y + 1; y < topRightCorner.y; y++) {
                allPositons.Add(new Vector2(x, y));
            }
        }

        List<Vector2> locations = new List<Vector2>();
        foreach(GameObject g in bases) {
            locations.Add(Vector2.zero);
        }
        for(int i = 0; i < locations.Count; i++) {
            if (allPositons.Count == 0) {
                Debug.LogError("Base Gerenation Failed, No Valid position left");
                break;
            }
            Vector2 candidateLocation = allPositons[Random.Range(0, allPositons.Count)]; //random place for a base
            locations[i] = candidateLocation;
            RemovePointsAround(minBaseSeperationDistance, candidateLocation);
        }
        return locations;
    }

    //will remove all position is range of center (a starting base location)
    private void RemovePointsAround(float dis, Vector2 center)
    {
        List<Vector2> deadList = new List<Vector2>();
        foreach (Vector2 v in allPositons) {
            if (Vector2.Distance(center, v) < dis) {
                deadList.Add(v);
            }
        }
        foreach (Vector2 v in deadList) {
            allPositons.Remove(v);
        }
    }


}


//Walker will randomly walk the grid leaving tile behind
public class Walker
{
    private Vector2 curLocation;
    private Dictionary<Vector2, ConnectionInfo> map;
    private Vector2 topRightCorner;
    private Vector2 bottomLeftCorner;

    public ConnectionInfo connected;

    //walkable directions, N W S E
    private Vector2[] directions = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

    //all directions, will be used to mark tiles around starting location as visited
    private Vector2[] allDirections = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0),
                                        new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) };

    public Walker(Vector2 startLocation, Dictionary<Vector2, ConnectionInfo> _map, Vector2 _topRightCorner, Vector2 _bottomLeftCorner)
    {
        connected = new ConnectionInfo(this);
        curLocation = startLocation;
        map = _map;
        topRightCorner = _topRightCorner;
        bottomLeftCorner = _bottomLeftCorner;

        //marking starting tile as visited
        if (!map.ContainsKey(startLocation))
            map.Add(startLocation, connected);

        //marking tiles adjasent to start location as visited
        //foreach (Vector2 d in allDirections) {
        //    if (!map.ContainsKey(startLocation + d))
        //        map.Add(startLocation + d, connected);
        //}
    }

    //returns true when conencted to four othter walkers
    //this method is the main thing that walks, chooses random direction and goes there and marks it down at part of the floor
    public bool Walk()    
    {
        // picking a direction to walk in
        Vector2 direction = Vector2.zero;
        Vector2 nextPos = Vector2.zero;
        bool notValidDirection = true;
        int count = 0;
        while (notValidDirection) {
            notValidDirection = false;
            direction = directions[Random.Range(0, 4)];
            nextPos = curLocation + direction;
            if(nextPos.x < bottomLeftCorner.x || nextPos.y < bottomLeftCorner.y) {
                notValidDirection = true;
            }
            if (nextPos.x > topRightCorner.x || nextPos.y > topRightCorner.y) {
                notValidDirection = true;
            }
            count++;
            if (count > 10)
                break;
        }
        //direction has been picked ^^^^^^

        ConnectionInfo con;
        if (!map.TryGetValue(nextPos, out con)) {
            map.Add(nextPos, connected);

            if(Random.Range(0f,1f) > .7f) {
                nextPos = nextPos + direction;
                if (!map.TryGetValue(nextPos, out con)) 
                    map.Add(nextPos, connected);
            }
        } else {
            //tile has been visited before so now we check the connection info on it
            connected.IsNewConnection(con);
        }

        curLocation = nextPos;

        foreach (Vector2 d in directions) {
            if (map.ContainsKey(curLocation + d))
                continue;

            if (!map.ContainsKey(curLocation + d))
                map.Add(curLocation + d, connected);
        }

        //if connected count == four than all walkers have a continous path to each other
        if (connected.count >= 4) {
            return true; 
        }

        return false;

    }

}


//this is the value field in the dictionary
//when a walker walks on a tile that has been visited by another walker, it knows because the tile is referencing a different ConnectionInfo than 
//      the one it has,
// but than the tow different walks take one Connection info, (the one who got walked about gets assigned this one as well)
// till all the walkers are referncing then same ConnectionInfo and count will be 4
public class ConnectionInfo
{
    public Dictionary<Walker, Walker> connectedTo = new Dictionary<Walker, Walker>();
    public Walker origin;
    public int count = 1;
    public ConnectionInfo(Walker walker)
    {
        connectedTo.Add(walker, walker);
        origin = walker;
    }

    public bool IsNewConnection(ConnectionInfo other)
    {
        if (connectedTo.ContainsKey(other.origin)) {
            return false;
        } else {
            other.origin.connected = this;
            connectedTo.Add(other.origin, other.origin);
            count++;
            return true;
        }
    }
}