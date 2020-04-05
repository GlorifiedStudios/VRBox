using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

public class FileLibrary : MonoBehaviour
{
    public static DynValue FileExists( string path ) {
        bool fileExists = false;
        if( Directory.Exists( path ) ) { fileExists = true; }
        if( File.Exists( path ) ) { fileExists = true; }
        return DynValue.NewBoolean( fileExists );
    }
}
