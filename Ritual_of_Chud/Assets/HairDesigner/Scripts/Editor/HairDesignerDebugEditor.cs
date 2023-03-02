using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kalagaan
{
    namespace HairDesignerExtension
    {
        [CustomEditor(typeof(HairDesignerDebug))]
        public class HairDesignerDebugEditor : Editor
        {
            //List<HairDesignerGenerator> m_BadGenerators = new List<HairDesignerGenerator>();
            //List<HairDesignerShader> m_BadShaders = new List<HairDesignerShader>();
            List<HairDesignerGenerator> m_AllGenerators = new List<HairDesignerGenerator>();
            List<HairDesignerShader> m_AllShaders = new List<HairDesignerShader>();

            public override void OnInspectorGUI()
            {


                if( GUILayout.Button("Check All scripts") )
                {
                    CheckAll();
                }

                /*
                if (GUILayout.Button("Check unlinked scripts"))
                {
                    m_BadGenerators.Clear();
                    m_BadShaders.Clear();
                    HairDesignerDebug dbg = target as HairDesignerDebug;

                    HairDesignerGenerator[] generators = dbg.GetComponents<HairDesignerGenerator>();
                    for (int i = 0; i < generators.Length; ++i)
                    {
                        //Debug.Log(lst[i].GetType().ToString() + " -> " + lst[i].hideFlags.ToString());
                        if (generators[i].m_hd == null)
                            m_BadGenerators.Add(generators[i]);
                    }


                    HairDesignerShader[] shaders = dbg.GetComponents<HairDesignerShader>();
                    for (int i = 0; i < shaders.Length; ++i)
                    {
                        //Debug.Log(lst[i].GetType().ToString() + " -> " + lst[i].hideFlags.ToString());
                        if (shaders[i].m_generator == null)
                            m_BadShaders.Add(shaders[i]);
                    }
                }
                */
                

                /*
                if (m_BadGenerators.Count>0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Label("Unused generator", EditorStyles.boldLabel);
                    for (int i = 0; i < m_BadGenerators.Count; ++i)
                        GUILayout.Label(m_BadGenerators[i].m_name + " " + m_BadGenerators[i].GetType().ToString()  );
                    GUILayout.EndVertical();
                }


                if (m_BadShaders.Count > 0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Label("Unused shaders", EditorStyles.boldLabel);
                    for (int i = 0; i < m_BadShaders.Count; ++i)
                        GUILayout.Label( m_BadShaders[i].GetType().ToString());
                    GUILayout.EndVertical();
                }
                */


                if (m_AllGenerators.Count > 0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Label("All generator", EditorStyles.boldLabel);
                    for (int i = 0; i < m_AllGenerators.Count; ++i)
                    {
                        GUILayout.Label(m_AllGenerators[i].m_name + " " + m_AllGenerators[i].GetType().ToString() + "\n" + m_AllGenerators[i].m_shaderParams.Count);
                        for(int j=0; j< m_AllGenerators[i].m_shaderParams.Count; ++j)
                        {
                            if (m_AllGenerators[i].m_shaderParams[j].m_generator == null)
                                GUILayout.Label("Unlinked shader param");
                        }

                    }
                    GUILayout.EndVertical();
                }
                
                if (m_AllShaders.Count > 0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Label("All shaders", EditorStyles.boldLabel);
                    int cnt = 0;
                    for (int i = 0; i < m_AllShaders.Count; ++i)
                    {
                        bool unlinked = m_AllShaders[i].m_generator == null;
                        GUI.color = unlinked ? Color.red : Color.white;
                        GUI.color = Color.red;
                        GUILayout.Label(m_AllShaders[i].GetType().ToString(), unlinked ?  EditorStyles.boldLabel : EditorStyles.label);
                        cnt += unlinked ? 1:0;
                    }
                    GUI.color = Color.white;


                    if (cnt > 0)
                    {
                        if (GUILayout.Button("Clean unused"))
                        {
                            for (int i = 0; i < m_AllShaders.Count; ++i)
                            {
                                if (m_AllShaders[i].m_generator == null)
                                {
                                    DestroyImmediate(m_AllShaders[i]);
                                    //m_AllShaders[i].hideFlags = HideFlags.DontSave;
                                }
                            }
                            CheckAll();
                        }
                    }
                    GUILayout.EndVertical();


                }
            }

            
            void CheckAll()
            {
                m_AllGenerators.Clear();
                m_AllShaders.Clear();

                HairDesignerDebug dbg = target as HairDesignerDebug;

                HairDesignerGenerator[] generators = dbg.GetComponents<HairDesignerGenerator>();
                for (int i = 0; i < generators.Length; ++i)
                {
                    m_AllGenerators.Add(generators[i]);
                }


                HairDesignerShader[] shaders = dbg.GetComponents<HairDesignerShader>();
                for (int i = 0; i < shaders.Length; ++i)
                {
                    m_AllShaders.Add(shaders[i]);
                }
                
            }
            
        }
    }
}