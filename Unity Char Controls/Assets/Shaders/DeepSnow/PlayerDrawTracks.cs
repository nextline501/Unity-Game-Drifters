using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawTracks : MonoBehaviour
{
    private RenderTexture _splatmap;
    public Shader _drawShader;
    private Material _snowMaterial, _drawMaterial;
    private RaycastHit _deepSnowHit;
   
    public GameObject terrain;
    public Transform feet;
    int _deepSnowLayerMask;

    [Range(0,2)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;

    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        _deepSnowLayerMask = LayerMask.GetMask("DeepSnow");
        _drawMaterial = new Material(_drawShader);
        _snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        _snowMaterial.SetTexture("_Splat", _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(feet.position, -Vector3.up, out _deepSnowHit, 1f, _deepSnowLayerMask) && lastPosition != feet.position)
        {
            lastPosition = feet.position;
            for(int i = 0; i < 4; i++)
            {
                Debug.Log("WE ARE HITTING DEEP SNOW");
                _drawMaterial.SetVector("_Coordinate", new Vector4(_deepSnowHit.textureCoord.x, _deepSnowHit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", _brushStrength);
                _drawMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            } 
        }     
    }
}
