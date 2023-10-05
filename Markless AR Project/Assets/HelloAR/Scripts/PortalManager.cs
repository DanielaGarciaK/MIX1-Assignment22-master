using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Video;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject Video;

    public Material[] VideoMaterials;

    public Material PortalPlaneMaterial;

    public VideoPlayer VideoPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        VideoMaterials = Video.GetComponent<Renderer>().sharedMaterials;
        PortalPlaneMaterial = GetComponent<Renderer>().sharedMaterial;
        VideoPlayer = Video.GetComponent<VideoPlayer>();
        VideoPlayer.Stop();
        Video.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

        //when the camera/player is inside the virtual world
        if (camPositionInPortalSpace.y <= 0.0f)
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int) CompareFunction.NotEqual);
            }
            
            Video.SetActive(true);
            
            VideoPlayer.Play();
            
            PortalPlaneMaterial.SetInt("_CullMode", (int) CullMode.Front);
        }
        
        //when they're are really close but outside the virtual world.
        else if (camPositionInPortalSpace.y < 0.5f)
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
            
            Video.SetActive(true);

            VideoPlayer.Play();
            
            PortalPlaneMaterial.SetInt("_CullMode", (int) CullMode.Off);
        }
        
        //when they are in the real world and far from the portal, at least half a meter
        else 
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
            
            Video.SetActive(false);

            VideoPlayer.Pause();
            
            PortalPlaneMaterial.SetInt("_CullMode", (int) CullMode.Back);
        }
    }
}
