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
    [SerializeField] private string _placementScene;
    [SerializeField] private string _menuScene;
    [SerializeField] List<AudioClip> _crowdCheerSound;

    [Header("End Text")]
    [SerializeField] private string _wonText;
    [SerializeField] private string _loseText;

    public void RestartGame()
    {
        GameManager.Instance.ResetValues();
        SceneManager.LoadScene(_placementScene);
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.ResetValues();
        SceneManager.LoadScene(_menuScene);
    }

    private void OnEnable()
    {
        _resultText.text = GameManager.Instance.IsPlayerHit ? _wonText : _loseText;
        Cursor.visible = true;

        SoundManager.instance.SpawnRandomSound(_crowdCheerSound, Vector3.zero);
    }
}
