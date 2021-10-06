Shader "EmbryoFlask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmbryoOffset ("Embryo Offset", Vector) = (0,0,0,0)
        _FlaskTop ("Flask Top And Glass", 2D) = "white" {}
        _FlaskBase ("Flask Base", 2D) = "white" {}
    }
    SubShader
    {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float2 _EmbryoOffset;
            sampler2D _FlaskTop;
            sampler2D _FlaskBase;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 combine(float4 under, float4 over)
            {
                float3 rgb = over.a * over.rgb + (1-over.a) * under.rgb;
                float a = max (under.a, over.a);
                return float4(rgb, a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 base = tex2D (_FlaskBase, uv);
                float4 embryo = tex2D (_MainTex, uv + _EmbryoOffset);
                float4 top = tex2D (_FlaskTop, uv);

                float4 col = combine(combine(base, embryo), top);
                return col;
            }
            ENDCG
        }
    }
}
