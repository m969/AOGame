Shader "Hidden/VFoldersOutline"
{
    Properties
    {
        _MainTex ("Texture", Any) = "white" {}
        [HDR] _Color ("Tint", Color) = (1,1,1,1)
    }

    CGINCLUDE
    
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"

    struct appdata_t 
    {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f 
    {
        float4 vertex : SV_POSITION;
        float2 texcoord : TEXCOORD0;
        float2 clipUV : TEXCOORD1;
    };

    sampler2D _MainTex;
    sampler2D _GUIClipTexture;
    bool _ManualTex2SRGB;
    float4 _MainTex_ST;
    float4 _Color;
    float4x4 unity_GUIClipTextureMatrix;

    v2f vert (appdata_t v)
    {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        o.vertex = UnityObjectToClipPos(v.vertex);
        float3 eyePos = UnityObjectToViewPos(v.vertex);
        o.clipUV = mul(unity_GUIClipTextureMatrix, float4(eyePos.xy, 0, 1.0));
        o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
        return o;
    }

    fixed4 frag (v2f i) : SV_Target
    {
        float4 c = tex2D(_MainTex, i.texcoord);
        c.rgb = _Color.rgb;
        c.a *= _Color.a;
        c.a *= tex2D(_GUIClipTexture, i.clipUV).a;
        return c;
    }
    
    ENDCG

    SubShader 
    {
        Blend SrcAlpha OneMinusSrcAlpha, One One
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            ENDCG
        }
    }

    SubShader 
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest Always

        Pass 
        {
            CGPROGRAM
            ENDCG
        }
    }
}
