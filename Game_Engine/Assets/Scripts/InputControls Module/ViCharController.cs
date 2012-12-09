using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Pipes;

public class ViCharController : MonoBehaviour
{
    public bool Moving
    {
        get
        {
            return (DateTime.Now - movingActivation) < TimeSpan.FromMilliseconds(movingDuration);
        }
    }

    public bool TurningLeft
    {
        get
        {
            return (DateTime.Now - turningLeftActivation) < TimeSpan.FromMilliseconds(turningLeftDuration);
        }
    }

    public bool TurningRight
    {
        get
        {
            return (DateTime.Now - turningRightActivation) < TimeSpan.FromMilliseconds(turningRightDuration);
        }
    }

    public bool Jumping
    {
        get
        {
            return (DateTime.Now - jumpingActivation) < TimeSpan.FromMilliseconds(jumpingDuration);
        }
    }

    public bool Attacking
    {
        get
        {
            return (DateTime.Now - attackActivation) < TimeSpan.FromMilliseconds(attackDuration);
        }
    }

    public bool Shielding
    {
        get
        {
            return (DateTime.Now - shieldActivation) < TimeSpan.FromMilliseconds(shieldDuration);
        }
    }

    private DateTime movingActivation;
    private int movingDuration;

    private DateTime turningLeftActivation;
    private int turningLeftDuration;

    private DateTime turningRightActivation;
    private int turningRightDuration;

    private DateTime jumpingActivation;
    private int jumpingDuration;

    private DateTime attackActivation;
    private int attackDuration;

    private DateTime shieldActivation;
    private int shieldDuration;

    private byte[] recvBuffer;
    private const int BufSize = 1;

    private const byte NONE = 0x00;
    private const byte MOVING = 0x01;
    private const byte TURNING_LEFT = 0x02;
    private const byte TURNING_RIGHT = 0x03;
    private const byte JUMPING = 0x04;
    private const byte VOICE_ATTACK = 0x05;
    private const byte VOICE_SHIELD = 0x06;

    NamedPipeClientStream clientPipe;

    void Start()
    {
        movingActivation = turningLeftActivation = turningRightActivation = jumpingActivation = attackActivation = shieldActivation = new DateTime(0);
        this.movingDuration = 500;
        this.turningLeftDuration = 125;
        this.turningRightDuration = 125;
        this.jumpingDuration = 500;
        this.attackDuration = 500;
        this.shieldDuration = 500;

        recvBuffer = new byte[BufSize];

        // Start our "thread" for listening for gestures
        StartCoroutine(waitForGestures());
    }

    void OnDestroy()
    {
        clientPipe.Close();
        clientPipe.Dispose();
    }

    private void DataReceived(IAsyncResult ar)
    {
        byte controllerEvent = recvBuffer[0];
        //string controllerEvent = Encoding.UTF8.GetString(recvBuffer, 0, BufSize);

        // Trim off the newline constant at the end
        //int lastChar = controllerEvent.IndexOf("\r\n");
        //controllerEvent = controllerEvent.Substring(0, lastChar);

        // Trim off any whitespace
        //controllerEvent = controllerEvent.Trim();

        //Debug.Log("Event captured: " + controllerEvent);

        // Figure out which controller event it is
        // Proposed changes:
        /*
         * Moving = 1
         * TurningLeft = 2
         * TurningRight = 3
         * Jumping = 4
         * VoiceAttack = 5
         * VoiceShield = 6
         */
        switch (controllerEvent)
        {
            case MOVING: movingActivation = DateTime.Now;
                break;
            case TURNING_LEFT: turningLeftActivation = DateTime.Now;
                break;
            case TURNING_RIGHT: turningRightActivation = DateTime.Now;
                break;
            case JUMPING: jumpingActivation = DateTime.Now;
                break;
            case VOICE_ATTACK: attackActivation = DateTime.Now;
                break;
            case VOICE_SHIELD: shieldActivation = DateTime.Now;
                break;
            case NONE:
                break;
            default: Debug.LogWarning("Unknown: " + controllerEvent);
                break;
        }

        recvBuffer = new byte[BufSize];

        // Read the next stream of data asynchronously
        clientPipe.BeginRead(recvBuffer, 0, BufSize, DataReceived, null);
    }

    private IEnumerator waitForGestures()
    {
        Debug.Log("Waiting for gestures...");

        // Create the client stream that listens to the pipe.
        clientPipe = new NamedPipeClientStream(".", "viCharControllerPipe", PipeDirection.In, PipeOptions.Asynchronous);

        while (true)
        {
            try
            {
                if (clientPipe.IsConnected == false)
                {
                    clientPipe.Connect();

                    // Read the next stream of data asynchronously via a callback
                    clientPipe.BeginRead(recvBuffer, 0, BufSize, DataReceived, null);

                    Debug.Log("Connected!");
                }
            }
            catch (Exception) { }

            // Very hacky. This will be changed soon...
            // TODO: need to figure out why Connect() returns but doesn't seem to actually connect (or disconnects a few milliseconds later).
            yield return new WaitForSeconds(1);
        }
    }
}