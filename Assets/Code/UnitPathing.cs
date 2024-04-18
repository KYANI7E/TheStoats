using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathing : MonoBehaviour, ISpeedBuff
{

    private Stack<Node> path = new Stack<Node>();

    public Node curNode;

    [SerializeField]
    private float speed;
    private float speedBuff;
    private float speedBuffTime;
    private float speedBuffCoolDown;
    [SerializeField]
    private ParticleSystem ps;

    [SerializeField]
    private int fogRange;
    [SerializeField]
    private CircleCollider2D cc;

    [SerializeField]
    private Searcher search;

    [SerializeField]
    private int lifeCost;

    private bool clearFog = false;
    // Start is called before the first frame update
    void Start()
    {

        cc.radius = fogRange;
    }

    // Update is called once per frame
    void Update()
    {

        if (curNode == null || path.Count == 0) {
            path = PathingMaster.instance.AStar(transform.position, Vector2.zero, search);
            if (curNode != null)
                if (curNode.isFogged)
                    clearFog = true;

            if (path.Count == 0) {
                GetComponent<Unit>().Fade();
            } else {
                curNode = path.Pop();
            }
        } else {
            if(GameState.instance.state == State.Play) {
                if (clearFog) {
                    PathingMaster.instance.ClearFog(curNode, fogRange);
                }
            }
            if (!curNode.isGoal && GameState.instance.state == State.Play) {
               Move();   
            }
            if (curNode.isGoal) {
                Win.instance.LoseLife(lifeCost);
                GetComponent<Unit>().DoDamage(Mathf.Infinity);
            }
            CheckSpeedBuff();
        }
    }

    private void CheckSpeedBuff()
    {
        speedBuffCoolDown += Time.deltaTime;
        if(speedBuffCoolDown < speedBuffTime) {
            return;
        } else {
            ps.Stop();
            speedBuff = 0;
        }

    }

    public void ClearFog()
    {
        clearFog = true;
    }

    private void Move()
    {
        if (path.Count == 0)
            return;
        if (curNode == null)
            curNode = path.Pop();

        Vector2 dir = (Vector2)curNode.pos - (Vector2)transform.position;
        transform.Translate(dir.normalized * (speed+speedBuff) * Time.deltaTime);

        if (Vector2.Distance(curNode.pos, transform.position) < 0.02f) {
            if (path.Count != 0)
            curNode = path.Pop();

        }

    }

    public void SpeedBuff(float speedinc, float speedT)
    {
        Debug.Log("SpeedBUFFED");
        ps.Play();
        speedBuff = speedinc;
        speedBuffTime = speedT;
        speedBuffCoolDown = 0;
    }
}
