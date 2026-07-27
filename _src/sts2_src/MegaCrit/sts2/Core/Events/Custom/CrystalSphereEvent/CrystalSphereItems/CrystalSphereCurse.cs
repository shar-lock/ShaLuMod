// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems.CrystalSphereCurse
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;

public class CrystalSphereCurse : CrystalSphereItem
{
  public override Vector2I Size => new Vector2I(2, 2);

  public override bool IsGood => false;

  public override async Task RevealItem(Player owner)
  {
    await base.RevealItem(owner);
    CardModel deck = await CardPileCmd.AddCurseToDeck<Doubt>(owner);
    if (deck == null)
      return;
    RunManager.Instance.RewardSynchronizer.SyncLocalObtainedCard(deck);
  }

  public override SerializableCrystalSphereItem ToSerializable()
  {
    return new SerializableCrystalSphereItem()
    {
      type = CrystalSphereItemType.Curse
    };
  }
}
