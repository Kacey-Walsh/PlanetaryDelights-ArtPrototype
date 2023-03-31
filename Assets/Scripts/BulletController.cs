using UnityEngine;

public class BulletController : MonoBehaviour
{

    private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 target {get; set; }
    public bool hit {get; set; }

     public bool weaponUpgrade1 = false;

    private void OnEnable()
    {
        // Once spawned destroy after 3 seconds if nothing is hit
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        // If nothing is hit fly towards center screen then destroy
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if object hit has tag "Enemy" do damage and destroy bullet
        if(collision.collider.tag == "Enemy")
        {
            Debug.Log("Enemy damaged by bullet");
            collision.gameObject.GetComponent<EnemyAI>().EnemyTakeDamage(1);
            Destroy(gameObject);
        }
    }
}