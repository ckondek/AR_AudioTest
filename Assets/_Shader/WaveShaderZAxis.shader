Shader "Custom/WaveShader"
{
    Properties
    {
        _Amplitude ("Wave Amplitude", Float) = 0.1
        _Frequency ("Wave Frequency", Float) = 1.0
        _Speed ("Wave Speed", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
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

            // Properties
            uniform float _Amplitude;
            uniform float _Frequency;
            uniform float _Speed;

            // Vertex Input and Output Structure
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

            // Vertex Shader
            v2f vert(appdata v)
            {
                v2f o;

                // Calculate wave effect based on the local Z position
                float wave = sin(v.vertex.z * _Frequency + _Time.y * _Speed) * _Amplitude;

                // Modify the vertex position's Y coordinate only
                v.vertex.y += wave;

                // Transform the modified vertex position back to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Pass through the UV coordinates
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader (for simple texture rendering)
            sampler2D _MainTex;
            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
