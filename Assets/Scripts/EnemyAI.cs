using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private PlayerInventory inventory;
    PlayerController playerController;

    [SerializeField] public int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject player;

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        inventory = gameManager.GetComponent<PlayerInventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            inventory.AddCoins(5);
            Debug.Log("Enemy Destroyed");
        }
    }
}
