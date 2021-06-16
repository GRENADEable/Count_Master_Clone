using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightTrigger : MonoBehaviour
{
    #region Public Variables
    public int playerDecrement;

    public delegate void SendEventsInt(int decrement);
    public static event SendEventsInt OnPlayerDecrement;
    #endregion

    #region Private Variables
    private TextMeshProUGUI _gateCounterText;
    private int _currNumberLeft;
    private Collider _col;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _gateCounterText = GetComponentInChildren<TextMeshProUGUI>();
        _currNumberLeft = playerDecrement;
        _gateCounterText.text = $"{_currNumberLeft}";
        _col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            OnPlayerDecrement?.Invoke(1); // Event sent to GameManager Script;
            _currNumberLeft--;
            _gateCounterText.text = $"{_currNumberLeft}";

            if (_currNumberLeft <= 0)
                _col.enabled = false;
        }
    }
    #endregion

    #region My Functions

    #endregion
}