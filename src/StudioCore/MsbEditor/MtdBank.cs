using SoulsFormats;
using StudioCore.Editor;
using System;
using System.Collections.Generic;
using System.IO;

namespace StudioCore.MsbEditor;

public class MtdBank
{
    private static Dictionary<string, MTD> _mtds = new();
    private static Dictionary<string, MATBIN> _matbins = new();

    public static bool IsMatbin { get; private set; }

    public static IReadOnlyDictionary<string, MTD> Mtds => _mtds;

    public static IReadOnlyDictionary<string, MATBIN> Matbins => _matbins;

    public static void ReloadMtds()
    {
        TaskManager.Run(new TaskManager.LiveTask("Resource - Load MTDs", TaskManager.RequeueType.WaitThenRequeue,
            true, () =>
            {
                try
                {
                    IBinder mtdBinder = null;
                    string mtdPath = null;
                    if (Locator.AssetLocator.Type == GameType.DarkSoulsIII || Locator.AssetLocator.Type == GameType.Sekiro)
                    {
                        mtdPath = Locator.AssetLocator.GetAssetPath(@"mtd\allmaterialbnd.mtdbnd.dcx");
                        IsMatbin = false;
                    }
                    else if (Locator.AssetLocator.Type is GameType.EldenRing or GameType.ArmoredCoreVI)
                    {
                        mtdPath = Locator.AssetLocator.GetAssetPath(@"material\allmaterial.matbinbnd.dcx");
                        IsMatbin = true;
                    }

                    // GetAssetPath returns null when the file is absent from both the project and
                    // the game directory. Handing null to BND4.Read reaches
                    // MemoryMappedFile.CreateFromFile, which throws ArgumentNullException and took
                    // the whole application down from a background task when a game was mounted
                    // without its material binder (for example an unpacked Sekiro install).
                    if (mtdPath == null)
                    {
                        TaskLogs.AddLog("Material files cannot be found",
                            Microsoft.Extensions.Logging.LogLevel.Warning, TaskLogs.LogPriority.Low);
                        _mtds = new Dictionary<string, MTD>();
                        _matbins = new Dictionary<string, MATBIN>();
                        return;
                    }

                    mtdBinder = BND4.Read(mtdPath);

                    if (mtdBinder == null)
                    {
                        return;
                    }

                    if (IsMatbin)
                    {
                        _matbins = new Dictionary<string, MATBIN>();
                        foreach (BinderFile f in mtdBinder.Files)
                        {
                            var matname = Path.GetFileNameWithoutExtension(f.Name);
                            // Because *certain* mods contain duplicate entries for the same material
                            if (!_matbins.ContainsKey(matname))
                            {
                                _matbins.Add(matname, MATBIN.Read(f.Bytes));
                            }
                        }
                    }
                    else
                    {
                        _mtds = new Dictionary<string, MTD>();
                        foreach (BinderFile f in mtdBinder.Files)
                        {
                            var mtdname = Path.GetFileNameWithoutExtension(f.Name);
                            // Because *certain* mods contain duplicate entries for the same material
                            if (!_mtds.ContainsKey(mtdname))
                            {
                                _mtds.Add(mtdname, MTD.Read(f.Bytes));
                            }
                        }
                    }

                    mtdBinder.Dispose();
                }
                catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
                {
                    TaskLogs.AddLog("Material files cannot be found", Microsoft.Extensions.Logging.LogLevel.Warning, TaskLogs.LogPriority.Low);
                    _mtds = new Dictionary<string, MTD>();
                    _matbins = new Dictionary<string, MATBIN>();
                }
                catch (Exception)
                {
                    // A malformed or partially unpacked material binder must not abort the
                    // application; the material bank is optional for everything except MTD
                    // preview, so degrade to an empty bank and report it once.
                    TaskLogs.AddLog("Material files could not be loaded",
                        Microsoft.Extensions.Logging.LogLevel.Warning, TaskLogs.LogPriority.Low);
                    _mtds = new Dictionary<string, MTD>();
                    _matbins = new Dictionary<string, MATBIN>();
                }
            }));
    }
}
