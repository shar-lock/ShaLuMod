// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.SentryConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves;
using Sentry;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class SentryConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "sentry";

  public override string Args => "<test|message|exception|crash|status> [text]";

  public override string Description
  {
    get
    {
      return "Test Sentry error reporting. 'test' sends a test message and exception, 'message <text>' sends a custom message, 'exception' throws a test exception, 'crash confirm' triggers a native crash (terminates game!), 'status' shows Sentry status.";
    }
  }

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "Usage: sentry <test|message|exception|crash|status> [text]");
    string lowerInvariant = args[0].ToLowerInvariant();
    CmdResult cmdResult;
    switch (lowerInvariant)
    {
      case "status":
        cmdResult = SentryConsoleCmd.GetStatus();
        break;
      case "test":
        cmdResult = SentryConsoleCmd.RunTest();
        break;
      case "message":
        cmdResult = SentryConsoleCmd.SendMessage(args);
        break;
      case "exception":
        cmdResult = SentryConsoleCmd.ThrowException();
        break;
      case "crash":
        cmdResult = SentryConsoleCmd.TriggerNativeCrash(args);
        break;
      default:
        cmdResult = new CmdResult(false, $"Unknown subcommand: {lowerInvariant}. Use test, message, exception, crash, or status.");
        break;
    }
    return cmdResult;
  }

  private static CmdResult GetStatus()
  {
    return new CmdResult(true, $"Sentry Status:\n  .NET SDK Enabled: {SentryService.IsEnabled}\n  Has release_info.json: {ReleaseInfoManager.Instance.ReleaseInfo != null}\n  User consent (UploadData): {SaveManager.Instance.PrefsSave.UploadData}\n  Version: {ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "N/A"}\n  Branch: {ReleaseInfoManager.Instance.ReleaseInfo?.Branch ?? "N/A"}");
  }

  private static CmdResult RunTest()
  {
    bool isEnabled = SentryService.IsEnabled;
    if (!isEnabled)
      SentryConsoleCmd.ForceInitializeForTesting();
    if (!SentryService.IsEnabled)
      return new CmdResult(false, "Failed to initialize Sentry for testing. Check logs for details.");
    SentryService.CaptureMessage("Test message from STS2 dev console", (SentryLevel) 1);
    try
    {
      throw new InvalidOperationException("Test exception from STS2 dev console");
    }
    catch (Exception ex)
    {
      SentryService.CaptureException(ex);
    }
    string msg = "Sent test message and exception to Sentry. Check your Sentry dashboard.";
    if (!isEnabled)
      msg += "\n(Sentry was temporarily enabled for this test)";
    return new CmdResult(true, msg);
  }

  private static CmdResult SendMessage(string[] args)
  {
    if (args.Length < 2)
      return new CmdResult(false, "Usage: sentry message <text>");
    string str = string.Join(" ", args, 1, args.Length - 1);
    if (!SentryService.IsEnabled)
      SentryConsoleCmd.ForceInitializeForTesting();
    if (!SentryService.IsEnabled)
      return new CmdResult(false, "Failed to initialize Sentry for testing.");
    SentryService.CaptureMessage("[DevConsole] " + str, (SentryLevel) 1);
    return new CmdResult(true, "Sent message to Sentry: " + str);
  }

  private static CmdResult ThrowException()
  {
    if (!SentryService.IsEnabled)
      SentryConsoleCmd.ForceInitializeForTesting();
    if (!SentryService.IsEnabled)
      return new CmdResult(false, "Failed to initialize Sentry for testing.");
    try
    {
      throw new InvalidOperationException("Test exception triggered via 'sentry exception' command");
    }
    catch (Exception ex)
    {
      SentryService.CaptureException(ex);
      return new CmdResult(true, "Captured test exception and sent to Sentry.");
    }
  }

  private static CmdResult TriggerNativeCrash(string[] args)
  {
    if (OS.HasFeature("editor"))
      return new CmdResult(false, "Native crash testing only works in builds, not the editor.");
    if (args.Length < 2 || !args[1].Equals("confirm", StringComparison.OrdinalIgnoreCase))
      return new CmdResult(false, "WARNING: This will crash the game!\nTo confirm, run: sentry crash confirm");
    OS.Crash("Sentry test crash triggered via dev console");
    return new CmdResult(true, "Crash triggered");
  }

  private static void ForceInitializeForTesting() => SentryService.InitializeForTesting();
}
