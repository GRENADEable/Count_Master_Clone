using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameManagerData gmData;

    [Space, Header("Player")]
    public float playerInputSpeed = 60f;
    public float playerSpeedClamp = 7f;

    public delegate void SendEvents();
    public static event SendEvents OnPlayerDead;
    public static event SendEvents OnLevelEndTrigger;
    public static event SendEvents OnLevelEndCount;
    #endregion

    #region Private Variables
    private Rigidbody _playerRb;
    #endregion

    #region Unity Callbacks
    void Start() => _playerRb = GetComponent<Rigidbody>();

    void Update()
    {
        if (gmData.currState == GameManagerData.GameState.Game)
            MovePlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            OnPlayerDead?.Invoke();
            Destroy(gameObject);
        }

        if (other.CompareTag("End_Trigger"))
        {
            other.enabled = false;
            OnLevelEndTrigger?.Invoke(); // Event sent to GameManager Script;
        }

        if (other.CompareTag("End"))
        {
            OnLevelEndCount?.Invoke(); // Event sent to GameManager Script;
            Destroy(gameObject);
        }
    }
    #endregion

    #region My Functions
    void MovePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            float horizontal = Input.GetAxis("Mouse X") * playerInputSpeed;

            _playerRb.AddForce(horizontal * Time.deltaTime * Vector3.right, ForceMode.Impulse);
            _playerRb.velocity = Vector3.ClampMagnitude(_playerRb.velocity, playerSpeedClamp);
        }
    }
    #endregion
}