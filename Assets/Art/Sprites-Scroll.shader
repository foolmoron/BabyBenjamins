Shader "Sprites/Scroll"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _OffsetX("Offset X", Range(0, 1)) = 0
        _PaddingX("Padding X", Range(0, 1)) = 0
        _OffsetY("Offset Y", Range(0, 1)) = 0
        _PaddingY("Padding Y", Range(0, 1)) = 0
	}

    SubShader
    {
        Tags { "Queue"="Transparent" }
 
        Pass
        {
            ZWrite On
            ColorMask 0
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON

            struct vertexInput
            {
                float4 vertex: POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct fragmentInput
            {
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float4 world : TEXCOORD1;
            };

            sampler2D _MainTex;
            half4 _Color;
            float _OffsetX;
            float _PaddingX;
            float _OffsetY;
            float _PaddingY;

            fragmentInput vert(vertexInput i)
            {
                fragmentInput o;
                o.pos = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
				o.world = mul(unity_ObjectToWorld, i.vertex);
                return o;
            }

            fixed4 frag(fragmentInput i) : COLOR
            {
                float2 uv = frac(i.uv + float2(_OffsetX, _OffsetY));
                uv.x = _PaddingX / 2 + uv.x * (1 - _PaddingX);
                uv.y = _PaddingY / 2 + uv.y * (1 - _PaddingY);
                fixed4 col = tex2D(_MainTex, uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}