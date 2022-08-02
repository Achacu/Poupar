using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsPlayer : MonoBehaviour
{
    [SerializeField] private CharMove move;
    [SerializeField] private FMODUnity.EventReference footstepsRef;

    public void Awake()
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance(footstepsRef);
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        move.OnMoveChange += ChangeFootstepsState;
    }
    void OnDisable()
    {
        move.OnMoveChange -= ChangeFootstepsState;
    }

    private FMOD.Studio.EventInstance footsteps;
    private void ChangeFootstepsState(bool moving)
    {
        if(moving)
        {
            footsteps.start();
        }
        else
        {
            footsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
