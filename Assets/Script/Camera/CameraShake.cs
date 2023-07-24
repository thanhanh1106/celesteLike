using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;

    float timer;
    CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopShake();
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0) StopShake();
        }
    }
    public void ShakeCamera(float ShakeIntensity, float ShakeFrequency, float time)
    {
        multiChannelPerlin.m_AmplitudeGain = ShakeIntensity;
        multiChannelPerlin.m_FrequencyGain = ShakeFrequency;
        timer = time;
    }
    void StopShake()
    {
        multiChannelPerlin.m_AmplitudeGain = 0;
        multiChannelPerlin.m_FrequencyGain = 0;
        timer = 0;
    }
}
