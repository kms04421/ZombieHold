Shader "Unlit/CurvedWorld"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurveStrength ("Curve Strength", Float) = 0.0005
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _CurveStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;

                // ¿ùµå ÁÂÇ¥·Î º¯È¯
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

            // ZÃà °î·ü
                worldPos.y -= (worldPos.z * worldPos.z) * _CurveStrength;

                // XÃà °î·ü
                worldPos.y -= (worldPos.x * worldPos.x) * _CurveStrength;
                // ´Ù½Ã Å¬¸³ °ø°£À¸·Î º¯È¯
                o.pos = mul(UNITY_MATRIX_VP, float4(worldPos, 1.0));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
