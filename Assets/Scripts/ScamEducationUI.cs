using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScamEducationUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject educationPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionEnglishText;
    public TextMeshProUGUI descriptionKhmerText;
    public Button closeButton;
    public Image iconImage;
    
    [Header("Scam Icons")]
    public Sprite phishingIcon;
    public Sprite ghostAccountIcon;
    public Sprite deepFakeIcon;
    
    private Dictionary<string, ScamData> scamDatabase;

    [System.Serializable]
    public class ScamData
    {
        public string titleEN;
        public string titleKH;
        public string descriptionEN;
        public string descriptionKH;
        public Sprite icon;
    }

    void Start()
    {
        InitializeScamDatabase();
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
        
        if (educationPanel != null)
        {
            educationPanel.SetActive(false);
        }
    }

    void InitializeScamDatabase()
    {
        scamDatabase = new Dictionary<string, ScamData>();
        
        // Phishing
        scamDatabase["Phishing"] = new ScamData
        {
            titleEN = "⚠️ PHISHING ATTACK",
            titleKH = "⚠️ ការវាយប្រហារ Phishing",
            descriptionEN = "Phishing attacks use fake links and messages to steal your personal information.\n\n" +
                          "Protection Tips:\n" +
                          "• Check sender email carefully\n" +
                          "• Don't click suspicious links\n" +
                          "• Verify on official websites\n" +
                          "• Never share passwords via email",
            descriptionKH = "ការវាយប្រហារ Phishing ប្រើ link ក្លែងក្លាយ និង message ដើម្បីលួចព័ត៌មានផ្ទាល់ខ្លួនរបស់អ្នក។\n\n" +
                          "វិធីការពារ:\n" +
                          "• ពិនិត្យ email អ្នកផ្ញើឱ្យបានល អ្\n" +
                          "• កុំចុច link គួរឱ្យសង្ស័យ\n" +
                          "• ផ្ទៀងផ្ទាត់នៅលើគេហទំព័រផ្លូវការ\n" +
                          "• កុំចែករំលែកពាក្យសម្ងាត់តាម email",
            icon = phishingIcon
        };
        
        // Ghost Account
        scamDatabase["GhostAccount"] = new ScamData
        {
            titleEN = "👻 GHOST ACCOUNTS",
            titleKH = "👻 គណនីក្លែងក្លាយ (Ghost Account)",
            descriptionEN = "Fake social media profiles used to scam people or spread misinformation.\n\n" +
                          "Warning Signs:\n" +
                          "• No profile picture or generic photo\n" +
                          "• Very few posts or followers\n" +
                          "• Sending suspicious messages\n" +
                          "• Asking for money or personal info",
            descriptionKH = "គណនី Social Media ក្លែងក្លាយប្រើដើម្បីបោកប្រាស់ ឬផ្សព្វផ្សាយព័ត៌មានមិនពិត។\n\n" +
                          "សញ្ញាព្រមាន:\n" +
                          "• គ្មានរូបភាព profile ឬរូបទូទៅ\n" +
                          "• មាន posts ឬ followers តិចតួច\n" +
                          "• ផ្ញើ message គួរឱ្យសង្ស័យ\n" +
                          "• សុំលុយ ឬព័ត៌មានផ្ទាល់ខ្លួន",
            icon = ghostAccountIcon
        };
        
        // DeepFake
        scamDatabase["DeepFake"] = new ScamData
        {
            titleEN = "🤖 DEEPFAKE AI",
            titleKH = "🤖 DeepFake (AI ក្លែងក្លាយ)",
            descriptionEN = "AI-generated fake videos or voices that impersonate real people.\n\n" +
                          "How to Spot:\n" +
                          "• Unnatural facial movements\n" +
                          "• Voice sounds slightly off\n" +
                          "• Unusual lighting or blurring\n" +
                          "• Always verify through official channels",
            descriptionKH = "វីដេអូ ឬសម្លេងក្លែងក្លាយដែលបង្កើតដោយ AI ធ្វើត្រាប់តាមមនុស្សពិត។\n\n" +
                          "របៀបរកឃើញ:\n" +
                          "• ចលនាមុខមិនធម្មជាតិ\n" +
                          "• សម្លេងស្តាប់ទៅប្លែក\n" +
                          "• ពន្លឺ ឬការព្រលប្លែកពីធម្មតា\n" +
                          "• ផ្ទៀងផ្ទាត់តាមបណ្តាញផ្លូវការជានិច្ច",
            icon = deepFakeIcon
        };
    }

    public void ShowScamInfo(string scamType)
    {
        if (!scamDatabase.ContainsKey(scamType)) return;
        
        ScamData data = scamDatabase[scamType];
        
        if (titleText != null)
        {
            titleText.text = $"{data.titleEN}\n{data.titleKH}";
        }
        
        if (descriptionEnglishText != null)
        {
            descriptionEnglishText.text = data.descriptionEN;
        }
        
        if (descriptionKhmerText != null)
        {
            descriptionKhmerText.text = data.descriptionKH;
        }
        
        if (iconImage != null && data.icon != null)
        {
            iconImage.sprite = data.icon;
        }
        
        if (educationPanel != null)
        {
            educationPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (educationPanel != null)
        {
            educationPanel.SetActive(false);
        }
    }
}
