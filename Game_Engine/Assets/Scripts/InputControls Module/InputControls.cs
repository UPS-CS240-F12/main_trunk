using UnityEngine;
using System.Collections;
using Controller_Core.Client;
using System.Diagnostics;

public class InputControls
{
    public static float Rotation()
    {
        return Input.GetAxis("Horizontal") + (kinectController.TurningLeft ? -1.0f : 0.0f) + (kinectController.TurningRight ? 1.0f : 0.0f);
    }

    public static float Movement()
    {
        return Input.GetAxis("Vertical") + (kinectController.Moving ? 1.0f : 0.0f);
    }

    public static bool Jump()
    {
        return Input.GetButton("Jump") || kinectController.Jumping;
    }

    public static bool Attack()
    {
        return false;
    }

    public static bool Roll()
    {
        return Input.GetButton("Roll");
    }

    public static bool Shield()
    {
        return false;
    }

    public static void Initialize()
    {
        if (kinectController == null)
        {
            // TODO: Spawn process for Kinect; wait for it to become responsive
            //Process p = Process.Start(ViCharKinectProcessName);
            //while (p.Responding == false)
            //    yield return new WaitForSeconds(0.010f);
            kinectController = new ViCharController();
        }
    }

    public static ViCharController kinectController = null;

    private static string ViCharKinectProcessName = "foo.exe";
}
