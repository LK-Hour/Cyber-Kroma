using UnityEditor;
using UnityEngine;
using TMPro;

/// <summary>
/// Editor helper to create a sample Canvas + TextMeshProUGUI object using the Battambang font (if a TMP font asset exists).
/// This creates a simple scene object you can use to preview Khmer text while you install HarfBuzz for shaping.
/// </summary>
public static class CreateKhmerExample
{
    [MenuItem("Tools/Khmer/Create Khmer TMP Example")]
    public static void CreateExample()
    {
        // Create Canvas
        var canvasGO = GameObject.FindObjectOfType<UnityEngine.Canvas>();
        if (canvasGO == null)
        {
            var go = new GameObject("Canvas", typeof(UnityEngine.Canvas));
            var canvas = go.GetComponent<UnityEngine.Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            go.AddComponent<UnityEngine.UI.CanvasScaler>();
            go.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            canvasGO = go.GetComponent<UnityEngine.Canvas>();
            Undo.RegisterCreatedObjectUndo(go, "Create Canvas");
        }

        // Create Text
        var textGO = new GameObject("Khmer Example Text", typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(textGO, "Create Khmer Example Text");
        textGO.transform.SetParent(canvasGO.transform, false);

        var rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.6f);
        rect.anchorMax = new Vector2(0.9f, 0.9f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = 48;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.enableWordWrapping = true;
        tmp.raycastTarget = false;

        // Attempt to find a TMP_FontAsset named Battambang or Khmer
        string[] guids = AssetDatabase.FindAssets("Battambang t:TMP_FontAsset");
        if (guids == null || guids.Length == 0)
        {
            guids = AssetDatabase.FindAssets("Khmer t:TMP_FontAsset");
        }

        if (guids != null && guids.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            if (fontAsset != null)
            {
                tmp.font = fontAsset;
                Debug.Log("Assigned TMP font asset: " + path);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("TMP Font Asset not found", "No TMP Font Asset named 'Battambang' or 'Khmer' was found. Use Tools → Khmer → Open TMP Font Asset Creator to generate one from your TTF (Battambang).", "OK");
        }

        // Add KhmerTextMeshPro component to manage shaping (will warn if HarfBuzz not installed)
        var kcomp = textGO.AddComponent<CyberKroma.Khmer.KhmerTextMeshPro>();
        kcomp.Text = "សួស្តី ពិភពលោក"; // Khmer: Hello world
        kcomp.UpdateText();

        Selection.activeGameObject = textGO;
        EditorGUIUtility.PingObject(textGO);
    }
}
