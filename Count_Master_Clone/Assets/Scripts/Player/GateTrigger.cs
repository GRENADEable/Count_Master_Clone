using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateTrigger : MonoBehaviour
{
    #region Public Variables
    public int playerIncrement;
    public Collider[] col;

    [Space, Header("Audios")]
    public AudioSource incrementAud;
    public AudioClip incrementSFX;

    public delegate void SendEventsInt(int increment);
    public static event SendEventsInt OnPlayerIncrement;
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
            OnPlayerIncrement?.Invoke(playerIncrement); // Evvent sent to GameManager Script;
            incrementAud.PlayOneShot(incrementSFX);
        }
    }
    #endregion

    #region My Functions
    void DisableColliders()
    {
        for (int i = 0; i < col.Length; i++)
            col[i].enabled = false;
    }
    #endregion
}