// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BloodWall
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BloodWall : CardModel
{
  private const string _bloodWallVfxPath = "vfx/vfx_blood_wall";
  private const string _bloodWallSfx = "event:/sfx/characters/ironclad/ironclad_bloodwall";

  public BloodWall()
    : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
  {
  }

  protected override IEnumerable<string> ExtraRunAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_blood_wall"));
    }
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new HpLossVar(2M),
        (DynamicVar) new BlockVar(16M, ValueProp.Move)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_bloody_impact");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, cardPlay);
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_bloodwall");
    VfxCmd.PlayOnCreature(this.Owner.Creature, "vfx/vfx_blood_wall");
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
  }

  protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(4M);
}
