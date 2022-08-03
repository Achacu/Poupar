using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
    [SerializeField] private CharMove move;
    [SerializeField] private MoveCamera moveCam;
    [SerializeField] private PlayerCam rotCam;
    [SerializeField] private bool playerInControl = true;

    [SerializeField] private EventSender startAnimSender;
    [SerializeField] private EventSender startGameSender;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerControl();
    }
    private void UpdatePlayerControl()
    {
        move.enabled = playerInControl;
        moveCam.enabled = playerInControl;
        rotCam.enabled = playerInControl;
    }
    //public void SetPlayerInControl(bool yes)
    //{
    //    playerInControl = yes;
    //    UpdatePlayerControl();
    //}
    public void OnEnable()
    {
        if(startAnimSender) startAnimSender.OnDeactivate += StartAnim;
        if (startGameSender) startGameSender.OnActivate += StartGame;
    }
    public void OnDisable()
    {
        if (startAnimSender) startAnimSender.OnDeactivate -= StartAnim;
        if (startGameSender) startGameSender.OnActivate -= StartGame;
    }

    private void StartGame(EventSender obj)
    {
        playerInControl = true;
        UpdatePlayerControl();
    }

    private void StartAnim(EventSender obj)
    {
        GetComponent<Animation>().Play();
    }

}
