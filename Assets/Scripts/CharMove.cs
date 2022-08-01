using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    //[SerializeField] private CharacterController controller;
    /*[SerializeField] */private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float maxSpeedChange = 1f;
    // Start is called before the first frame update

    public event Action<bool> OnMoveChange = delegate { };
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }
   
    Vector2 input = Vector2.zero; 
    // Update is called once per frame
    void Update()
    {
        //input = new Vector2 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        //input.Normalize();
        Vector3 desiredVelocity = new Vector3(input.x, 0, input.y) * speed; //* Time.deltaTime;        
        Vector3 desiredVelocityCorrected = Vector3.MoveTowards(rb.velocity, orientation.TransformVector(desiredVelocity), maxSpeedChange);
        rb.velocity = new Vector3(desiredVelocityCorrected.x, rb.velocity.y, desiredVelocityCorrected.z);
        transform.rotation = orientation.rotation;
        //controller.Move(orientation.TransformVector(moveDir));
    }

    public void OnEnable()
    {
        //playerInput.Controls.General.Enable();
        playerInput.Controls.General.Move.performed += UpdateInput;
    }
    public void OnDisable()
    {
        playerInput.Controls.General.Move.performed -= UpdateInput;
    }

    Vector2 lastInput = Vector2.zero;
    private void UpdateInput(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        input = obj.ReadValue<Vector2>();
        if (input != lastInput)
            OnMoveChange(input != Vector2.zero);
        lastInput = input;
    }
    //public void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    print("collided");
    //}
}
