using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class InputControls : MonoBehaviour
{
    public float Rotation()
    {
        return Input.GetAxis("Horizontal") + (GetComponent<ViCharController>().TurningLeft ? -1.0f : 0.0f) + (GetComponent<ViCharController>().TurningRight ? 1.0f : 0.0f);
    }

    public float Movement()
    {
        return Input.GetAxis("Vertical") + (GetComponent<ViCharController>().Moving ? 1.0f : 0.0f);
    }

    public bool Jump()
    {
        return Input.GetButton("Jump") || GetComponent<ViCharController>().Jumping;
    }

    public bool Attack()
    {
        return GetComponent<ViCharController>().Attacking;
    }

    public bool Roll()
    {
        return Input.GetButton("Roll");
    }

    public bool Shield()
    {
        return GetComponent<ViCharController>().Shielding;
    }

    void Start()
    {
        ViCharKinectProcessName = Application.dataPath + "/../Kinect/ViCharController.exe";

        // Start the process as hidden
        ProcessStartInfo info = new ProcessStartInfo(ViCharKinectProcessName);
        info.WorkingDirectory = ViCharKinectProcessName;
        info.WindowStyle = ProcessWindowStyle.Minimized;

        kinectProcess = Process.Start(info);

        kinectController = GetComponent<ViCharController>();
        UnityEngine.Debug.Log("Started kinect process!");
    }

    void OnDestroy()
    {
        //kinectController = null;

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
