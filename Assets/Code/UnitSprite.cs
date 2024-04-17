using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite unit;
    [SerializeField]
    private GameObject spawnPrefab;
    GameObject unitObj;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform myGoRect = gameObject.GetComponent<RectTransform>();
        Vector3 centerPoint = myGoRect.TransformPoint(myGoRect.rect.center);


        unitObj = new GameObject(unit.name, typeof(Image), typeof(PlaceUnit));
        unitObj.GetComponent<Image>().enabled = true;
        unitObj.GetComponent<Image>().sprite = unit;
        unitObj.GetComponent<Image>().color = Color.red;
        unitObj.transform.SetParent(transform);
        unitObj.transform.localPosition = Vector3.zero;
        unitObj.transform.localScale = new Vector3(1, 1, 1);

        unitObj.GetComponent<PlaceUnit>().spawnPrefab = spawnPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
