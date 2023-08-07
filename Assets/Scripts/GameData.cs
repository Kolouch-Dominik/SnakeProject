using UnityEngine;

public class GameData : MonoBehaviour
{
   
    public static GameData Instance;
    public int AreaWidth { get; set; }
    public int AreaHeight { get; set; }
    public float GameSpeed { get; set; } 
    private void Awake()
    {
        Instance= this;
        DontDestroyOnLoad(gameObject);
    }
}
