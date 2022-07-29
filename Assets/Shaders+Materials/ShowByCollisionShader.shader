Shader "Custom/ShowByCollisionShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Normalmap", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _UpperBlindTh ("Upper Blind Threshold", Range(0,20)) = 5.0        
        _LowerBlindTh ("Lower Blind Threshold", Range(0,20)) = 5.0
        _Colliding ("Colliding", Range(0,1)) = 0
        _ColPos ("Collision Position", Vector) = (0,0,0,0)
        _ColAreaRadius ("Collision Area Radius", Range(0,5)) = 1        
    }
    SubShader
    {
	Tags { "Queue"="Transparent" "Rendering"="Transparent" "IgnoreProjector" = "True"}

       Pass {
            ZWrite On
            ColorMask 0
        }
	ZWrite Off
    //Cull off
	//Blend SrcAlpha OneMinusSrcAlpha
    //Blend One One
    CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma vertex vert
        #pragma surface surf Standard fullforwardshadows alpha:fade 
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicGlossMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MetallicGlossMap;
            float3 posWorld;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed _LowerBlindTh;
        fixed _UpperBlindTh;

        fixed _Colliding;
        fixed4 _ColPos;
        fixed _ColAreaRadius;

        void vert (inout appdata_full v, out Input o) {
          //v.vertex.xyz += v.normal;
          UNITY_INITIALIZE_OUTPUT(Input,o);
          o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        }
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            
            fixed dstToHitPoint = (_Colliding != 0)? distance(_ColPos, IN.posWorld) : 0;
            o.Alpha = (_Colliding == 0)? 0 : (dstToHitPoint < _ColAreaRadius)? (_Colliding * c.a) : 0;
            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = tex2D (_MetallicGlossMap, IN.uv_MetallicGlossMap) * _Metallic;
            o.Smoothness = _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            //o.Alpha = IN.color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
