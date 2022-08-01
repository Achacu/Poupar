using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMaskWithCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<RectTransform>();
        tChild = t.GetChild(0) as RectTransform;
        lastPos = t.position;
    }

    private Vector3 lastPos;
    private RectTransform t;
    private RectTransform tChild;
    // Update is called once per frame
    void Update()
    {
        t.position = Input.mousePosition;
        tChild.position += -(t.position - lastPos); //corrects the displacement due to parent movement
        lastPos = t.position;
    }
}
