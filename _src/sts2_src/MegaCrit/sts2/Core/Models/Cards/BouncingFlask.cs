// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BouncingFlask
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
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BouncingFlask : CardModel
{
  private readonly Color _vfxTint = new Color("83eb85");

  public BouncingFlask()
    : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.RandomEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<PoisonPower>(3M),
        (DynamicVar) new RepeatVar(3)
      });
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
    Vector2 lastPos = Vector2.Zero;
    for (int i = 0; i < this.DynamicVars.Repeat.IntValue; ++i)
    {
      Creature enemy = this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.CombatState.HittableEnemies);
      if (enemy != null)
      {
        if (TestMode.IsOff)
        {
          if (i == 0)
            lastPos = NCombatRoom.Instance.GetCreatureNode(this.Owner.Creature).VfxSpawnPosition;
          NCreature targetNode = NCombatRoom.Instance.GetCreatureNode(enemy);
          if (targetNode != null)
          {
            ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NItemThrowVfx.Create(lastPos, targetNode.GetBottomOfHitbox(), ModelDb.Potion<PoisonPotion>().Image));
            lastPos = targetNode.VfxSpawnPosition;
            await Cmd.Wait(0.5f);
            ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NSplashVfx.Create(targetNode.VfxSpawnPosition, this._vfxTint));
            ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NLiquidOverlayVfx.Create(enemy, this._vfxTint));
            ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NGaseousImpactVfx.Create(targetNode.VfxSpawnPosition, this._vfxTint));
          }
          targetNode = (NCreature) null;
        }
        PoisonPower poisonPower = await PowerCmd.Apply<PoisonPower>(choiceContext, enemy, this.DynamicVars.Poison.BaseValue, this.Owner.Creature, (CardModel) this);
      }
      enemy = (Creature) null;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}
