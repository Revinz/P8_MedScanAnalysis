using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Min_Max_Slider;
[ExecuteInEditMode]
public class ModelClippingHandler : MonoBehaviour 
{

    //Properties used for the shader interaction
    public Vector3 ClipMax = new Vector3(1, 1, 1);
    public Vector3 ClipMin = new Vector3(0, 0, 0);
    public MinMaxSlider sliderX;
    public MinMaxSlider sliderY;
    public MinMaxSlider sliderZ;

    private Vector3 boundsMax;
    private Vector3 boundsMin;

    private List<ModelMesh> ModelMeshes;

    void OnEnable() {
        Debug.Log("Clipping handler setup - ");
        SetupShaders();
    }
    private void Start()
    {

        UpdateXValues();
        UpdateYValues();
        UpdateZValues();
    }
    // Update is called once per frame
    void Update()
    {
        updateObjectBoundsInfo();
        updateShaderProperties();
    }

    // Gets all the renderes and sets up the material property block
    // which allows for seperate instance values for the shader properties
    void SetupShaders() {
        ModelMeshes = new List<ModelMesh>();

        foreach (Transform child in transform) {
           ModelMesh mesh = child.GetComponent<ModelMesh>();
           mesh.SetupMesh();
           ModelMeshes.Add(mesh);
        }  

        //Allows for the model being 1 big model instead of multiple meshes
        ModelMesh rootModelMesh = GetComponent<ModelMesh>();
        if (rootModelMesh != null) {
            rootModelMesh.SetupMesh();
            ModelMeshes.Add(rootModelMesh);
        }
    }

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


    // Finds the max and min bounderies for the model.
    void updateObjectBoundsInfo() {
        Vector3 tmpMax = ModelMeshes[0].rend.bounds.max;
        Vector3 tmpMin = ModelMeshes[0].rend.bounds.min;;
        foreach (ModelMesh mesh in ModelMeshes)
        {   
            //Compare and get max bounds
            tmpMax.x = Mathf.Max(tmpMax.x, mesh.rend.bounds.max.x);
            tmpMax.y = Mathf.Max(tmpMax.y, mesh.rend.bounds.max.y);
            tmpMax.z = Mathf.Max(tmpMax.z, mesh.rend.bounds.max.z);

            //Compare and get min bounds
            tmpMin.x = Mathf.Min(tmpMin.x, mesh.rend.bounds.min.x);
            tmpMin.y = Mathf.Min(tmpMin.y, mesh.rend.bounds.min.y);
            tmpMin.z = Mathf.Min(tmpMin.z, mesh.rend.bounds.min.z);
        }

        boundsMax = tmpMax;
        boundsMin = tmpMin;
    }

    //Updates all the clipping shaders in the gameobject tree with the model
    void updateShaderProperties() {

        foreach (ModelMesh mesh in ModelMeshes)
        {
            mesh.UpdateShaderProperties(boundsMax, boundsMin, ClipMax, ClipMin);
        }

    }
}

