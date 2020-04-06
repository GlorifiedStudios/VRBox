using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaFileLibrary
{
    public DynValue Exists( string path ) {
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        path = Path.Combine( dataPath, path );

        if( path.Length < dataPath.Length || path.Substring( 0, dataPath.Length ) != dataPath ) {
            GameObject.FindGameObjectWithTag( "Player" ).GetComponent<ConsoleController>().AddLineToConsole(
                "<color=orange>FileExists path called out of data folder, cancelling.</color>"
            );
            return DynValue.NewBoolean( false );
        }

        bool fileExists = false;
        if( Directory.Exists( path ) ) { fileExists = true; }
        if( File.Exists( path ) ) { fileExists = true; }
        return DynValue.NewBoolean( fileExists );
    }
}