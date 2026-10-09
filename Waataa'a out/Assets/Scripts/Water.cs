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
        IIgnitable material = collision.GetComponent<IIgnitable>();
        if(material != null && this.tag == "Water")
        {
            this.gameObject.SetActive(false);
        }
    }
}
