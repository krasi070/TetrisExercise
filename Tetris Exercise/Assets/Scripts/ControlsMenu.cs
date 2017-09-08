using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    void Start()
    {
        //Assign menuPanel to the Panel object in our Canvas
        //Make sure it's not active when the game starts
        //buttons.gameObject.SetActive(false);
        waitingForKey = false;

        /*iterate through each child of the panel and check
		 * the names of each one. Each if statement will
		 * set each button's text component to display
		 * the name of the key that is associated
		 * with each command. Example: the ForwardKey
		 * button will display "W" in the middle of it
		 */
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Right")
                transform.GetChild(i).Find("RightButton").GetComponentInChildren<Text>().text = GameManager.Instance.Right.ToString();
            else if (transform.GetChild(i).name == "Left")
                transform.GetChild(i).Find("LeftButton").GetComponentInChildren<Text>().text = GameManager.Instance.Left.ToString();
            else if (transform.GetChild(i).name == "RotateRight")
                transform.GetChild(i).Find("RotateRightButton").GetComponentInChildren<Text>().text = GameManager.Instance.RotateRight.ToString();
            else if (transform.GetChild(i).name == "RotateLeft")
                transform.GetChild(i).Find("RotateLeftButton").GetComponentInChildren<Text>().text = GameManager.Instance.RotateLeft.ToString();
            else if (transform.GetChild(i).name == "SoftDrop")
                transform.GetChild(i).Find("SoftDropButton").GetComponentInChildren<Text>().text = GameManager.Instance.SoftDrop.ToString();
            else if (transform.GetChild(i).name == "HardDrop")
                transform.GetChild(i).Find("HardDropButton").GetComponentInChildren<Text>().text = GameManager.Instance.HardDrop.ToString();
            else if (transform.GetChild(i).name == "Hold")
                transform.GetChild(i).Find("HoldButton").GetComponentInChildren<Text>().text = GameManager.Instance.Hold.ToString();
        }
    }


    void Update()
    {
        //Escape key will open or close the panel
        //if (Input.GetKeyDown(KeyCode.Escape) && !buttons.gameObject.activeSelf)
        //    buttons.gameObject.SetActive(true);
        //else if (Input.GetKeyDown(KeyCode.Escape) && buttons.gameObject.activeSelf)
        //    buttons.gameObject.SetActive(false);
    }

    void OnGUI()
    {
        /*keyEvent dictates what key our user presses
		 * bt using Event.current to detect the current
		 * event
		 */
        keyEvent = Event.current;

        //Executes if a button gets pressed and
        //the user presses a key
        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
            waitingForKey = false;
        }
    }

    /*Buttons cannot call on Coroutines via OnClick().
	 * Instead, we have it call StartAssignment, which will
	 * call a coroutine in this script instead, only if we
	 * are not already waiting for a key to be pressed.
	 */
    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    //Assigns buttonText to the text component of
    //the button that was pressed
    public void SendText(Text text)
    {
        buttonText = text;
    }

    //Used for controlling the flow of our below Coroutine
    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    /*AssignKey takes a keyName as a parameter. The
	 * keyName is checked in a switch statement. Each
	 * case assigns the command that keyName represents
	 * to the new key that the user presses, which is grabbed
	 * in the OnGUI() function, above.
	 */
    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey(); //Executes endlessly until user presses a key

        switch (keyName)
        {
            case "Right":
                GameManager.Instance.Right = newKey; //Set forward to new keycode
                buttonText.text = GameManager.Instance.Right.ToString(); //Set button text to new key
                PlayerPrefs.SetString("RightKey", GameManager.Instance.Right.ToString()); //save new key to PlayerPrefs
                break;
            case "Left":
                GameManager.Instance.Left = newKey; //set backward to new keycode
                buttonText.text = GameManager.Instance.Left.ToString(); //set button text to new key
                PlayerPrefs.SetString("LeftKey", GameManager.Instance.Left.ToString()); //save new key to PlayerPrefs
                break;
            case "RotateRight":
                GameManager.Instance.RotateRight = newKey; //set left to new keycode
                buttonText.text = GameManager.Instance.RotateRight.ToString(); //set button text to new key
                PlayerPrefs.SetString("RotateRightKey", GameManager.Instance.RotateRight.ToString()); //save new key to playerprefs
                break;
            case "RotateLeft":
                GameManager.Instance.RotateLeft = newKey; //set right to new keycode
                buttonText.text = GameManager.Instance.RotateLeft.ToString(); //set button text to new key
                PlayerPrefs.SetString("RotateLeftKey", GameManager.Instance.RotateLeft.ToString()); //save new key to playerprefs
                break;
            case "SoftDrop":
                GameManager.Instance.SoftDrop = newKey; //set jump to new keycode
                buttonText.text = GameManager.Instance.SoftDrop.ToString(); //set button text to new key
                PlayerPrefs.SetString("SoftDropKey", GameManager.Instance.SoftDrop.ToString()); //save new key to playerprefs
                break;
            case "HardDrop":
                GameManager.Instance.HardDrop = newKey; //set jump to new keycode
                buttonText.text = GameManager.Instance.HardDrop.ToString(); //set button text to new key
                PlayerPrefs.SetString("HardDropKey", GameManager.Instance.HardDrop.ToString()); //save new key to playerprefs
                break;
            case "Hold":
                GameManager.Instance.Hold = newKey; //set jump to new keycode
                buttonText.text = GameManager.Instance.Hold.ToString(); //set button text to new key
                PlayerPrefs.SetString("HoldKey", GameManager.Instance.Hold.ToString()); //save new key to playerprefs
                break;
        }

        yield return null;
    }
}