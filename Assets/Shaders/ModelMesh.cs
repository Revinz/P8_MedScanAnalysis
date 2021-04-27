using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

public class ModelMesh : MonoBehaviour
{   
    public VolumeDataset dataset;
    [HideInInspector]
    public Renderer rend;
    private MaterialPropertyBlock propBlock;

    public void SetupMesh() {
        Debug.Log("Mesh setup");
        rend = GetComponent<MeshRenderer>();
        propBlock = new MaterialPropertyBlock();

        //Setup the texture3D for the mesh
        rend.GetPropertyBlock(propBlock);
        propBlock.SetTexture("_Volume", dataset.GetDataTexture());
        propBlock.SetTexture("_Gradient", dataset.GetGradientTexture()); 
        rend.SetPropertyBlock(propBlock); 
    }

    public void UpdateShaderProperties(ClippingShaderProps props)
    {
        rend.GetPropertyBlock(propBlock);

        //Update clipping positions and darkening thresholds
        propBlock.SetVector("_ClipMax", props.ClipMax);
        propBlock.SetVector("_ClipMin", props.ClipMin);
        propBlock.SetFloat("_DarkeningThresholdFront", props.darkeningAmountFront);
        propBlock.SetFloat("_DarkeningThresholdBack", props.darkeningAmountBack);
        propBlock.SetFloat("_renderingMode", (int)props.renderMode);
        propBlock.SetFloat("_NumSteps", (float)props.Quality);
        rend.SetPropertyBlock(propBlock); 
    }
}

