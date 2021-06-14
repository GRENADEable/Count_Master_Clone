using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameManagerData gmData;

    [Space, Header("Ground")]
    public GameObject ground;
    public float groundSpeed = 1f;

    [Space, Header("Panels")]
    public GameObject menuPanel;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks
    void Start()
    {
        gmData.ChangeState("Menu");
    }

    void Update()
    {
        if (gmData.currState == GameManagerData.GameState.Game)
            ground.transform.Translate(Vector3.back * groundSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && gmData.currState == GameManagerData.GameState.Menu)
            IntialiseGame();
    }
    #endregion

    #region My Functions

    #region Buttons
    public void OnClick_ExitGame()
    {
        gmData.QuitGame();
    }

    void IntialiseGame()
    {
        gmData.ChangeState("Game");
        menuPanel.SetActive(false);
    }
    #endregion

    #endregion
}