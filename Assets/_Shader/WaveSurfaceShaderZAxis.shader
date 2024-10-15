Shader "Custom/WaveSurfaceShaderZAxis"
{
    Properties
    {
        _Amplitude ("Wave Amplitude", Float) = 0.1
        _Frequency ("Wave Frequency", Float) = 1.0
        _Speed ("Wave Speed", Float) = 1.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Include UnityCG for common helper functions
        #include "UnityCG.cginc"

        // Properties to control the wave
        uniform float _Amplitude;
        uniform float _Frequency;
        uniform float _Speed;

        // Input structure for surface shader
        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos; // We need world position for wave calculation
        };

        // Texture sampler
        sampler2D _MainTex;

        // Wave Vertex Shader
        void vert(inout appdata_full v)
        {
            // Calculate the wave based on the Z position
            float wave = sin(v.vertex.z * _Frequency + _Time.y * _Speed) * _Amplitude;

            // Apply the wave effect to the y-coordinate (height)
            v.vertex.y += wave;
        }

        // Surface function for lighting and texture
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the albedo texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb; // Assign the sampled color to the Albedo
        }
        ENDCG
    }
    FallBack "Diffuse"
}
