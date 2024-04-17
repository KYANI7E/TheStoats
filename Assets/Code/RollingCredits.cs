using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollingCredits : MonoBehaviour
{
    private float speed = 100.0f;
    private float textPosBegin = -900.0f;
    private float textPosEnd = 2950.0f;
    private RectTransform myGoRect;

    [SerializeField]
    private TextMeshProUGUI mainText;

    private void OnEnable() {
        myGoRect = gameObject.GetComponent<RectTransform>();
        this.transform.localPosition = new Vector3(transform.localPosition.x, textPosBegin, transform.localPosition.z);

        StartCoroutine(AutoScroll());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator AutoScroll() {
        while (myGoRect.localPosition.y < textPosEnd) {
            myGoRect.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }

    }
}
