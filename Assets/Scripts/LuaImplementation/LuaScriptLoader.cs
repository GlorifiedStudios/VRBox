using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class LuaScriptLoader : MonoBehaviour
{
    private static string[] unwriteableGlobals = new string[] {
        "PushGlobal",
        "GetGlobal"
    };

    private static Dictionary<string, DynValue> luaGlobals = new Dictionary<string, DynValue>();

    // Notice: PushGlobal does not support Lua functions & tables for some reason, will look into soon
    // Resources: https://forums.tabletopsimulator.com/showthread.php?4231-Passing-functions-between-scripts
    private static DynValue PushGlobal( string identifier, DynValue value ) {
        if( unwriteableGlobals.Contains( identifier ) ) { return DynValue.NewNil(); }
        luaGlobals[identifier] = value;
        return value;
    }

    private static DynValue GetGlobal( string identifier ) {
        return luaGlobals[identifier];
    }

    private void AssignLuaGlobals( Script luaScript ) {
        luaScript.Globals["PushGlobal"] = (Func<string, DynValue, DynValue>)PushGlobal;
        luaScript.Globals["GetGlobal"] = (Func<string, DynValue>)GetGlobal;
        foreach( KeyValuePair<string, DynValue> globalString in luaGlobals ) {
            luaScript.Globals[globalString.Key] = globalString.Value;
        }
    }

    void Start()
    {
        Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();

        string modulesPath = Path.Combine( Application.streamingAssetsPath, "modules" );
        string[] moduleFolders = Directory.GetDirectories( modulesPath, "*.*", SearchOption.AllDirectories );
        foreach( var folder in moduleFolders ) {
            string moduleFolder = Path.Combine( modulesPath, folder );
            string autorunFolder = Path.Combine( moduleFolder, "autorun" );
            if( Directory.Exists( autorunFolder ) ) {
                string[] autorunFiles = Directory.GetFiles( autorunFolder, "*.*", SearchOption.AllDirectories );
                foreach ( var file in autorunFiles ) {
                    if( file.Substring( Mathf.Max( 0, file.Length - 4 ) ) == ".lua" ) {
                        Script luaScript = new Script();
                        Debug.Log(moduleFolder);
                        ((ScriptLoaderBase)luaScript.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths( moduleFolder + "/?;" + moduleFolder + "/?.lua" );
                        AssignLuaGlobals( luaScript );
                        DynValue luaOutput = luaScript.DoFile( file );
                        Debug.Log( luaOutput );
                    }
                }
            }
        }
    }
}