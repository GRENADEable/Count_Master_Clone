using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerData", menuName = "Manager/GameManagerData")]
public class GameManagerData : ScriptableObject
{
    #region Public Variables
    [Space, Header("Enums")]
    public GameState currState = GameState.Game;
    public enum GameState { Menu, Game, Dead, Paused, End };
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks
    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region My Functions

    #region Scenes
    public void NextLevel(int index) => Application.LoadLevel($"Lvl_{index}");

    public void RestartGame() => Application.LoadLevel("Lvl_1");

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }
    #endregion

    #region Game States

    #region Cursor
    public void LockCursor(bool isLocked)
    {
        if (isLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void VisibleCursor(bool isVisible)
    {
        if (isVisible)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }
    #endregion

    #region Game
    public void TogglePause(bool isPaused)
    {
        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void ChangeState(string state)
    {
        if (state.Contains("Menu"))
            currState = GameState.Menu;

        if (state.Contains("Game"))
            currState = GameState.Game;

        if (state.Contains("Dead"))
            currState = GameState.Dead;

        if (state.Contains("Paused"))
            currState = GameState.Paused;

        if (state.Contains("End"))
            currState = GameState.End;

    }
    #endregion

    #endregion

    #endregion
}