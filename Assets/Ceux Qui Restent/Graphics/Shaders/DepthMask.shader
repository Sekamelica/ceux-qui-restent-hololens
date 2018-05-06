// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Mask/DepthMask"
{
	SubShader
	{
		Tags { "Queue" = "Geometry-1" }
		ColorMask 0
		ZWrite On
		Pass {}
	}
}
