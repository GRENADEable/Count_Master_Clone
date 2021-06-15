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
    public GameObject player;
    public float playerSpeed = 1f;
    public float playerSpeedClamp = 2f;
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
    private List<Rigidbody> _playerRb = new List<Rigidbody>();
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        GateTrigger.OnPlayerTrigger += OnPlayerTriggerEventReceived;
    }

    void OnDisable()
    {
        GateTrigger.OnPlayerTrigger -= OnPlayerTriggerEventReceived;
    }

    void OnDestroy()
    {
        GateTrigger.OnPlayerTrigger -= OnPlayerTriggerEventReceived;
    }
    #endregion

    void Start() => IntialiseGame();

    void Update()
    {
        if (gmData.currState == GameManagerData.GameState.Game)
        {
            MovePlayer();
            ground.transform.Translate(Vector3.back * groundSpeed * Time.deltaTime);
        }

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

    void IntialiseGame()
    {
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Menu");
        _playerRb.Add(player.GetComponent<Rigidbody>());
    }

    void StartGame()
    {
        gmData.ChangeState("Game");
        menuPanel.SetActive(false);
        DisableCursor();
    }

    void MovePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            float horizontal = Input.GetAxis("Mouse X") * playerSpeed;

            for (int i = 0; i < _playerRb.Count; i++)
            {
                _playerRb[i].AddForce(Vector3.right * horizontal * Time.deltaTime, ForceMode.Impulse);

                _playerRb[i].velocity = Vector3.ClampMagnitude(_playerRb[i].velocity, playerSpeedClamp);
            }

        }
    }

    #endregion

    #region Events
    void OnPlayerTriggerEventReceived(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, player.transform.position, Quaternion.identity, playerPosParent);
            _playerRb.Add(playerObj.GetComponent<Rigidbody>());
        }
    }
    #endregion
}