using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathing : MonoBehaviour
{

    private Stack<Node> path = new Stack<Node>();

    private Node curNode;

    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        path = PathingMaster.instance.AStar(transform.position, Vector2.zero);
        curNode = path.Pop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!curNode.isGoal) {
           Move();   
        }
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
