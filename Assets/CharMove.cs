using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    //[SerializeField] private CharacterController controller;
    /*[SerializeField] */private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Transform orientation;
    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    Vector2 input = Vector2.zero; 
    // Update is called once per frame
    void Update()
    {
        input = new Vector2 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        input.Normalize();
        Vector3 moveDir = new Vector3(input.x, 0, input.y) * speed; //* Time.deltaTime;        
        Vector3 correctMoveDir = orientation.TransformVector(moveDir);
        rb.velocity = new Vector3(correctMoveDir.x, rb.velocity.y, correctMoveDir.z);
        transform.rotation = orientation.rotation;
        //controller.Move(orientation.TransformVector(moveDir));
    }

    //public void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    print("collided");
    //}
}
