// Adapted from Lab 4 Code for wave movements and Lab 5 for Blinn Phong shader lighting
// Combining the wave and Blinn-Phong shading into one shader to create realistic waves and good lighting.
Shader "Unlit/WaterPhong" {
    
    Properties {

        // Water properties
        _MainTex ("Texture", 2D) = "white" {}
        _Transparency("Transparency", Range(0.0,1.0)) = 0.5

        // Configure the physics of the wave
        _Amplitude("Amplitude", Range(-10,10)) = 0.2
        _Wavelength("Wavelength", Range(-10,10)) = 0.25
        _Speed("Speed", Range(-10,10)) = 1

    }
    SubShader { 
        // Create transparent water
        Tags {"Queue"="Transparent" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass {
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)

            // The lighting for the water
            uniform float3 _PointLightColor = (0, 0, 0);
			uniform float3 _PointLightPosition = (100.0, 100.0, 100.0);
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Transparency;
            float _Amplitude, _Wavelength, _Speed;

            struct Input {
                float2 uv_MainTex;
            };

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

				float4 normal : NORMAL;
				float4 color : COLOR;
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;

				float4 color : COLOR;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
            };

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                // Displace original vertex to model space
                vertOut o;

                /******************************** Water ********************************/

                // Configure the displacement of the waves and model realistic movements for the waves
                float period = 2 * UNITY_PI/_Wavelength;
                float4 displacement = float4(0, _Amplitude * sin(period * (v.vertex.x - _Speed * _Time.y)), _Amplitude * cos(period * (v.vertex.z - _Speed * _Time.y)), 0);
                v.vertex += displacement;
                
                /******************************** Light ********************************/

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
                
                o.color = v.color;

                o.worldVertex = worldVertex;
                o.worldNormal = worldNormal;

                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                  
                return o;
            }
                        
            // Code from: Original Cg/HLSL code stub copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
				float Kd = 2;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				// calculate the reflection
                float LdotN = dot(L, interpNormal);
				// color of each pixel
                float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);
				
                /** Specular Light Calculation **/

				// Calculate specular reflections
				float Ks = 2;
				float specN = 1000; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 R = float3(0.0, 0.0, 0.0);

                // Using the Blinn-Phong Formula to replace the R.L with N.H
                float3 H = normalize(V + L);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);
            
                float4 returnColor = float4(0.0f,0.0f,0.0f,0.0f);

                // Blinn-Phong Light Formula
                returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
                returnColor.a = v.color.a;

				// return the shader texture with the Blinn-Phong Lighting
                fixed4 col = tex2D(_MainTex, v.uv) * returnColor * _Transparency;
                return col;
            }
            ENDCG
        }
    }
}