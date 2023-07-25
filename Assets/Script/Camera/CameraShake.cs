using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance // bản thể này thay đổi theo camera change
    {
        get => instance;
        set
        {
            if (instance != null) instance = null; // hủy bản thể cũ trước khi gán cái mới
            instance = value;
        }
    }
    private CameraShake() { }

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
