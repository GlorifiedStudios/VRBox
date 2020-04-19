using UnityEngine;
using Unity.Collections;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField = null;
    [ReadOnly] public bool consoleActive;
    public TMP_InputField consoleInput;
    private string consoleText = "";

    public void AddLineToConsole(string newText, string colorName) {
        if( inputField == null ) { return; }
        consoleText = "<color=" + colorName + ">" + consoleText + newText + "</color>\n";
        inputField.text = consoleText;
    }

    void OnGUI()
    {
        if (consoleInput.isFocused && consoleInput.text != "" && Input.GetKey(KeyCode.Return))
        {
            AddLineToConsole(consoleInput.text, "white");
            consoleInput.text = "";
        }
    }

    void Update() {
        consoleActive = inputField.gameObject.activeSelf;
        if( Input.GetButtonDown( "OpenConsole" ) ) {
            inputField.gameObject.SetActive( !consoleActive );
        }

        if( consoleActive ) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}