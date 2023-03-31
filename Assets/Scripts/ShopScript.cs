using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject gameManager;
    private PlayerInventory inventory;
    public GameObject displayItem;
    public Canvas shopCanvas;

    [SerializeField] private GameObject desplayItem;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        inventory = gameManager.GetComponent<PlayerInventory>();
        shopCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        if(inventory.killCoins >= 5)
        {
        inventory.SubtractCoinsAmmo(5);
        gameObject.SetActive(false);
        displayItem.SetActive(false);
        }
    }

    public void OpenShop()
    {
        shopCanvas.gameObject.SetActive(true);
    }
}
