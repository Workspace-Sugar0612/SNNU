Shader "UI/EdgeFlow/AlphaEdge"
{
    Properties
    {
        _MainTex("Sprite", 2D) = "white" {}
        _Color("Glow Color", Color) = (0,1,1,1)
        _SweepWidth("Sweep Width", Range(0.01,0.5)) = 0.2
        _SweepPos("Sweep Position", Range(0,1)) = 0
        _Intensity("Intensity", Range(0,5)) = 1
        _EdgeSoft("Edge Softness", Range(0.001,0.1)) = 0.02
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        LOD 100

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            float4 _Color;
            float _SweepWidth;
            float _SweepPos;
            float _Intensity;
            float _EdgeSoft;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;

                // 计算边缘: 0 = 中心, 1 = 边缘
                float edge = smoothstep(0.0, _EdgeSoft, alpha) - smoothstep(1.0 - _EdgeSoft, 1.0, alpha);

                // 沿边缘做流光
                float sweep = smoothstep(0.0, _SweepWidth, frac(_SweepPos + i.uv.x + i.uv.y));

                half4 col = _Color * edge * sweep * _Intensity;
                col.a = col.a * edge; // 保证内部透明
                return col;
            }

            ENDHLSL
        }
    }
}