using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaTimerLibrary
{
    private IEnumerator Start( float seconds, DynValue function, Script luaScript ) {
        yield return new WaitForSeconds( seconds );
        luaScript.Call( function );
    }
}
