// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.FanOfKnives
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class FanOfKnives : CardModel
{
  private const string _shivsKey = "Shivs";

  public FanOfKnives()
    : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar("Shivs", 4));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>());
    }
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NFanOfKnivesVfx.AssetPaths;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    FanOfKnivesPower fanOfKnivesPower = await PowerCmd.Apply<FanOfKnivesPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
    for (int i = 0; i < this.DynamicVars["Shivs"].IntValue; ++i)
    {
      CardModel inHand = await Shiv.CreateInHand(this.Owner, this.CombatState);
      await Cmd.CustomScaledWait(0.1f, 0.2f);
    }
  }

  public override async Task OnEnqueuePlayVfx(Creature? target)
  {
    Control backVfxContainer = this.Owner.Creature.GetBackVfxContainer();
    if (backVfxContainer != null)
      ((Node) backVfxContainer).AddChildSafely((Node) NFanOfKnivesVfx.Create(this.Owner.Creature));
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
  }

  protected override void OnUpgrade() => this.DynamicVars["Shivs"].UpgradeValueBy(1M);
}
