// 这是一段Unity Shader代码，用于为精灵添加轮廓效果。

// Shader的名称和类型
Shader "Sprites/Outline" {
    // 属性定义块，这里定义了Shader需要的属性
    Properties {
        // _MainTex是精灵的纹理，类型为2D
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        // _Color是精灵的着色，使用RGBA颜色值
        _Color ("Tint", Color) = (1,1,1,1)
        // PixelSnap用于像素对齐，是一个开关
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

        // 轮廓效果的开关和颜色
        [PerRendererData] _Outline ("Outline", Float) = 0
        [PerRendererData] _OutlineColor("Outline Color", Color) = (1,1,1,1)

        //这是我自己定义的采样偏移量属性，用于调整轮廓效果的锐利度(方便在C#脚本外部控制并更改采样偏移量)
        [PerRendererData]  _SampleOffset("Sample Offset",Float) = 1
    }

    // SubShader定义了一组渲染设置和Pass
    SubShader {
        // Tags定义了渲染队列、是否忽略投影等
        Tags {
            "Queue"="Transparent" // 渲染队列设置为透明
            "IgnoreProjector"="True" // 忽略投影
            "RenderType"="Transparent" // 渲染类型设置为透明
            "PreviewType"="Plane" // 预览类型
            "CanUseSpriteAtlas"="True" // 可以使用精灵图集
        }

        // 渲染状态设置
        Cull Off // 不剔除面
        Lighting Off // 关闭光照效果
        ZWrite Off // 关闭深度写入
        Blend One OneMinusSrcAlpha // 设置混合模式，用于透明物体

        // Pass定义了一个渲染通道
        // Properties是属性定义，但要在Pass中再次声明，否则无法在Pass中使用
        Pass {
            // CGPROGRAM和ENDCG包裹了HLSL代码
            CGPROGRAM
            // 定义顶点和片段着色器的入口点
            #pragma vertex vert
            #pragma fragment frag
            // 多编译指令，用于像素对齐
            #pragma multi_compile _ PIXELSNAP_ON
            // 特性开关，用于ETC1格式的外部Alpha通道
            #pragma shader_feature ETC1_EXTERNAL_ALPHA
            // 包含Unity的CG库
            #include "UnityCG.cginc"

            // 定义顶点数据结构
            struct appdata_t {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            // 定义从顶点着色器到片段着色器的结构
            struct v2f {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            // 定义全局变量
            fixed4 _Color;
            float _Outline;
            fixed4 _OutlineColor;

            // 顶点着色器
            v2f vert(appdata_t IN) {
                v2f OUT;
                // 将顶点位置转换到裁剪空间
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                // 应用颜色和颜色混合
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                // 如果启用了像素对齐，则应用
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            // 定义纹理采样器
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_TexelSize;
            // 采样偏移量，用于调整轮廓效果的锐利度
            float _SampleOffset;
            // 采样精灵纹理的函数
            fixed4 SampleSpriteTexture (float2 uv) {
                fixed4 color = tex2D (_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                // 如果定义了ETC1_EXTERNAL_ALPHA，从外部纹理采样Alpha值
                color.a = tex2D (_AlphaTex, uv).r;
                #endif //ETC1_EXTERNAL_ALPHA

                return color;
            }

            // 片段着色器
            fixed4 frag(v2f IN) : SV_Target {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                
                // 如果轮廓效果启用且当前像素不透明
                if (_Outline > 0 && c.a != 0) {

                    // 采样偏移量，用于调整轮廓效果的锐利度
                    float2 offest =float2(_MainTex_TexelSize.x*_SampleOffset, _MainTex_TexelSize.y*_SampleOffset);

                    // 获取周围四个像素的颜色
                    fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, offest.y));
                    fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, offest.y));
                    fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(offest.x, 0));
                    fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(offest.x, 0));

                    // 如果周围四个像素中任意一个像素是透明的，则渲染轮廓
                    if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a == 0) {
                        // 使用轮廓颜色
                        c.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
                    }
                }

                // 应用预乘Alpha，使得颜色值仅在Alpha非零时显示
                c.rgb *= c.a;

                return c;
            }
            ENDCG
        }
    }
}