using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMesh : MonoBehaviour
{   
    public Color meshColor = Color.white;
    [HideInInspector]
    public Renderer rend;
    private MaterialPropertyBlock propBlock;

    public void SetupMesh() {
        Debug.Log("Mesh setup");
        rend = GetComponent<MeshRenderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void UpdateShaderProperties(Vector3 boundsMax, Vector3 boundsMin,
                                         Vector3 ClipMax, Vector3 ClipMin)
    {
        //Debug.Log(i);
        rend.GetPropertyBlock(propBlock);

        //Update boundary box and clipping positions in the shader
        propBlock.SetVector("BoundsMax", boundsMax);
        propBlock.SetVector("BoundsMin", boundsMin);
        propBlock.SetVector("ClipMax", ClipMax);
        propBlock.SetVector("ClipMin", ClipMin);

        //Other props
        propBlock.SetVector("ClippingModelColor", meshColor);

        rend.SetPropertyBlock(propBlock); 
    }
}

