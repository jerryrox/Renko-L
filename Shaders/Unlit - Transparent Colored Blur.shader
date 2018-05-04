Shader "Unlit/Transparent Colored Blur"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : SV_Target
			{
				half2 coord = IN.texcoord;
				fixed blurSize = 0.005;
				fixed iterations = 4;
				fixed4 result = fixed4(0, 0, 0, 0);

				if(iterations == 0) {
					result = tex2D(_MainTex, fixed2(coord.x, coord.y));
				}
				else {
					// Initial blend addition.
					fixed totalWeight = (iterations + 1) * 4;
					result += tex2D(_MainTex, fixed2(coord.x, coord.y)) * totalWeight;

					// Blend-in from other coordinates.
					for(int i=0; i<iterations; i++) {
						fixed blendAmount = i * blurSize;
						fixed weight = i + 1;
						totalWeight += weight * 4;

						result += tex2D(_MainTex, fixed2(coord.x - blendAmount, coord.y)) * weight;
						result += tex2D(_MainTex, fixed2(coord.x + blendAmount, coord.y)) * weight;
						result += tex2D(_MainTex, fixed2(coord.x, coord.y - blendAmount)) * weight;
						result += tex2D(_MainTex, fixed2(coord.x, coord.y + blendAmount)) * weight;
					}

					result /= totalWeight;
				}

				return result * IN.color;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			//ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
