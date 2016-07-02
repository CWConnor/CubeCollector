using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private Cube MainCube;
    private Text MovesLeft;
    // Use this for initialization
    void Start()
    {
        MainCube = GameObject.Find("MainCube").GetComponent<Cube>();
        MovesLeft = GetComponent<Text>();
        MovesLeft.text = MainCube.TotalMoves.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        MovesLeft.text = MainCube.TotalMoves.ToString();
    }
}
