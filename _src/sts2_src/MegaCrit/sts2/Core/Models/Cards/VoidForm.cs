// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.VoidForm
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class VoidForm : CardModel
{
  public VoidForm()
    : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<VoidFormPower>(2M));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
    VoidFormPower voidFormPower = await PowerCmd.Apply<VoidFormPower>(choiceContext, this.Owner.Creature, this.DynamicVars["VoidFormPower"].BaseValue, this.Owner.Creature, (CardModel) this);
    PlayerCmd.EndTurn(this.Owner, false);
  }

  protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Ethereal);
}
