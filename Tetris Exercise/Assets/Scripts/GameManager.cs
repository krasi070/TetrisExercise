using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Used for singleton
    public static GameManager Instance;

    //Create Keycodes that will be associated with each of our commands.
    //These can be accessed by any other script in our game
    public KeyCode HardDrop { get; set; }

    public KeyCode RotateLeft { get; set; }

    public KeyCode RotateRight { get; set; }

    public KeyCode SoftDrop { get; set; }

    public KeyCode Left { get; set; }

    public KeyCode Right { get; set; }

    public KeyCode Hold { get; set; }



    void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        /*Assign each keycode when the game starts.
		 * Loads data from PlayerPrefs so if a user quits the game, 
		 * their bindings are loaded next time. Default values
		 * are assigned to each Keycode via the second parameter
		 * of the GetString() function
		 */
        HardDrop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HardDropKey", "Space"));
        RotateLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RotateLeftKey", "UpArrow"));
        RotateRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RotateRightKey", "Q"));
        SoftDrop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SoftDropKey", "DownArrow"));
        Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftKey", "LeftArrow"));
        Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightKey", "RightArrow"));
        Hold = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HoldKey", "RightShift"));
    }
}