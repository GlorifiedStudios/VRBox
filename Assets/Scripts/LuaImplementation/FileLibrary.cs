using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

public class FileLibrary : MonoBehaviour
{
    public static DynValue FileExists( string path ) {
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        path = Path.Combine( dataPath, path );
        Debug.Log( path );

        if( path.Length < dataPath.Length || path.Substring( 0, dataPath.Length ) != dataPath ) {
            Debug.Log( "Path out of data folder!" );
            return DynValue.NewBoolean( false );
        }

        bool fileExists = false;
        if( Directory.Exists( path ) ) { fileExists = true; }
        if( File.Exists( path ) ) { fileExists = true; }
        return DynValue.NewBoolean( fileExists );
    }
}