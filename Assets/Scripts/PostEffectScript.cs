using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour {

    public Material mat;

    //src is the fully rendered scene that you would normally send directly to monitor.
    //Here we are intercepting this src so we can change it before passing it on.
	void OnRenderImage( RenderTexture src, RenderTexture dest )
    {


        //To pass it on back to the monitor.
        Graphics.Blit(src, dest, mat);
    }
}
