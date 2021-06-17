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

    [Space, Header("Camera")]
    public GameObject cam1;
    public GameObject cam2;

    [Space, Header("UI")]
    public Animator fadeBG;
    public Animator fadeFastBG;
    public TextMeshProUGUI totalPlayerCountText;
    public TextMeshProUGUI totalScoreText;

    [Space, Header("Ground")]
    public GameObject ground;
    public float groundSpeed = 1f;

    [Space, Header("Panels")]
    public GameObject menuPanel;
    public GameObject deathPanel;
    public GameObject winPanel;

    [Space, Header("Obstacles")]
    public Animator[] movingSpikes;
    #endregion

    #region Private Variables
    private int _currTotalPlayerCount;
    private int _currScore;
    [SerializeField] private int _currLevel;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        GateTrigger.OnPlayerIncrement += OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement += OnPlayerDecrementEventReceived;

        PlayerController.OnLevelEndTrigger += OnLevelEndTriggerEventReceived;
        PlayerController.OnPlayerDead += OnPlayerDeadEventReceived;
        PlayerController.OnLevelEndCount += OnLevelEndCountEventRecieved;
    }

    void OnDisable()
    {
        GateTrigger.OnPlayerIncrement -= OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement -= OnPlayerDecrementEventReceived;

        PlayerController.OnLevelEndTrigger -= OnLevelEndTriggerEventReceived;
        PlayerController.OnPlayerDead -= OnPlayerDeadEventReceived;
        PlayerController.OnLevelEndCount -= OnLevelEndCountEventRecieved;
    }

    void OnDestroy()
    {
        GateTrigger.OnPlayerIncrement -= OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement -= OnPlayerDecrementEventReceived;

        PlayerController.OnLevelEndTrigger -= OnLevelEndTriggerEventReceived;
        PlayerController.OnPlayerDead -= OnPlayerDeadEventReceived;
        PlayerController.OnLevelEndCount -= OnLevelEndCountEventRecieved;
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

    #region Starting
    void IntialiseGame()
    {
        UpdateIncremenText(true, 1);
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Menu");
    }

    void StartGame()
    {
        totalPlayerCountText.gameObject.SetActive(true);
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

        if (_currTotalPlayerCount <= 0)
        {
            gmData.ChangeState("Dead");
            EnableCursor();
            deathPanel.SetActive(true);
        }
    }
    #endregion

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
    public void OnClick_NextLevel()
    {
        _currLevel++;
        StartCoroutine(StartNextLevelDelay());
    }

    public void OnClick_RestartGame() => StartCoroutine(RestartGameDelay());

    public void OnClick_RestartLevel() => StartCoroutine(RestartLevelDelay());

    public void OnClick_ExitGame() => StartCoroutine(ExitDelay());
    #endregion

    #endregion

    #region Events

    #region Player
    void OnPlayerIncrementEventReceived(int count)
    {
        for (int i = 0; i < count; i++)
            Instantiate(playerPrefab, playerPosParent.position, Quaternion.identity, playerPosParent);

        UpdateIncremenText(true, count);
    }

    void OnPlayerDecrementEventReceived(int count) => UpdateIncremenText(false, count);
    void OnPlayerDeadEventReceived() => UpdateIncremenText(false, 1);
    #endregion

    #region Level
    void OnLevelEndTriggerEventReceived() => StartCoroutine(LevelEndDelay());

    void OnLevelEndCountEventRecieved()
    {
        winPanel.SetActive(true);
        _currScore++;
        totalScoreText.text = $"{_currScore}";
        EnableCursor();
    }
    #endregion

    #region Level Triggers
    public void OnMovingSpikesEventReceived()
    {
        for (int i = 0; i < movingSpikes.Length; i++)
            movingSpikes[i].Play("SpikeAnim");
    }
    #endregion

    #endregion

    #region Coroutines
    IEnumerator RestartLevelDelay()
    {
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1);
        gmData.NextLevel(_currLevel);
    }

    IEnumerator RestartGameDelay()
    {
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1);
        gmData.RestartGame();
    }

    IEnumerator ExitDelay()
    {
        gmData.ChangeState("End");
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1);
        gmData.QuitGame();
    }

    IEnumerator LevelEndDelay()
    {
        fadeFastBG.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        totalPlayerCountText.gameObject.SetActive(false);
        cam1.SetActive(false);
        cam2.SetActive(true);
        fadeFastBG.Play("FadeIn");
    }

    IEnumerator StartNextLevelDelay()
    {
        fadeFastBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.NextLevel(_currLevel);
    }
    #endregion
}