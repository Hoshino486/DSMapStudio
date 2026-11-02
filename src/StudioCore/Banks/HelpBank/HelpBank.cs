using StudioCore.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StudioCore.Banks.HelpBank;

public class HelpBank
{
    private List<HelpEntry> _helpArticles;
    private List<HelpEntry> _helpGlossary;
    private List<HelpEntry> _helpTutorials;
    private List<HelpEntry> _helpLinks;
    private List<HelpEntry> _helpCredits;
    private List<HelpEntry> _helpMassEdit;
    private List<HelpEntry> _helpRegex;

    public HelpBank()
    {
        Reload();
    }

    /// <summary>
    ///     (Re)load every help section for the active UI language. Chinese reads Assets\Help_zh,
    ///     English reads Assets\Help; both trees have the same file names and layout.
    /// </summary>
    public void Reload()
    {
        _helpArticles = LoadHelpResource("Articles");
        _helpTutorials = LoadHelpResource("Tutorials");
        _helpGlossary = LoadHelpResource("Glossary");
        _helpMassEdit = LoadHelpResource("MassEdit");
        _helpRegex = LoadHelpResource("Regex");
        _helpLinks = LoadHelpResource("Links");
        _helpCredits = LoadHelpResource("Credits");
    }

    /// <summary>Root folder holding the help tree for the active language.</summary>
    private static string HelpRoot =>
        Localization.Loc.IsEnglish ? "Help" : "Help_zh";

    private List<HelpEntry> LoadHelpResource(string directory)
    {
        List<HelpEntry> helpEntries = new();

        var folder = AppContext.BaseDirectory + $@"\Assets\{HelpRoot}\{directory}\";
        if (!Directory.Exists(folder))
        {
            // Fall back to the English tree so a missing translation never blanks the help window.
            folder = AppContext.BaseDirectory + $@"\Assets\Help\{directory}\";
        }
        if (!Directory.Exists(folder))
            return helpEntries;

        IEnumerable<string> articleDirFiles =
            from file in Directory.EnumerateFiles(folder)
            select file;
        foreach (var file in articleDirFiles)
            helpEntries.Add(LoadHelpJSON(file));

        return helpEntries;
    }

    private HelpEntry LoadHelpJSON(string path)
    {
        HelpEntry resource = new();

        if (File.Exists(path))
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                TypeInfoResolver = HelpEntrySerializerContext.Default
            };
            resource = JsonSerializer.Deserialize(File.OpenRead(path), typeof(HelpEntry), options) as HelpEntry;
        }

        return resource;
    }

    public List<HelpEntry> GetArticles()
    {
        return _helpArticles;
    }

    public List<HelpEntry> GetTutorials()
    {
        return _helpTutorials;
    }

    public List<HelpEntry> GetGlossaryEntries()
    {
        return _helpGlossary;
    }

    public List<HelpEntry> GetLinks()
    {
        return _helpLinks;
    }

    public List<HelpEntry> GetCredits()
    {
        return _helpCredits;
    }

    public List<HelpEntry> GetMassEditHelp()
    {
        return _helpMassEdit;
    }

    public List<HelpEntry> GetRegexHelp()
    {
        return _helpRegex;
    }
}
