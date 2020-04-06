using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaTimerLibrary
{
    public void Begin( float seconds, Closure function ) {
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<LuaScriptLoader>().StartCoroutine( StartEnumerator( seconds, function ) );
    }

    [MoonSharpHidden] Dictionary<string, int> repeatingTimers = new Dictionary<string, int>();
    public void Repeating( string identifier, float seconds, int repetitions, Closure function ) {
        repeatingTimers[identifier] = repetitions;
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<LuaScriptLoader>().StartCoroutine( StartRepeatingEnumerator( identifier, seconds, function ) );
    }

    public void Remove( string identifier ) {
        repeatingTimers.Remove( identifier );
    }

    [MoonSharpHidden]
    private IEnumerator StartEnumerator( float seconds, Closure function ) {
        yield return new WaitForSeconds( seconds );
        Script luaScript = function.OwnerScript;
        luaScript.Call( function );
    }

    [MoonSharpHidden]
    private IEnumerator StartRepeatingEnumerator( string identifier, float seconds, Closure function ) {
        while( repeatingTimers.ContainsKey( identifier ) && repeatingTimers[identifier] > 0 ) {
            repeatingTimers[identifier] = repeatingTimers[identifier] - 1;
            if( repeatingTimers[identifier] <= 0 ) { repeatingTimers.Remove( identifier ); }
            yield return new WaitForSeconds( seconds );
            Script luaScript = function.OwnerScript;
            luaScript.Call( function );
        }
    }
}
