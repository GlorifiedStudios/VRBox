using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaTimerLibrary
{
    public void Begin( float seconds, Closure function ) {
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<LuaScriptLoader>().StartCoroutine( StartEnumerator( seconds, function ) );
    }

    public void Repeating( float seconds, Closure function ) {
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<LuaScriptLoader>().StartCoroutine( StartRepeatingEnumerator( seconds, function ) );
    }

    [MoonSharpHidden]
    private IEnumerator StartEnumerator( float seconds, Closure function ) {
        yield return new WaitForSeconds( seconds );
        Script luaScript = function.OwnerScript;
        luaScript.Call( function );
    }

    [MoonSharpHidden]
    private IEnumerator StartRepeatingEnumerator( float seconds, Closure function ) {
        while( true ) {
            yield return new WaitForSeconds( seconds );
            Script luaScript = function.OwnerScript;
            luaScript.Call( function );
        }
    }
}
