using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Advertisements;

public class Cube : MonoBehaviour
{
    [HideInInspector]
    public ColourMixer Mixer;
    [HideInInspector]
    public Slider MixerSlider;
    public int TotalMoves;
    private bool Failed;
    private TouchGesture touch;
    private Animation CubeAnimation;
    private Quaternion resetRotation;

    [Header("Set these boxes to max bounds")]
    public float MaxX;
    public float MinX;
    public float MaxZ;
    public float MinZ;

    // Use this for initialization
    void Start()
    {
        resetRotation = new Quaternion(0, 0, 0, 0);
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

        transform.rotation = resetRotation;
        if (Input.GetKeyDown(KeyCode.W))
            MoveUp();
        if (Input.GetKeyDown(KeyCode.S))
            MoveDown();
        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
        transform.rotation = resetRotation;
    }
    // All cube movement code.
    #region Cube Movement
    /// <summary>
    /// Move the cube left.
    /// </summary>
    void MoveLeft()
    {
        Debug.Log(transform.position.z);
        if(transform.position.z == MinZ) return;
        transform.Translate(0, 0, -1);
        CubeAnimation.Play("CubeLeft");
        TotalMoves -= 1;
    }

    /// <summary>
    /// Move the cube right.
    /// </summary>
    void MoveRight()
    {
        Debug.Log(transform.position.z);
        if (transform.position.z == MaxZ) return;
        transform.Translate(0, 0, 1);
        CubeAnimation.Play("CubeRight");
        TotalMoves -= 1;
    }

    /// <summary>
    /// Move the cube up.
    /// </summary>
    void MoveUp()
    {
        Debug.Log(transform.position.x);
        if (transform.position.x == MaxX) return;
        transform.Translate(-1, 0, 0);
        CubeAnimation.Play("CubeUp");
        TotalMoves -= 1;
    }

    /// <summary>
    /// Move the cube down.
    /// </summary>
    void MoveDown()
    {
        Debug.Log(transform.position.x);
        if (transform.position.x == MinX) return;
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
                    if (!gridCube.ColourTaken)
                    {
                        UpdateColour(gridCube.Colour);
                        if (CheckForPreMix(gridCube))
                        {
                            Mixer.CurrentColours.Add(gridCube.ColourName);
                        }
                        Mixer.UpdateSliderColour(gridCube.Colour);
                        MixerSlider.value += 1;
                        gridCube.Disable();
                    }
                    break;
                case GridCube.CubeType.Finish:
                    if (CanFinish(gridCube.Colour))
                    {
                        CubeAnimation.Play("CubeShrink");
                        StartCoroutine(FinishAnimation(CubeAnimation["CubeShrink"].length));
                    }
                    break;
                case GridCube.CubeType.Switcher:
                    {
                        gridCube.Switch();
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
    private bool CanFinish(Color32 gridCubeColour)
    {
        Debug.Log(gridCubeColour);
        Debug.Log(GetComponent<Renderer>().material.color);
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

    /// <summary>
    /// Waits for the animation to finish before loading the next scene.
    /// </summary>
    /// <param name="waitTime">Time to wait.</param>
    /// <returns></returns>
    IEnumerator FinishAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int sceneNumber = SceneManager.GetActiveScene().buildIndex;
        ShowAd();
        SceneManager.LoadScene(sceneNumber + 1);
    }
   
    /// <summary>
    /// Changes the colour of the cube.
    /// </summary>
    /// <param name="colour">Colour to change to.</param>
    public void UpdateColour(Color32 colour)
    {
        Debug.Log("Colour changed to" + colour);
        GetComponent<Renderer>().material.color = colour;
    }

    public void ShowAd()
    {
        //if (Advertisement.IsReady())
        //{
        //    Advertisement.Show();
        //}

        // Disabled ads because they were pissing me off during tests.
    }

    /// <summary>
    /// Checks if you need multiple of the same colour before you can mix.
    /// </summary>
    /// <param name="gridCube">Grid cube.</param>
    /// <returns>True if the colour can be added to the mixer, False otherwise.</returns>
    private bool CheckForPreMix(GridCube gridCube)
    {
        bool addToMixer = true;

        if (gridCube.MoreToMix)
        {
            if (Mixer.PreMixer.ContainsKey(gridCube.ColourName))
            {
                int mixerAmount = Mixer.PreMixer[gridCube.ColourName];
                mixerAmount -= 1;
                if (mixerAmount <= 0)
                {
                    Mixer.PreMixer.Remove(gridCube.ColourName);
                    addToMixer = true;
                }
                else
                {
                    Mixer.PreMixer[gridCube.ColourName] = mixerAmount;
                    addToMixer = false;
                }
            }
            else
            {
                Mixer.PreMixer.Add(gridCube.ColourName, gridCube.AmountToMix - 1);
                addToMixer = false;
            }
        }
        return addToMixer;
    }
}  
    

