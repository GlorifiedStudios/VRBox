using System;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class LuaScriptLoader : MonoBehaviour
{
    private void AssignLuaGlobals( Script luaScript ) {
        luaScript.Globals["FileExists"] = (Func<string, DynValue>)LuaFileLibrary.FileExists;
    }

    private void LoadAutorunFiles() {
        string modulesPath = Path.Combine( Application.streamingAssetsPath, "modules" );
        foreach( var folder in Directory.GetDirectories( modulesPath, "*.*", SearchOption.AllDirectories ) ) {
            string moduleFolder = Path.Combine( modulesPath, folder + "/lua" );
            string autorunFolder = Path.Combine( moduleFolder, "autorun" );
            if( Directory.Exists( autorunFolder ) ) {
                foreach ( var file in Directory.GetFiles( autorunFolder, "*.*", SearchOption.AllDirectories ) ) {
                    if( file.Substring( Mathf.Max( 0, file.Length - 4 ) ) == ".lua" ) {
                        try {
                            Script luaScript = new Script();
                            ((ScriptLoaderBase)luaScript.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths( moduleFolder + "/?;" + moduleFolder + "/?.lua" );
                            AssignLuaGlobals( luaScript );
                            DynValue luaOutput = luaScript.DoFile( file );
                        } catch( SyntaxErrorException ex ) {
                            string niceMessage = ex.DecoratedMessage;
                            niceMessage = niceMessage.Replace( @"\", "/" );
                            int modulesIndex = niceMessage.IndexOf( "modules" );
                            niceMessage = niceMessage.Substring( modulesIndex, modulesIndex );
                            niceMessage = niceMessage + " " + ex.Message;
                            GameObject.FindGameObjectWithTag( "Player" ).GetComponent<ConsoleController>().AddLineToConsole(
                                "<color=red>" + niceMessage + "</color>"
                            );
                        }
                    }
                }
            }
        }
    }

    void Start() {
        Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();
        Script.DefaultOptions.DebugPrint = s => GameObject.FindGameObjectWithTag( "Player" ).GetComponent<ConsoleController>().AddLineToConsole( "<color=white>" + s + "</color>" );
        LoadAutorunFiles();
    }
}