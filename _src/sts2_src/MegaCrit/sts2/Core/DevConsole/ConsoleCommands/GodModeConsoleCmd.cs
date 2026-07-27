// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.GodModeConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class GodModeConsoleCmd : AbstractConsoleCmd
{
  private bool _godModeActive;
  private Player? _godModePlayer;

  public override string CmdName => "godmode";

  public override string Args => "";

  public override string Description => "Become invincible!";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run does not appear to be in progress");
    if (this._godModeActive)
    {
      this._godModeActive = false;
      CombatManager.Instance.CombatSetUp -= new Action<CombatState>(this.OnCombatSetUp);
      this._godModePlayer = (Player) null;
      return new CmdResult(GodModeConsoleCmd.DisableGodMode(issuingPlayer), true, "Godmode deactivated!");
    }
    this._godModeActive = true;
    this._godModePlayer = issuingPlayer;
    CombatManager.Instance.CombatSetUp += new Action<CombatState>(this.OnCombatSetUp);
    return new CmdResult(GodModeConsoleCmd.EnableGodMode(issuingPlayer), true, "Godmode activated!");
  }

  private void OnCombatSetUp(CombatState combatState)
  {
    if (this._godModePlayer == null || !RunManager.Instance.IsInProgress || LocalContext.GetMe((IPlayerCollection) combatState.RunState) != this._godModePlayer)
      return;
    TaskHelper.RunSafely(GodModeConsoleCmd.EnableGodMode(this._godModePlayer));
  }

  private static async Task DisableGodMode(Player player)
  {
    Creature playerCreature = player.Creature;
    await PowerCmd.Remove<StrengthPower>(playerCreature);
    await PowerCmd.Remove<BufferPower>(playerCreature);
    await PowerCmd.Remove<RegenPower>(playerCreature);
    playerCreature = (Creature) null;
  }

  private static async Task EnableGodMode(Player player)
  {
    Creature playerCreature = player.Creature;
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 999999999M, playerCreature, (CardModel) null);
    BufferPower bufferPower = await PowerCmd.Apply<BufferPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 999999999M, playerCreature, (CardModel) null);
    RegenPower regenPower = await PowerCmd.Apply<RegenPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 999999999M, playerCreature, (CardModel) null);
    playerCreature = (Creature) null;
  }
}
