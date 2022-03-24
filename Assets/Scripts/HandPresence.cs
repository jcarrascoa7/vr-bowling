using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{

    // VARIABLES // 

    // True if the oculus controller prefab is currently present.
    // private bool showController = false;
    // Variable con las caracteristicas de los dispositivos a escuchar. No se instancia mediante codigo, sino que en unity.
    public InputDeviceCharacteristics controllerCharacteristics;
    // Lista de todos los prefabs de manos. No se instancian mediante código, sino que se agregan en unity.
    // public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    public GameObject handModelPrefab;
    // private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;


    // Start is called before the first frame update
    private void Start()
    {
        // print("HandPresence.Start()");
        // // Obtenemos una lista de los dispositivos
        // List<InputDevice> devices = new List<InputDevice>();

        // InputDevices.GetDevices(devices); // Obtenemos todos los dispositivos

        // print("Printing devices:");
        // print(devices);
        // print("Printing devices.Count:" + devices.Count);
        // foreach (var item in devices)
        // {
        //     print("Entre al foreach");
        //     Debug.Log(item.name + item.characteristics);
        // }

        // // Obtenemos el dispositivo que cumpla con los requisitos y los insertamos en la lista
        // InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        // // Buscamos el dispositivo con los características indicadas
        // if (devices.Count > 0)
        // {
        //     targetDevice = devices[0];
        //     // GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

        //     // if (prefab)
        //     // {
        //     //     spawnedController = Instantiate(prefab, transform);
        //     //     spawnedHandModel = Instantiate(handModelPrefab, transform);
        //     //     handAnimator = spawnedHandModel.GetComponent<Animator>();
        //     //     spawnedHandModel.SetActive(false);
        //     // }
        //     // else
        //     // {
        //     //     Debug.LogError("No se encontró el prefab para el dispositivo " + targetDevice.name);
        //     //     // spawnedController = Instantiate(controllerPrefabs[0], transform);
        //     // }

        //     // Quitar esto de abajo si se quieren mostrar manos y controles a la vez        
        //     spawnedHandModel = Instantiate(handModelPrefab, transform);
        //     handAnimator = spawnedHandModel.GetComponent<Animator>();
        // }

        StartCoroutine(InitializeDevices());
    }

IEnumerator InitializeDevices()
{
    WaitForEndOfFrame wait = new WaitForEndOfFrame();
    List<InputDevice> devices = new List<InputDevice>();
    InputDevices.GetDevices(devices);
    while ( devices.Count < 3 )
    {
        yield return wait;
        InputDevices.GetDevices(devices);
    }

    Debug.Log("Devices found: " + devices.Count);
    Debug.Log(devices);
    foreach (var item in devices)
    {
        Debug.Log(item.name + item.characteristics);
    }

    InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

    if (devices.Count > 0)
    {
        targetDevice = devices[0];

        if (targetDevice != null)
        {
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }

        else
        {
            Debug.LogError("No se encontró el dispositivo " + targetDevice.name);
        }
    }
}

void UpdateHandAnimation()
{
    if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
    {
        handAnimator.SetFloat("Trigger", triggerValue);
    }
    else
    {
        handAnimator.SetFloat("Trigger", 0);
    }

    if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
    {
        handAnimator.SetFloat("Grip", gripValue);
    }
    else
    {
        handAnimator.SetFloat("Grip", 0);
    }
}
    // Update is called once per frame
    void Update()
    {
        // if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        // {
        //     if (showController)
        //     {
        //         print("Mostrando mano");
        //         spawnedController.SetActive(false);
        //         spawnedHandModel.SetActive(true);
        //         showController = false;
        //     }
        //     else
        //     {
        //         print("Ocultando mano");
        //         spawnedController.SetActive(true);
        //         spawnedHandModel.SetActive(false);
        //         showController = true;
        //     }
        // }

        // if (showController == false)
        // {
            // UpdateHandAnimation();
        // }

        UpdateHandAnimation();
    }
}
