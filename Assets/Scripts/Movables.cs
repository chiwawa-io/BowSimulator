using UnityEngine;

public class Movables : MonoBehaviour
{
    [SerializeField] private int id;
    void Update()
    {
        if (id == 0)
            transform.Translate(-Vector3.forward * Time.deltaTime * 15f);
        if (id == 1) 
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * 20f);
            transform.Translate(-Vector3.right * Time.deltaTime * 1.5f);
        }

        if (transform.position.z < -87f && id == 0)
        {
            Destroy(gameObject, 10f);
        }
    }
}
