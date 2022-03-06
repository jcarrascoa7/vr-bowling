using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{

    // VARIABLES // 


    // True if the oculus controller prefab is currently present.
    private bool showController = true;
    // Variable con las caracteristicas de los dispositivos a escuchar. No se instancia mediante codigo, sino que en unity.
    public InputDeviceCharacteristics controllerCharacteristics;
    // Lista de todos los prefabs de manos. No se instancian mediante código, sino que se agregan en unity.
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    public GameObject handModelPrefab;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;


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
            Debug.Log("Reconocido:" + item.name + item.characteristics);
        }

        // Buscamos el dispositivo con los características indicadas
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
                spawnedHandModel = Instantiate(handModelPrefab, transform);
                spawnedHandModel.SetActive(false);
            }
            else
            {
                Debug.LogError("No se encontró el prefab para el dispositivo " + targetDevice.name);
                // spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            if (showController)
            {
                Debug.Log("Boton primario pulsado");
                Debug.Log("Oculus ocultados, mostrando manos");
                spawnedHandModel.SetActive(true);
                Debug.Log("spawnedHandModel status: " + spawnedHandModel.activeSelf);
                spawnedController.SetActive(false);
                Debug.Log("spawnedController status: " + spawnedController.activeSelf);
                showController = false;
                Debug.Log("showController boolean status: " + showController);
            }
            else
            {
                Debug.Log("Boton primario pulsado");
                Debug.Log("Manos ocultadas, mostrando oculus");
                spawnedController.SetActive(true);
                Debug.Log("spawnedController status: " + spawnedController.activeSelf);
                spawnedHandModel.SetActive(false);
                Debug.Log("spawnedHandModel status: " + spawnedHandModel.activeSelf);
                showController = true;
                Debug.Log("showController boolean status: " + showController);
            }
        }
    }
}
