using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

///-////////////////////////////////////////////////////////////////////////////////
/// 
public class ToonShader : MonoBehaviour
{
    [SerializeField] private Material[] _shaders;

    [Space] [SerializeField] private float _saturationTweenDuration;    
    
    [Header("Properties")]
    [Tooltip("The shader's color.")]
    [SerializeField] private Color _color = Color.white;
    [Tooltip("The brightness of the shadow color.")] 
    [SerializeField] private float _brightness = 0.3f;
    [Tooltip("The saturation of the shader")]
    [SerializeField] private float _saturation = 1f;
    [Tooltip("The strength of the shadow lighting edge.")] 
    [SerializeField] private float _lightingEdge = 0.07f;
    [Tooltip("The strength of the outer lighting edge.")] 
    [SerializeField] private float _outerLightingEdge = 0.4f;

    private ArrayList _materials;
    
    private static int TOON_SATURATION_ID = Shader.PropertyToID("_Saturation");

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    public void SaturateToonShaders()
    {
        foreach (Material m in _shaders)
        {
            m.DOFloat(1f, TOON_SATURATION_ID, _saturationTweenDuration);
        }
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    public void DesaturateToonShaders()
    {
        foreach (Material m in _shaders)
        {
            m.DOFloat(0, TOON_SATURATION_ID, _saturationTweenDuration);
        }
    }
}
