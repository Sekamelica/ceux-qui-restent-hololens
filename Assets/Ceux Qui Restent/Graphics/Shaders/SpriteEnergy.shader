// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/SpriteEnergy"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_AnimTex("Animation Texture", 2D) = "white" {}
		_AnimColor("Animation Color", Color) = (0.2, 0.2, 1, 1)
		_HorizontalSpeed("Horizontal Speed", Float) = 1.0
		_VerticalSpeed("Vertical Speed", Float) = 1.0
		_VerticalMultiplicator("Vertical Multiplicator", Float) = 1.0
		_VerticalPeriod("Vertical Period", Float) = 1.0
		_AlphaAnimationPeriod("Alpha Animation Period", Float) = 1.0
		_AlphaAnimationSpeed("Alpha Animation Speed", Float) = 1.0
		_AnimTex2("Animation Texture", 2D) = "white" {}
		_AnimColor2("Animation Color", Color) = (0.2, 0.2, 1, 1)
		_HorizontalSpeed2("Horizontal Speed", Float) = 1.0
		_VerticalSpeed2("Vertical Speed", Float) = 1.0
		_VerticalMultiplicator2("Vertical Multiplicator", Float) = 1.0
		_VerticalPeriod2("Vertical Period", Float) = 1.0
		_AlphaAnimationPeriod2("Alpha Animation Period", Float) = 1.0
		_AlphaAnimationSpeed2("Alpha Animation Speed", Float) = 1.0
		_OffsetX("Offset X", Float) = 1.0
		_OffsetY("Offset Y", Float) = 1.0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON
#include "UnityCG.cginc"

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	fixed4 _Color;

	// Anim layer 1
	fixed4 _AnimColor;
	sampler2D _AnimTex;
	half2 animTexcoord;
	float _HorizontalSpeed;
	float _VerticalSpeed;
	float _VerticalMultiplicator;
	float _VerticalPeriod;
	float _AlphaAnimationPeriod;
	float _AlphaAnimationSpeed;

	// Anim layer 2
	fixed4 _AnimColor2;
	sampler2D _AnimTex2;
	half2 animTexcoord2;
	float _HorizontalSpeed2;
	float _VerticalSpeed2;
	float _VerticalMultiplicator2;
	float _VerticalPeriod2;
	float _AlphaAnimationPeriod2;
	float _AlphaAnimationSpeed2;
	float _OffsetX;
	float _OffsetY;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;

		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;

	fixed4 blendColorMask(fixed4 color1, fixed4 color2, fixed4 texColor)
	{
		fixed4 color;
		float blendValue = 1 - (texColor.r * color2.a);
		color.rgb = (blendValue * color1.rgb + (texColor.r * color2.a * color2.rgb)) * color1.a;
		color.a = color1.a;
		return color;
	}

	fixed4 blendColor(fixed4 color1, fixed4 color2, fixed4 texColor)
	{
		fixed4 color;
		if (color1.a > 0)
		{
			float blendValue = 1 - (texColor.r * color2.a);
			color.rgb = (blendValue * color1.rgb + (texColor.r * color2.a * color2.rgb)) * color1.a;
			color.a = color1.a;
		}
		else
		{
			color.rgb = (texColor.r * color2.a * color2.rgb);
			color.a = (texColor.r * color2.a);
		}
		return color;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		// Color Background
		half2 baseTexcoord = IN.texcoord;
		fixed4 cBg = tex2D(_MainTex, baseTexcoord) * IN.color;
		
		// Anim 1
		animTexcoord = IN.texcoord;
		//animTexcoord.y = animTexcoord.y * 2 + 0.5;
		animTexcoord.x += _Time.y * _HorizontalSpeed;
		animTexcoord.y += _VerticalMultiplicator * sin((_VerticalSpeed * _Time.y) + (_VerticalPeriod * animTexcoord.x));

		// Color Anim 1
		fixed4 cAnim1 = tex2D(_AnimTex, animTexcoord);
		cAnim1.r *= saturate(1 + sin(_AlphaAnimationPeriod * animTexcoord + _Time.y * _AlphaAnimationSpeed));

		// Blend anim 1
		fixed4 c = blendColorMask		(cBg, _AnimColor, cAnim1);
		//float blend = 1 - (cAnim1.r * _AnimColor.a);
		//c.rgb = (blend * cBg.rgb + (cAnim1.r * _AnimColor.a * _AnimColor.rgb)) * cBg.a;
		//c.a = cBg.a;

		// Anim 2
		animTexcoord2 = IN.texcoord;
		animTexcoord2.x += _OffsetX + _Time.y * _HorizontalSpeed2;
		animTexcoord2.y += _OffsetY + _VerticalMultiplicator2 * sin((_VerticalSpeed2 * _Time.y) + (_VerticalPeriod2 * animTexcoord2.x));

		// Color Anim 2
		fixed4 cAnim2 = tex2D(_AnimTex2, animTexcoord2);
		cAnim2.r *= saturate(1 + sin(_AlphaAnimationPeriod2 * animTexcoord + _Time.y * _AlphaAnimationSpeed2));

		// Blend anim 2
		c = blendColorMask(c, _AnimColor2, cAnim2);
		//blend = 1 - (cAnim2.r * _AnimColor2.a);
		//c.rgb = (blend * c.rgb + (cAnim2.r * _AnimColor2.a * _AnimColor2.rgb)) * c.a;
		//c.a = c.a;

		return c;
	}
		ENDCG
	}
	}
}