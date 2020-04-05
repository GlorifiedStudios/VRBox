using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class NativeFiles : MonoBehaviour
{
    void Start()
    {
        string filePath = System.IO.Path.Combine( Application.streamingAssetsPath, "Modules" );
        filePath = System.IO.Path.Combine( filePath, "Native" );
        filePath = System.IO.Path.Combine( filePath, "Native.lua" );

        DynValue luaOutput = Script.RunString( System.IO.File.ReadAllText( filePath ) );
        Debug.Log( luaOutput );
    }
}
