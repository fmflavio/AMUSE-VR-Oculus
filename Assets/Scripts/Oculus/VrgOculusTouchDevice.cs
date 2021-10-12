#if !UNITY_WSA
using UnityEngine;

namespace VrGrabber
{

public class VrgOculusTouchDevice : IDevice
{
    private OVRInput.Controller GetOVRController(ControllerSide side)
    {
        //return (side == ControllerSide.Right) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        return (side == ControllerSide.Left) ? OVRInput.Controller.LHand : OVRInput.Controller.RHand;
    }

    public Vector3 GetLocalPosition(ControllerSide side)
    {
        return OVRInput.GetLocalControllerPosition(GetOVRController(side));
    }

    public Quaternion GetLocalRotation(ControllerSide side)
    {
        return OVRInput.GetLocalControllerRotation(GetOVRController(side));
    }

    public bool GetHold(ControllerSide side)
    {
            //One = a; Two = b; Three = x; Four = y; Any = All;
            //PrimaryHandTrigger = grabber; SecondaryThumbstick = analogic

            //return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
            return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
    }

    public bool GetRelease(ControllerSide side)
    {
        //return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
        return OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger);
    }

    public bool GetHover(ControllerSide side)
    {
        //return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
        return OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger);
    }

    public bool GetClick(ControllerSide side)
    {
        //return OVRInput.Get(OVRInput.Button.PrimaryTouchpad);
        return OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
    }

    public Vector2 GetCoord(ControllerSide side)
    {
        //return OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    }
}

}
#endif
