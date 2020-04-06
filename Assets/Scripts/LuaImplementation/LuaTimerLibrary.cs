using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaTimerLibrary
{
    public void Begin( float seconds, Closure function ) {
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<LuaScriptLoader>().StartCoroutine( StartEnumerator( seconds, function ) );
    }

    [MoonSharpHidden]
    private IEnumerator StartEnumerator( float seconds, Closure function ) {
        yield return new WaitForSeconds( seconds );
        Script luaScript = function.OwnerScript;
        luaScript.Call( function );
    }
}
