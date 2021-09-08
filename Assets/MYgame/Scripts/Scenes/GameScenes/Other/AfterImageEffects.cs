using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum RenderingMode
{
    Opaque,
    Cutout,
    Fade,
    Transparent,
}


class AfterImage
{
    //afterimage grid
    public Mesh _Mesh;
    //Afterimage texture
    public Material _Material;
    //Afterimage position
    public Matrix4x4 _Matrix;
    //afterimage transparency
    public float _Alpha;
    //Afterimage start time
    public float _StartTime;
    //afterimage retention time
    public float _Duration;

    public AfterImage(Mesh mesh, Material material, Matrix4x4 matrix4x4, float alpha, float startTime, float duration)
    {
        _Mesh = mesh;
        _Material = material;
        _Matrix = matrix4x4;
        _Alpha = alpha;
        _StartTime = startTime;
        _Duration = duration;
    }
}

///<summary>
///Afterimage effects
///</summary>
public class AfterImageEffects : MonoBehaviour
{
    public enum SurfaceType
    {
        Opaque,
        Transparent
    }
    public enum BlendMode
    {
        Alpha,
        Premultiply,
        Additive,
        Multiply
    }


    public Material MeshMaterial;

    //Open afterimage
    public bool _OpenAfterImage;

    //afterimage color
    public Color _AfterImageColor = Color.black;
    //The survival time of the afterimage
    public float _SurvivalTime = 1;
    //The interval between generating afterimages
    public float _IntervalTime = 0.2f;
    public float _Time = 0;
    //The initial transparency of the afterimage
    [Range(0.1f, 1.0f)]
    public float _InitialAlpha = 1.0f;

    private List<AfterImage> _AfterImageList;
    private SkinnedMeshRenderer _SkinnedMeshRenderer;

    void Awake()
    {
        _AfterImageList = new List<AfterImage>();
        _SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if (_OpenAfterImage && _AfterImageList != null)
        {
            if (_SkinnedMeshRenderer == null)
            {
                _OpenAfterImage = false;
                return;
            }

            _Time += Time.deltaTime;
            //Generate afterimages
            CreateAfterImage();
            //Refresh the afterimage
            UpdateAfterImage();
        }
    }

    ///<summary>
    ///Generate afterimage
    ///</summary>
    void CreateAfterImage()
    {
        //Generate afterimages
        if (_Time >= _IntervalTime)
        {
            _Time = 0;

            Mesh mesh = new Mesh();
            _SkinnedMeshRenderer.BakeMesh(mesh);
           
            Material material = new Material(_SkinnedMeshRenderer.material);
            material.SetFloat("_Surface", (float)SurfaceType.Transparent);
            material.SetFloat("_Blend", (float)BlendMode.Additive);
            material.mainTexture = null;
            SetupMaterialBlendMode(material);
            //_SkinnedMeshRenderer.material = material;
            SetMaterialRenderingMode(material, RenderingMode.Fade);

            _AfterImageList.Add(new AfterImage(
                mesh,
                material,
                transform.localToWorldMatrix,
                _InitialAlpha,
                Time.realtimeSinceStartup,
                _SurvivalTime));
        }
    }

    ///<summary>
    ///Refresh the afterimage
    ///</summary>
    void UpdateAfterImage()
    {
        //Refresh the residual image and destroy the obsolete residual image according to the survival time
        for (int i = _AfterImageList.Count - 1; i >= 0; i--)
        {
            float _PassingTime = Time.realtimeSinceStartup - _AfterImageList[i]._StartTime;

            if (_PassingTime > _AfterImageList[i]._Duration)
            {
                _AfterImageList.Remove(_AfterImageList[i]);
                Destroy(_AfterImageList[i]._Mesh);
                
              //  Destroy(_AfterImageList[i]);
                continue;
            }

            if (_AfterImageList[i]._Material.HasProperty("_BaseColor"))
            {
                _AfterImageList[i]._Alpha *= (1 - _PassingTime / _AfterImageList[i]._Duration);
                _AfterImageColor.a = _AfterImageList[i]._Alpha;
                _AfterImageList[i]._Material.SetColor("_BaseColor", _AfterImageColor);
            }

            Graphics.DrawMesh(_AfterImageList[i]._Mesh, _AfterImageList[i]._Matrix, _AfterImageList[i]._Material, gameObject.layer);
        }
        
    }

    ///<summary>
    ///Set texture rendering mode
    ///</summary>
    void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
              //  material.("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

    void SetupMaterialBlendMode(Material material)
    {
        bool alphaClip = material.GetFloat("_AlphaClip") == 1;
        if (alphaClip)
            material.EnableKeyword("_ALPHATEST_ON");
        else
            material.DisableKeyword("_ALPHATEST_ON");
        SurfaceType surfaceType = (SurfaceType)material.GetFloat("_Surface");
        if (surfaceType == 0)
        {
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
            material.SetShaderPassEnabled("ShadowCaster", true);
        }
        else
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_ZWrite", 0);
            material.SetShaderPassEnabled("ShadowCaster", false);
            BlendMode blendMode = (BlendMode)material.GetFloat("_Blend");
            switch (blendMode)
            {
                case BlendMode.Alpha:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
                case BlendMode.Premultiply:
                    //material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    //material.SetInt("_ZWrite", 0);
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    //material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
                case BlendMode.Additive:
                   // material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                   // material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    //material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
                case BlendMode.Multiply:
                    //material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                   // material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    //material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
            }
        }
    }
}
