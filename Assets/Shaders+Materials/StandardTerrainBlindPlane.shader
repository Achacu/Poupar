Shader "Custom/StandardTerrainBlindPlane"{

    Properties{
        // used in fallback on old cards & base map
        [HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _Color("Main Color", Color) = (1,1,1,1)
        [HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}

        _UpperBlindTh ("Upper Blind Threshold", Range(0,20)) = 5.0        
        _LowerBlindTh ("Lower Blind Threshold", Range(0,20)) = 5.0
        _Visible ("Visible", Range(-1,1)) = 1
    }

        SubShader{
            Tags {
                "Queue"="Transparent" "Rendering"="Transparent" "IgnoreProjector" = "True"
                // "Queue" = "Geometry-100"
                // "RenderType" = "Opaque"
                "TerrainCompatible" = "True"
            }

            CGPROGRAM
            #pragma surface surf Standard vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer addshadow fullforwardshadows alpha:fade
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
            #pragma multi_compile_fog // needed because finalcolor oppresses fog code generation.
            #pragma target 3.0
            #include "UnityPBSLighting.cginc"

            #pragma multi_compile_local_fragment __ _ALPHATEST_ON
            #pragma multi_compile_local __ _NORMALMAP

            #define TERRAIN_STANDARD_SHADER
            #define TERRAIN_INSTANCED_PERPIXEL_NORMAL
            #define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard
            #include "TerrainSplatmapCommonBlind.cginc"

            half _Metallic0;
            half _Metallic1;
            half _Metallic2;
            half _Metallic3;

            half _Smoothness0;
            half _Smoothness1;
            half _Smoothness2;
            half _Smoothness3;

            fixed _LowerBlindTh;
            fixed _UpperBlindTh;
            fixed _Visible;

            void surf(Input IN, inout SurfaceOutputStandard o) {
                half4 splat_control;
                half weight;
                fixed4 mixedDiffuse;
                half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);
                SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal);
                o.Albedo = mixedDiffuse.rgb;

                float dstToCam = length(IN.worldPos.xyz - _WorldSpaceCameraPos.xyz);
                o.Alpha = weight * ((_Visible >= 0)? _Visible : (dstToCam > _UpperBlindTh)? 0 : (dstToCam < _LowerBlindTh)? 1
            : (_UpperBlindTh - dstToCam)/(_UpperBlindTh - _LowerBlindTh));
            
                o.Smoothness = mixedDiffuse.a;
                o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
            }
            ENDCG

            UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
            UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
    }

        Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
                Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"
                Dependency "BaseMapGenShader" = "Hidden/TerrainEngine/Splatmap/Standard-BaseGen"

                Fallback "Nature/Terrain/Diffuse"
}
