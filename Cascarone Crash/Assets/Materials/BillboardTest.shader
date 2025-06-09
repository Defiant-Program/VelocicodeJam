Shader "Custom/BillboardTest"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Size("Size Multiplier", Float) = 1.0
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _Color;
                float _Size;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;

                    // World center of the object
                    float3 worldPos = mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;

                    // Get camera vectors (for full-facing billboard)
                    float3 camRight = UNITY_MATRIX_IT_MV._m00_m10_m20;
                    float3 camUp = UNITY_MATRIX_IT_MV._m01_m11_m21;

                    // Get object's world scale
                    float3 objScale = float3(
                        length(unity_ObjectToWorld._m00_m01_m02),
                        length(unity_ObjectToWorld._m10_m11_m12),
                        length(unity_ObjectToWorld._m20_m21_m22)
                    );

                    // Apply local vertex offset using camera-aligned right & up
                    float3 offset = (v.vertex.x * camRight + v.vertex.y * camUp) * _Size * objScale.x;

                    // Final position
                    float3 finalPos = worldPos + offset;

                    // Project to screen
                    o.vertex = UnityWorldToClipPos(float4(finalPos, 1.0));
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 tex = tex2D(_MainTex, i.uv) * _Color;
                    return tex;
                }
                ENDCG
            }
        }
    /*
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1,1,1,1)
        _Size("Size", Float) = 1.0
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float _Size;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;

                    // Center of the object in world space
                    float3 worldCenter = mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;

                    // Full-facing billboard: build a quad in view space
                    float3 right = UNITY_MATRIX_IT_MV._m00_m10_m20; // right
                    float3 up = UNITY_MATRIX_IT_MV._m01_m11_m21; // up

                    float3 billboardPos = worldCenter + (v.vertex.x * right + v.vertex.y * up) * _Size;

                    // Project to clip space
                    o.vertex = UnityWorldToClipPos(float4(billboardPos, 1.0));
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 tex = tex2D(_MainTex, i.uv) * _Color;
                    return tex;
                }
                ENDCG
            }
        }
        */
}