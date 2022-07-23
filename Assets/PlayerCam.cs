using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform orientation;
    float yRot = 0;
    float xRot = 0;
    // Update is called once per frame
    void LateUpdate()
    {
        yRot += Input.GetAxisRaw("Mouse X") /** Time.deltaTime*/ * sensX;
        xRot -= Input.GetAxisRaw("Mouse Y") /** Time.deltaTime */* sensY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);

        orientation.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
