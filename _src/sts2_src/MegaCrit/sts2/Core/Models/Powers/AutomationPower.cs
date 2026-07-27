// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AutomationPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class AutomationPower : PowerModel
{
  private const int _baseCardsLeft = 10;
  private const string _baseCardsKey = "BaseCards";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.GetInternalData<AutomationPower.Data>().cardsLeft;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BaseCards", 10M));
    }
  }

  protected override object InitInternalData() => (object) new AutomationPower.Data();

  public override async Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    AutomationPower.Data data;
    if (card.Owner != this.Owner.Player)
    {
      data = (AutomationPower.Data) null;
    }
    else
    {
      data = this.GetInternalData<AutomationPower.Data>();
      --data.cardsLeft;
      this.InvokeDisplayAmountChanged();
      if (data.cardsLeft > 0)
      {
        data = (AutomationPower.Data) null;
      }
      else
      {
        this.Flash();
        await PlayerCmd.GainEnergy((Decimal) this.Amount, this.Owner.Player);
        data.cardsLeft = 10;
        this.InvokeDisplayAmountChanged();
        data = (AutomationPower.Data) null;
      }
    }
  }

  private class Data
  {
    public int cardsLeft = 10;
  }
}
