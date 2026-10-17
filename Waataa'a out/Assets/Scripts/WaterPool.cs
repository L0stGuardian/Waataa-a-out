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
    [Tooltip("Variation at spawning the water")] 
    [SerializeField] private float _rangeToInstance = 0.5f;
    [Tooltip("How many water will be instanced")] 
    [SerializeField] private float _waterToInstance;
    [SerializeField] private GameObject _water;
    [SerializeField] private Collider2D _colliderWithCharacter;
    [SerializeField] private InputActionAsset _waterPool;
    private bool _isPlayerInZone;
    private InputAction _click;
    private List<GameObject> _instancedWater;

    private void Awake()
    {
        _instancedWater = new List<GameObject>();
        for (int instance = 0; instance < _waterToInstance; instance++)
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
        if (collision.CompareTag("Player")) { _isPlayerInZone = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { _isPlayerInZone = false; }
    }

    private void Update()
    {
        if (_isPlayerInZone && _click.WasPressedThisFrame()) { CallWater(); }
    }

    private void CallWater()
    {
        foreach (GameObject gameObject in _instancedWater)
        {
            if (gameObject.activeInHierarchy) { continue; }

            Vector3 randomOffset = new Vector3(Random.Range(-_rangeToInstance, _rangeToInstance), Random.Range(-_rangeToInstance, _rangeToInstance), 0);
            gameObject.transform.position = this.transform.position + randomOffset;
            gameObject.SetActive(true);
        }
    }
}
