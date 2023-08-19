using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [field: SerializeField] private string levelToLoad;
    [field: SerializeField] private InputField levelWidhtInput;
    [field: SerializeField] private InputField levelHeightInput;
    [field: SerializeField] private Slider gameSpeedSlider;
    [field: SerializeField] private Toggle obstaclesCheckBox;

    public void StartGame()
    {
        int width = 0, height = 0;
        if (!int.TryParse(levelWidhtInput.text, out width) || !int.TryParse(levelHeightInput.text, out height))
        {
            return;
        }
        if (width < 7)
        {
            width = 7;
        }
        if (height < 7)
        {
            height = 7;
        }
        GameData.Instance.AreaWidth = width;
        GameData.Instance.AreaHeight = height;
        GameData.Instance.GameSpeed = gameSpeedSlider.value;
        GameData.Instance.GenerateObstacles = obstaclesCheckBox.isOn;
        SceneManager.LoadScene(levelToLoad);
    }
}
