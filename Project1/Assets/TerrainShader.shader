//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/TerrainShader"
{
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float3 _PointLightColor = (0, 0, 0);
			uniform float3 _PointLightPosition = (100.0, 100.0, 100.0);

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;

				float4 normal: NORMAL;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;

				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Light
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;

				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;
				return o;
			}
			
			// Code from: Original Cg/HLSL code stub copyright (c) 2010-2012 SharpDX - Alexandre Mutel
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				// Estimate normal based on each pixel, may not be of lenght 1
                float3 interpNormal = normalize(v.worldNormal);

                /** Ambient Light Calculation **/
				float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

                /** Diffuse Light Calculation **/

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 0.5;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				// calculate the reflection
                float LdotN = dot(L, interpNormal);
				// color of each pixel
                float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);
				
                /** Specular Light Calculation **/
				// Calculate specular reflections
				float Ks = 0.2;
				float specN = 0.5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);

                // Using the Bling-Phong Formula to replace the R.L with N.H
                float3 H = normalize(V + L);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);
            
                float4 returnColor = float4(0.0f,0.0f,0.0f,0.0f);

                // Blinn-Phong Light Formula
                returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
                returnColor.a = v.color.a;

                return returnColor;
			}
			ENDCG
		}
	}
}
