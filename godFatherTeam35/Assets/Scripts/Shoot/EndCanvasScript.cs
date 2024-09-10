using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCanvasScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _resultText;

    [Space(5)]
    [SerializeField] private string _firstScene;
    [Header("End Text")]
    [SerializeField] private string _wonText;
    [SerializeField] private string _loseText;

    public void RestartGame()
    {
        SceneManager.LoadScene(_firstScene);
    }

    private void OnEnable()
    {
        _resultText.text = GameManager.Instance.IsPlayerHit ? _wonText : _loseText;
    }
}
