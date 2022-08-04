using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShaderLink : MonoBehaviour
{
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private List<MeshRenderer> meshes;

    void OnValidate()
    {
        if (meshes.Count == 0 || meshes[0] == null)
        {
            meshes.AddRange(GetComponentsInChildren<MeshRenderer>());
            if (meshes[0] == null) meshes.RemoveAt(0);
        }
    }
    public void Awake()
    {
        OnValidate();
        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
        {
            meshes[i].material.SetFloat("_Sounding", 0);
            meshes[i].material.renderQueue = 3000;
            meshes[i].materials[j].SetFloat("_OverrideAlpha", -1);
        }
    }


    // Start is called before the first frame update
    public void OnEnable()
    {
        soundPlayer.OnPlaySound += StartSoundMetering;
    }
    public void OnDisable()
    {
        soundPlayer.OnPlaySound -= StartSoundMetering;
    }

    private EventInstance instance;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.DSP dsp;
    private FMOD.DSP_METERING_INFO dspMeterInfo;
    private void StartSoundMetering(EventInstance obj)
    {
        instance = obj;
        instance.getChannelGroup(out channelGroup);
        channelGroup.getDSP(0, out dsp);
        dsp.setMeteringEnabled(true, true);
        dsp.getMeteringInfo(new IntPtr(), out dspMeterInfo);

        if(meteringCorot != null)
        {
            StopCoroutine(meteringCorot);
            meteringCorot = null;
        }
        meteringCorot = StartCoroutine(MeterSound());
    }
    private Coroutine meteringCorot;
    [SerializeField] private float maxValue = 0.1f;
    [SerializeField] private float minValue = 0.0001f;
    [SerializeField] private float maxDecrease = 0.1f;
    private float currentValue;
    private float lastValue;
    private IEnumerator MeterSound()
    {
        while (true)
        {
            instance.getChannelGroup(out channelGroup);
            channelGroup.getDSP(0, out dsp);
            dsp.setMeteringEnabled(true, true);
            dsp.getMeteringInfo(new IntPtr(), out dspMeterInfo);
            //print(dspMeterInfo.peaklevel[0] + dspMeterInfo.peaklevel[1]);
            currentValue = Mathf.Min(Mathf.Max(dspMeterInfo.peaklevel[0] + dspMeterInfo.peaklevel[1], minValue) / maxValue, 1);
            //print(currentValue);
            currentValue = (currentValue > lastValue) ? currentValue : Mathf.MoveTowards(lastValue, currentValue, maxDecrease);
            lastValue = currentValue;

            for (int i = 0; i < meshes.Count; i++) meshes[i].material.SetFloat("_Sounding", currentValue);

            yield return null;
        }
    }
}
