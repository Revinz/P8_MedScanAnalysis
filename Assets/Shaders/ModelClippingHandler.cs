using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Min_Max_Slider;
using UnityVolumeRendering;

public class ModelClippingHandler : MonoBehaviour 
{   
    public Material[] _mat;
    [Header("Shader properties")]
    public ClippingShaderProps props;


    private List<ModelMesh> ModelMeshes = new List<ModelMesh>();

    private void createModelObject(VolumeDataset volumeDataset) {
        props = new ClippingShaderProps();
        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Cube);
        model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        model.transform.parent = this.transform;
        model.GetComponent<MeshRenderer>().materials = _mat;

        model.AddComponent<ModelMesh>();
        ModelMesh newModel = model.GetComponent<ModelMesh>();
        newModel.dataset = volumeDataset;
        ModelMeshes.Add(newModel);
        newModel.SetupMesh();

        model.transform.Rotate(new Vector3(-90, 0, 0));
    }

    private void Start()
    {
        string path = @"D:\Projects\Spine_HardTissue";
        VolumeDataset dataset = new DICOMLoader().LoadFolder(path);
        createModelObject(dataset);
        // UpdateXValues();
        // UpdateYValues();
        // UpdateZValues();
    }
    // Update is called once per frame
    void Update()
    {
        //updateObjectBoundsInfo();
        updateShaderProperties();
    }

    // Gets all the renderes and sets up the material property block
    // which allows for seperate instance values for the shader properties
    // void SetupShaders() {
    //     ModelMeshes = new List<ModelMesh>();

    //     foreach (Transform child in transform) {
    //        ModelMesh mesh = child.GetComponent<ModelMesh>();
    //        mesh.SetupMesh();
    //        ModelMeshes.Add(mesh);
    //     }  

    //     //Allows for the model being 1 big model instead of multiple meshes
    //     ModelMesh rootModelMesh = GetComponent<ModelMesh>();
    //     if (rootModelMesh != null) {
    //         rootModelMesh.SetupMesh();
    //         ModelMeshes.Add(rootModelMesh);
    //     }
    // }

    // // Finds the max and min bounderies for the model.
    // void updateObjectBoundsInfo() {
    //     Vector3 tmpMax = ModelMeshes[0].rend.bounds.max;
    //     Vector3 tmpMin = ModelMeshes[0].rend.bounds.min;;
    //     foreach (ModelMesh mesh in ModelMeshes)
    //     {   
    //         //Compare and get max bounds
    //         tmpMax.x = Mathf.Max(tmpMax.x, mesh.rend.bounds.max.x);
    //         tmpMax.y = Mathf.Max(tmpMax.y, mesh.rend.bounds.max.y);
    //         tmpMax.z = Mathf.Max(tmpMax.z, mesh.rend.bounds.max.z);

    //         //Compare and get min bounds
    //         tmpMin.x = Mathf.Min(tmpMin.x, mesh.rend.bounds.min.x);
    //         tmpMin.y = Mathf.Min(tmpMin.y, mesh.rend.bounds.min.y);
    //         tmpMin.z = Mathf.Min(tmpMin.z, mesh.rend.bounds.min.z);
    //     }

    //     boundsMax = tmpMax;
    //     boundsMin = tmpMin;
    // }

    //Updates all the clipping shaders in the gameobject tree with the model
    void updateShaderProperties() {

        Debug.Log(ModelMeshes.Count);
        foreach (ModelMesh mesh in ModelMeshes)
        {
            mesh.UpdateShaderProperties(props);
        }

    }
}

