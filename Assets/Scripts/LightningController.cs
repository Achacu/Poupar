using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] private EventSender sender;
    [SerializeField] private Animation anim;
    [SerializeField] private float soundToLightDelay = 0f;
    [SerializeField] private float minCooldown = 2f;
    [SerializeField] private float maxCooldown = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LightningLoop());
    }

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
