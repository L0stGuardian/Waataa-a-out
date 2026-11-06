using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Collider2D _waterCollider;

    private void Start()
    {
        _waterCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag == "WaterInPlayer") { return; }
 
        if (collision.CompareTag("Player") || collision.CompareTag("Faucet")) { return; }

        this.gameObject.SetActive(false);
    }
}
