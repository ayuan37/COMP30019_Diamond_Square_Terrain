// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Adapted from Lab 4 Code for WaveShader and Lab 5 PhongShader

// Combining the wave and Bling-Phong lighting into one shader to create realistic waves and good lighting.
Shader "Unlit/WaterPhong" {
    
    Properties {

        // Water properties
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

            // The lighting for the water
            uniform float3 _PointLightColor = (0, 0, 0);
			uniform float3 _PointLightPosition = (100.0, 100.0, 100.0);
            
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

                // phong
				float4 normal : NORMAL;
				float4 color : COLOR;
                
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;

                // phong
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
            };

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                // Displace original vertex to model space
                vertOut o;
                
                /******************************** Light ********************************/
                
                // Convert Vertex position and corresponding normal into world coords.
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                // multiply to transpose it
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;

                o.worldVertex = worldVertex; // vertex in relation to light
                o.worldNormal = worldNormal;

                /************** WATER ****************/

                // Configure the displacement of the waves
                float period = 2 * UNITY_PI/_Wavelength;
                float4 displacement = float4(0.0f, _Amplitude * sin(period * (v.vertex.x - _Speed * _Time.y)), _Amplitude * cos(period * (v.vertex.z - _Speed * _Time.y)), 0.0f);
                v.vertex += displacement;  
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.uv = v.uv;
                  
                return o;
            }
            
            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target {

                // Estimate normal based on each pixel, may not be of lenght 1
                float3 interpNormal = normalize(v.worldNormal);

                /** Ambient Light Calculation **/
				float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

                /** Diffuse Light Calculation **/

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 1;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				// calculate the reflection
                float LdotN = dot(L, interpNormal);
				// color of each pixel
                float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);
				
                /** Specular Light Calculation **/
				// Calculate specular reflections
				float Ks = 1;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 R = float3(0.0, 0.0, 0.0);

                // Using the Bling-Phong Formula to replace the R.L with N.H
                float3 H = (L+V) / length(L+V);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

                float4 returnColor = float4(0.0f,0.0f,0.0f,0.0f);

                // Bling-Phong Light Formula
                returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
                returnColor.a = v.color.a;

				// return the shader texture with the Bling-Phong Lighting
                fixed4 col = tex2D(_MainTex, v.uv) * returnColor;
                return col;
            }
            ENDCG
        }
    }
}