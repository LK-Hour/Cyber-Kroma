using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Connects UI buttons to player actions (Shoot, Reload, Roll)
/// Attach this to the Player GameObject
/// </summary>
public class ButtonConnector : MonoBehaviour
{
    [Header("Required Components")]
    public CharacterShooting shootingScript;
    public CharacterMovement movementScript;
    
    [Header("UI Button References")]
    public EventTrigger shootButton;
    public GameObject reloadButton;
    public GameObject rollButton;

    void Start()
    {
        // Auto-find components if not assigned
        if (shootingScript == null)
            shootingScript = GetComponent<CharacterShooting>();
        
        if (movementScript == null)
            movementScript = GetComponent<CharacterMovement>();

        // Auto-find UI buttons if not assigned
        if (shootButton == null)
        {
            GameObject btnObj = GameObject.Find("Btn_Shoot");
            if (btnObj != null) shootButton = btnObj.GetComponent<EventTrigger>();
        }
        
        if (reloadButton == null)
            reloadButton = GameObject.Find("Btn_Reload");
        
        if (rollButton == null)
            rollButton = GameObject.Find("Btn_Roll");

        // Connect buttons
        ConnectShootButton();
        ConnectReloadButton();
        ConnectRollButton();
    }

    void ConnectShootButton()
    {
        if (shootButton == null || shootingScript == null) return;

        // Clear existing triggers
        shootButton.triggers.Clear();

        // Add PointerDown event (start shooting)
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { shootingScript.StartFiring(); });
        shootButton.triggers.Add(pointerDown);

        // Add PointerUp event (stop shooting)
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => { shootingScript.StopFiring(); });
        shootButton.triggers.Add(pointerUp);

        Debug.Log("✅ Shoot button connected!");
    }

    void ConnectReloadButton()
    {
        if (reloadButton == null || shootingScript == null) return;

        UnityEngine.UI.Button btn = reloadButton.GetComponent<UnityEngine.UI.Button>();
        if (btn == null)
        {
            btn = reloadButton.AddComponent<UnityEngine.UI.Button>();
        }

        // Remove old listeners and add new one
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => shootingScript.Reload());

        Debug.Log("✅ Reload button connected!");
    }

    void ConnectRollButton()
    {
        if (rollButton == null || movementScript == null) return;

        UnityEngine.UI.Button btn = rollButton.GetComponent<UnityEngine.UI.Button>();
        if (btn == null)
        {
            btn = rollButton.AddComponent<UnityEngine.UI.Button>();
        }

        // Remove old listeners and add new one
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => movementScript.DoRoll());

        Debug.Log("✅ Roll button connected!");
    }
}
