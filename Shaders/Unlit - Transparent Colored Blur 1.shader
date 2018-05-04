Shader "Hidden/Unlit/Transparent Colored Blur 1"
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
			Offset -1, -1
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
			};

			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.worldPos = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
				return o;
			}

			half4 frag (v2f IN) : SV_Target
			{
				half2 coord = IN.texcoord;
				half blurSize = 0.005;
				half iterations = 4;
				half4 result = fixed4(0, 0, 0, 0);

				if(iterations == 0) {
					result = tex2D(_MainTex, half2(coord.x, coord.y));
				}
				else {
					// Initial blend addition.
					half totalWeight = (iterations + 1) * 4;
					result += tex2D(_MainTex, half2(coord.x, coord.y)) * totalWeight;

					// Blend-in from other coordinates.
					for(int i=0; i<iterations; i++) {
						half blendAmount = i * blurSize;
						half weight = i + 1;
						totalWeight += weight * 4;

						result += tex2D(_MainTex, half2(coord.x - blendAmount, coord.y)) * weight;
						result += tex2D(_MainTex, half2(coord.x + blendAmount, coord.y)) * weight;
						result += tex2D(_MainTex, half2(coord.x, coord.y - blendAmount)) * weight;
						result += tex2D(_MainTex, half2(coord.x, coord.y + blendAmount)) * weight;
					}

					result /= totalWeight;
				}

				// Softness factor
				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipArgs0;

				// Sample the texture
				result = result * IN.color;
				result.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
				return result;
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
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
