using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Min_Max_Slider;
using UnityVolumeRendering;

public class ModelClippingHandler : MonoBehaviour 
{   
    public Material[] _mat;
    //Properties used for the shader interaction
    [Header("Shader properties")]
    [Range(0.0f, 1.0f)]
    public float darkeningAmountFront = 0;
    [Range(0.0f, 1.0f)]
    public float darkeningAmountBack = 1;
    public Vector3 ClipMax = new Vector3(1, 1, 1);
    public Vector3 ClipMin = new Vector3(0, 0, 0);

    [Header("Slider Objects")]
    public MinMaxSlider sliderX;
    public MinMaxSlider sliderY;
    public MinMaxSlider sliderZ;

    private List<ModelMesh> ModelMeshes = new List<ModelMesh>();

    private void createModelObject(VolumeDataset volumeDataset) {
        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Cube);
        model.transform.parent = this.transform;
        model.GetComponent<MeshRenderer>().materials = _mat;

        model.AddComponent<ModelMesh>();
        ModelMesh newModel = model.GetComponent<ModelMesh>();
        newModel.dataset = volumeDataset;
        Debug.Log("Pre-add");
        Debug.Log(ModelMeshes.Count);
        ModelMeshes.Add(newModel);
        Debug.Log(ModelMeshes.Count);
        Debug.Log("Post-add");
        newModel.SetupMesh();

        Debug.Log("Model created with volume dataset");
    }

    private void Start()
    {
        string path = @"CHANGE PATH TO DICOM MODEL FOLDER´THAT CONTAINS ALL THE .DCM FILES - Can be anywhere on the PC";
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

    public void UpdateXValues()
    {
        ClipMin.x = sliderX.minValue/sliderX.maxLimit;
        ClipMax.x = sliderX.maxValue/sliderX.maxLimit;

    }
    public void UpdateYValues()
    {
        ClipMin.y = sliderY.minValue / sliderY.maxLimit;
        ClipMax.y = sliderY.maxValue / sliderY.maxLimit;
    }
    public void UpdateZValues()
    {
        ClipMin.z = sliderZ.minValue / sliderZ.maxLimit;
        ClipMax.z = sliderZ.maxValue / sliderZ.maxLimit;
    }


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
            mesh.UpdateShaderProperties(ClipMax, ClipMin, darkeningAmountBack, darkeningAmountFront);
        }

    }
}

