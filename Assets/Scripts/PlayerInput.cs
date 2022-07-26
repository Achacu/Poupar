using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Controls controls;
    public Controls Controls
    {
        get
        {
            if (controls == null)
            {
                controls = new Controls();
                controls.Enable();
            }
            return controls;
        }
    }
    private void OnEnable()
    {
        Controls.Enable();
    }
    private void OnDisable()
    {
        Controls.Disable();
    }
}
