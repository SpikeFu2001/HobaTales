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

    private static int TOON_SATURATION_ID = Shader.PropertyToID("_Saturation");

    private ArrayList _materials;
    
    private void SetSaturation(Material material, float newSaturation)
    {
        material.SetFloat(TOON_SATURATION_ID, _saturation);
    }

    private void Update()
    {
        foreach (Material material in _shaders)
        {
            SetSaturation(material, _saturation);
        }
    }
}
