// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Glam
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Glam : EnchantmentModel
{
  private const string _timesKey = "Times";
  private bool _usedThisCombat;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Times", 1M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, this.DynamicVars["Times"]));
    }
  }

  private bool UsedThisCombat
  {
    get => this._usedThisCombat;
    set
    {
      this.AssertMutable();
      this._usedThisCombat = value;
    }
  }

  public override int EnchantPlayCount(int originalPlayCount)
  {
    return this.UsedThisCombat ? originalPlayCount : originalPlayCount + this.DynamicVars["Times"].IntValue;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.UsedThisCombat || cardPlay.Card != this.Card)
      return Task.CompletedTask;
    this.UsedThisCombat = true;
    this.Status = EnchantmentStatus.Disabled;
    return Task.CompletedTask;
  }
}
