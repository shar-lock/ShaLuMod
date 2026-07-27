// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.GetLogsConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class GetLogsConsoleCmd : AbstractConsoleCmd
{
  private const string _dateTimeSpecifier = "yyyy_MM_dd_HH_mm_ss";
  private const long _maxLogFileBytes = 8388608 /*0x800000*/;
  private const long _maxFeedbackCrashDumpBytes = 52428800 /*0x03200000*/;
  private const long _maxCoreDumpBytes = 209715200 /*0x0C800000*/;

  public override string CmdName => "getlogs";

  public override string Args => "[test-feedback] <name:string>";

  public override string Description
  {
    get
    {
      return "Gathers logs, automatically zips them to a file containing 'name', and opens the directory containing the zip file.";
    }
  }

  public override bool IsNetworked => false;

  public override bool DebugOnly => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    string extraName = "";
    bool flag = false;
    if (args.Length != 0)
    {
      if (args[0] == "test-feedback")
        flag = true;
      else
        extraName = "-" + args[0];
    }
    string bugReportPath = GetLogsConsoleCmd.GetBugReportPath(extraName);
    if (flag)
    {
      using (FileAccessStream outputStream = new FileAccessStream(bugReportPath, (FileAccess.ModeFlags) 2L))
        GetLogsConsoleCmd.ZipFeedbackLogs((Stream) outputStream, SaveManager.Instance.CurrentProfileId);
    }
    else
      TaskHelper.RunSafely(GetLogsConsoleCmd.GrabLogs(bugReportPath));
    return new CmdResult(true, $"Zipping files to '{bugReportPath}'...");
  }

  public static string GetBugReportPath(string extraName = "")
  {
    string userDataDir = OS.GetUserDataDir();
    if (userDataDir == null)
      throw new InvalidOperationException("Unable to open the user data directory.");
    string str = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    return Path.Combine(userDataDir, $"BugReport{extraName}-{str}.zip");
  }

  public static async Task GrabLogs(string bugReportPath)
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      bugReportPath = bugReportPath.Replace('/', '\\');
    bool consoleVisible = ((CanvasItem) NDevConsole.Instance).Visible;
    if (consoleVisible)
    {
      NDevConsole.Instance.HideConsole();
      double num1 = (double) await ((Node) NDevConsole.Instance).AwaitProcessFrame();
      double num2 = (double) await ((Node) NDevConsole.Instance).AwaitProcessFrame();
    }
    Image image = ((Texture2D) ((Node) NDevConsole.Instance).GetViewport().GetTexture()).GetImage();
    if (consoleVisible)
      NDevConsole.Instance.ShowConsole();
    FileStream outputStream = new FileStream(bugReportPath, (FileMode) 1);
    try
    {
      GetLogsConsoleCmd.ZipFiles((Stream) outputStream, image.SavePngToBuffer());
    }
    finally
    {
      if (outputStream != null)
        await ((Stream) outputStream).DisposeAsync();
    }
    Error error = OS.ShellShowInFileManager(ProjectSettings.GlobalizePath(bugReportPath), true);
    if (error != null)
      Log.Error($"Error {error}: Cannot open OS file manager. Files zipped to '{bugReportPath}'");
    else
      Log.Info($"Files zipped to '{bugReportPath}'");
  }

  public static void ZipFiles(Stream outputStream, byte[] screenshotBytes)
  {
    if (RunManager.Instance.IsInProgress && RunManager.Instance.CombatReplayWriter.IsRecordingReplay)
      RunManager.Instance.WriteReplay(false);
    string baseDir = StringExtensions.GetBaseDir(OS.GetExecutablePath());
    string accountBasePath = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath(""));
    string str1 = ProjectSettings.GlobalizePath("user://");
    string str2 = Path.Combine(str1, "logs");
    string file1 = Path.Combine(baseDir, "release_info.json");
    string str3 = Path.Combine(str1, "sentry", "reports");
    using (ZipArchive archive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
    {
      if (Directory.Exists(str2))
      {
        foreach (string enumerateFile in Directory.EnumerateFiles(str2, "*", (SearchOption) 1))
        {
          string relativePath = Path.GetRelativePath(str1, enumerateFile);
          GetLogsConsoleCmd.ArchiveLogFile(enumerateFile, archive, relativePath, 8388608L /*0x800000*/);
        }
      }
      List<string> source = new List<string>();
      if (Directory.Exists(str3))
        source.AddRange((IEnumerable<string>) Directory.GetFiles(str3));
      if (OS.GetName() == "Windows")
      {
        string str4 = Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder) 28), "CrashDumps");
        if (Directory.Exists(str4))
        {
          string exeName = Path.GetFileNameWithoutExtension(OS.GetExecutablePath());
          source.AddRange(((IEnumerable<string>) Directory.GetFiles(str4)).Where<string>((Func<string, bool>) (f => Path.GetFileName(f).StartsWith(exeName, StringComparison.OrdinalIgnoreCase))));
        }
      }
      if (source.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string file2 = source.OrderByDescending<string, DateTime>(GetLogsConsoleCmd.\u003C\u003EO.\u003C0\u003E__GetLastWriteTime ?? (GetLogsConsoleCmd.\u003C\u003EO.\u003C0\u003E__GetLastWriteTime = new Func<string, DateTime>(File.GetLastWriteTime))).First<string>();
        DateTimeOffset lastWriteTime = (DateTimeOffset) File.GetLastWriteTime(file2);
        string format = "yyyy-MM-dd_HH-mm-ss";
        GetLogsConsoleCmd.ArchiveFile(file2, archive, $"crashes/crash_{lastWriteTime.ToString(format)}{Path.GetExtension(file2)}");
      }
      GetLogsConsoleCmd.TryCollectLinuxCoreDump(archive);
      foreach (string allSaveFile in GetLogsConsoleCmd.GetAllSaveFiles(accountBasePath))
      {
        string entryName = "saves/" + Path.GetRelativePath(accountBasePath, allSaveFile);
        if (Path.GetExtension(allSaveFile).Equals(".json", StringComparison.OrdinalIgnoreCase))
          GetLogsConsoleCmd.ArchiveLogFile(allSaveFile, archive, entryName, 0L);
        else
          GetLogsConsoleCmd.ArchiveFile(allSaveFile, archive, entryName);
      }
      if (File.Exists(file1))
        GetLogsConsoleCmd.ArchiveFile(file1, archive, "release_info.json");
      GetLogsConsoleCmd.ArchiveBytes(screenshotBytes, archive, "screenshot.png");
    }
  }

  public static void ZipFeedbackLogs(Stream outputStream, int profileId)
  {
    if (RunManager.Instance.IsInProgress && RunManager.Instance.CombatReplayWriter.IsRecordingReplay)
      RunManager.Instance.WriteReplay(false);
    string baseDir = StringExtensions.GetBaseDir(OS.GetExecutablePath());
    string accountBasePath = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath(""));
    string str1 = ProjectSettings.GlobalizePath("user://");
    string str2 = Path.Combine(str1, "logs");
    string file1 = Path.Combine(baseDir, "release_info.json");
    string str3 = Path.Combine(str1, "sentry", "reports");
    using (ZipArchive archive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
    {
      if (Directory.Exists(str2))
      {
        foreach (string enumerateFile in Directory.EnumerateFiles(str2, "*", (SearchOption) 1))
        {
          string relativePath = Path.GetRelativePath(str1, enumerateFile);
          GetLogsConsoleCmd.ArchiveLogFile(enumerateFile, archive, relativePath, 8388608L /*0x800000*/);
        }
      }
      List<string> source = new List<string>();
      if (Directory.Exists(str3))
        source.AddRange((IEnumerable<string>) Directory.GetFiles(str3));
      if (OS.GetName() == "Windows")
      {
        string str4 = Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder) 28), "CrashDumps");
        if (Directory.Exists(str4))
        {
          string exeName = Path.GetFileNameWithoutExtension(OS.GetExecutablePath());
          source.AddRange(((IEnumerable<string>) Directory.GetFiles(str4)).Where<string>((Func<string, bool>) (f => Path.GetFileName(f).StartsWith(exeName, StringComparison.OrdinalIgnoreCase))));
        }
      }
      if (source.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string file2 = source.OrderByDescending<string, DateTime>(GetLogsConsoleCmd.\u003C\u003EO.\u003C0\u003E__GetLastWriteTime ?? (GetLogsConsoleCmd.\u003C\u003EO.\u003C0\u003E__GetLastWriteTime = new Func<string, DateTime>(File.GetLastWriteTime))).First<string>();
        DateTimeOffset lastWriteTime = (DateTimeOffset) File.GetLastWriteTime(file2);
        string format = "yyyy-MM-dd_HH-mm-ss";
        long length = new FileInfo(file2).Length;
        if (length <= 52428800L /*0x03200000*/)
          GetLogsConsoleCmd.ArchiveFile(file2, archive, $"crashes/crash_{lastWriteTime.ToString(format)}{Path.GetExtension(file2)}");
        else
          Log.Warn($"Crash dump too large for feedback upload ({length / 1048576L /*0x100000*/} MB), skipping: {file2}");
      }
      GetLogsConsoleCmd.TryCollectLinuxCoreDump(archive);
      int capacity = 7;
      List<string> stringList1 = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(stringList1, capacity);
      Span<string> span = CollectionsMarshal.AsSpan<string>(stringList1);
      int num1 = 0;
      span[num1] = ProfileSaveManager.GetProfileSavePath();
      int num2 = num1 + 1;
      span[num2] = "settings.save";
      int num3 = num2 + 1;
      span[num3] = ProgressSaveManager.GetProgressPathForProfile(profileId);
      int num4 = num3 + 1;
      span[num4] = RunSaveManager.GetRunSavePath(profileId, "current_run.save");
      int num5 = num4 + 1;
      span[num5] = RunSaveManager.GetRunSavePath(profileId, "current_run_mp.save");
      int num6 = num5 + 1;
      span[num6] = PrefsSaveManager.GetPrefsPath(profileId);
      int num7 = num6 + 1;
      span[num7] = Path.Combine(UserDataPathProvider.GetProfileDir(profileId), "replays/latest.mcr");
      List<string> stringList2 = stringList1;
      string str5 = Path.Combine(accountBasePath, RunHistorySaveManager.GetHistoryPath(profileId));
      if (Directory.Exists(str5))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string str6 = Directory.EnumerateFiles(str5, "*", (SearchOption) 0).OrderByDescending<string, DateTime>(GetLogsConsoleCmd.\u003C\u003EO.\u003C0\u003E__GetLastWriteTime ?? (GetLogsConsoleCmd.\u003C\u003EO.\u003C0\u003E__GetLastWriteTime = new Func<string, DateTime>(File.GetLastWriteTime))).FirstOrDefault<string>();
        if (str6 != null)
          stringList2.Add(Path.GetRelativePath(accountBasePath, str6));
      }
      foreach (string allSaveFile in GetLogsConsoleCmd.GetAllSaveFiles(accountBasePath))
      {
        if (allSaveFile.EndsWith(".corrupt") && DateTimeOffset.Now - (DateTimeOffset) File.GetLastWriteTime(allSaveFile) < TimeSpan.FromDays(1))
          stringList2.Add(Path.GetRelativePath(accountBasePath, allSaveFile));
      }
      foreach (string str7 in stringList2)
      {
        string file3 = Path.Combine(accountBasePath, str7);
        if (File.Exists(file3))
        {
          string entryName = "saves/" + str7;
          if (Path.GetExtension(file3).Equals(".json", StringComparison.OrdinalIgnoreCase))
            GetLogsConsoleCmd.ArchiveLogFile(file3, archive, entryName, 0L);
          else
            GetLogsConsoleCmd.ArchiveFile(file3, archive, entryName);
        }
      }
      if (!File.Exists(file1))
        return;
      GetLogsConsoleCmd.ArchiveFile(file1, archive, "release_info.json");
    }
  }

  private static void TryCollectLinuxCoreDump(ZipArchive archive)
  {
    if (OS.GetName() != "Linux")
      return;
    string fileName = Path.GetFileName(OS.GetExecutablePath());
    try
    {
      using (System.Diagnostics.Process process = new System.Diagnostics.Process())
      {
        process.StartInfo = new ProcessStartInfo()
        {
          FileName = "coredumpctl",
          Arguments = "info -1 --no-pager " + fileName,
          RedirectStandardOutput = true,
          UseShellExecute = false
        };
        process.Start();
        Task<string> endAsync = ((TextReader) process.StandardOutput).ReadToEndAsync();
        if (!((Task) endAsync).Wait(10000))
        {
          process.Kill();
          process.WaitForExit(5000);
          Log.Warn("coredumpctl info timed out after 10s");
        }
        else
        {
          string result = endAsync.Result;
          process.WaitForExit();
          if (process.ExitCode == 0)
          {
            if (!string.IsNullOrWhiteSpace(result))
            {
              using (StreamWriter streamWriter = new StreamWriter(archive.CreateEntry("crashes/coredump_info.txt").Open()))
                ((TextWriter) streamWriter).Write(result);
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      Log.Warn("Could not collect coredumpctl info: " + ex.Message);
    }
    string file = Path.Combine(Path.GetTempPath(), $"sts2_coredump_{Guid.NewGuid()}.core");
    try
    {
      using (System.Diagnostics.Process process = new System.Diagnostics.Process())
      {
        process.StartInfo = new ProcessStartInfo()
        {
          FileName = "coredumpctl",
          Arguments = $"dump -1 --no-pager -o \"{file}\" {fileName}",
          UseShellExecute = false
        };
        process.Start();
        if (!process.WaitForExit(10000))
        {
          Log.Warn("coredumpctl dump timed out after 10s, killing process");
          process.Kill();
          process.WaitForExit(5000);
        }
        else if (process.ExitCode != 0)
          Log.Warn($"coredumpctl dump exited with code {process.ExitCode}");
        if (!File.Exists(file))
          return;
        long length = new FileInfo(file).Length;
        if (length > 209715200L /*0x0C800000*/)
        {
          Log.Warn($"Core dump is {length / 1048576L /*0x100000*/} MB which exceeds the {200L} MB limit, skipping");
        }
        else
        {
          if (length <= 0L)
            return;
          GetLogsConsoleCmd.ArchiveFile(file, archive, "crashes/coredump.core");
        }
      }
    }
    catch (Exception ex)
    {
      Log.Warn("Could not collect core dump: " + ex.Message);
    }
    finally
    {
      try
      {
        File.Delete(file);
      }
      catch
      {
      }
    }
  }

  private static IEnumerable<string> GetAllSaveFiles(string accountBasePath)
  {
    if (Directory.Exists(accountBasePath))
    {
      foreach (string enumerateFile in Directory.EnumerateFiles(accountBasePath, "*", (SearchOption) 1))
        yield return enumerateFile;
    }
  }

  public static string ReadTailText(Stream stream, long maxBytes)
  {
    bool flag = false;
    if (maxBytes > 0L && stream.Length > maxBytes)
    {
      stream.Seek(stream.Length - maxBytes, (SeekOrigin) 0);
      while (stream.Position < stream.Length)
      {
        int num = stream.ReadByte();
        if (num != -1)
        {
          if ((num & 192 /*0xC0*/) != 128 /*0x80*/)
          {
            stream.Seek(-1L, (SeekOrigin) 1);
            break;
          }
        }
        else
          break;
      }
      flag = true;
    }
    using (StreamReader streamReader = new StreamReader(stream, (Encoding) null, true, -1, true))
    {
      string str1 = ((TextReader) streamReader).ReadToEnd();
      if (flag)
      {
        int num = str1.IndexOf('\n');
        if (num >= 0)
        {
          string str2 = str1;
          int startIndex = num + 1;
          str1 = str2.Substring(startIndex, str2.Length - startIndex);
        }
        str1 = $"[...truncated, showing last ~{maxBytes / 1048576L /*0x100000*/} MB...]\n" + str1;
      }
      return str1;
    }
  }

  private static void ArchiveLogFile(
    string file,
    ZipArchive archive,
    string entryName,
    long maxBytes)
  {
    entryName = entryName.Replace("\\", "/");
    ZipArchiveEntry entry = archive.CreateEntry(entryName);
    try
    {
      using (FileStream fileStream = new FileStream(file, (FileMode) 3, (FileAccess) 1, (FileShare) 3))
      {
        string str = LogSanitizer.Sanitize(GetLogsConsoleCmd.ReadTailText((Stream) fileStream, maxBytes));
        using (StreamWriter streamWriter = new StreamWriter(entry.Open()))
          ((TextWriter) streamWriter).Write(str);
      }
    }
    catch (FileNotFoundException ex)
    {
      Log.Error("Could not find file for zipping: " + file);
    }
  }

  private static void ArchiveFile(string file, ZipArchive archive, string entryName)
  {
    entryName = entryName.Replace("\\", "/");
    ZipArchiveEntry entry = archive.CreateEntry(entryName);
    try
    {
      using (FileStream fileStream = new FileStream(file, (FileMode) 3, (FileAccess) 1, (FileShare) 3))
      {
        using (Stream stream = entry.Open())
          ((Stream) fileStream).CopyTo(stream);
      }
    }
    catch (FileNotFoundException ex)
    {
      Log.Error("Could not find file for zipping: " + file);
    }
  }

  private static void ArchiveBytes(byte[] bytes, ZipArchive archive, string entryName)
  {
    using (Stream stream = archive.CreateEntry(entryName).Open())
      stream.Write(ReadOnlySpan<byte>.op_Implicit(bytes));
  }
}
