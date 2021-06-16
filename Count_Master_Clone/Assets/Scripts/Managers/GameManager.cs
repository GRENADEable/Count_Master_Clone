using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameManagerData gmData;

    [Space, Header("Player")]
    public GameObject playerPrefab;
    public Transform playerPosParent;

    [Space, Header("UI")]
    public Animator fadeBG;
    public TextMeshProUGUI totalPlayerCountText;

    [Space, Header("Ground")]
    public GameObject ground;
    public float groundSpeed = 1f;

    [Space, Header("Panels")]
    public GameObject menuPanel;
    #endregion

    #region Private Variables
    private int _currTotalPlayerCount;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        GateTrigger.OnPlayerIncrement += OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement += OnPlayerDecrementEventReceived;
    }

    void OnDisable()
    {
        GateTrigger.OnPlayerIncrement -= OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement -= OnPlayerDecrementEventReceived;
    }

    void OnDestroy()
    {
        GateTrigger.OnPlayerIncrement -= OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement -= OnPlayerDecrementEventReceived;
    }
    #endregion

    void Start() => IntialiseGame();

    void Update()
    {
        if (gmData.currState == GameManagerData.GameState.Game)
            ground.transform.Translate(groundSpeed * Time.deltaTime * Vector3.back);

        if (Input.GetMouseButtonDown(0) && gmData.currState == GameManagerData.GameState.Menu)
            StartGame();
    }
    #endregion

    #region My Functions

    #region Cursor
    void EnableCursor()
    {
        gmData.VisibleCursor(true);
        gmData.LockCursor(false);
    }

    void DisableCursor()
    {
        gmData.VisibleCursor(false);
        gmData.LockCursor(true);
    }
    #endregion

    #region Buttons
    public void OnClick_ExitGame()
    {
        gmData.QuitGame();
    }
    #endregion

    #region Starting
    void IntialiseGame()
    {
        UpdateIncremenText(true, 1);
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Menu");
    }

    void StartGame()
    {
        gmData.ChangeState("Game");
        menuPanel.SetActive(false);
        DisableCursor();
    }
    #endregion

    #region Player
    void UpdateIncremenText(bool isIncreasing, int players)
    {
        if (isIncreasing)
        {
            _currTotalPlayerCount += players;
            totalPlayerCountText.text = $"{_currTotalPlayerCount}";
        }
        else
        {
            _currTotalPlayerCount -= players;
            totalPlayerCountText.text = $"{_currTotalPlayerCount}";
        }
    }
    #endregion

    #endregion

    #region Events
    void OnPlayerIncrementEventReceived(int count)
    {
        for (int i = 0; i < count; i++)
            Instantiate(playerPrefab, playerPosParent.position, Quaternion.identity, playerPosParent);

        UpdateIncremenText(true, count);
    }

    void OnPlayerDecrementEventReceived(int count)
    {
        UpdateIncremenText(false, count);
    }
    #endregion
}