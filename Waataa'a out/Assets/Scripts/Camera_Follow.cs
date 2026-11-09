using UnityEngine;

public class Camera_Follow : MonoBehaviour // Usar PascalCase
{
    [Header("Target Settings")]
    [Tooltip("Drag your Player GameObject here, or leave unassigned to auto-find by tag/name.")]
    [SerializeField] private GameObject _player;

    [Header("Offset Settings")]
    [SerializeField] private float _zOffset = -10f;
    [SerializeField] private Vector3 _additionalOffset = Vector3.zero;

    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
            if (_player == null) { Debug.LogError("Camera_Follow: No Player found! Please assign one in the Inspector or tag your player as 'Player'."); }
        }
    }
    void LateUpdate()
    {
        if (_player != null) { MoveCamera(); }
    }

    private void MoveCamera()
    {
        Vector3 targetPosition = _player.transform.position + new Vector3(_additionalOffset.x, _additionalOffset.y, _zOffset);
        transform.position = targetPosition;
    }
}
