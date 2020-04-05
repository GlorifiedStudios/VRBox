using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

public class IncludeLuaFiles : MonoBehaviour
{
    void Start()
    {
        string modulePath = Path.Combine( Application.streamingAssetsPath, "Modules" );
        string[] allModules = Directory.GetFiles( modulePath, "*.*", SearchOption.AllDirectories );
        foreach ( var file in allModules ){
            if( file.Substring( Mathf.Max( 0, file.Length - 4 ) ) == ".lua" ) {
                DynValue luaOutput = Script.RunString( File.ReadAllText( file ) );
                Debug.Log( luaOutput );
            }
        }
    }
}