using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    void Start()
    {
        Server.Start( 4, 34197 );
    }
}
