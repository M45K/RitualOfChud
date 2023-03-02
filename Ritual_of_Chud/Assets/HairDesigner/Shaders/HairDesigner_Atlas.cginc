#include "UnityPBSLighting.cginc"

struct Input
{	
	float2 uv_MainTex;	
	float3 viewDir;
	float4 color: COLOR;
	float4 tangent_input;
	float4 screenPos;	
	float face : VFACE;
};



sampler2D _MainTex;
sampler2D _NormalTex;
sampler2D _AOTex;
sampler2D _SpecularTex;

sampler2D _GrabTexture: register(s0);

float _Smoothness;
float _AO;
float4 _RootColor;
float4 _TipColor;
float _ColorSlider;
float4 _RimColor;
float _RimPower;
float _Cut;
float _Stripes;
float _Emission;
float _ColorBias;
float _ColorPower;
float _NormalMode;
float _NormalPower;
float _AlphaCutoff;
float _Alpha;
float _RndSeed;
float _AtlasSize;
float3 _Color;
float _Shift1;
float _Shift2;
float4 _SpecularColor1;
float4 _SpecularColor2;
float _SpecularExp1;
float _SpecularExp2;

float _TranslucencyStrength = 2;
float _TransluscencyNormal = .5;
float _TransluscencyPower = 2;
float _TransluscencyDirect = 1;
float _TransluscencyIndirect = .2;

float _TwoSided;
float _TransparentShadowReceive;
float _Length = .5f;






#define f3_f(c) (dot(round((c) * 255), float3(65536, 256, 1)))
#define f_f3(f) (frac((f) / float3(16777216, 65536, 256)))

struct SurfaceOutputHairDesigner
{
	fixed3 Albedo;      // diffuse color
	fixed3 Specular;    // specular color
	fixed3 Normal;      // tangent space normal, if written
	half3 Emission;
	half Smoothness;    // 0=rough, 1=smooth
	half Occlusion;     // occlusion (default 1)
	fixed Alpha;        // alpha for transparencies
	fixed SpecShift;	
	fixed SpecMask;
	half3 Tangent;
	fixed3 ScreenColor;
	float Face;
	float Tip;
};

void vert(inout appdata_full v, out Input o)
{
	UNITY_INITIALIZE_OUTPUT(Input, o);
	HairDesigner(v);
	o.color = v.color;
	o.tangent_input = normalize(v.tangent);
	//float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
	if (TRANSPARENT_PASS == 1)
		o.screenPos = ComputeGrabScreenPos(UnityObjectToClipPos(v.vertex.xyz));
	
}



void surf(Input IN, inout SurfaceOutputHairDesigner o)
{

	

	float strandFactor = 0;
	float4 strandFactor4 = float4(0,0,0,0);
	float strandColor = 0;
	float3 n = float3(0, 0, 0);
	
	float2 uv = IN.uv_MainTex;
	//clip((1-_Cut)- uv.y);
	//clip((_Length) - uv.y);
	clip((_Length - _Cut) - uv.y);


	float as = floor(_AtlasSize);
	float2 uvTex = uv / as;
	uvTex.x += round(RND(IN.color.a * (_RndSeed+1)) * (as - 1)) * (1 / as);
	uvTex.y += round( RND( IN.color.a * ((_RndSeed+1)+ (_RndSeed+1))) * (as -1) ) * (1 / as);
	
	
	

	float _ColorPower = 1;
	float f = (uv.y + _ColorBias*2.0);
	float k = uv.y / _Length;
	float4 col = lerp(_RootColor, _TipColor, clamp(  f  * (abs(sin(k*k*pi*_Stripes))), 0, 1));
	

	uvTex.y += (1 - _Length) / as ; //length uvShift method
	
	//clip(uvTex.y > 1);
	float4 diffuse = tex2D(_MainTex, uvTex) * col;// *lerp(_StartColor, _EndColor, clamp(pow(f, _ColorPower)  * (abs(sin(uv.y*pi*2.0*_Stripes))), 0, 1));
		
	diffuse.a *= uv.y*_Length < .1 ? uv.y*_Length*10.0 : 1;
	
	//diffuse.a *= lerp(_Length*_Length*_Length,1, _Length- uv.y);//length alpha method
	
	if (TRANSPARENT_PASS == 0)
		clip(diffuse.a - _AlphaCutoff);
	
	
	o.Tip = uv.y*_Length;
	o.Albedo = diffuse.rgb*col.rgb;
	o.Normal = lerp(float3(0, 0, 1), UnpackNormal(tex2D(_NormalTex, uvTex)), _NormalPower);
	o.Occlusion =  lerp(1,tex2D(_AOTex, uvTex),_AO);

	o.Smoothness = _Smoothness;

	float a = diffuse.a*col.a*_Alpha / (_AlphaCutoff*(1 - _Alpha*col.a*.5));
	
	float tipAlphaThreshold = (_Length - _Cut)*.9;
	if (uv.y>tipAlphaThreshold)
		a *= 1.0 - (uv.y - tipAlphaThreshold) / ((_Length - _Cut)*0.1);

	o.Alpha = a;

	
	if (TRANSPARENT_PASS == 0)
		clip(a - _AlphaCutoff );
		
	o.Emission = o.Albedo * _Emission;

	o.Tangent = IN.tangent_input;

	//o.Normal = lerp(o.Normal, cross(o.Tangent, o.Normal), (uv.x - .5)*2);
	o.Normal = normalize(o.Normal);
	o.Face = IN.face;

	fixed3 spec = tex2D(_SpecularTex, uvTex).rgb;
	o.Specular = spec.r;
	o.SpecShift = spec.g;
	o.SpecMask = spec.b;
	
	if (TRANSPARENT_PASS == 1)
	{
		o.ScreenColor = tex2Dproj(_GrabTexture, IN.screenPos);
	}
}



//------------------------------------
//scheuermann shift

half3 ShiftTangent(half3 T, half3 N, float shift)
{
	half3 shiftedT = T + shift * N;
	return normalize(shiftedT);
}

float StrandSpecular(half3 T, half3 V, half3 L, float exponent)
{
	half3 H = normalize(L + V);
	float dotTH = dot(T, H);
	float sinTH = sqrt(1.0 - dotTH * dotTH);
	float dirAtten = smoothstep(-1.0, 0.0, dotTH);
	return dirAtten * pow(sinTH, exponent);
}
//------------------------------------

#if UNITY_VERSION <= 549
half LinearRgbToLuminance(half3 linearRgb)
{
	return dot(linearRgb, half3(0.2126729f, 0.7151522f, 0.0721750f));
}
#endif


inline half4 Transluscency(SurfaceOutputHairDesigner s, half3 viewDir,  UnityGI gi)
{
	half3 lightDir = gi.light.dir + s.Normal * _TransluscencyNormal;
	half power = pow(saturate(dot(viewDir, -lightDir)), _TransluscencyPower);
	half3 translucency = gi.light.color * (power * _TransluscencyDirect + gi.indirect.diffuse * _TransluscencyIndirect) * _TranslucencyStrength * (1 - abs(dot(s.Tangent, viewDir))) * s.Tip;
	return half4(s.Albedo * translucency * _TranslucencyStrength, 0) * saturate(dot(-gi.light.dir, viewDir));
}

inline half4 LightingHairAtlas(SurfaceOutputHairDesigner s, half3 viewDir, UnityGI gi)
{
	//s.Normal *= dot(s.Normal, viewDir);
	s.Normal *= s.Face;
	s.Normal = normalize(s.Normal);

	float VdotL = dot(normalize(viewDir), normalize(gi.light.dir));
	half3 T = -normalize(cross(s.Normal, s.Tangent));
	float NdotL = saturate(dot(s.Normal, gi.light.dir));
	half3 tan = s.Tangent *.5 + .5;

	float shiftTex = s.SpecShift - .5;
	half3 t1 = ShiftTangent(T, s.Normal, _Shift1 + shiftTex);
	half3 t2 = ShiftTangent(T, s.Normal, _Shift2 + shiftTex);
	half3 spec = 0;
	spec += _SpecularColor1.rgb * StrandSpecular(t1, viewDir, gi.light.dir, _SpecularExp1) *_SpecularColor1.a;
	spec += _SpecularColor2.rgb * s.SpecMask * StrandSpecular(t2, viewDir, gi.light.dir, _SpecularExp2) *_SpecularColor2.a;

	float3 h = normalize(gi.light.dir + viewDir);
	float VdotH = dot(viewDir, h);
	
	spec *= saturate( clamp( VdotL, 0 ,1 ) );
	
	s.Specular = lerp(spec, s.Smoothness, s.Smoothness*s.Smoothness);
	//s.Specular = spec;
	s.Albedo *= s.Occlusion;

	SurfaceOutputStandardSpecular r;
	r.Albedo = s.Albedo;
	r.Normal = s.Normal;
	//better light orientation

	if(dot(-s.Normal, gi.light.dir) <  dot(-s.Tangent, gi.light.dir) )		
		r.Normal = lerp(normalize(s.Normal + gi.light.dir), s.Normal, abs(dot(-s.Normal, gi.light.dir)));	
	else		
		r.Normal = lerp(normalize(s.Normal + gi.light.dir), s.Tangent, abs(dot(-s.Tangent, gi.light.dir)));
	
	
	r.Emission = s.Emission;
	r.Specular = s.Specular;
	r.Smoothness = s.Smoothness;
	r.Occlusion = s.Occlusion;
	r.Alpha = s.Alpha;

	half4 c = LightingStandardSpecular(r, viewDir, gi);
	
	if (TRANSPARENT_PASS == 1)
	{
		//Fake light attenuation
		float l = LinearRgbToLuminance(s.ScreenColor) / LinearRgbToLuminance(c);
		l = clamp(l, 1 - _TransparentShadowReceive, 1);
		c.rgb = lerp(c.rgb*l, c.rgb, l);
	}	
	c += Transluscency(s, viewDir, gi);

	return c;

}


inline void LightingHairAtlas_GI(
	SurfaceOutputHairDesigner s,
	UnityGIInput data,
	inout UnityGI gi)
{

#if UNITY_VERSION <= 549
	UNITY_GI(gi, s, data);
#elif defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
	gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
#else
	Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(s.Smoothness, data.worldViewDir, s.Normal, s.Specular);
	gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal, g);
#endif
}
