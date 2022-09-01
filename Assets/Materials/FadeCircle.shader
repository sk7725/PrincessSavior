Shader "Custom/FadeCircle"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _Cutoff("Cutoff", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest Always
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float _Cutoff;

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

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = float2((i.uv.x - 0.5) * (_ScreenParams.x / _ScreenParams.y), (i.uv.y - 0.5));
                if (pos.x * pos.x + pos.y * pos.y < _Cutoff * _Cutoff * 1.5 * 1.5) return fixed4(.0,.0,.0,.0);
                return _Color;
            }
            ENDCG
        }
    }
}
