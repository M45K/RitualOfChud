using UnityEngine;
using UnityEditor;
using System.Collections;


namespace Kalagaan
{
    namespace HairDesignerExtension
    {
        [CustomEditor(typeof(HairDesignerShaderFurShell))]
        public class HairDesignerShaderFurShellEditor : HairDesignerShaderEditor
        {
            public static HairDesignerShaderFurShell m_copy;
            

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (_destroyed) return;

                HairDesignerShaderFurShell s = target as HairDesignerShaderFurShell;
                

                GUILayoutTextureSlot("Main texture", ref s.m_mainTex, ref s.m_mainTexScale, ref s.m_mainTexOffset);
                GUILayoutTextureSlot("Density texture", ref s.m_densityTex, ref s.m_densityTexScale, ref s.m_densityTexOffset);
                GUILayoutTextureSlot("Mask texture", ref s.m_maskTex, ref s.m_maskTexScale, ref s.m_maskTexOffset);
                GUILayoutTextureSlot("Color texture", ref s.m_colorTex, ref s.m_colorTexScale, ref s.m_colorTexOffset);
                s.m_brushTex = EditorGUILayout.ObjectField( "Brush texture", s.m_brushTex, typeof(Texture2D), false) as Texture2D;
                

                s.m_RootColor = EditorGUILayout.ColorField("Root Color", s.m_RootColor);
                s.m_TipColor = EditorGUILayout.ColorField("Tip Color", s.m_TipColor);
                s.m_ColorBias = EditorGUILayout.Slider("Color bias 1", s.m_ColorBias, 0f, 1f);
                s.m_ColorBiasLength = EditorGUILayout.Slider("Color bias 2", s.m_ColorBiasLength, 0f, 1f);

                s.m_furLength = Mathf.Clamp( EditorGUILayout.FloatField("Fur length", s.m_furLength), 0, float.MaxValue);
                s.m_thickness = EditorGUILayout.Slider("Thickness", s.m_thickness, 0, 1);
                s.m_curlNumber = EditorGUILayout.Slider("Curl number", s.m_curlNumber, 0, 50);
                s.m_curlRadius = EditorGUILayout.Slider("Curl radius", s.m_curlRadius, 0, 10);
                s.m_curlRnd = EditorGUILayout.Slider("Curl random", s.m_curlRnd, 0, 1);
                s.m_gravity = EditorGUILayout.Slider("Gravity", s.m_gravity, 0, 1);
                s.m_wind = EditorGUILayout.FloatField("Wind", s.m_wind);
                s.m_windTurbulenceFrequency = EditorGUILayout.FloatField("Wind turbulence", s.m_windTurbulenceFrequency);

                GUI.enabled = s.m_brushTex != null;
                s.m_brushFactor = EditorGUILayout.Slider("BrushFactor", s.m_brushFactor, 0, 1);
                GUI.enabled = true;

                s.m_smoothness = EditorGUILayout.Slider("Smoothness", s.m_smoothness, 0, 1);
                s.m_metallic = EditorGUILayout.Slider("Metallic", s.m_metallic, 0, 1);
                s.m_emission = EditorGUILayout.Slider("Emission", s.m_emission, 0, 1);
                s.m_emissionPower = EditorGUILayout.Slider("Emission power", s.m_emissionPower/10f, 0, 1) * 10f;
                s.m_AO = EditorGUILayout.Slider("AO", s.m_AO, 0, 1);

                s.m_RimColor = EditorGUILayout.ColorField("Rim color", s.m_RimColor);
                s.m_RimPower = EditorGUILayout.Slider("Rim power", s.m_RimPower, 0, 1);


                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy parameters"))
                {
                    m_copy = s.gameObject.AddComponent<HairDesignerShaderFurShell>();
                    m_copy.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
                    EditorUtility.CopySerialized(s, m_copy);
                }

                GUI.enabled = m_copy != null;
                if (GUILayout.Button("Paste parameters"))
                {
                    Undo.RecordObject(s, "Shader parameters paste");
                    EditorUtility.CopySerialized(m_copy, s);
                }
                GUILayout.EndHorizontal();
            }



        }
    }
}
