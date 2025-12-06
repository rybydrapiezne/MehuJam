using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 20f;
    public float ttl = 10f;
    private float time = 0;
    public float knockbackForce = 1f; // >0 is left, <0 is right

    void Update()
    {
        time += Time.deltaTime;
        if (time > ttl) Destroy(gameObject);

        transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter");
        if(collision != null)
        {
           PlayerController controller = collision.transform.parent.gameObject.GetComponent<PlayerController>();
           controller.ApplyPhysicalForce(knockbackForce);
        }
    }
}
