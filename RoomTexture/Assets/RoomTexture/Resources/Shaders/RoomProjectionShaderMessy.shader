// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

// AssetDatabase.CreateAsset
// PrefabUtility class
// Texture2DArray can be serialized - look into how it needs to be written to make it serializable

Shader "RoomProjectionShader" {
	//Fallback Off // ["Shader's Name", Off] - Lists a shader to fall back to and use if none of the passes work in this subshader
	//
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_ProjectionTexture("Texture", 2D) = "" {}
		// _ShadowTex("Cookie", 2D) = "" {}
		// _FalloffTex("FallOff", 2D) = "" {}
		_SliceRange("Slices", Range(0,1)) = 0 // must be dynamically set through script
			// check material.setfloat
			// rend.material.setfloat("_SliceRange", range); (in script)
		_FalloffTex("Falloff Texture", 2D) = "" {}
	}

	// Category{}

	Subshader{
		//Tags{ "Queue" = "Transparent" }
		Pass{
//			Material{
				// Diffuse [_Color]
//				Diffuse (1,1,1,1)
				// Ambient [_Color]
//				Ambient (1,1,1,1)
				// Shininess [_Shininess]
				// Specular [_SpecColor]
				// Emission [_Emission]
//			}
			Lighting On
			
			// Old settings
			// ZWrite Off
			ColorMask RGB
			//Blend DstColor One // DstColor as the SrcFactor means that the value of the stage is multiplied by the frame buffer source color value
			// Offset - 1, -1

			// New settings
			Cull Back // [Back, Front, Off] - Doesn't render textures on the backs of polygons
			ZWrite On // [On, Off] - Determine if z buffering should be done; occludes objects behind others
			ZTest LEqual // [Less, Greater, LEqual, GEqual, Equal, NotEqual, Always] - How should depth testing be performed
			Offset -1, -1 // [Factor, Units]
				// Allows you to specify a depth offset with factor and units.
				// Factor scales max z slope
				// Units scale min resolvable depth buffer value
			// Offset value allows the polygon to be pushed back a tiny bit to properly render the texture in its place
			//Lighting On //
			//BlendOp Max
			Blend One DstColor

			// Lighting

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
			// ERROR TESTING REMOVE? originally in
// #pragma multi_compile_fog
//
// to use texture arrays we need to target DX10/OpenGLES3 which
// is shader model 3.5 minimum
//
// #pragma target 3.5
#include "UnityCG.cginc"

			struct v2f {
				//float4 uvShadow : TEXCOORD0;
				float4 uv : TEXCOORD0;
				// float4 uvFalloff : TEXCOORD1;
				//UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
			};

			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			float uMax;
			float vMax;

			v2f vert(float4 vertex : POSITION)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, vertex);
				o.uv = mul(unity_Projector, vertex);
				// set the z setting for uv
				// the z setting for uv when using a 2D texture array is the index
				// o.uv.xy = (vertex.xy + 0.5) * _UVScale; // ERROR TESTING remove?
				// o.z = (vertex.z + 0.5) * _SliceRange;

				return o;
			}

			// actually pulling in the 2d texture array
			// UNITY_DECLARE_TEX2DARRAY(_MyArr);

			fixed4 _Color;
			sampler2D _ProjectionTexture;
			//sampler2D _ShadowTex;
			//sampler2D _FalloffTex;

			fixed4 frag(v2f i) : SV_Target {
				// sample the texture 2d array
				// return UNITY_SAMPLE_TEX2DARRAY(_MyArr, i.uv);

				fixed4 tex = tex2Dproj (_ProjectionTexture, UNITY_PROJ_COORD(i.uv));
				tex.rgb *= _Color.rgb;
				// tex.a = 1.0 - tex.a; // ERROR TESTING REMOVE?

				// fixed4 texF = tex2Dproj(_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
				//fixed4 res = tex * texF.a;

				// UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(0,0,0,0));

				fixed4 res = tex;

				// do not render the pixel if it lies outside of the frustrum plane
				clip(i.uv.x < 0.0f ? -1 : 1);
				clip(i.uv.x > 10.0f ? -1 : 1);
				clip(i.uv.y < 0.0f ? -1 : 1);
				clip(i.uv.y > 10.0f ? -1 : 1);
				
				return res;
			}


			// SetTexture MUST take place at the end of a Pass
			//SetTexture[_MainTex]{

			// Combine texture * primary DOUBLE, texture * primary // rgb, alpha
			// combine src1 lerp (src2) src3 // interpolates between src3 and src1 using the alpha of src2
			// src = Previous (last settexture result); Primary (color of vertex color or lighting calcs); Texture (TextureName); Constant (ConstantColor);
			// Double; Quad; - make resulting color 2x or 4x as bright
			// one - src (make resulting color negated)
			// src alpha (take only alpha channel)

			//}

			ENDCG
		}
	}
}

/*









Shader "Custom/RoomProjectionShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
*/