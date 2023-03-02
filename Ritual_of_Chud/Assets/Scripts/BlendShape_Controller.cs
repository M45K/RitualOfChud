using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlendShape_Controller : MonoBehaviour {

	public SkinnedMeshRenderer skmr;
    public Mesh mesh;
  
    public int blendindex;
    public static float getblendshapeweighttrn;
    public Slider sliderx;
    public GameObject Loader;
    public float SliderShapeStart;

    void Start()
	{
		var Holder = Loader.GetComponent<ProtocolliLoaderClass>().Dholder;
		SliderShapeStart =Holder.StartCorpo ;
		sliderx.value = SliderShapeStart*100;
    }



    
     public void Update()
    {   
        skmr = GetComponent<SkinnedMeshRenderer>();
        mesh = skmr.sharedMesh;

        skmr.SetBlendShapeWeight(blendindex, sliderx.value);
        getblendshapeweighttrn = skmr.GetBlendShapeWeight(blendindex);

    }
 }
