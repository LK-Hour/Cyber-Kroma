using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Small Editor utilities for Khmer workflow.
/// - Menu: Tools/Khmer/Open TMP Font Asset Creator -> tries to open the TextMeshPro Font Asset Creator window via reflection
/// </summary>
public static class KhmerTools
{
    [MenuItem("Tools/Khmer/Open TMP Font Asset Creator")]
    public static void OpenTMPFontAssetCreator()
    {
        // TextMeshPro places the font asset creator in a type internal to the TMPro.EditorAssembly.
        // We'll attempt to find the type by name and show the window via reflection.
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type creatorType = null;
        foreach (var asm in assemblies)
        {
            try
            {
                var t = asm.GetType("TMPro.Editor.TMP_FontAssetCreatorWindow");
                if (t != null)
                {
                    creatorType = t;
                    break;
                }
            }
            catch { }
        }

        if (creatorType == null)
        {
            EditorUtility.DisplayDialog("TMP Font Asset Creator not found", "TextMeshPro Font Asset Creator window type not found in loaded assemblies. Make sure TextMeshPro is installed in the project.", "OK");
            return;
        }

        EditorWindow.GetWindow(creatorType);
    }
}
