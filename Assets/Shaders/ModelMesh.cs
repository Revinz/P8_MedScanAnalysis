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
        rend.SetPropertyBlock(propBlock); 
    }

    public void UpdateShaderProperties(Vector3 ClipMax, Vector3 ClipMin, float DarkeningAmountBack, float DarkeningAmountFront)
    {
        rend.GetPropertyBlock(propBlock);

        //Update clipping positions and darkening thresholds
        propBlock.SetVector("_ClipMax", ClipMax);
        propBlock.SetVector("_ClipMin", ClipMin);
        propBlock.SetFloat("_DarkeningThresholdFront", DarkeningAmountFront);
        propBlock.SetFloat("_DarkeningThresholdBack", DarkeningAmountBack);

        rend.SetPropertyBlock(propBlock); 
    }
}

