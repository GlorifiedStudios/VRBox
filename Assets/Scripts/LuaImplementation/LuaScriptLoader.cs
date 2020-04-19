using System;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class LuaScriptLoader : MonoBehaviour
{
    private void AssignLuaGlobals( Script luaScript ) {
	    UserData.RegisterAssembly();
        luaScript.Globals["File"] = new LuaFileLibrary();
        luaScript.Globals["Timer"] = new LuaTimerLibrary();
    }

    private GameObject playerObject;
    private void Start() {
        playerObject = GameObject.FindGameObjectWithTag( "Player" );
        Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();
        Script.DefaultOptions.DebugPrint = s => playerObject.GetComponent<ConsoleController>().AddLineToConsole(s, "white");
        LoadAutorunFiles();
    }

    public void ThrowExceptionToConsole( InterpreterException ex ) {
        string niceMessage = ex.DecoratedMessage;
        niceMessage = niceMessage.Replace( @"\", "/" );
        int modulesIndex = niceMessage.IndexOf( "modules" );
        niceMessage = niceMessage.Substring( modulesIndex, niceMessage.Length - modulesIndex );
        playerObject.GetComponent<ConsoleController>().AddLineToConsole(
            niceMessage,
            "red"
        );
    }

    private void LoadAutorunFiles() {
        string modulesPath = Path.Combine( Application.streamingAssetsPath, "modules" );
        foreach( var folder in Directory.GetDirectories( modulesPath, "*.*", SearchOption.AllDirectories ) ) {
            string moduleFolder = Path.Combine( modulesPath, folder + "/lua" );
            string autorunFolder = Path.Combine( moduleFolder, "autorun" );
            if( Directory.Exists( autorunFolder ) ) {
                foreach ( var file in Directory.GetFiles( autorunFolder, "*.*", SearchOption.AllDirectories ) ) {
                    if( Path.GetExtension( file ) == ".lua" ) {
                        try {
                            Script luaScript = new Script();
                            ((ScriptLoaderBase)luaScript.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths( moduleFolder + "/?;" + moduleFolder + "/?.lua" );
                            AssignLuaGlobals( luaScript );
                            DynValue luaOutput = luaScript.DoFile( file );
                        } catch( InterpreterException ex ) {
                            ThrowExceptionToConsole( ex );
                        }
                    }
                }
            }
        }
    }
}