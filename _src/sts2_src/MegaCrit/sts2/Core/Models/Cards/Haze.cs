// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Haze
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Haze : CardModel
{
  public Haze()
    : base(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<PoisonPower>(4M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    this.SpawnVfx();
    await Cmd.CustomScaledWait(0.2f, 0.4f);
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
    {
      PoisonPower poisonPower = await PowerCmd.Apply<PoisonPower>(choiceContext, hittableEnemy, this.DynamicVars.Poison.BaseValue, this.Owner.Creature, (CardModel) this);
    }
  }

  private void SpawnVfx()
  {
    Node combatVfxContainer = (Node) NCombatRoom.Instance?.CombatVfxContainer;
    if (combatVfxContainer == null)
      return;
    NSmokyVignetteVfx child = NSmokyVignetteVfx.Create(new Color(0.8f, 0.8f, 0.3f, 0.66f), new Color(0.0f, 4f, 0.0f, 0.33f));
    combatVfxContainer.AddChildSafely((Node) child);
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
      combatVfxContainer.AddChildSafely((Node) NSmokePuffVfx.Create(hittableEnemy, NSmokePuffVfx.SmokePuffColor.Green));
  }

  protected override void OnUpgrade() => this.DynamicVars.Poison.UpgradeValueBy(2M);
}
