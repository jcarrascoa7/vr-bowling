using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Instanciamos UnityXR and UnityXRInteractionToolkit para poder obtener el input del touchpad

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Instanciamos Unity.CoreUtils para poder usar el objeto XROrigin en lugar del antiguo XRRig
using Unity.XR.CoreUtils;

public class ContinuousMovement : MonoBehaviour
{

    // Declaramos la variable del input que queremos usar. Se selecciona en Unity
    public XRNode inputSource;

    // Vector que tendrá la dirección del movimiento
    private Vector2 inputAxis;

    // Controlador del jugador.
    private CharacterController character;

    // Velocidad de movimiento
    public float speed = 1.0f;

    // Instanciamos el XRORigin para rotarlo dependiendo de la direccion donde apunta la cabeza del jugador
    private XROrigin origin;

    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos el componente CharacterController del objeto
        character = GetComponent<CharacterController>();

        // Obtenemos el componente XRRig del objeto
        origin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        // Es una manera análoga al uso de InputDevices.GetDevicesWithCharacteristics
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);

        // Ahora escuchamos el input del touchpad en cada frame
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    // Update is called once per frame. El movimiento lo gestionamos acá en lugar de en la función Update(). De esta forma el movimiento
    // se actualizará cada vez que unity actualice las físicas del juego.
    private void FixedUpdate()
    {
        Quaternion headYaw = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);
        Debug.Log("headYaw " + headYaw);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        Debug.Log("direction " + direction);
        character.Move(direction * Time.fixedDeltaTime * speed);
    }
}
