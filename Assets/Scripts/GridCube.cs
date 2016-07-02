using UnityEngine;
using System.Collections;

public class GridCube : MonoBehaviour
{
    // Don't want to show these properties in the inspector since changing them is done automatically.
    [HideInInspector]
    public Color Colour;
    [HideInInspector]
    public string ColourName;
    
    // Show these ones.
    public int ColourValue;
    public CubeType GridCubeType;
    
    // Use this for initialization
    void Start()
    {
        Colour = GetComponent<Renderer>().material.color;
        ColourName = GetComponent<Renderer>().material.name.Replace("Cube", string.Empty).Replace("(Instance)",string.Empty).ToLower().Trim();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// All types of cube.
    /// </summary>
    public enum CubeType
    {
        Danger,
        Colour,
        Finish,
        Normal
    }
}
