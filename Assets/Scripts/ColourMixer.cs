using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColourMixer : MonoBehaviour
{
    public List<string> CurrentColours = new List<string>();
    public List<MixedColour> MixedColours = new List<MixedColour>();
    private Cube MainCube;
    private Image SliderFill;

    [HideInInspector]
    public Dictionary<string, int> PreMixer = new Dictionary<string, int>();

    public class MixedColour
    {
        public string FirstColour;
        public string SecondColour;
        public string ThirdColour;
        public string MyColour;

        /// <summary>
        /// Initialises the MixedColour class.
        /// </summary>
        /// <param name="firstColour">First colour.</param>
        /// <param name="secondColour">Second colour.</param>
        /// <param name="thirdColour">Third colour.</param>
        /// <param name="myColour">Combined colour.</param>
        public MixedColour(string firstColour, string secondColour, string thirdColour, string myColour)
        {
            FirstColour = firstColour;
            SecondColour = secondColour;
            ThirdColour = thirdColour;
            MyColour = myColour;
        }

        /// <summary>
        /// Check if the colours can be mixed.
        /// </summary>
        /// <param name="colours">Colours that can be mixed.</param>
        /// <returns>True if the colours can be mixed. False otherwise.</returns>
        public bool CanMake(List<string> colours)
        {
            if (colours.Contains(FirstColour) && colours.Contains(SecondColour))
            {
                if (string.IsNullOrEmpty(ThirdColour))
                {
                    return true;
                }
                else
                {
                    return colours.Contains(ThirdColour);
                }
            }

            return false;
        }
    }
    // Use this for initialization
    void Start()
    {
        // Declare all of the possible mixable colours here.
        MainCube = GameObject.Find("MainCube").GetComponent<Cube>();
        SliderFill = GameObject.Find("Fill").GetComponent<Image>();

        MixedColour green = new MixedColour("yellow", "blue", string.Empty, "green");
        MixedColour orange = new MixedColour("red", "yellow", string.Empty, "orange");
        MixedColour purple = new MixedColour("blue", "red", string.Empty, "purple");
        MixedColour teal = new MixedColour("blue", "green", string.Empty, "teal");
        MixedColour magenta = new MixedColour("purple", "red", string.Empty, "magenta");
        MixedColour vermilion = new MixedColour("red", "orange", string.Empty, "vermilion");
        MixedColour chartreuse = new MixedColour("yellow", "green", string.Empty, "chartreuse");
        MixedColour amber = new MixedColour("yellow", "orange", string.Empty, "amber");
        MixedColour violet = new MixedColour("purple", "blue", string.Empty, "violet");


        MixedColours.Add(green);
        MixedColours.Add(orange);
        MixedColours.Add(purple);
        MixedColours.Add(teal);
        MixedColours.Add(magenta);
        MixedColours.Add(vermilion);
        MixedColours.Add(chartreuse);
        MixedColours.Add(amber);
        MixedColours.Add(violet);
    }

    // Update is called once per frame
    void Update()
    {
        MixedColour colourToMake = CanMixColours();
        if (colourToMake != null)
        {
            MainCube.UpdateColour(MixColours(colourToMake));
        }
    }

    /// <summary>
    /// Checks if the colours can be mixed.
    /// </summary>
    /// <returns>True if the colours can be mixed. False otherwise.</returns>
    private MixedColour CanMixColours()
    {
        bool canMix = false;
        foreach (var mixedColour in MixedColours)
        {
            canMix = mixedColour.CanMake(CurrentColours);

            if (canMix)
            {
                return mixedColour;
            }
        }
        return null;
    }

    /// <summary>
    /// Mixes the colours.
    /// </summary>
    /// <param name="colourToMake">The colour that is being made.</param>
    /// <returns>The created colour.</returns>
    public Color MixColours(MixedColour colourToMake)
    {
        Color32 newColour;
        switch (colourToMake.MyColour.ToLower())
        {
            case "green":
                newColour = Color.green;
                break;

            case "orange":
                newColour = new Color32(255, 128, 0, 255);
                break;

            case "purple":
                newColour = new Color32(153, 0, 153, 255);
                break;

            case "teal":
                newColour = new Color32(0, 128, 128, 255);
                break;

            case "magenta":
                newColour = new Color32(255, 0, 144, 255);
                break;

            case "vermilion":
                newColour = new Color32(217, 96, 59, 255);
                break;

            case "chartreuse":
                newColour = new Color32(127, 255, 0, 255);
                break;

            case "amber":
                newColour = new Color32(255, 191, 0, 255);
                break;

            case "violet":
                newColour = new Color32(159, 0, 255, 255);
                break;

            default:
                newColour = Color.white;
                break;
        }
        UpdateSliderColour(newColour);
        CurrentColours.Clear();
        CurrentColours.Add(colourToMake.MyColour.ToLower());
        Debug.Log("New colour =" + newColour);
        return newColour;
    }

    /// <summary>
    /// Updates the colour of the slider in the UI.
    /// </summary>
    /// <param name="colour">Colour to change the slider to.</param>
    public void UpdateSliderColour(Color32 colour)
    {
        SliderFill.color = colour;
    }
}
