using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interact : MonoBehaviour
{
    public event Action<bool> OnCanInteractChange = delegate { };
    private bool canInteract = false;
    [SerializeField] private float interactCheckDelta = 1f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private Transform camT;
    [SerializeField] private float maxInteractRayDst = 1f;
    [SerializeField] private PlayerInput playerInput;
    WaitForSeconds waitTime;    
    // Start is called before the first frame update
    void Start()
    {
        waitTime = new WaitForSeconds(interactCheckDelta);
        StartCoroutine(CheckCanInteract());
    }

    public void OnEnable()
    {
        playerInput.Controls.General.Interact.performed += PressedInteract;
    }
    public void OnDisable()
    {
        playerInput.Controls.General.Interact.performed -= PressedInteract;
    }

    private void PressedInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (canInteract) print("interacted!");
        else print("can't interact");
    }

    private IEnumerator CheckCanInteract()
    {
        if (interactCheckDelta == 0)
        {
            Debug.LogError("zero delta interact check value");
            yield break;
        }
        while(true)
        {
            RaycastHit hit;
            //Debug.DrawLine(camT.position, camT.position + camT.forward * maxInteractRayDst, Color.white, 1);
            if(Physics.Raycast(camT.position, camT.forward, out hit, maxInteractRayDst, interactMask))
            {
                if(!canInteract)
                {
                    OnCanInteractChange(true);
                    canInteract = true;
                    print("started interact: "+canInteract);
                } 
            }
            else
            {
                if (canInteract)
                {
                    OnCanInteractChange(false);
                    canInteract = false;
                    print("ended interact: " + canInteract);
                }
            }
                
            yield return waitTime;
        }
    }
}
