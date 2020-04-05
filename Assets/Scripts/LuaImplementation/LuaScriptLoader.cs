using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class LuaScriptLoader : MonoBehaviour
{
    private void AssignLuaGlobals( Script luaScript ) {
        luaScript.Globals["PushGlobal"] = (Func<string, DynValue, DynValue>)LuaGlobalsLibrary.PushGlobal;
        luaScript.Globals["GetGlobal"] = (Func<string, DynValue>)LuaGlobalsLibrary.GetGlobal;
        luaScript.Globals["FileExists"] = (Func<string, DynValue>)FileLibrary.FileExists;
        foreach( KeyValuePair<string, DynValue> globalString in LuaGlobalsLibrary.luaGlobals ) {
            luaScript.Globals[globalString.Key] = globalString.Value;
        }
    }

    void Start()
    {
        Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();

        string modulesPath = Path.Combine( Application.streamingAssetsPath, "modules" );
        string[] moduleFolders = Directory.GetDirectories( modulesPath, "*.*", SearchOption.AllDirectories );
        foreach( var folder in moduleFolders ) {
            string moduleFolder = Path.Combine( modulesPath, folder + "/lua" );
            string autorunFolder = Path.Combine( moduleFolder, "autorun" );
            if( Directory.Exists( autorunFolder ) ) {
                string[] autorunFiles = Directory.GetFiles( autorunFolder, "*.*", SearchOption.AllDirectories );
                foreach ( var file in autorunFiles ) {
                    if( file.Substring( Mathf.Max( 0, file.Length - 4 ) ) == ".lua" ) {
                        Script luaScript = new Script();
                        
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