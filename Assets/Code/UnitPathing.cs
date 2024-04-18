using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathing : MonoBehaviour
{

    private Stack<Node> path = new Stack<Node>();

    public Node curNode;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int fogRange;
    [SerializeField]
    private CircleCollider2D cc;

    [SerializeField]
    private Searcher search;

    [SerializeField]
    private int lifeCost;

    // Start is called before the first frame update
    void Start()
    {
        path = PathingMaster.instance.AStar(transform.position, Vector2.zero, search);
        curNode = path.Pop();
        cc.radius = fogRange + .5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!curNode.isGoal && GameState.instance.state == State.Play) {
           Move();   
        }
        if (curNode.isGoal) {
            Win.instance.LoseLife(lifeCost);
        }
    }

    public void ClearFog()
    {
        PathingMaster.instance.ClearFog(curNode, fogRange);
    }

    private void Move()
    {
        if (curNode == null)
            curNode = path.Pop();

        Vector2 dir = (Vector2)curNode.pos - (Vector2)transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if (Vector2.Distance(curNode.pos, transform.position) < 0.02f) {
            curNode = path.Pop();

        }

    }


}
