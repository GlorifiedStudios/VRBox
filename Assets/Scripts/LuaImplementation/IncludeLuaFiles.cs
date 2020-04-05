using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

public class IncludeLuaFiles : MonoBehaviour
{
    private static Dictionary<string, string> luaGlobalStrings = new Dictionary<string, string>();

    private static string PushGlobalString( string identifier, string value ) {
        luaGlobalStrings[identifier] = value;
        return value;
    }

    private void AssignLuaGlobals( Script luaScript ) {
        luaScript.Globals["PushGlobalString"] = (Func<string, string, string>)PushGlobalString;
        foreach( KeyValuePair<string, string> globalString in luaGlobalStrings ) {
            luaScript.Globals[globalString.Key] = globalString.Value;
        }
    }

    void Start()
    {
        string modulePath = Path.Combine( Application.streamingAssetsPath, "Modules" );
        string[] allModules = Directory.GetFiles( modulePath, "*.*", SearchOption.AllDirectories );
        foreach ( var file in allModules ){
            if( file.Substring( Mathf.Max( 0, file.Length - 4 ) ) == ".lua" ) {
                Script luaScript = new Script();
                AssignLuaGlobals( luaScript );
                DynValue luaOutput = luaScript.DoString( File.ReadAllText( file ) );
                Debug.Log( luaOutput );
            }
        }
    }
}