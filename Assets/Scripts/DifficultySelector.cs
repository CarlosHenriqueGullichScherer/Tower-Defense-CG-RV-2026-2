
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public void SelectEasy()
    {
        GameSettings.Difficulty = Difficulty.Easy;
        SceneManager.LoadScene("LevelEasy");
    }

    public void SelectHard()
    {
        GameSettings.Difficulty = Difficulty.Hard;
        SceneManager.LoadScene("LevelHard");
    }
}
