using System;
using System.Collections.Generic;
using System.Text;

namespace StudioCore.Localization;

/// <summary>
///     Runtime localization switchboard for DSMapStudio.
///     The game-facing side is a single lookup table (see <see cref="LocalizationDict" />) that
///     the ImGui label interceptor in Andre.Native consults whenever a *translatable* label is
///     handed to Dear ImGui.
/// </summary>
public static class Loc
{
    public enum Lang
    {
        Chinese = 0,
        English = 1
    }

    private static Lang _lang = Lang.Chinese;

    /// <summary>The language the UI renders in. Defaults to Simplified Chinese.</summary>
    public static Lang Language
    {
        get => _lang;
        set
        {
            if (_lang == value)
                return;
            _lang = value;
            Install();
            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }
    }

    public static bool IsEnglish => _lang == Lang.English;
    public static bool IsChinese => _lang == Lang.Chinese;

    /// <summary>Raised when the language changes; used to rebuild fonts.</summary>
    public static event EventHandler LanguageChanged;

    /// <summary>
    ///     Translate an English source string to the active language. Strings that are not in the
    ///     table are returned unchanged, so partial coverage degrades gracefully to English.
    /// </summary>
    public static string T(string en)
    {
        if (en == null || _lang == Lang.English)
            return en;
        return Lookup(en);
    }

    /// <summary>
    ///     Table lookup with the hand-maintained supplement as a fallback. The generated table wins
    ///     so a regenerated extractor pass always overrides the supplement. An entry that maps a
    ///     string to itself carries no translation (identifiers, format fragments, keyboard
    ///     gestures), so it is treated as a miss and the supplement gets a turn.
    /// </summary>
    private static string Lookup(string en)
    {
        if (LocalizationDict.Chinese.TryGetValue(en, out var zh)
            && !string.Equals(zh, en, StringComparison.Ordinal))
            return zh;

        // The extractor recorded some labels with their "##id" suffix attached
        // ("Controls##ViewportControls"), but the ImGui hook splits the suffix off before it calls
        // in, so those entries would never match. Retry on the visible part alone.
        var marker = en.IndexOf("##", StringComparison.Ordinal);
        if (marker > 0)
        {
            var visible = en.Substring(0, marker);
            if (LocalizationDict.Chinese.TryGetValue(visible, out zh)
                && !string.Equals(zh, visible, StringComparison.Ordinal))
                return zh;
            if (LocalizationExtra.Strings.TryGetValue(visible, out zh))
                return zh;
        }

        return LocalizationExtra.Strings.TryGetValue(en, out zh) ? zh : en;
    }

    /// <summary>Translate, then fill {placeholders} from the supplied pairs.</summary>
    public static string T(string en, params string[] pairs)
    {
        var s = T(en);
        if (pairs == null || pairs.Length < 2)
            return s;
        for (var i = 0; i + 1 < pairs.Length; i += 2)
            s = s.Replace("{" + pairs[i] + "}", pairs[i + 1]);
        return s;
    }

    // ---------------------------------------------------------------- templates
    //
    // Interpolated strings from the C# source arrive already expanded ("Save Map"), so an exact
    // table lookup misses them. These patterns recover the translation for the common shapes.

    private static readonly string[] _prefixTranslations =
    {
        // "Save {x}" / "Save all"
        "Save ", "Load ", "Open ", "Create ", "Delete ", "Remove ", "Clear ", "Add ", "Copy ",
        "Go to ", "Goto ", "Unload ", "Import ", "Export ", "Select ", "Set ", "Update ", "Reset ",
    };

    /// <summary>
    ///     Best-effort translation for strings built with string interpolation. Handles the exact
    ///     match first, then a set of well-known "Verb rest" shapes where the verb is a table entry.
    /// </summary>
    public static string Template(string en)
    {
        if (en == null || _lang == Lang.English)
            return en;

        var direct = Lookup(en);
        if (!string.Equals(direct, en, StringComparison.Ordinal))
            return direct;

        // "Verb + tail": translate the verb if the table knows it, keep the tail as data.
        foreach (var verb in _prefixTranslations)
        {
            if (en.Length <= verb.Length || !en.StartsWith(verb, StringComparison.Ordinal))
                continue;
            if (!LocalizationDict.Chinese.TryGetValue(verb.TrimEnd(), out var zhVerb))
                continue;
            // "Save " -> "保存"; reproduce the original one-space join.
            return zhVerb + en.Substring(verb.TrimEnd().Length);
        }

        return en;
    }

    /// <summary>True when the table has an entry for this exact English string.</summary>
    public static bool Has(string en) => en != null && LocalizationDict.Chinese.ContainsKey(en);

    // ------------------------------------------------------------------ wiring

    /// <summary>
    ///     Push the persisted language choice into the runtime, install the ImGui label hook and
    ///     keep the CJK font flag in step with it. Called once the config is loaded.
    /// </summary>
    public static void ApplyConfig()
    {
        var cfg = CFG.Current;
        if (cfg == null)
            return;

        Language = string.Equals(cfg.Language, "English", StringComparison.OrdinalIgnoreCase)
            ? Lang.English
            : Lang.Chinese;

        // Chinese needs the CJK glyph range loaded; English can keep it for safety.
        if (Language == Lang.Chinese)
            cfg.FontChinese = true;

        Install();
    }

    /// <summary>Install (or refresh) the Andre.Native label hook. Idempotent.</summary>
    public static void Install()
    {
        Andre.Native.ImGuiLoc.Hook = _lang == Lang.English ? null : ImGuiLabel;
    }

    /// <summary>
    ///     Entry point used by the ImGui wrapper. Behaves like <see cref="Template" /> but stays on
    ///     the hot path: exact matches return immediately and misses hand back the input.
    /// </summary>
    private static string ImGuiLabel(string en) => Template(en);
}
