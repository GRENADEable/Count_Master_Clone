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
    public Animator fastFadeBG;
    public TextMeshProUGUI totalPlayerCountText;
    public TextMeshProUGUI totalScoreText;

    [Space, Header("Ground")]
    public GameObject ground;
    public float groundSpeed = 1f;

    [Space, Header("Panels")]
    public GameObject menuPanel;
    public GameObject deathPanel;
    public GameObject winPanel;
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
        PlayerController.OnLevelEndCount += OnLevelEndCountEventRecieved;
    }

    void OnDisable()
    {
        GateTrigger.OnPlayerIncrement -= OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement -= OnPlayerDecrementEventReceived;

        PlayerController.OnLevelEndTrigger -= OnLevelEndTriggerEventReceived;
        PlayerController.OnLevelEndCount -= OnLevelEndCountEventRecieved;
    }

    void OnDestroy()
    {
        GateTrigger.OnPlayerIncrement -= OnPlayerIncrementEventReceived;
        FightTrigger.OnPlayerDecrement -= OnPlayerDecrementEventReceived;

        PlayerController.OnLevelEndTrigger -= OnLevelEndTriggerEventReceived;
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
    public void OnClick_RestartGame() => StartCoroutine(RestartDelay());

    public void OnClick_ExitGame() => StartCoroutine(ExitDelay());
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

        if (_currTotalPlayerCount <= 0)
        {
            gmData.ChangeState("Dead");
            EnableCursor();
            deathPanel.SetActive(true);
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

    void OnPlayerDecrementEventReceived(int count) => UpdateIncremenText(false, count);

    void OnLevelEndTriggerEventReceived() => StartCoroutine(LevelEndDelay());

    void OnLevelEndCountEventRecieved()
    {
        winPanel.SetActive(true);
        _currScore++;
        totalScoreText.text = $"{_currScore}";
        EnableCursor();
    }
    #endregion

    #region Coroutines
    IEnumerator RestartDelay()
    {
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1);
        gmData.NextLevel(_currLevel);
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
        fastFadeBG.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        totalPlayerCountText.gameObject.SetActive(false);
        cam1.SetActive(false);
        cam2.SetActive(true);
        fastFadeBG.Play("FadeIn");
    }
    #endregion
}