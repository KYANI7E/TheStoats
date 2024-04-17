using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathingMaster : MonoBehaviour
{

    public static PathingMaster instance;

    private Dictionary<Vector2, Node> map = new Dictionary<Vector2, Node>();

    [SerializeField]
    private Tilemap groundTileMap;

    [SerializeField]
    private GameObject crystal;
    private GameObject crystalDown;
   

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
    void Update()
    {
        
    }

    public Stack<Node> AStar(Vector2 startPos, Vector2 goal)
    {

        startPos = new Vector2((int)startPos.x, (int)startPos.y);
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
        curNode = null;

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

            timeOut++;
            if (timeOut > 6000) {
                Debug.LogError("making a star path time out");
                break;
            }

            List<Node> adjacent = GetNeighbors(curNode);


            foreach(Node child in adjacent) {
                if (closed.Contains(child) || child.type != TraversType.Walkable)
                    continue;

                child.G = curNode.G + 1;
                child.H = Heuristic(child.pos, goal);
                child.F = child.G + child.H;

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
            if (curNode.isGoal)
                newNode.isGoal = true;

            path.Push(newNode);
            curNode = curNode.parent;
            timeOut++;
            if (timeOut > 1000) {
                Debug.LogError("building path parent time out");
                break;
            }
        }

        return path;

    }

    private float Heuristic(Vector2 pos, Vector2 goal)
    {
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
        foreach(Vector2 v in dirs) {

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

    public void SetMap(Dictionary<Vector2, ConnectionInfo> _map)
    {
        Vector2 sum = Vector2.zero;
        int amount = 0;
        if(crystalDown != null)
            DestroyImmediate(crystalDown);

        map = new Dictionary<Vector2, Node>();
        foreach (KeyValuePair<Vector2, ConnectionInfo> entry in _map) {
            map.Add(entry.Key, new Node(TraversType.Walkable, entry.Key));
            sum = sum + entry.Key;
            amount++;
        }
        Vector2 center = sum / amount;
        center.x = (int)center.x;
        center.y = (int)center.y;
        crystalDown = Instantiate(crystal, center, Quaternion.identity);
        map[center].isGoal = true;
    }

    public bool ValidSpawnPosition(Vector2 pos)
    {
        if (map.ContainsKey(pos))
            return true;

        return false;
    }
}

public enum TraversType { Walkable, NotWalkable};
public class Node
{
    public TraversType type;

    public Vector2 pos;

    public float F;
    public float G;
    public float H;

    public Node parent = null;

    public bool isGoal = false;

    public Node(TraversType _type, Vector2 _pos)
    {
        type = _type;
        pos = _pos;
    }
}
