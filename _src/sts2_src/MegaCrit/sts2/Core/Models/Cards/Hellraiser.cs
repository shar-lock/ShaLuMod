// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Hellraiser
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Hellraiser : CardModel
{
  public Hellraiser()
    : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NHellraiserVfx.AssetPaths;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    HellraiserPower hellraiserPower = await PowerCmd.Apply<HellraiserPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
  }

  public override async Task OnEnqueuePlayVfx(Creature? target)
  {
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NHellraiserVfx.Create(this.Owner.Creature));
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
