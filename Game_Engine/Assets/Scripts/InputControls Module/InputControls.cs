using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class InputControls : MonoBehaviour
{
    public float Rotation()
    {
        return Input.GetAxis("Horizontal") + (kinectController.TurningLeft ? -1.0f : 0.0f) + (GetComponent<ViCharController>().TurningRight ? 1.0f : 0.0f);
    }

    public float Movement()
    {
        return Input.GetAxis("Vertical") + (kinectController.Moving ? 1.0f : 0.0f);
    }

    public bool Jump()
    {
        return Input.GetButton("Jump") || kinectController.Jumping;
    }

    public bool Attack()
    {
        return Input.GetButton("Attack") || kinectController.Attacking;
    }

    public bool Roll()
    {
        return Input.GetButton("Roll");
    }

    public bool Shield()
    {
        return kinectController.Shielding;
    }

    void Start()
    {
        ViCharKinectProcessName = Application.dataPath + "/../Kinect/ViCharController.exe";

        // Start the process as hidden
        ProcessStartInfo info = new ProcessStartInfo(ViCharKinectProcessName);
        info.WorkingDirectory = ViCharKinectProcessName;
        //info.WindowStyle = ProcessWindowStyle.Minimized;

        kinectProcess = Process.Start(info);

        kinectController = GetComponent<ViCharController>();
    }

    void OnDestroy()
    {
        if (kinectProcess.HasExited == true)
            return;
        else if (kinectProcess.Responding == false)
            kinectProcess.Kill();
        else
            kinectProcess.Close();
    }

    private Process kinectProcess;
    private ViCharController kinectController;

    private static string ViCharKinectProcessName;
}
