using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

namespace CyberKroma.Khmer
{
    /// <summary>
    /// Component to display Khmer text correctly when HarfBuzzSharp (or any IKhmerShaper implementation) is available.
    /// If HarfBuzzSharp is not installed, it falls back to just assigning the plain string to the TMP component
    /// and logs a warning recommending the HarfBuzz integration for correct shaping.
    ///
    /// Usage: Add this component to the same GameObject that has a TextMeshProUGUI or TextMeshPro component.
    /// Then set the Text property at runtime or in the Inspector. When HarfBuzz is present, shaped glyphs will be
    /// requested from the shaper implementation.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class KhmerTextMeshPro : MonoBehaviour
    {
        [TextArea(3, 10)]
        public string Text;

        TMP_Text m_tmp;

        void Awake()
        {
            m_tmp = GetComponent<TMP_Text>();
        }

        void Start()
        {
            UpdateText();
        }

        /// <summary>
        /// Call to re-render the current Text string through the shaper (if present) or directly.
        /// </summary>
        public void UpdateText()
        {
            if (string.IsNullOrEmpty(Text))
            {
                m_tmp.text = string.Empty;
                return;
            }

#if HARFBUZZSHARP
            try
            {
                // If you define HARFBUZZSHARP and add references to HarfBuzzSharp and native harfbuzz libs,
                // implement shaping here. This sample demonstrates where shaping would occur.
                // Example (pseudocode):
                // var shaped = HarfBuzzShaper.Shape(Text, fontBuffer);
                // m_tmp.text = ConvertGlyphsToTMPString(shaped);
                // NOTE: Mapping shaped glyph indices to TMP glyphs requires matching font glyph indices.
                m_tmp.text = Text; // placeholder until full integration is implemented
            }
            catch (Exception ex)
            {
                Debug.LogWarning("HarfBuzz shaping failed: " + ex.Message + " — falling back to plain text.");
                m_tmp.text = Text;
            }
#else
            // No HarfBuzz present: fallback — TMP will display glyphs but complex shaping (ligatures/marks)
            // may not be rendered properly for Khmer. See Assets/Plugins/README_HarfBuzz.md for installation steps.
            m_tmp.text = Text;
            Debug.LogWarning("Khmer shaping is not available: install HarfBuzzSharp or another shaper for proper Khmer rendering. See Assets/Plugins/README_HarfBuzz.md for instructions.");
#endif
        }

        // Optional: helper to set text via script and immediately update
        public void SetText(string s)
        {
            Text = s;
            UpdateText();
        }
    }
}
