using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cube : MonoBehaviour
{
    [HideInInspector]
    public ColourMixer Mixer;
    [HideInInspector]
    public Color CubeColour;
    public int TotalMoves;
    private Slider MixerSlider;
    private bool Failed;
    private TouchGesture touch;
    private Animation CubeAnimation;

    // Use this for initialization
    void Start()
    {
        CubeAnimation = GetComponent<Animation>();
        Mixer = GameObject.Find("Slider").GetComponent<ColourMixer>();
        MixerSlider = GameObject.Find("Slider").GetComponent<Slider>();
        touch = GetComponent<TouchGesture>();
        StartCoroutine(touch.CheckHorizontalSwipes(
            onLeftSwipe: () => { MoveLeft(); },
            onRightSwipe: () => { MoveRight(); }
            ));
        StartCoroutine(touch.CheckVerticalSwipes(
            onUpSwipe: () => { MoveUp(); },
            onDownSwipe: () => { MoveDown(); }
            ));
    }

    // Update is called once per frame
    void Update()
    {
        if (TotalMoves == 0 && !Failed)
        {
            Failed = true;
            Failure();
        }

        CubeColour = GetComponent<Renderer>().material.color;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if (Input.GetKeyDown(KeyCode.W))
            MoveUp();
        if (Input.GetKeyDown(KeyCode.S))
            MoveDown();
        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
        transform.rotation = new Quaternion(0, 0, 0, 0);

        if (Mixer.HasMixed)
        {
            GetComponent<Renderer>().material.color = Mixer.Colour;
        }
    }
    // All cube movement code.
    #region Cube Movement
    /// <summary>
    /// Move the cube left.
    /// </summary>
    void MoveLeft()
    {
        transform.Translate(0, 0, -1);
        CubeAnimation.Play("CubeLeft");
        TotalMoves -= 1;
    }

    /// <summary>
    /// Move the cube right.
    /// </summary>
    void MoveRight()
    {
        transform.Translate(0, 0, 1);
        CubeAnimation.Play("CubeRight");
        TotalMoves -= 1;
    }

    /// <summary>
    /// Move the cube up.
    /// </summary>
    void MoveUp()
    {
        transform.Translate(-1, 0, 0);
        CubeAnimation.Play("CubeUp");
        TotalMoves -= 1;
    }

    /// <summary>
    /// Move the cube down.
    /// </summary>
    void MoveDown()
    {
        transform.Translate(1, 0, 0);
        CubeAnimation.Play("CubeDown");
        TotalMoves -= 1;
    }
    #endregion

    /// <summary>
    /// Handles when the main cube collides with a grid cube.
    /// </summary>
    /// <param name="collider">Collider name.</param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "GridCube")
        {
            GridCube gridCube = GameObject.Find(collider.gameObject.name).GetComponent<GridCube>();
            switch (gridCube.GridCubeType)
            {
                case GridCube.CubeType.Danger:
                    TotalMoves -= 1;
                    break;
                case GridCube.CubeType.Colour:
                    GetComponent<Renderer>().material.color = gridCube.Colour;
                    Mixer.CurrentColours.Add(gridCube.ColourName);
                    MixerSlider.value += gridCube.ColourValue;
                    break;
                case GridCube.CubeType.Finish:
                    if (CanFinish(gridCube.Colour))
                    {
                        CubeAnimation.Play("CubeShrink");
                        StartCoroutine(FinishAnimation(CubeAnimation["CubeShrink"].length));
                    }
                    break;
                case GridCube.CubeType.Normal:
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Checks if the level is completed.
    /// </summary>
    /// <param name="gridCubeColour">Colour of the finish cube.</param>
    /// <returns>True if the level is complete. False otherwise.</returns>
    private bool CanFinish(Color gridCubeColour)
    {
        return GetComponent<Renderer>().material.color == gridCubeColour;
    }

    /// <summary>
    /// Fails the level.
    /// </summary>
    private void Failure()
    {
        CubeAnimation.Play("CubeShrink");
        // Show failure screen.
    }

    IEnumerator FinishAnimation(float waitTime)
    {
       yield return new WaitForSeconds(waitTime);
        int sceneNumber = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNumber + 1);
    }
}   
    

