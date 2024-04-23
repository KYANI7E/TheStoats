using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum Searcher { Fog, Goal, Tower}
public class PathingMaster : MonoBehaviour
{

    public static PathingMaster instance;

    private Dictionary<Vector2, Node> map = new Dictionary<Vector2, Node>();

    [SerializeField]
    private Tilemap groundTileMap;

    [SerializeField]
    private GameObject crystal;

    private GameObject crystalDown;
    private List<GameObject> towersDown = new List<GameObject>();

    [SerializeField]
    private int amountOfTowers;
    [SerializeField]
    private GameObject[] towers;
    [SerializeField]
    private float[] spawnProbility;


    [SerializeField]
    private Tilemap fogTileMap;


    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null) {
            instance = this;
        }else if(instance != this) {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Start()
    {
        SetMap();
    }

    public void ClearFog(float xx, float yy, int range)
    {
        Vector2 center = RoundVec(xx, yy);
        Vector2Int TR = new Vector2Int((int)center.x + (range + 1), (int)center.y + (range + 1));
        Vector2Int BL = new Vector2Int((int)center.x - (range + 1), (int)center.y - (range + 1));

        //Debug.Log(center);
        //Debug.Log(TR);
        //Debug.Log(BL);
        for (int x = BL.x; x <= TR.x; x++)
            for (int y = BL.y; y <= TR.y; y++) {
                //Debug.Log(new Vector3Int(x, y));
                Vector2 pos = new Vector3(x, y);
                if (map.ContainsKey(pos))
                    map[new Vector3(x, y)].isFogged = false;

                fogTileMap.SetTile(new Vector3Int(x, y), null);
            }
    }

    private Vector2 RoundVec(float x, float y)
    {
        return new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));

    }

    public Stack<Node> AStar(Vector2 startPos, Vector2 goal, Searcher searcher)
    {

        startPos = new Vector2(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y));
        if(goal == Vector2.zero) {
            goal = crystalDown.transform.position;
        }
        goal = new Vector2((int)goal.x, (int)goal.y);
        
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        Node curNode = map[startPos];
        //map.TryGetValue(startPos, out curNode);
        curNode.F = 0;
        curNode.parent = null;
        open.Add(curNode);

        Node goalNode = null;

        int timeOut = 0;


        while (open.Count > 0) {

            curNode = null;
            foreach(Node t in open) {
                if(curNode == null) 
                    curNode = t;
                if (t.F < curNode.F)
                    curNode = t;
            }

            open.Remove(curNode);
            closed.Add(curNode);

            if (curNode.pos == goal) {
                goalNode = curNode;
                break;
            }

            if (/*map[goal].isFogged || */searcher == Searcher.Fog) {
                if (curNode.isFogged) {
                    goalNode = curNode;
                    break;
                }
            }

            if (searcher == Searcher.Tower) {
                if (curNode.type == TraversType.Built) {
                    goalNode = curNode;
                    break;
                }
            }

            timeOut++;
            if (timeOut > 6000) {
                Debug.LogError("making a star path time out");
                break;
            }

            List<Node> adjacent = GetNeighbors(curNode);


            foreach(Node child in adjacent) {
                if (closed.Contains(child) || (child.type != TraversType.Walkable && searcher != Searcher.Tower))
                    continue;

                if (searcher == Searcher.Fog) {
                    if (child.isFogged) {
                        child.G = curNode.G - 1;
                        child.H = FogHueristic(child.pos, goal);
                        child.F = child.G + child.H;
                    }
                    child.G = curNode.G + 4;
                    child.H = Heuristic(child.pos, goal);
                    child.F = child.G + child.H;

                } else if (searcher == Searcher.Tower) {
                    if (child.type == TraversType.Built) {
                        child.G = curNode.G - 6;
                        child.H = Heuristic(child.pos, goal);
                        child.F = child.G + child.H;
                    }
                    child.G = curNode.G + 4;
                    child.H = Heuristic(child.pos, goal);
                    child.F = child.G + child.H;

                } else {
                    child.G = curNode.G + 1;
                    child.H = Heuristic(child.pos, goal);
                    child.F = child.G + child.H;
                }


                if (open.Contains(child)) {
                    bool dontAddTo = false;
                    Node nodeInOpenAlready = null;
                    foreach(Node n in open) {
                        if (n.pos != child.pos)
                            continue;
                        if (child.G > n.G)
                            dontAddTo = true;
                        nodeInOpenAlready = n;
                        break;
                    }
                    if (dontAddTo)
                        continue;
                    else {
                        open.Remove(nodeInOpenAlready);
                        child.parent = curNode;
                        open.Add(child);
                    }
                } else {
                    child.parent = curNode;
                    open.Add(child);
                }
            }
        }


        Stack<Node> path = new Stack<Node>();
        curNode = goalNode;
        timeOut = 0;
        while(curNode != null) {
            Node newNode = new Node(curNode.type, curNode.pos);
            newNode.Copy(curNode);
            path.Push(newNode);
            timeOut++;
            Node prev = curNode;
            curNode = curNode.parent;
            prev.parent = null;
            if (timeOut > 1000) {
                Debug.LogError("building path parent time out");
                break;
            }

        }
        return path;

    }


    private float Heuristic(Vector2 pos, Vector2 goal)
    {

        //if (map[goal].isFogged) {
        //    return 0;
        //}
        float x = pos.x - goal.x;
        float y = pos.y - goal.y;
        return y + x;

        //return Vector2.Distance(pos, goal);
    }

    private float FogHueristic(Vector2 pos, Vector2 goal)
    {
        if (map[pos].isFogged) {
            return 0;
        }
        if (map[goal].isFogged) {
            return 0;
        }
        float x = pos.x - goal.x;
        float y = pos.y - goal.y;
        return y + x;

        //return Vector2.Distance(pos, goal);
    }

    private Vector2[] directions = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
    private List<Node> GetNeighbors(Node node)
    {
        Vector2[] dirs = Shuffle(directions);
        List<Node> ls = new List<Node>();
        foreach(Vector2 v in directions) {

            if(map.ContainsKey(node.pos + v)) {
                ls.Add(map[node.pos + v]);
            }
        }
        return ls;
    }

    public Vector2[] Shuffle(Vector2[] ts)
    {
        var count = ts.Length;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
        return ts;
    }

    public void SetMap()
    {
        Vector2 sum = Vector2.zero;
        int amount = 0;
        if(crystalDown != null)
            DestroyImmediate(crystalDown);

        map = new Dictionary<Vector2, Node>();
        foreach (KeyValuePair<Vector2, ConnectionInfo> entry in MapGenerator.instance.GetMap()) {
            map.Add(entry.Key, new Node(TraversType.Walkable, entry.Key));
            sum = sum + entry.Key;
            amount++;
        }

        Vector2 center = sum / amount;
        center.x = (int)center.x;
        center.y = (int)center.y;
        crystalDown = Instantiate(crystal, center, Quaternion.identity);
        map[center].isGoal = true;



        // setup fog map
        foreach (KeyValuePair<Vector2, ConnectionInfo> entry in MapGenerator.instance.GetFog()) {
            if (map.ContainsKey(entry.Key)) {
                map[entry.Key].isFogged = true;
            }
        }


        //gen towers
        GenerateTowers(center);
    }

    private void GenerateTowers(Vector2 center)
    {
        foreach(GameObject g in towersDown) {
            DestroyImmediate(g);
        }
        towersDown.Clear();

        bool reLook = false;
        Vector2 BL = Vector2.zero;
        Vector2 TR = Vector2.zero;
        for (int i = 0; i < amountOfTowers; i++) {
            if (reLook == false) {
                float rangeFromCenter = Random.Range(7, 40);
                BL = center - new Vector2(rangeFromCenter, rangeFromCenter);
                TR = center + new Vector2(rangeFromCenter, rangeFromCenter);

            }
            Vector2 pos = GetRandomPosition(BL, TR);
            if (ValidBuildLocation(pos)) {
                GameObject tower = Instantiate(SelectTower(), pos, Quaternion.identity);
                map[pos].type = TraversType.Built;
                map[pos].tower = tower.GetComponent<Tower>();
                tower.GetComponent<Tower>().node = map[pos];
                towersDown.Add(tower);
                reLook = false;
            } else {
                reLook = true;
                i--;
            }
        }
    }

    public bool ValidBuildLocation(Vector2 pos)
    {
        if (!map.ContainsKey(pos))
            return false;

        if (map[pos].isFogged == false)
            return false;

        if (map[pos].type == TraversType.Walkable)
            return true;


        return false;
    }

    public void UpdateNodeWithBuilding(Vector2 pos, GameObject building)
    {
        map[pos].type = TraversType.Built;
        map[pos].tower = building.GetComponent<Tower>();
    }

    private GameObject SelectTower()
    {
        float num = Random.Range(0f, 1f);
        int index = 0;
        float temp = 0;
        while (true) {
            temp += spawnProbility[index];
            if (num > temp) {
                index++;
                continue;
            }
            return towers[index];
        }
    }

    private Vector2 GetRandomPosition(Vector2 bottomLeft, Vector2 topRight)
    {
        int x = Random.Range((int)bottomLeft.x, (int)topRight.x);
        int y = Random.Range((int)bottomLeft.y, (int)topRight.y);
        Vector2 pos = new Vector2(x, y);
        return pos;
    }

    public bool ValidSpawnPosition(Vector2 pos)
    {
        if (!map.ContainsKey(pos))
            return false;


        if (map[pos].isFogged) {
            return false;
        }
        
        if (map[pos].type == TraversType.Walkable)
            return true;


        return false;
    }
}

public enum TraversType { Walkable, NotWalkable, Built};
public class Node
{
    public TraversType type;

    public Vector2 pos;

    public float F;
    public float G;
    public float H;

    public Node parent = null;

    public bool isGoal = false;

    public bool isFogged = false;

    public Tower tower = null;

    public Node realNode;

    public Node(TraversType _type, Vector2 _pos)
    {
        type = _type;
        pos = _pos;
    }

    public void Copy(Node n)
    {
        parent = n.parent;
        isFogged = n.isFogged;
        isGoal = n.isGoal;
        pos = n.pos;
        type = n.type;
        tower = n.tower;
        realNode = n;
    }

    public void LooseTower()
    {
        tower = null;
        type = TraversType.Walkable;

    }
}
