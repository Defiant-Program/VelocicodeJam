// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TileRegionTiling_WithRepeat"
{
    Properties
    {
        _MainTex("Tilemap", 2D) = "white" {}
        _TileScale("Tile Size", Vector) = (0.125, 0.125, 0, 0)
        _TileOffset("Tile Offset", Vector) = (0.375, 0.25, 0, 0)
        _TileRepeat("Tile Repeat Count", Float) = 4.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float2 _TileScale;
                float2 _TileOffset;
                float _TileRepeat;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                
                fixed4 frag(v2f i) : SV_Target
                {
                    // Scale UVs by repeat factor to shrink the tiles
                    float2 uv = frac(i.uv * _TileRepeat);

                    // Now tile within the tile region
                    uv = frac(uv / _TileScale);  // local tile repeat
                    uv *= _TileScale;            // scale back into tile range
                    uv += _TileOffset;           // shift to the correct tile

                    return tex2D(_MainTex, uv);
                }
                
                ENDCG
            }
        }
}