// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.LightningRodPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class LightningRodPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
      });
    }
  }

  public override async Task AfterEnergyReset(Player player)
  {
    if (player != this.Owner.Player)
      return;
    await OrbCmd.Channel<LightningOrb>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Player);
    await PowerCmd.Decrement((PowerModel) this);
  }
}
