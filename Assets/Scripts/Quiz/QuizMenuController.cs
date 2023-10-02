using UnityEngine;

public class QuizMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _startGameMenu;
    [SerializeField] private GameObject _playingMenu;

    public void PlayGame()
    {
        _playingMenu.SetActive(true);
        _startGameMenu.SetActive(false);
    }
    public void GoToStarGameMenu()
    {

        _playingMenu.SetActive(false);
        _startGameMenu.SetActive(true);
    }
}
