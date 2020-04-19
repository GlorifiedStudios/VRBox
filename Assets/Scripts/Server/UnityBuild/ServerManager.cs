using UnityEngine;

public class ServerManager : MonoBehaviour
{
    void Start()
    {
        Server.Start( 4, 34197 );
    }
}
