using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateTrigger : MonoBehaviour
{
    #region Public Variables
    public int playerIncrement;
    public Collider[] _col;

    public delegate void SendEventsInt(int increment);
    public static event SendEventsInt OnPlayerTrigger;
    #endregion

    #region Private Variables
    private TextMeshProUGUI _gateCounterText;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _gateCounterText = GetComponentInChildren<TextMeshProUGUI>();
        _gateCounterText.text = $"{playerIncrement}";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableColliders();
            OnPlayerTrigger?.Invoke(playerIncrement); // Evvent sent to GameManager Script;
        }
    }
    #endregion

    #region My Functions
    void DisableColliders()
    {
        for (int i = 0; i < _col.Length; i++)
            _col[i].enabled = false;
    }
    #endregion
}