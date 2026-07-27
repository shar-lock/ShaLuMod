// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Stratagem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Stratagem : CardModel
{
  public Stratagem()
    : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    StratagemPower stratagemPower = await PowerCmd.Apply<StratagemPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
