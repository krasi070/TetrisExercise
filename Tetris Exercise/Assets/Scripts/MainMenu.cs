using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	void Start()
    {
		
	}
	
	void Update()
    {
		
	}

    public void Play()
    {
        SceneManager.LoadScene("Level");
    }

    public void Exit()
    {
        Application.Quit();
    }
}