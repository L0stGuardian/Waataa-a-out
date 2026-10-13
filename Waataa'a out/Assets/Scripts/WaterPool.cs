// Sugerencia general de Clean Code: usar if de una sola línea con llaves cuando sea legible.

using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using System.Collections;

public class WaterPool : MonoBehaviour
{
    [Header("Water")]
    [Tooltip("")] private float _rangeToInstance;
    [Tooltip("")][SerializeField] private float _waterToInstance; // Aclarar el significado o las unidades con Tooltip
    [Tooltip("")][SerializeField] private float _waterDelay; // Aclarar el significado o las unidades con Tooltip
    [SerializeField] private GameObject _water;
    [SerializeField] private Collider2D _colliderWithCharacter;
    [SerializeField] private InputActionAsset _waterPool;
    private bool _isPlayerInZone;
    InputAction _click;
    private List<GameObject> _instancedWater;

    private void Awake()
    {
        _instancedWater = new List<GameObject>();
        for (int i = 0; i < _waterToInstance; i++) // Aclarar el significado o las unidades con Tooltip
        {
            GameObject newWater = Instantiate(_water, this.transform.position, Quaternion.identity);
            newWater.SetActive(false);
            _instancedWater.Add(newWater);
        }
        _click = InputSystem.actions.FindAction("Click");
    }
    private void OnEnable()
    {
        _waterPool.FindActionMap("WaterPool").Enable();
    }

    private void OnDisable()
    {
        _waterPool.FindActionMap("WaterPool").Disable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInZone = false;
        }
    }

    private void Update()
    {
        if (_isPlayerInZone && _click.WasPressedThisFrame())
        {
            CallWater();
        }
    }

    private void CallWater()
    {
        foreach (GameObject gameObject in _instancedWater)
        {
            if (gameObject.activeInHierarchy) { continue; }
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, .5f), Random.Range(-.5f, .5f), 0); // Revisar: usar una constante o configuración con nombre
            gameObject.transform.position = this.transform.position + randomOffset;
            gameObject.SetActive(true);
            StartCoroutine(WaitForWater());
        }
    }

    IEnumerator WaitForWater() // El método debe hacer lo que indica su nombre
    {
        yield return new WaitForSeconds(_waterDelay);
    }
}
