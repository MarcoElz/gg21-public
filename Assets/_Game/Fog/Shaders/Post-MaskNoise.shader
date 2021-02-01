// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fog/Post-MaskNoise"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Mask("Mask", 2D) = "white" {}
		_NightColor("NightColor", Color) = (1,1,1,1)
		_LightColor("LightColor", Color) = (1,1,1,1)
		_FogColor("FogColor", Color) = (1,1,1,1)
		_NoiseScale("NoiseScale", Float) = 8
		_FogStrength("FogStrength", Range( 0 , 1)) = 0.1
		_AngleSpeed("AngleSpeed", Float) = 1
		_WindSpeed("WindSpeed", Vector) = (-0.1,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float4 _LightColor;
			uniform sampler2D _Mask;
			uniform float4 _Mask_ST;
			uniform float4 _NightColor;
			uniform float _NoiseScale;
			uniform float _AngleSpeed;
			uniform float2 _WindSpeed;
			uniform float _FogStrength;
			uniform float4 _FogColor;
					float2 voronoihash11( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi11( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash11( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 uv_Mask = i.uv.xy * _Mask_ST.xy + _Mask_ST.zw;
				float4 tex2DNode2 = tex2D( _Mask, uv_Mask );
				float4 temp_output_8_0 = ( 1.0 - tex2DNode2 );
				float time11 = ( _AngleSpeed * _Time.y );
				float2 texCoord24 = i.uv.xy * float2( 1,1 ) + ( _WindSpeed * _Time.y );
				float2 coords11 = texCoord24 * _NoiseScale;
				float2 id11 = 0;
				float2 uv11 = 0;
				float fade11 = 0.5;
				float voroi11 = 0;
				float rest11 = 0;
				for( int it11 = 0; it11 <3; it11++ ){
				voroi11 += fade11 * voronoi11( coords11, time11, id11, uv11, 0 );
				rest11 += fade11;
				coords11 *= 2;
				fade11 *= 0.5;
				}//Voronoi11
				voroi11 /= rest11;
				float clampResult30 = clamp( ( voroi11 + _FogStrength ) , 0.0 , 1.0 );
				

				finalColor = ( ( tex2D( _MainTex, uv_MainTex ) * ( ( _LightColor * tex2DNode2 ) + ( temp_output_8_0 * _NightColor ) ) ) + ( temp_output_8_0 * clampResult30 * _FogColor ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
0;36;1920;993;2112.689;782.672;1.6;True;False
Node;AmplifyShaderEditor.Vector2Node;25;-1309.438,363.5882;Inherit;False;Property;_WindSpeed;WindSpeed;7;0;Create;True;0;0;0;False;0;False;-0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;21;-1297.438,735.588;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1047.438,571.5881;Inherit;False;Property;_AngleSpeed;AngleSpeed;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1081.438,429.5882;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;27;-1046.438,239.5883;Inherit;False;Constant;_NoiseTiling;NoiseTiling;7;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1051.3,-445.8998;Inherit;True;Property;_Mask;Mask;0;0;Create;True;0;0;0;False;0;False;None;2bad26f129e8da3438962ffbe494e8a0;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-859.4383,358.5882;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-675.4383,678.3882;Inherit;False;Property;_NoiseScale;NoiseScale;4;0;Create;True;0;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-618.5384,546.9883;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-819.2993,-447.8998;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-499.9312,799.3931;Inherit;False;Property;_FogStrength;FogStrength;5;0;Create;True;0;0;0;False;0;False;0.1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;11;-420.7,475.3997;Inherit;True;0;0;1;3;3;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;8.69;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.OneMinusNode;8;-468.1994,-214.5002;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;-867.9001,-204.7002;Inherit;False;Property;_NightColor;NightColor;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.5283019,0.5283019,0.5283019,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-786.987,-645.7015;Inherit;False;Property;_LightColor;LightColor;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.5283019,0.5283019,0.5283019,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-465.887,-493.6019;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-267.7,21.79998;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-237.6383,475.7883;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-450.4005,-824.9993;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-324.4003,-825.9993;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;267.1143,416.0978;Inherit;False;Property;_FogColor;FogColor;3;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.5283019,0.5283019,0.5283019,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-36,-172.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;30;-13.13827,391.3883;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;15,-319.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;118.4128,-12.30176;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;255.213,-189.5018;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;464.4,-358;Float;False;True;-1;2;ASEMaterialInspector;0;2;Fog/Post-MaskNoise;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;26;0;25;0
WireConnection;26;1;21;0
WireConnection;24;0;27;0
WireConnection;24;1;26;0
WireConnection;22;0;20;0
WireConnection;22;1;21;0
WireConnection;2;0;3;0
WireConnection;11;0;24;0
WireConnection;11;1;22;0
WireConnection;11;2;19;0
WireConnection;8;0;2;0
WireConnection;16;0;15;0
WireConnection;16;1;2;0
WireConnection;6;0;8;0
WireConnection;6;1;7;0
WireConnection;28;0;11;0
WireConnection;28;1;31;0
WireConnection;5;0;1;0
WireConnection;10;0;16;0
WireConnection;10;1;6;0
WireConnection;30;0;28;0
WireConnection;4;0;5;0
WireConnection;4;1;10;0
WireConnection;18;0;8;0
WireConnection;18;1;30;0
WireConnection;18;2;14;0
WireConnection;17;0;4;0
WireConnection;17;1;18;0
WireConnection;0;0;17;0
ASEEND*/
//CHKSM=F5034488483A99A8A64872BB2B9F0A8459EA3BFF