using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    private CharacterController _characterController;

    [Header("Movement")]
    [SerializeField] private float _jumpStrength;
    private int _xInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CharacterMovement(int _xInput, float _jumpStrength)
    {

    }
}
