using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [field: SerializeField] public string LevelToLoad { get; set; }
    [field: SerializeField] public InputField LevelWidhtInput { get; set; }
    [field: SerializeField] public InputField LevelHeightInput { get; set; }

    public void StartGame()
    {
        int width = 0, height = 0;
        int.TryParse(LevelWidhtInput.text, out width);
        int.TryParse(LevelHeightInput.text, out height);
        GameData.Instance.AreaWidth= width;
        GameData.Instance.AreaHeight= height;
        SceneManager.LoadScene(LevelToLoad);
    }
}
