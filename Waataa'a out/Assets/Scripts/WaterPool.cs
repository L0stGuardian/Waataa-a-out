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
    [SerializeField] private float _waterToInstance;
    [SerializeField] private float _waterDelay;
    [SerializeField] private GameObject _water;
    [SerializeField] private Collider2D _colliderWithCharacter;
    [SerializeField] private InputActionAsset _waterPool;
    private bool _playerInZone;
    InputAction _click;
    private List<GameObject> _instancedWater;

    private void Awake()
    {
        _instancedWater = new List<GameObject>();
        for (int i = 0; i < _waterToInstance; i++)
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
        if(collision.CompareTag("Player"))
        {
            _playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerInZone = false;
        }
    }

    private void Update()
    {
        if (_playerInZone && _click.WasPressedThisFrame())
        {
            CallWater();
        }
    }

    private void CallWater()
    {
        foreach (GameObject obj in _instancedWater)
        {
            if (obj.activeInHierarchy) { continue; }
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, .5f), Random.Range(-.5f, .5f), 0);
            obj.transform.position = this.transform.position + randomOffset;
            obj.SetActive(true);
            StartCoroutine(WaitForWater());
        }
    }

    IEnumerator WaitForWater()
    {
        yield return new WaitForSeconds(_waterDelay);
    }
}
