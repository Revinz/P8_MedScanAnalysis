using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

[ExecuteInEditMode]
public class TestVolumeShader : MonoBehaviour
{

    void OnEnable()
    {
        Debug.Log("Loading slices");
        DICOMLoader importer = new DICOMLoader();
        VolumeDataset volume = importer.LoadFolder(@"C:\Users\PStaa\OneDrive\Skrivebord\MED8\Dicom3DModel\VitreaDVD\DICOM\ST00001\SE00001 - Copy (2)");
        Debug.Log("Loaded volume data");
        Debug.Log(volume);


        Debug.Log("Updating 3D texture on shader");
        gameObject.GetComponent<Renderer>().sharedMaterial.SetTexture("_Volume", volume.GetDataTexture());
        Debug.Log("Shader update complete.");
    }
}
