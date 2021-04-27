using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ClippingShaderProps
{
    public enum RenderingMode {
        XRAY = 1,
        SURFACE = 2,
    }

    [Range(0.0f, 1.0f)]
    public float darkeningAmountFront = 0;
    [Range(0.0f, 1.0f)]
    public float darkeningAmountBack = 1;
    public Vector3 ClipMax = new Vector3(1, 1, 1);
    public Vector3 ClipMin = new Vector3(0, 0, 0);
    public RenderingMode renderMode = RenderingMode.XRAY;
    public float Quality=512f;
    
    public ClippingShaderProps() {}

    public ClippingShaderProps(float _darkeningAmountFront, float _darkeningAmountBack,
                         Vector3 _ClipMax, Vector3 _ClipMin, RenderingMode _renderMode,float quality) {
        darkeningAmountFront = _darkeningAmountFront;
        darkeningAmountBack = _darkeningAmountBack;
        ClipMax = _ClipMax;
        ClipMin = _ClipMin;
        renderMode = _renderMode;
        Quality = quality;
    }
}
