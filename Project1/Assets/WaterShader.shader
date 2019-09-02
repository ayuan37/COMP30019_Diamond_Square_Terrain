//UNITY_SHADER_NO_UPGRADE

// Modified Lab 4 Code for Waves
Shader "Unlit/WaterShader" {
    
    Properties {
        _Color("Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}

        // Configure the physics of the wave
        _Amplitude("Amplitude", Range(-10,10)) = 3
        _Wavelength("Wavelength", Range(-10,10)) = 3
        _Speed("Speed", Range(-10,10)) = 1

    }
    SubShader { 

        Tags {"RenderType" = "Opaque"}
        
        Pass {
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            fixed4 _Color;
            float _Amplitude, _Wavelength, _Speed;

            struct Input {
                float2 uv_MainTex;
            };

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                // Displace the original vertex in model space
                vertOut o;

                // Configure the displacement of the waves
                float period = 2 * UNITY_PI/_Wavelength;
                float4 displacement = float4(0.0f, _Amplitude * sin(period * (v.vertex.x - _Speed * _Time.y)), _Amplitude * cos(period * (v.vertex.z - _Speed * _Time.y)), 0.0f);
                v.vertex += displacement;  
                
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                
                o.uv = v.uv;
                  
                return o;
            }
            
            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target {
                fixed4 col = tex2D(_MainTex, v.uv);
                return col;
            }
            ENDCG

        }
    }
}