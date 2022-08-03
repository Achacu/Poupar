using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] private bool playOnAwake = false;
    [SerializeField] private EventSender sender;
    [SerializeField] private EventSender triggerSender;
    [SerializeField] private Animation anim;
    [SerializeField] private float soundToLightDelay = 0f;
    [SerializeField] private float minCooldown = 2f;
    [SerializeField] private float maxCooldown = 5f;
    // Start is called before the first frame update
    public void OnEnable()
    {
        if (sender) triggerSender.OnActivate += StartLightningLoop;
    }
    public void OnDisable()
    {
        if (sender) triggerSender.OnActivate -= StartLightningLoop;
    }

    void Start()
    {
        if(playOnAwake) StartCoroutine(LightningLoop());
    }

    private void StartLightningLoop(EventSender s) => StartCoroutine(LightningLoop());

    private IEnumerator LightningLoop()
    {
        while(true)
        {
            anim.Play();
            yield return new WaitForSeconds(soundToLightDelay);
            sender.TriggerEvent(true);
            yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        }
    }
}
