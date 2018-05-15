Shader "Custom/LinkVertexAnimation"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)

		_Amplitude("Amplitude", float) = 0.5
		_Period("Period", float) = 0.5
		_Speed("Speed", float) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float4 _Color;
			float _Amplitude;
			float _Period;
			float _Speed;

			float parabola(float x, float k) {
				return pow(4.0*x*(1.0 - x), k);
			}

			float animationRuban(float x, float z, float t, float s, float p, float a)
			{
				return pow(cos(3.14 * (x + z + t * s) * p / 2.0), 2.0) * a;
			}

			v2f vert (appdata v)
			{
				v2f o;
				// Sinus animation
				//v.vertex.y += sin((v.vertex.x + v.vertex.z + _Time.y * _Speed) * _Period) * _Amplitude;

				// Parabola animation
				//v.vertex.y += parabola((v.vertex.x + v.vertex.z + _Time.y * _Speed) * _Period, 1) * _Amplitude;

				// Other animation
				v.vertex.y += pow(cos(3.14 * (v.vertex.x + v.vertex.z + _Time.y * _Speed) * _Period / 2.0), 2.0)  * _Amplitude;
				v.vertex.x += pow(cos(3.14 * (v.vertex.y + v.vertex.z + _Time.y * _Speed) * _Period / 2.0), 2.0)  * _Amplitude;
				v.vertex.z += pow(cos(3.14 * (v.vertex.x + v.vertex.y + _Time.y * _Speed) * _Period / 2.0), 2.0)  * _Amplitude;

				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
