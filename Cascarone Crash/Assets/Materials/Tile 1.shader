// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TileRegionTiling_WithRepeat_With_Scale"
{
    Properties
    {
        _MainTex("Tilemap", 2D) = "white" {}
        _TileScale("Tile Size", Vector) = (0.25, 1.0, 0, 0) // One tile is 1/4 wide, full height
        _TileOffset("Tile Offset", Vector) = (0.0, 0.0, 0, 0)
        _TileRepeat("Tile Repeat Count", Float) = 4.0
        _AtlasAspect("Atlas Aspect (Width / Height)", Float) = 4.0
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
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldPos : TEXCOORD0;
                    float3 worldNormal : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float3 wp = i.worldPos;
                    float3 wn = normalize(i.worldNormal);

                    // Blending weights for triplanar
                    float3 blend = abs(wn);
                    blend = pow(blend, 4.0);
                    blend /= (blend.x + blend.y + blend.z);

                    // Base UVs from world projections
                    float2 uvX = frac(wp.zy * _TileRepeat);
                    float2 uvY = frac(wp.xz * _TileRepeat);
                    float2 uvZ = frac(wp.xy * _TileRepeat);

                    // ✅ FIX: Compensate for 4-column aspect ratio (wide tiles)
                    uvX.x /= _AtlasAspect;
                    uvY.x /= _AtlasAspect;
                    uvZ.x /= _AtlasAspect;

                    // Tile selection within region
                    uvX = frac(uvX / _TileScale) * _TileScale + _TileOffset;
                    uvY = frac(uvY / _TileScale) * _TileScale + _TileOffset;
                    uvZ = frac(uvZ / _TileScale) * _TileScale + _TileOffset;

                    // Triplanar blend
                    fixed4 colX = tex2D(_MainTex, uvX);
                    fixed4 colY = tex2D(_MainTex, uvY);
                    fixed4 colZ = tex2D(_MainTex, uvZ);

                    return colX * blend.x + colY * blend.y + colZ * blend.z;
                }
                ENDCG
            }
        }
}