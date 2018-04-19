using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Datamosh : MonoBehaviour {
    

    public Material DMmat; //datamosh material
    public bool hardDatamosh = false;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        //generate the motion vector texture @ '_CameraMotionVectorsTexture'
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Shader.SetGlobalInt("_Button", hardDatamosh ? 1 : 0);
        Graphics.Blit(src, dest, DMmat);
    }

    public void EnableHardDatamosh()
    {
        hardDatamosh = true;
    }

    public void DisableHardDatamosh()
    {
        hardDatamosh = false;
    }
}
