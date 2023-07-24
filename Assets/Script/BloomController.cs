using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BloomController : Singleton<BloomController>
{
    public float bloomDuration = 2f; // Thời gian hiệu ứng bloom
    public float bloomIntensity = 1000f; // Cường độ bloom khi chết
    public float bloomEndIntensity = 0f; // Cường độ bloom sau khi hồi sinh

    private bool isDead = false;
    private float timer = 0f;

    private PostProcessVolume postProcessVolume;
    private Bloom bloomLayer;

    protected override void Awake()
    {
        MakeSingleton(false);
    }
    void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out bloomLayer);
    }

    void Update()
    {
        if (isDead)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / bloomDuration);
            bloomLayer.intensity.value = Mathf.Lerp(bloomIntensity, bloomEndIntensity, t);

            if (timer >= bloomDuration)
            {
                // Hoàn thành hiệu ứng bloom, bạn có thể thực hiện các xử lý hồi sinh ở đây
                isDead = false;
            }
        }
    }

    public void PlayBloomEffect()
    {
        isDead = true;
        timer = 0f;
    }
}
