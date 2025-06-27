Shader "Custom/TileRegionTiling_ScaledUV"
{
    Properties
    {
        _MainTex("Tilemap", 2D) = "white" {}
        _TileScale("Tile Size", Vector) = (0.2, 0.3333, 0, 0)     // 1/5 x, 1/3 y
        _TileOffset("Tile Offset", Vector) = (0.0, 0.0, 0, 0)     // tile offset in atlas
        _TileRepeat("Tile Repeat Count", Float) = 1.0             // how many times per unit
        _AtlasAspect("Atlas Aspect (Width / Height)", Float) = 1.6667
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
                float _AtlasAspect;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 scale : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;

                    // Get object scale from object-to-world matrix
                    float3 scaleX = float3(unity_ObjectToWorld._m00, unity_ObjectToWorld._m01, unity_ObjectToWorld._m02);
                    float3 scaleY = float3(unity_ObjectToWorld._m10, unity_ObjectToWorld._m11, unity_ObjectToWorld._m12);
                    float3 scaleZ = float3(unity_ObjectToWorld._m20, unity_ObjectToWorld._m21, unity_ObjectToWorld._m22);

                    // Approximate XY scale magnitude
                    float scaleU = length(scaleX); // typically affects horizontal tiling
                    float scaleV = length(scaleY); // typically affects vertical tiling

                    o.scale = float3(scaleU, scaleV, 0);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 scaledUV = i.uv * i.scale.xy * _TileRepeat;

                    // Compensate for atlas shape (e.g., 5x3 = 1.6667)
                    scaledUV.x /= _AtlasAspect;

                    // Sample the tile from atlas
                    float2 uv = frac(scaledUV / _TileScale) * _TileScale + _TileOffset;

                    return tex2D(_MainTex, uv);
                }
                ENDCG
            }
        }
}