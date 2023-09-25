using UnityEngine;
using UnityEngine.UI;

public class TetrisMenuUI : MonoBehaviour
{
    [Header("Play & Pause")]
    public Text playAndPauseText;
    public Button playAndPauseButton;

    [Header("Help")]
    public Button helpButton;

    private void Start()
    {
        playAndPauseButton.onClick.AddListener(OnClickPlayAndPauseButton);
    }

    private void OnClickPlayAndPauseButton()
    {
        if(playAndPauseText.text.Equals("Play"))
        {
            GameManager.instance.PlayGame();

            playAndPauseText.text = "Pause";

            gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.PauseGame();

            playAndPauseText.text = "Play";
        }
    }
}
