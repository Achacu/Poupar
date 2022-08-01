using System;
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
    [SerializeField] private PlayerInput playerInput;
    float yRot = 0;
    float xRot = 0;

    public void OnEnable()
    {
        playerInput.Controls.General.TurnCam.performed += UpdateTurnCam;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnDisable()
    {
        playerInput.Controls.General.TurnCam.performed -= UpdateTurnCam;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void UpdateTurnCam(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        mouseDelta = obj.ReadValue<Vector2>();   
    }
    private Vector2 mouseDelta = Vector2.zero;
    // Update is called once per frame
    void LateUpdate()
    {
        //yRot += Input.GetAxisRaw("Mouse X") /** Time.deltaTime*/ * sensX;
        //xRot -= Input.GetAxisRaw("Mouse Y") /** Time.deltaTime */* sensY;
        yRot += mouseDelta.x * sensX;
        xRot -= mouseDelta.y * sensY;

        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);

        orientation.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
