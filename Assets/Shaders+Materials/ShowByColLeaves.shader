// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/ShowByColLeaves" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        [PowerSlider(5.0)] _Shininess("Shininess", Range(0.01, 1)) = 0.078125
        _MainTex("Base (RGB) Alpha (A)", 2D) = "white" {}
        _BumpMap("Normalmap", 2D) = "bump" {}
        _GlossMap("Gloss (A)", 2D) = "black" {}
        _TranslucencyMap("Translucency (A)", 2D) = "white" {}
        _ShadowOffset("Shadow Offset (A)", 2D) = "black" {}

        // These are here only to provide default values
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.3
        [HideInInspector] _TreeInstanceColor("TreeInstanceColor", Vector) = (1,1,1,1)
        [HideInInspector] _TreeInstanceScale("TreeInstanceScale", Vector) = (1,1,1,1)
        [HideInInspector] _SquashAmount("Squash", Float) = 1

        // _UpperBlindTh ("Upper Blind Threshold", Range(0,20)) = 5.0        
        // _LowerBlindTh ("Lower Blind Threshold", Range(0,20)) = 5.0
        _Colliding ("Colliding", Range(0,1)) = 0
        _ColPos ("Collision Position", Vector) = (0,0,0,0)
        _ColAreaRadius ("Collision Area Radius", Range(0,5)) = 1
        _Sounding ("Sounding", Range(0,1)) = 0        
        _OverrideAlpha ("OverrideAlpha", Range(-1,1)) = 1
    }

        SubShader{
            Tags { "IgnoreProjector" = "True" "RenderType" = "TreeLeaf" }
            LOD 200

        CGPROGRAM
        #pragma surface surf TreeLeaf alphatest:_Cutoff vertex:TreeVertLeaf addshadow nolightmap noforwardadd
        #include "UnityBuiltin3xTreeLibrary.cginc"

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _GlossMap;
        sampler2D _TranslucencyMap;
        half _Shininess;

        // fixed _LowerBlindTh;
        // fixed _UpperBlindTh;
        fixed _Colliding;
        fixed4 _ColPos;
        fixed _ColAreaRadius;
        fixed _OverrideAlpha;
        fixed _Sounding;

        fixed4 _ColPoints[10];

        struct Input {
            float2 uv_MainTex;
            fixed4 color : COLOR; // color.a = AO
            float3 worldPos;
        };

        void surf(Input IN, inout LeafSurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * IN.color.rgb * IN.color.a;
            o.Translucency = tex2D(_TranslucencyMap, IN.uv_MainTex).rgb;
            o.Gloss = tex2D(_GlossMap, IN.uv_MainTex).a;


            //fixed dstToHitPoint = (_Colliding != 0)? distance(_ColPos, IN.worldPos) : 0;
            //o.Alpha = (_Colliding == 0)? 0 : (dstToHitPoint < _ColAreaRadius)? (_Colliding * c.a) : 0;
            //o.Alpha = c.a * _Colliding;

            fixed dstToHitPoint = 0;
            bool closeToColPos = false; 

            //The loop is aborted when there's no collision or the current point is within reach of a colPoint.
            for(int i = 0; (_Colliding != 0) && (i < 15) && !closeToColPos; i++) {

                //1st check avoid calculating distance to null positions
                dstToHitPoint = (_ColPoints[i].xyz == float3(0,0,0))? 100 : distance(_ColPoints[i], IN.worldPos);
                closeToColPos = (dstToHitPoint < _ColAreaRadius);
            }
            o.Alpha = (_OverrideAlpha >= 0)? _OverrideAlpha * c.a : 
            (closeToColPos? (max(_Colliding, _Sounding) * c.a) : (_Sounding * c.a));


            // //The loop is aborted when there's no collision or the current point is within reach of a colPoint.
            // for(int i = 0; (_Colliding != 0) && (i < 10) && !closeToColPos; i++) {
            //     //1st check avoid calculating distance to null positions
            //     dstToHitPoint = (_ColPoints[i].xyz == float3(0,0,0))? 100 : distance(_ColPoints[i], IN.worldPos);
            //     closeToColPos = (dstToHitPoint < _ColAreaRadius);
            // }
            // o.Alpha = closeToColPos? (_Colliding * c.a) : 0;


            o.Specular = _Shininess;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
        }
        ENDCG
        }

            Dependency "OptimizedShader" = "Hidden/Nature/Tree Creator Leaves Optimized"
            FallBack "Diffuse"
}
