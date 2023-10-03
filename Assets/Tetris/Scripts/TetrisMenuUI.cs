using UnityEngine;
using UnityEngine.UI;

public class TetrisMenuUI : MonoBehaviour
{
    [Header("Start")]
    public Button startButton;

    [Header("Play")]
    public Button playButton;

    [Header("Exit")]
    public Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        playButton.onClick.AddListener(PlayGame);
        exitButton.onClick.AddListener(QuitApplication);
    }

    private void StartGame()
    {
        GameManager.instance.StartGame();

        startButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);

        PlayGame();
    }

    private void PlayGame()
    {
        GameManager.instance.PlayGame();

        gameObject.SetActive(false);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
