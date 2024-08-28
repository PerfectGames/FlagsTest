Shader "Custom/Intersection"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _FadeLength("Fade Length", Range(0, 10)) = 1
    }
    SubShader {
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite On

        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        struct Input {
            float2 uv_MainTex;
            float4 screenPos;
            float3 worldPos;
        };

        sampler2D _CameraDepthTexture;
        fixed4 _Color;
        float _FadeLength;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
            float surfZ = -mul(UNITY_MATRIX_V, float4(IN.worldPos.xyz, 1)).z;
            float diff = sceneZ - surfZ;
            float intersect = 1 - saturate(diff / _FadeLength);

            fixed4 col = _Color;
            col.a *= pow(intersect, 4);
            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}