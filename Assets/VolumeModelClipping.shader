// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Volume/Test" {
    Properties {
        _Volume ("Volume Texture (generated)", 3D) = "" {}
        _DarkeningThresholdFront ("Front Darkening Amount", Range(0, 1)) = 0
        _DarkeningThresholdBack ("Back Darkening Amount", Range(0, 1)) = 1
        _ClipMax ("Model Render End (user-adjusted)", Float) =  (1, 1, 1)
        _ClipMin ("Model Render Start (user-adjusted)", Float) =  (0, 0, 0) 
        _NumSteps ("Number of Steps for volume rendering", Float) = 512
    }
SubShader {

    Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
    LOD 100
    Cull Front
    ZTest LEqual
    ZWrite Off    
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        
        //Properties for the model
        sampler3D _Volume;
        float _Radius;
        float3 _ClipMax;
        float3 _ClipMin;
        float _DarkeningThresholdFront;
        float _DarkeningThresholdBack;

        //properties for the volume rendering
        float _NumSteps;
        
        sampler2D _CameraDepthTexture;

        #include "UnityCG.cginc"
        
            struct vs_input {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float4 normal : NORMAL;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;    // Not used. See vert method for more info.
                float3 vertexLocal : TEXCOORD1; //vertex position in object space
                float2 uv : TEXCOORD0;
                //float3 normal : NORMAL;
            };
            
            
            v2f vert (vs_input v)
            {
                v2f o;
                o.uv = v.uv;
                //o.normal = v.normal;
                o.vertexLocal = v.vertex;
                o.pos = UnityObjectToClipPos(v.vertex); //Not sure why removing this breaks the shader - it is not used.
                return o;
            }
            
            float4 getDensity(float3 pos) {
                return tex3Dlod(_Volume, float4(pos.x, pos.y, pos.z, 0.0f));
            }
        
            float4 frag (v2f i) : COLOR
            {

                // The 1.732f is the greatest distance in a box (the diagonals)
                // By dividing this largest distance of a box by 2 
                // + not using the normalized objSpaceViewDir for the ray direction
                // fixes duplicate and inverted rendering
                // Not entirely sure how it works.
                // -- Seems to be that only using half the max length in a box forces the rendering 
                //    of the volume behind the camera to be rendered outside the box instead of inside it. 
                //    Together with not normalizing the objSpaceViewDir which prevents rendering the volume outside the box.
                const float stepSize = 1.732f / 2.0f / _NumSteps; 

                //vertex pos starts at -0.5 instead of 0 in object space.
                float3 rayStartPos = i.vertexLocal + float3(0.5f, 0.5f, 0.5f);
                float3 rayDir = ObjSpaceViewDir(float4(i.vertexLocal, 0.0f)); //Doesn't need to be normalized -> prevents rendering outside the box
                float4 col = float4(0.0f, 0.0f, 0.0f, 0.0f);

                //Max density approach
                float maxDensity = 0.0f;
                for (uint iStep = 0; iStep < _NumSteps; iStep++) {
                    const float t = iStep * stepSize; //Distance covered by the ray for the current iter
                    const float3 currPos = rayStartPos + rayDir * t; //location in object space

                    //Check if curr pos is outside the bounderies of the object
                    // If outside there is not need to continue because nothing will be rendered anyway.
                    if (currPos.x < -0.0001f || currPos.x >= 1.0001f || currPos.y < -0.0001f || currPos.y > 1.0001f || currPos.z < -0.0001f || currPos.z > 1.0001f)
                        break;

                    //Check if the currPos is inside the area that should be rendered, otherwise continue.
                    //Simply disregard this position's value and move on to the next.
                    if (currPos.x < _ClipMin.x || currPos.x >= _ClipMax.x || currPos.y < _ClipMin.y || currPos.y >= _ClipMax.y || currPos.z < _ClipMin.z || currPos.z >= _ClipMax.z )
                        continue;

                    
                    const float density = getDensity(currPos);
                    maxDensity = max(density, maxDensity);
                }
                
                //Might be possible to use clip() instead.
                if (maxDensity <= _DarkeningThresholdFront || maxDensity >= _DarkeningThresholdBack)
                    maxDensity = 0.0f;  

                col = float4(1.0f, 1.0f, 1.0f, maxDensity);
                return col;
            }
        
        ENDCG
        
        }
    }

}
 