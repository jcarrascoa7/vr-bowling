using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{

    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    // El {get; set;} es para que esta variable sea modificable mediante un evento custom (por ejemplo al tener la pistola en la mano)
    public bool EnableRightTeleport {get; set; } = true;

    // Update is called once per frame
    void Update()
    {
        if (rightTeleportRay && EnableRightTeleport)
        {
            rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
