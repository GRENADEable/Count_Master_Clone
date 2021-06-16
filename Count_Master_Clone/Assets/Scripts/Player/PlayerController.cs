using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameManagerData gmData;

    [Space, Header("Player")]
    public float playerSpeed = 50f;
    public float playerSpeedClamp = 7f;
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
    #endregion

    #region My Functions
    void MovePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            float horizontal = Input.GetAxis("Mouse X") * playerSpeed;

            _playerRb.AddForce(horizontal * Time.deltaTime * Vector3.right, ForceMode.Impulse);
            _playerRb.velocity = Vector3.ClampMagnitude(_playerRb.velocity, playerSpeedClamp);
        }
    }
    #endregion
}