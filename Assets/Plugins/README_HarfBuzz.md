HarfBuzz integration (optional)
=================================

Why you need HarfBuzz
---------------------
Khmer (and many other complex scripts) require OpenType shaping: combining consonants, reordering marks, forming ligatures, and positioning diacritics. Unity's built-in text rendering and TextMeshPro do not perform full OpenType shaping, so Khmer text often looks broken (marks separated, incorrect stacking).

HarfBuzz is the de-facto library for OpenType shaping. To render Khmer correctly at runtime you should:

1. Add HarfBuzz native libraries (libharfbuzz) for your target platforms.
2. Add a C# binding such as HarfBuzzSharp (or another managed wrapper) to call HarfBuzz from Unity.
3. Use a shaping integration that converts shaped glyph information (glyph indices and positions) into geometry shown by TextMeshPro (or direct mesh generation).

Quick steps (developer):
-------------------------
1. Install HarfBuzzSharp / bindings

   - Project: https://github.com/HarfBuzzSharp/HarfBuzzSharp
   - You can add HarfBuzzSharp as a Unity package or compile the managed assembly and place it under `Assets/Plugins`.

2. Add native harfbuzz libraries

   - Download or build `libharfbuzz` for your platforms (Windows DLL, macOS dylib, Linux so, Android .so etc.)
   - Put them under `Assets/Plugins/<platform>` conventions so Unity will include them.

3. Implement shaping

   - The `Assets/Scripts/Khmer/KhmerTextMeshPro.cs` component in this repo contains the hook point. If you define the compile symbol `HARFBUZZSHARP` and provide the HarfBuzzSharp assemblies + native libs, you can implement shaping in the UpdateText() method.
   - Mapping shaped glyphs back to a TextMeshPro font asset requires matching glyph indices — this can be done by using the TTF font you already have (Battambang) and ensuring the TMP SDF font asset contains glyphs with the same indices.

4. Editor helper

   - Use Tools → Khmer → Open TMP Font Asset Creator to generate a TMP Font Asset from the Battambang TTF (include Khmer unicode ranges U+1780–U+17FF and related blocks).

If you'd like, I can:
- Add the HarfBuzzSharp managed DLL and example native lib stubs for the platforms you care about (requires you to agree to include those binaries in the repo), and
- Implement the full shaping flow to convert HarfBuzz output into a TMP mesh.

If you want me to proceed with a full HarfBuzz integration now, tell me which target platforms you need (Editor / Windows / macOS / Linux / Android / iOS). Otherwise I can implement the shaping glue once the HarfBuzz binaries are present.
