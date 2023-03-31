using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI fuelText;

    public int killCoins;

    private PlayerController controller;
    private BulletController bulletController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("PlayerObj");
        controller = player.GetComponent<PlayerController>();
        killsText.text = "enemies killed coins: " + killCoins;
        killCoins = 0;
        ammoText.text = "Current ammo: " + controller.currentAmmo;
        fuelText.text = "Current fuel: " + controller.currentFuel;
    }

    // Update is called once per frame
    void Update()
    {
        killsText.text = "Enemies killed coins; " + killCoins;
        ammoText.text = "Current ammo: " + controller.currentAmmo;
        fuelText.text = "Current fuel: " + controller.currentFuel;
    }

    public void AddCoins(int amount)
    {
        killCoins += amount;
    }

    public void SubtractCoinsAmmo(int amount)
    {
        if(killCoins > 0)
        {
           killCoins -= 5;
           controller.AddAmmo(5);
        }
        else
        {
            return;
        }
    }

       public void SubtractCoinsHealth(int amount)
    {
        if(killCoins > 0)
        {
           killCoins -= 5;
           controller.AddHealth(5);
        }
        else
        {
            return;
        }
    }

    public void SubtractJetFuel(int amount)
    {
        if(killCoins > 0)
        {
           killCoins -= 5;
           controller.AddJetpackFuel(5);
        }
        else
        {
            return;
        }
    }

    
    public void SubtractWeaponUpgrade1(int amount)
    {
        if(killCoins >= 10)
        {
           killCoins -= 10;
        }
        else
        {
            return;
        }
    }
}
