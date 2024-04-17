using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlaceUnit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Vector3 mousePos;
    [SerializeField]
    private GameObject objCopy;


    [SerializeField]
    public GameObject spawnPrefab;

    bool placeUnit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        
        if (objCopy != null) {
            objCopy.transform.position = mousePos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Color curColor = gameObject.GetComponent<Image>().color;
        curColor.a = 0.75f;
        gameObject.GetComponent<Image>().color = curColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Color curColor = gameObject.GetComponent<Image>().color;
        curColor.a = 1.0f;
        gameObject.GetComponent<Image>().color = curColor;
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        if (!placeUnit) {
            objCopy = Instantiate(transform.GetComponent<Image>(), mousePos, Quaternion.identity).gameObject;
            objCopy.transform.SetParent(transform, false);
            Destroy(objCopy.GetComponent<PlaceUnit>());

            placeUnit = true;
        } else {
            Destroy(objCopy);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
            Instantiate(spawnPrefab, worldPosition, Quaternion.identity);

            placeUnit = false;
        }

    }
}
