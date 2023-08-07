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
        width = int.Parse(LevelWidhtInput.text);
        height = int.Parse(LevelHeightInput.text);
        if (width < 7) width = 7;
        if (height < 7) height = 7;
        GameData.Instance.AreaWidth = width;
        GameData.Instance.AreaHeight = height;
        SceneManager.LoadScene(LevelToLoad);
    }
}
