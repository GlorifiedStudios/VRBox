using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaGlobalsLibrary : MonoBehaviour
{
    private static string[] unwriteableGlobals = new string[] {
        "PushGlobal",
        "GetGlobal",
        "fileExists"
    };

    public static Dictionary<string, DynValue> luaGlobals = new Dictionary<string, DynValue>();

    // Notice: PushGlobal does not support Lua functions & tables for some reason, will look into soon
    // Resources: https://forums.tabletopsimulator.com/showthread.php?4231-Passing-functions-between-scripts
    public static DynValue PushGlobal( string identifier, DynValue value ) {
        if( !IsWriteableGlobal( identifier ) ) { return DynValue.NewNil(); }
        luaGlobals[identifier] = value;
        return value;
    }

    public static DynValue GetGlobal( string identifier ) {
        return luaGlobals[identifier];
    }

    public static bool IsWriteableGlobal( string identifier ) {
        return unwriteableGlobals.Contains( identifier );
    }
}
