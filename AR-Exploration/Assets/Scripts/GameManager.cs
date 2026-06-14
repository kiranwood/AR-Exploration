using System;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _spawner;

    public bool IsPlaying {  get; private set; }

    private int score;

    private void Awake()
    {
        Instance = this;
        score = 0;
    }

    // Starts the game
    public void StartGame()
    {
        IsPlaying = true;
        _spawner.SetActive(true);
        _gameplayPanel.SetActive(true);
    }

    // Disables Spawner and 
    public void PauseGame()
    {
        IsPlaying = false;
        _spawner.SetActive(false);
        _gameplayPanel.SetActive(false);
    }

    // Adds a point in the score
    public void AddPoint()
    {
        if (!IsPlaying) return;

        score++;
        UpdateScoreUI();
    }

    // Updates the score UI
    private void UpdateScoreUI()
    {
        _scoreText.text = "Score: " + score.ToString("00");
    }
}
