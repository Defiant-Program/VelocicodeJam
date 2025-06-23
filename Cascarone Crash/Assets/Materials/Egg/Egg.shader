Shader "UI/EggDecorOverlay"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {}
        _DecorTex("Decor Overlay", 2D) = "white" {}
        _Blend("Decor Blend", Range(0,1)) = 1
        _Color("Tint", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
            Lighting Off Cull Off ZWrite Off ZTest[unity_GUIZTestMode] Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _DecorTex;
                float4 _DecorTex_ST;
                float4 _Color;
                float _Blend;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 baseCol = tex2D(_MainTex, i.uv) * _Color;
                    fixed4 decorCol = tex2D(_DecorTex, i.uv);
                    baseCol.rgb = lerp(baseCol.rgb, decorCol.rgb, _Blend);
                    return baseCol;
                }
                ENDCG
            }
        }
}