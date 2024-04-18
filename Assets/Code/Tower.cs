using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public Node node;

    public void KillTower()
    {
        node.LooseTower();
        try {
            Destroy(this.gameObject);
        } catch {

        }
    }
}
