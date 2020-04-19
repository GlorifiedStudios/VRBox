using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class Client
{
    public static int dataBufferSize = 4096;

    public int id;
    public TCP tcp;

    public Client( int clientId ) {
        id = clientId;
        tcp = new TCP( clientId );
    }

    public class TCP {
        public TcpClient socket;
        private readonly int id;
        private NetworkStream stream;
        private byte[] receiveBuffer;

        public TCP( int suppliedId ) {
            id = suppliedId;
        }

        public void Connect( TcpClient suppliedSocket ) {
            socket = suppliedSocket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();
            receiveBuffer = new byte[dataBufferSize];
            stream.BeginRead( receiveBuffer, 0, dataBufferSize, ReceiveCallback, null );
        }

        private void ReceiveCallback( IAsyncResult result ) {
            try {
                int byteLength = stream.EndRead( result );
                if( byteLength <= 0 ) {
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy( receiveBuffer, data, byteLength );
                stream.BeginRead( receiveBuffer, 0, dataBufferSize, ReceiveCallback, null );
            } catch( Exception ex ) {
                Debug.Log( $"Error receiving TCP data: {ex}" );
            }
        }
    }
}
