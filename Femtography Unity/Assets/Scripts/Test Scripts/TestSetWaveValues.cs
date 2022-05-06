using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveMaker;

public class TestSetWaveValues : MonoBehaviour
{
    private WaveMakerSurface waveMakerSurface;

    private MaterialPropertyBlock materialPropertyBlock;

    public float Sensitivity { get; set; } = 10;
    public float WaveHeight { get; set; } = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<WaveMakerSurface>())
            waveMakerSurface = GetComponent<WaveMakerSurface>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSensitivity(float sensitivity)
    {
        Sensitivity = sensitivity;
        Debug.Log(Sensitivity);
        materialPropertyBlock.SetFloat("Sensitivity_", Sensitivity);
        GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    }
    public void SetWaveHeight(float waveHeight)
    {
        WaveHeight = waveHeight;
        waveMakerSurface.verticalPushScale = WaveHeight;
    }
}
