using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpawnMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    private RectTransform myGoRect;
    private float speed = 100.0f;
    [SerializeField]
    private float shownPos;
    [SerializeField]
    private float hiddenPos;
    private bool hidden = false;

    // Start is called before the first frame update
    void Start()
    {
        myGoRect = menu.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlideMenu() {
        if (hidden) {
            StartCoroutine(Show());
            hidden = false;
        } else {
            StartCoroutine(Hide());
            hidden = true;
        }
    }

    IEnumerator Hide() {
        while (myGoRect.localPosition.x < hiddenPos) {
            myGoRect.Translate(Vector3.right * speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Show() {
        while (myGoRect.localPosition.x > shownPos) {
            myGoRect.Translate(Vector3.left * speed * Time.deltaTime);
            yield return null;
        }
    }
}
