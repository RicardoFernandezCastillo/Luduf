using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaShopController : MonoBehaviour
{
    // Start is called before the first frame update
    Player player;

    public AudioClip buySound;
    public AudioClip noMoneySound;

    public AudioClip shopUISound;
    // definir los 3 botones de la tienda para ocultar y mostrar


    void Start()
    {
        //instanciamos al player
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BuySuperShoot()
    {
        if (player.tokens >= 1)
        {
            //revisar si ya tiene el super shoot
            if (player.isSuperShoot != 1)
            {
                player.tokens -= 1;
                player.tokensText.text = $"Tokens: {player.tokens}";
                player.tokensTextShop.text = $"Tokens: {player.tokens}";

                player.isSuperShoot = 1;
                player.CheckButtons();

                AudioManager.instance.PlaySFX(buySound);
            }


        }
        else
        {
            AudioManager.instance.PlaySFX(noMoneySound);
        }

    }

    public void BuyTripleShoot()
    {
        if (player.tokens >= 1)
        {
            //revisar si ya tiene el triple shoot
            if (player.isTripleShoot != 1)
            {
                player.tokens -= 1;
                player.tokensText.text = $"Tokens: {player.tokens}";
                player.tokensTextShop.text = $"Tokens: {player.tokens}";
                player.isTripleShoot = 1;
                player.CheckButtons();
                AudioManager.instance.PlaySFX(buySound);
            }
        }
        else
        {
            AudioManager.instance.PlaySFX(noMoneySound);
        }
    }

    public void BuyMele()
    {
        if (player.tokens >= 1)
        {
            //revisar si ya tiene el mele
            if (player.isMele != 1)
            {
                player.tokens -= 1;
                player.tokensText.text = $"Tokens: {player.tokens}";
                player.tokensTextShop.text = $"Tokens: {player.tokens}";
                player.isMele = 1;
                player.CheckButtons();
                AudioManager.instance.PlaySFX(buySound);
            }

        }
        else
        {
            AudioManager.instance.PlaySFX(noMoneySound);
        }
    }

    public void BuyAmmo()
    {

        if (player.coins >= 1)
        {
            player.coins -= 1;
            player.coinText.text = $"Coins: {player.coins}";
            player.coinTextShop.text = $"Coins: {player.coins}";

            player.total_Ammo += 60;
            player.totalAmmoText.text = $"{player.total_Ammo}";
            AudioManager.instance.PlaySFX(buySound);
        }
        else
        {

           AudioManager.instance.PlaySFX(noMoneySound);
        }
    }

    public void BuyReloadTime()
    {

       if (player.coins >= 1)
        {
            player.coins -= 1;
            player.coinText.text = $"Coins: {player.coins}";
            player.coinTextShop.text = $"Coins: {player.coins}";
            // reducir el tiempo de recarga en 50%
            player.timeToReload /= 2;

            AudioManager.instance.PlaySFX(buySound);
       }
       else
        {
            AudioManager.instance.PlaySFX(noMoneySound);
        }
    }

    public void buyMagazineSize()
    {
        if (player.coins >= 1)
        {
            player.coins -= 1;
            player.coinText.text = $"Coins: {player.coins}";
            player.coinTextShop.text = $"Coins: {player.coins}";

            player.magazineSize += 10;
            player.currentAmmoText.text = $"{player.currentAmmo} / {player.magazineSize}";

            AudioManager.instance.PlaySFX(buySound);
        }
        else
        {
            AudioManager.instance.PlaySFX(noMoneySound);
        }
    }

    public void buyHealth()
    {
        if (player.coins >= 1)
        {
            player.coins -= 1;
            player.coinText.text = $"Coins: {player.coins}";
            float h_aux = player.health;
            //aumentar la vida en un 50% del maximo
            player.health += player.maxHealth * 0.5f;
            if (player.health > player.maxHealth)
            {
                player.health = player.maxHealth;
            }
            player.HealthCheck(true);
            AudioManager.instance.PlaySFX(buySound);


            
        }
        else
        {
            AudioManager.instance.PlaySFX(noMoneySound);
        }
    }

    public void MakeUIsound()
    {
        AudioManager.instance.PlaySFX(shopUISound);
    }
}
