Shader "Hidden/Unlit/Transparent Colored SqrBlur 3"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_BlurSize ("Blur size", int) = 1
		_Decay ("Decay", Range(0, 0.125)) = 0.05
		_SampleDistance ("Sample distance", Range(0.001, 0.1)) = 0.01
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
			fixed4 _MainTex_ST;
			int _BlurSize;
			fixed _Decay;
			fixed _SampleDistance;
			fixed4 _ClipRange0 = fixed4(0.0, 0.0, 1.0, 1.0);
			fixed4 _ClipArgs0 = fixed4(1000.0, 1000.0, 0.0, 1.0);
			fixed4 _ClipRange1 = fixed4(0.0, 0.0, 1.0, 1.0);
			fixed4 _ClipArgs1 = fixed4(1000.0, 1000.0, 0.0, 1.0);
			fixed4 _ClipRange2 = fixed4(0.0, 0.0, 1.0, 1.0);
			fixed4 _ClipArgs2 = fixed4(1000.0, 1000.0, 0.0, 1.0);
	
			struct appdata_t
			{
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				fixed4 worldPos : TEXCOORD1;
				fixed2 worldPos2 : TEXCOORD2;
			};

			fixed2 Rotate (fixed2 v, fixed2 rot)
			{
				fixed2 ret;
				ret.x = v.x * rot.y - v.y * rot.x;
				ret.y = v.x * rot.x + v.y * rot.y;
				return ret;
			}
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				o.worldPos.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
				o.worldPos.zw = Rotate(v.vertex.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
				o.worldPos2 = Rotate(v.vertex.xy, _ClipArgs2.zw) * _ClipRange2.zw + _ClipRange2.xy;
				return o;
			}
				
			fixed4 frag (v2f IN) : SV_Target
			{
				fixed2 coord = IN.texcoord;
				fixed4 result = fixed4(0, 0, 0, 0);

				if(_BlurSize == 0 || _SampleDistance <= 0)
				{
					result = tex2D(_MainTex, fixed2(coord.x, coord.y));
				}
				else
				{
					fixed totalWeight = 1;
					result += tex2D(_MainTex, fixed2(coord.x, coord.y));

					// Loop by blur size.
					fixed blendAmount = totalWeight;
					for(int i=0; i<_BlurSize; i++)
					{
						blendAmount = _Decay / (i+1);

						int centerSize = i * 2 + 1;
						totalWeight += blendAmount * (centerSize * 4 + 4);

						fixed topCoord = coord.y - _SampleDistance * (i+1);
						fixed centerBound = _SampleDistance * (i+1);
						fixed botCoord = coord.y + _SampleDistance * (i+1);
						for(int t=0; t<centerSize+2; t++)
							result += tex2D(_MainTex, fixed2(coord.x - _SampleDistance * (t-i-1), topCoord)) * blendAmount;
						for(int c=0; c<centerSize; c++)
						{
							fixed centerCoordY = coord.y - _SampleDistance * (c-i);
							result += tex2D(_MainTex, fixed2(coord.x - centerBound, centerCoordY)) * blendAmount;
							result += tex2D(_MainTex, fixed2(coord.x + centerBound, centerCoordY)) * blendAmount;
						}
						for(int b=0; b<centerSize+2; b++)
							result += tex2D(_MainTex, fixed2(coord.x - _SampleDistance * (b-i-1), botCoord)) * blendAmount;
					}

					result /= totalWeight;
				}

				// First clip region
				fixed2 factor = (fixed2(1.0, 1.0) - abs(IN.worldPos.xy)) * _ClipArgs0.xy;
				fixed f = min(factor.x, factor.y);

				// Second clip region
				factor = (fixed2(1.0, 1.0) - abs(IN.worldPos.zw)) * _ClipArgs1.xy;
				f = min(f, min(factor.x, factor.y));

				// Third clip region
				factor = (fixed2(1.0, 1.0) - abs(IN.worldPos2)) * _ClipArgs2.xy;
				f = min(f, min(factor.x, factor.y));

				// Sample the texture
				result = result * IN.color;
				result.a *= clamp(f, 0.0, 1.0);
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
