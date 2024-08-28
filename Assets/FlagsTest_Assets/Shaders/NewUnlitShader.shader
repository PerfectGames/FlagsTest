// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent" {
Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _FadeLength("Fade Length", Range(0, 2)) = 1
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv_MainTex,screenPos,worldPos)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
                float2 uv_MainTex;
                float4 screenPos;
                float3 worldPos;
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            fixed4 _Color;
            float _FadeLength;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float surfZ = -mul(UNITY_MATRIX_V, float4(IN.worldPos.xyz, 1)).z;
                float diff = sceneZ - surfZ;
                float intersect = 1 - saturate(diff / _FadeLength);

                fixed4 col = tex2D(_MainTex, i.texcoord);
                col.a *= intersect;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
        ENDCG
    }
}

}