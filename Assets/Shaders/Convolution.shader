Shader "Custom/Convolution" {
    Properties{
        [KeywordEnum(Nothing, BoxBlurSmall, BoxBlurLarge, GaussBlur, Sharpen)] _Kernel("Kernel", Float) = 0
        _Size("Size", Range(1,100)) = 1
        _MainTex("Masking Texture", 2D) = "white" {}
        _AdditiveColor("Additive Tint color", Color) = (0, 0, 0, 0)
        _MultiplyColor("Multiply Tint color", Color) = (1, 1, 1, 1)
    }

    Category{

        // We must be transparent, so other objects are drawn before this one.
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }


        SubShader
        {
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f {
                    float4 vertex : POSITION;
                    float4 uvgrab : TEXCOORD0;
                    float2 uvmain : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif

                    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
                    o.uvgrab.zw = o.vertex.zw;

                    o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }

                int _Kernel;
                float _Size;
                sampler2D _CameraOpaqueTexture;
                float4 _CameraOpaqueTexture_TexelSize;
                float4 _AdditiveColor;
                float4 _MultiplyColor;

                half4 frag(v2f i) : COLOR
                {
                    const int KERNEL_SIZE = 37;
                    float mat[KERNEL_SIZE][KERNEL_SIZE];
                    #define SETMAT(MAT, normalize) {\
                        ylen = (int)(MAT).Length;\
                        xlen = (int)(MAT)[0].Length;\
                        float abs = normalize ? 0 : 1;\
                        if (normalize)\
                            for (int y = 0; y < ylen; y++)\
                                for (int x = 0; x < xlen; x++)\
                                    abs += (MAT)[y][x];\
                        for (int y = 0; y < ylen; y++)\
                            for (int x = 0; x < xlen; x++)\
                                mat[y][x] = (MAT)[y][x] / abs;\
                    }
                    
                    int ylen;
                    int xlen;
                    if (_Kernel == 1) { // BoxBlurSmall
                        float MAT[5][5] = {
                            {1,1,1,1,1},
                            {1,1,1,1,1},
                            {1,1,1,1,1},
                            {1,1,1,1,1},
                            {1,1,1,1,1}
                        };
                        SETMAT(MAT, true)
                    }
                    else if (_Kernel == 2) { // BoxBlurLarge
                        float MAT[21][21] = {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
                        };
                        SETMAT(MAT, true)
                    }
                    else if (_Kernel == 3) { // GaussBlur
                        float MAT[5][5] = {
                            {1, 4, 7, 4, 1},
                            {4, 16,26,16,4},
                            {7, 26,41,26,7},
                            {4, 16,26,16,4},
                            {1, 4, 7, 4, 1}
                        };
                        SETMAT(MAT, true)
                    }
                    else if (_Kernel == 4) { // Sharpen
                        float MAT[3][3] = {
                            {0,-1,0},
                            {-1,5,-1},
                            {0,-1,0}
                        };
                        SETMAT(MAT, false)
                    }
                    else { // Nothing
                        float MAT[1][1] = { {1} };
                        SETMAT(MAT, false)
                    }
                    int yhalf = ylen / 2;
                    int xhalf = xlen / 2;

                    half4 sum = half4(0,0,0,0);

                    #define GRABPIXEL(kernelx,kernely) tex2Dproj( _CameraOpaqueTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _CameraOpaqueTexture_TexelSize.x * (kernelx) * _Size, i.uvgrab.y + _CameraOpaqueTexture_TexelSize.y * (kernely) * _Size, i.uvgrab.z, i.uvgrab.w)))

                    for (int y = 0; y < ylen; y++)
                        for (int x = 0; x < xlen; x++)
                            sum += mat[y][x] * GRABPIXEL(x - xhalf, y - yhalf);

                    half4 result = half4(sum.r * _MultiplyColor.r + _AdditiveColor.r,
                                        sum.g * _MultiplyColor.g + _AdditiveColor.g,
                                        sum.b * _MultiplyColor.b + _AdditiveColor.b,
                                        tex2D(_MainTex, i.uvmain).a);
                    return result;
                }
                ENDCG
            }
        }
    }
}