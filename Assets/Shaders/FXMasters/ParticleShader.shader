	// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ParticleShader"
{
	Properties
	{
		_MainText("MainText", 2D) = "white" {}
		_PannerSpeed("PannerSpeed", Vector) = (0,0,0,0)
		_TimeScale("TimeScale", Float) = 1
		_CutOffTexture("CutOffTexture", 2D) = "white" {}
		_SecondCutOffTexture("SecondCutOffTexture", 2D) = "white" {}
		_SeconCutOffMin("SeconCutOffMin", Range( 0 , 1)) = 0
		_FirstCutOffMin("FirstCutOffMin", Range( 0 , 1)) = 0.35
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
	LOD 100

		Cull Off
		CGINCLUDE
		#pragma target 3.0 
		ENDCG
		
		
		Pass
		{
			
			Name "ForwardBase"
			Tags { "LightMode"="ForwardBase" }

			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend Off
			AlphaToMask On
			Cull Off
			ColorMask RGBA
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

			uniform sampler2D _MainText;
			uniform float _TimeScale;
			uniform float3 _PannerSpeed;
			uniform float _FirstCutOffMin;
			uniform sampler2D _CutOffTexture;
			uniform float4 _CutOffTexture_ST;
			uniform float _SeconCutOffMin;
			uniform sampler2D _SecondCutOffTexture;
			uniform float4 _SecondCutOffTexture_ST;


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				
				o.ase_color = v.ase_color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				#if ASE_SHADOWS
					#if UNITY_VERSION >= 560
						UNITY_TRANSFER_SHADOW( o, v.texcoord );
					#else
						TRANSFER_SHADOW( o );
					#endif
				#endif
				return o;
			}
			
			float4 frag (v2f i ) : SV_Target
			{
				float3 outColor;
				float outAlpha;

				float mulTime20 = _Time.y * _TimeScale;
				float2 texCoord14 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner13 = ( mulTime20 * _PannerSpeed.xy + texCoord14);
				float4 tex2DNode11 = tex2D( _MainText, panner13 );
				
				float4 temp_cast_2 = (tex2DNode11.a).xxxx;
				float4 temp_cast_3 = (_FirstCutOffMin).xxxx;
				float2 uv_CutOffTexture = i.ase_texcoord1.xy * _CutOffTexture_ST.xy + _CutOffTexture_ST.zw;
				float4 smoothstepResult26 = smoothstep( temp_cast_3 , float4( 1,1,1,0 ) , tex2D( _CutOffTexture, uv_CutOffTexture ));
				float4 ifLocalVar39 = 0;
				if( _FirstCutOffMin > 0.0 )
				ifLocalVar39 = ( 1.0 - smoothstepResult26 );
				float4 temp_cast_4 = (_SeconCutOffMin).xxxx;
				float2 uv_SecondCutOffTexture = i.ase_texcoord1.xy * _SecondCutOffTexture_ST.xy + _SecondCutOffTexture_ST.zw;
				float4 smoothstepResult32 = smoothstep( temp_cast_4 , float4( 0.5377358,0.5377358,0.5377358,0 ) , tex2D( _SecondCutOffTexture, uv_SecondCutOffTexture ));
				float4 ifLocalVar38 = 0;
				if( _SeconCutOffMin > 0.0 )
				ifLocalVar38 = ( 1.0 - smoothstepResult32 );
				
				
				outColor = ( i.ase_color * tex2DNode11 ).rgb;
				outAlpha = ( ( temp_cast_2 - ifLocalVar39 ) - ifLocalVar38 ).r;
				clip(outAlpha);
				return float4(outColor,outAlpha);
			}
			ENDCG
		}
		
		
		Pass
		{
			Name "ForwardAdd"
			Tags { "LightMode"="ForwardAdd" }
			ZWrite Off
			Blend One One
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd_fullshadows
			#define UNITY_PASS_FORWARDADD
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

			uniform sampler2D _MainText;
			uniform float _TimeScale;
			uniform float3 _PannerSpeed;
			uniform float _FirstCutOffMin;
			uniform sampler2D _CutOffTexture;
			uniform float4 _CutOffTexture_ST;
			uniform float _SeconCutOffMin;
			uniform sampler2D _SecondCutOffTexture;
			uniform float4 _SecondCutOffTexture_ST;


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				
				o.ase_color = v.ase_color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				#if ASE_SHADOWS
					#if UNITY_VERSION >= 560
						UNITY_TRANSFER_SHADOW( o, v.texcoord );
					#else
						TRANSFER_SHADOW( o );
					#endif
				#endif
				return o;
			}
			
			float4 frag (v2f i ) : SV_Target
			{
				float3 outColor;
				float outAlpha;

				float mulTime20 = _Time.y * _TimeScale;
				float2 texCoord14 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner13 = ( mulTime20 * _PannerSpeed.xy + texCoord14);
				float4 tex2DNode11 = tex2D( _MainText, panner13 );
				
				float4 temp_cast_2 = (tex2DNode11.a).xxxx;
				float4 temp_cast_3 = (_FirstCutOffMin).xxxx;
				float2 uv_CutOffTexture = i.ase_texcoord1.xy * _CutOffTexture_ST.xy + _CutOffTexture_ST.zw;
				float4 smoothstepResult26 = smoothstep( temp_cast_3 , float4( 1,1,1,0 ) , tex2D( _CutOffTexture, uv_CutOffTexture ));
				float4 ifLocalVar39 = 0;
				if( _FirstCutOffMin > 0.0 )
				ifLocalVar39 = ( 1.0 - smoothstepResult26 );
				float4 temp_cast_4 = (_SeconCutOffMin).xxxx;
				float2 uv_SecondCutOffTexture = i.ase_texcoord1.xy * _SecondCutOffTexture_ST.xy + _SecondCutOffTexture_ST.zw;
				float4 smoothstepResult32 = smoothstep( temp_cast_4 , float4( 0.5377358,0.5377358,0.5377358,0 ) , tex2D( _SecondCutOffTexture, uv_SecondCutOffTexture ));
				float4 ifLocalVar38 = 0;
				if( _SeconCutOffMin > 0.0 )
				ifLocalVar38 = ( 1.0 - smoothstepResult32 );
				
				
				outColor = ( i.ase_color * tex2DNode11 ).rgb;
				outAlpha = ( ( temp_cast_2 - ifLocalVar39 ) - ifLocalVar38 ).r;
				clip(outAlpha);
				return float4(outColor,outAlpha);
			}
			ENDCG
		}

		
		Pass
		{
			Name "Deferred"
			Tags { "LightMode"="Deferred" }

			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend Off
			AlphaToMask Off
			Cull Back
			ColorMask RGBA
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_prepassfinal
			#define UNITY_PASS_DEFERRED
			#include "UnityCG.cginc"
			
			

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				
				
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				#if ASE_SHADOWS
					#if UNITY_VERSION >= 560
						UNITY_TRANSFER_SHADOW( o, v.texcoord );
					#else
						TRANSFER_SHADOW( o );
					#endif
				#endif
				return o;
			}
			
			void frag (v2f i , out half4 outGBuffer0 : SV_Target0, out half4 outGBuffer1 : SV_Target1, out half4 outGBuffer2 : SV_Target2, out half4 outGBuffer3 : SV_Target3)
			{
				
				
				outGBuffer0 = 0;
				outGBuffer1 = 0;
				outGBuffer2 = 0;
				outGBuffer3 = 0;
			}
			ENDCG
		}
		
		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }
			ZWrite On
			ZTest LEqual
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

			uniform sampler2D _MainText;
			uniform float _TimeScale;
			uniform float3 _PannerSpeed;
			uniform float _FirstCutOffMin;
			uniform sampler2D _CutOffTexture;
			uniform float4 _CutOffTexture_ST;
			uniform float _SeconCutOffMin;
			uniform sampler2D _SecondCutOffTexture;
			uniform float4 _SecondCutOffTexture_ST;


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				V2F_SHADOW_CASTER;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				
				o.ase_color = v.ase_color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}
			
			float4 frag (v2f i ) : SV_Target
			{
				float3 outColor;
				float outAlpha;

				float mulTime20 = _Time.y * _TimeScale;
				float2 texCoord14 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner13 = ( mulTime20 * _PannerSpeed.xy + texCoord14);
				float4 tex2DNode11 = tex2D( _MainText, panner13 );
				
				float4 temp_cast_2 = (tex2DNode11.a).xxxx;
				float4 temp_cast_3 = (_FirstCutOffMin).xxxx;
				float2 uv_CutOffTexture = i.ase_texcoord1.xy * _CutOffTexture_ST.xy + _CutOffTexture_ST.zw;
				float4 smoothstepResult26 = smoothstep( temp_cast_3 , float4( 1,1,1,0 ) , tex2D( _CutOffTexture, uv_CutOffTexture ));
				float4 ifLocalVar39 = 0;
				if( _FirstCutOffMin > 0.0 )
				ifLocalVar39 = ( 1.0 - smoothstepResult26 );
				float4 temp_cast_4 = (_SeconCutOffMin).xxxx;
				float2 uv_SecondCutOffTexture = i.ase_texcoord1.xy * _SecondCutOffTexture_ST.xy + _SecondCutOffTexture_ST.zw;
				float4 smoothstepResult32 = smoothstep( temp_cast_4 , float4( 0.5377358,0.5377358,0.5377358,0 ) , tex2D( _SecondCutOffTexture, uv_SecondCutOffTexture ));
				float4 ifLocalVar38 = 0;
				if( _SeconCutOffMin > 0.0 )
				ifLocalVar38 = ( 1.0 - smoothstepResult32 );
				
				
				outColor = ( i.ase_color * tex2DNode11 ).rgb;
				outAlpha = ( ( temp_cast_2 - ifLocalVar39 ) - ifLocalVar38 ).r;
				clip(outAlpha);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
		
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;73;1077;450;1765.586;284.0885;1.6;True;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-1287.509,506.5056;Inherit;False;Property;_FirstCutOffMin;FirstCutOffMin;6;0;Create;True;0;0;0;False;0;False;0.35;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1286.703,304.8193;Inherit;True;Property;_CutOffTexture;CutOffTexture;3;0;Create;True;0;0;0;False;0;False;-1;None;394ff4dedc00a9d4bae0a9158ffd2654;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-1397.123,158.9043;Inherit;False;Property;_TimeScale;TimeScale;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-895.6795,800.9066;Inherit;False;Property;_SeconCutOffMin;SeconCutOffMin;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-1017.434,595.8144;Inherit;True;Property;_SecondCutOffTexture;SecondCutOffTexture;4;0;Create;True;0;0;0;False;0;False;-1;None;b9489ada51ea2274d86abf73bfed25ef;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;26;-930.9404,250.6553;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.3018868,0.3018868,0.3018868,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1219.304,-121.9438;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;16;-1210.918,-0.2188654;Inherit;False;Property;_PannerSpeed;PannerSpeed;1;0;Create;True;0;0;0;False;0;False;0,0,0;-1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1182.139,167.3684;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-843.5818,-67.93089;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;32;-598.1729,548.9357;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.4245283,0.4245283,0.4245283,0;False;2;COLOR;0.5377358,0.5377358,0.5377358,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;36;-647.8591,246.1254;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;37;-315.464,545.7879;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;-540.7183,-99.20908;Inherit;True;Property;_MainText;MainText;0;0;Create;True;0;0;0;False;0;False;-1;None;22b3437dd4c85614e9ba14dd99e5da00;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;39;-340.3228,237.2844;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-83.08476,52.16315;Inherit;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;38;-70.57211,497.5027;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;18;-650.239,-281.8383;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-215.1891,-170.1132;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;33;171.4396,223.4274;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;23;570.5379,551.3042;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;10;63.7,-19.5;Float;False;False;-1;2;ASEMaterialInspector;100;8;New Amplify Shader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;ShadowCaster;0;3;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;8;754.7717,53.66196;Float;False;False;-1;2;ASEMaterialInspector;100;8;New Amplify Shader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;ForwardAdd;0;1;ForwardAdd;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;False;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;True;1;LightMode=ForwardAdd;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;9;781.9057,551.3887;Float;False;False;-1;2;ASEMaterialInspector;100;8;New Amplify Shader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;Deferred;0;2;Deferred;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Deferred;True;2;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;7;754.7717,-56.33804;Float;False;True;-1;2;ASEMaterialInspector;100;8;ParticleShader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;ForwardBase;0;0;ForwardBase;3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;True;2;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=ForwardBase;True;2;0;;0;0;Standard;0;0;4;True;True;True;True;False;;False;0
WireConnection;26;0;24;0
WireConnection;26;1;35;0
WireConnection;20;0;21;0
WireConnection;13;0;14;0
WireConnection;13;2;16;0
WireConnection;13;1;20;0
WireConnection;32;0;30;0
WireConnection;32;1;34;0
WireConnection;36;0;26;0
WireConnection;37;0;32;0
WireConnection;11;1;13;0
WireConnection;39;0;35;0
WireConnection;39;2;36;0
WireConnection;27;0;11;4
WireConnection;27;1;39;0
WireConnection;38;0;34;0
WireConnection;38;2;37;0
WireConnection;17;0;18;0
WireConnection;17;1;11;0
WireConnection;33;0;27;0
WireConnection;33;1;38;0
WireConnection;7;0;17;0
WireConnection;7;1;33;0
ASEEND*/
//CHKSM=4E920EF8F3C37B93B6FDE46E047524C1A0EF50E5