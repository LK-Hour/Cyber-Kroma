using UnityEngine;

public class LokTaShop : MonoBehaviour
{
    public GameObject shopPanel; // Drag your ShopUI here

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None; // Unlocks mouse to click buttons
            Cursor.visible = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; // Locks mouse back for playing
            Cursor.visible = false;
        }
    }

    // Function for the Button
    public void BuyItem()
    {
        Debug.Log("Item Purchased!");
        // Add your healing or credit logic here later
    }
}