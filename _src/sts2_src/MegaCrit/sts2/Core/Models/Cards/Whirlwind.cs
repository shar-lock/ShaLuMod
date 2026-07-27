// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Whirlwind
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Whirlwind : CardModel
{
  private const string _whirlwindSfx = "event:/sfx/characters/ironclad/ironclad_whirlwind";

  public Whirlwind()
    : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
  {
  }

  protected override bool HasEnergyCostX => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(5M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int hitCount = this.ResolveEnergyXValue();
    if (hitCount > 0)
    {
      Color color;
      // ISSUE: explicit constructor call
      ((Color) ref color).\u002Ector("FFFFFF80");
      double num = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
      NCombatRoom instance1 = NCombatRoom.Instance;
      if (instance1 != null)
        ((Node) instance1.CombatVfxContainer).AddChildSafely((Node) NHorizontalLinesVfx.Create(color, 0.8 + (double) Mathf.Min(8, hitCount) * num));
      SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind");
      NRun instance2 = NRun.Instance;
      if (instance2 != null)
        ((Node) instance2.GlobalUi).AddChildSafely((Node) NSmokyVignetteVfx.Create(color, color));
    }
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_giant_horizontal_slash").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
