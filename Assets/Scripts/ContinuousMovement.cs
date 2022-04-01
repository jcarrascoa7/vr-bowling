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

    // Declaramos la variable float que representa el offset extra en la altura de la camara
    public float additionalHeight = 0.2f;

    // Declaramos la variable del input que queremos usar. Se selecciona en Unity
    public XRNode inputSource;

    // Vector que tendrá la dirección del movimiento
    private Vector2 inputAxis;

    // Controlador del jugador.
    private CharacterController character;

    // Variables físicas
    public float speed = 1.0f;
    public float gravity = -9.81f;

    private float fallingSpeed;

    // Instanciamos el XRORigin para rotarlo dependiendo de la direccion donde apunta la cabeza del jugador
    private XROrigin origin;

    // Layer que tendra contacto con el rayCast
    public LayerMask groundLayer;

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
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);
        // Debug.Log("headYaw " + headYaw);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        // Debug.Log("direction " + direction);
        character.Move(direction * Time.fixedDeltaTime * speed);

        // Si el jugador está cayendo, aplicamos una velocidad de caída
        bool isGrounded = CheckIfGrounded();
        
        if (isGrounded)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    // Funcion que hace que el collider del jugador siga el headset
    void CapsuleFollowHeadset()
    {
        // Definimos la altura TOTAL del collider como la altura del headset + el offset extra
        character.height = origin.CameraInOriginSpaceHeight + additionalHeight;

        // Creamos un vector3 a partir de la posicion del headset, pasando la posicion real a una virtual
        Vector3 capsuleCenter = transform.InverseTransformPoint(origin.Camera.transform.position);
        // Debug.Log("capsuleCenter " + capsuleCenter);

        character.center = new Vector3(capsuleCenter.x, (character.height / 2) + character.skinWidth, capsuleCenter.z);
    }


    // Función que comprueba si el jugador está en el suelo
    // Se usa un SphereCast en vez de un RayCast ya que con el RayCast el jugador tendra menos colisiones y puede caerse en las orillas.

    bool CheckIfGrounded()
    {
        // Rayo que va desde el centro del collider del jugador hasta el suelo
        Vector3 rayStart = transform.TransformPoint(character.center);
        // Debug.Log("rayStart " + rayStart);
        float rayLength = character.center.y + 0.01f;
        // Debug.Log("rayLength " + rayLength);

        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

}
