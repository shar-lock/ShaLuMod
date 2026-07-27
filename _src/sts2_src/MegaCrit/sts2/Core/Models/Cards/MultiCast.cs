// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.MultiCast
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class MultiCast : CardModel
{
  public MultiCast()
    : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override bool HasEnergyCostX => true;

  public override OrbEvokeType OrbEvokeType => OrbEvokeType.All;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    int evokeCount = this.ResolveEnergyXValue();
    if (this.IsUpgraded)
      ++evokeCount;
    for (int i = 0; i < evokeCount; ++i)
    {
      await OrbCmd.EvokeNext(choiceContext, this.Owner, i == evokeCount - 1);
      await Cmd.Wait(0.25f);
    }
  }
}
