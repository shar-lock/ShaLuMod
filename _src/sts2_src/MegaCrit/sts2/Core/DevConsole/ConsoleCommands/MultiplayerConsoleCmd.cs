// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.MultiplayerConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class MultiplayerConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "multiplayer";

  public override string Args => "";

  public override string Description
  {
    get => "Opens the multiplayer menu, or the test scene if test is the first argument";
  }

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length != 0 && args[0] == "test")
    {
      NGame.Instance.RootSceneContainer.SetCurrentScene((Control) SceneHelper.Instantiate<NMultiplayerTest>("debug/multiplayer_test"));
      TaskHelper.RunSafely(NGame.Instance.Transition.FadeIn());
      return new CmdResult(true, "Opened multiplayer test scene");
    }
    NGame.Instance.MainMenu.OpenMultiplayerSubmenu((NButton) null);
    return new CmdResult(true, "Opened multiplayer submenu");
  }
}
