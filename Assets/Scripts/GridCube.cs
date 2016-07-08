using UnityEngine;
using System.Collections;

public class GridCube : MonoBehaviour
{
    // Don't want to show these properties in the inspector since changing them is done automatically.
    [HideInInspector]
    public Color Colour;
    [HideInInspector]
    public string ColourName;
    [HideInInspector]
    public bool ColourTaken;

    public Color32 SecondaryColour;
    public string SecondaryColourName;

    [Header("Only use this if you need multiple colours to mix.")]
    public bool MoreToMix;
    public int AmountToMix;
    // Show these ones.
    public CubeType GridCubeType;
    private Cube MainCube;
    private GameObject[] Borders;
    private GameObject[] GridCubes;

    // Use this for initialization
    void Start()
    {
        Colour = GetComponent<Renderer>().material.color;
        ColourName = GetComponent<Renderer>().material.name.Replace("Cube", string.Empty).Replace("(Instance)", string.Empty).ToLower().Trim();
        Borders = GameObject.FindGameObjectsWithTag("FinishBorders");
        GridCubes = GameObject.FindGameObjectsWithTag("GridCube");
        MainCube = GameObject.Find("MainCube").GetComponent<Cube>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCube.MixerSlider.maxValue == MainCube.MixerSlider.value)
        {
            SetBorders();
        }
    }

    /// <summary>
    /// All types of cube.
    /// </summary>
    public enum CubeType
    {
        Danger,
        Colour,
        Finish,
        Normal,
        Switcher
    }

    /// <summary>
    /// Switch the cube colour to the secondary colour.
    /// </summary>
    public void Switch()
    {
        if (GridCubeType != CubeType.Switcher) return; 
        Color defaultColour = new Color32(0, 0, 0, 0);
        foreach (var gridCube in GridCubes)
        {
            GridCube gCube = gridCube.GetComponent<GridCube>();
            if (gCube.SecondaryColour != defaultColour)
            {
                Color32 tempColour = gCube.SecondaryColour;
                string tempName = gCube.SecondaryColourName;
                gCube.SecondaryColourName = gCube.ColourName;
                gCube.ColourName = tempName;

                gCube.GetComponent<Renderer>().material.color = gCube.SecondaryColour;
                gCube.SecondaryColour = gCube.Colour;
                gCube.Colour = tempColour;
                gCube.ColourTaken = false;
            }
        }
    }

    public void SetBorders()
    {
        foreach (var border in Borders)
        {
            border.GetComponent<Renderer>().material.color = GetFinishCubeColour();
        }
    }

    private Color32 GetFinishCubeColour()
    {
        foreach (var gridCube in GridCubes)
        {
            GridCube gCube = gridCube.GetComponent<GridCube>();
            if (gCube.GridCubeType == CubeType.Finish)
            {
                return gCube.Colour;
            }
        }
        return new Color32(0,0,0,0);
    }

    /// <summary>
    /// Disable the cube after taking the colour
    /// </summary>
    public void Disable()
    {
        ColourTaken = true;
        GetComponent<Renderer>().material.color = new Color32(128, 128, 128, 128);
    }
}
