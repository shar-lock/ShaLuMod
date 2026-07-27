// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.HammerTimePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class HammerTimePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

  public override async Task AfterForge(Decimal amount, Player forger, AbstractModel? source)
  {
    if (source is HammerTimePower || forger != this.Owner.Player)
      return;
    foreach (Player player in this.CombatState.Players.Where<Player>((Func<Player, bool>) (p => p.Creature.IsAlive && p != forger)))
    {
      IEnumerable<SovereignBlade> sovereignBlades = await ForgeCmd.Forge(amount, player, (AbstractModel) this);
    }
  }
}
