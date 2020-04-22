using UnityEngine;
using Unity.Collections;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField = null;
    [ReadOnly] public bool consoleActive;
    public TMP_InputField consoleInput;
    private string consoleText = "";

    public void AddLineToConsole( string newText ) {
        if( inputField == null ) { return; }
        consoleText = consoleText + newText + "\n";
        inputField.text = consoleText;
    }

    public void ThrowError( string errorText ) { AddLineToConsole( "<color=red>" + errorText + "</color>" ); }
    public void ThrowWarning( string warningText ) { AddLineToConsole( "<color=orange>" + warningText + "</color>" ); }
    public void PrintToConsole( string printText ) { AddLineToConsole( "<color=white>" + printText + "</color>" ); }

    void OnGUI()
    {
        if( consoleInput.isFocused && consoleInput.text != "" && Input.GetKey( KeyCode.Return ) )
        {
            PrintToConsole( consoleInput.text );
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