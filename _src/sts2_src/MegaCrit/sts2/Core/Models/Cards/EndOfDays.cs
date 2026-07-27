// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.EndOfDays
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
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

public sealed class EndOfDays : CardModel
{
  public const int doomAmount = 29;

  public EndOfDays()
    : base(3, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<DoomPower>(29M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    Vector2? sideCenterFloor = VfxCmd.GetSideCenterFloor(CombatSide.Enemy, this.CombatState);
    if (sideCenterFloor.HasValue)
    {
      NLargeMagicMissileVfx child = NLargeMagicMissileVfx.Create(sideCenterFloor.Value, new Color("8c2447"));
      if (child != null)
      {
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
        await Cmd.Wait(child.WaitTime);
      }
    }
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
    {
      DoomPower doomPower = await PowerCmd.Apply<DoomPower>(choiceContext, hittableEnemy, this.DynamicVars.Doom.BaseValue, this.Owner.Creature, (CardModel) this);
    }
    await DoomPower.DoomKill(DoomPower.GetDoomedCreatures(this.CombatState.HittableEnemies));
  }

  protected override void OnUpgrade() => this.DynamicVars.Doom.UpgradeValueBy(8M);
}
