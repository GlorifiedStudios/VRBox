using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class FileLibrary : MonoBehaviour
{
    public static DynValue FileExists( string path ) {
        bool fileExists = false;
        if( Directory.Exists( path ) ) { fileExists = true; }
        if( File.Exists( path ) ) { fileExists = true; }
        return DynValue.NewBoolean( fileExists );
    }
}
