using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kalagaan.HairDesignerExtension; // use the namespace
using UnityEngine.UI;

public class Pelo_Controller : MonoBehaviour {


public HairDesigner m_hair; // this is the reference of the hairDesigner instance
	public float LunghezzaPelo ;
	public static float peloholder;
	public GameObject Loader;
	public Slider mSlider;
// Update is called once per frame
	void Start()
	{
		var Holder = Loader.GetComponent<ProtocolliLoaderClass>().Dholder;
		LunghezzaPelo =Holder.StartPelo ;
		mSlider.value = LunghezzaPelo;
		
	 HairDesignerGenerator layer = m_hair.GetLayer(0);
		HairDesignerShaderAtlas shader = layer.GetShaderParams() as HairDesignerShaderAtlas;
		shader.m_cut = LunghezzaPelo;
		layer.m_shaderNeedUpdate = true;
	}
	void Update () {
	
	 HairDesignerGenerator layer = m_hair.GetLayer(0);
		//get the layer by id or by name
		//the shader parameters have to be casted into the shader class used by the layer
		//All the shader classes can be found in the folder "HairDesigner/scripts/"
		HairDesignerShaderAtlas shader = layer.GetShaderParams() as HairDesignerShaderAtlas;
		//all the shader parameters can be modified
		shader.m_cut = LunghezzaPelo;
		layer.m_shaderNeedUpdate = true;
		//peloholder = LunghezzaPelo;
		
	}

	public void AdjustLength(float newLunghezzaPelo){
		LunghezzaPelo = newLunghezzaPelo;
		//Debug.Log("Pelo = " + newLunghezzaPelo.ToString());
		
	}
}
