using System;
using System.Runtime.CompilerServices;

namespace Andre.Native;

/// <summary>
///     Interception point between Dear ImGui and the application.
///     Every label the application hands to ImGui passes through <see cref="L" />, which swaps the
///     English source text for the active language while leaving ImGui ID suffixes intact.
/// </summary>
public static class ImGuiLoc
{
    /// <summary>
    ///     Set by the application to its translation function. Left null, the hook is inert and
    ///     ImGui sees exactly the string the caller passed in.
    /// </summary>
    public static Func<string, string> Hook;

    /// <summary>
    ///     Translate a label. ImGui hashes the whole string to derive widget, window and tab IDs,
    ///     so every identity-bearing part is cut out, kept verbatim and re-attached unchanged:
    ///     the "##" suffix (widget id), and the "###" anchor (window / tab id). Only the visible
    ///     text in front of them is offered to the hook.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string L(string s)
    {
        var hook = Hook;
        if (hook == null || string.IsNullOrEmpty(s))
            return s;

        // The visible part ends at the first "##" or "###"; everything after is identity.
        var marker = s.IndexOf("##", StringComparison.Ordinal);
        if (marker < 0)
            return hook(s);

        // Whole string is an id ("##value" / "###value"): nothing to translate.
        if (marker == 0)
            return s;

        var id = s.Substring(marker);
        var translated = hook(s.Substring(0, marker));
        if (ReferenceEquals(translated, null))
            return s;
        return translated + id;
    }
}
