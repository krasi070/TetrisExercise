using UnityEngine;

public class GameOptionsMenu : MonoBehaviour
{
	void Start()
    {
		
	}
	
	void Update()
    {
		
	}

    public void ToggleGrid()
    {
        GameObject grid = GameObject.Find("Grid");
        grid.SetActive(!grid.activeSelf);
    }

    public void ToggleHold()
    {
        Grid.holdingOn = !Grid.holdingOn;
    }

    public void ToggleGhost()
    {
        Grid.ghostOn = !Grid.ghostOn;
    }
}