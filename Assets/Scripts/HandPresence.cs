using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{

    // Variable con las caracteristicas de los dispositivos a escuchar. No se instancia mediante codigo, sino que en unity.
    public InputDeviceCharacteristics controllerCharacteristics;

    // Lista de todos los prefabs de manos. No se instancian mediante código, sino que se agregan en unity.
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    private GameObject spawnedController;

    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos una lista de los dispositivos
        List<InputDevice> devices = new List<InputDevice>();

        // InputDevices.GetDevices(devices); // Obtenemos todos los dispositivos

        // Obtenemos el dispositivo que cumpla con los requisitos y los insertamos en la lista
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        // Buscamos el dispositivo con los características indicadas
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("No se encontró el prefab para el dispositivo " + targetDevice.name);
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log("Primary button pressed");
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            Debug.Log("Trigger value: " + triggerValue);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Primary 2D axis value: " + primary2DAxisValue);
        }
    }
}
