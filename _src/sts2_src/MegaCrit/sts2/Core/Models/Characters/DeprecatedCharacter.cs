// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.DeprecatedCharacter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Characters;

public sealed class DeprecatedCharacter : CharacterModel
{
  private MockCardPool? _mockCardPool;

  public override bool IsPlayable => false;

  public override Color NameColor => StsColors.gold;

  public override CharacterGender Gender => CharacterGender.Neutral;

  protected override CharacterModel? UnlocksAfterRunAs => (CharacterModel) null;

  protected override string IconPath
  {
    get => SceneHelper.GetScenePath("ui/character_icons/ironclad_icon");
  }

  public override int StartingHp => 1000;

  public override int StartingGold => 99;

  public override int MaxEnergy => 100;

  public override CardPoolModel CardPool
  {
    get => (CardPoolModel) this._mockCardPool ?? (CardPoolModel) ModelDb.CardPool<MockCardPool>();
  }

  public override RelicPoolModel RelicPool
  {
    get => (RelicPoolModel) ModelDb.RelicPool<IroncladRelicPool>();
  }

  public override PotionPoolModel PotionPool
  {
    get => (PotionPoolModel) ModelDb.PotionPool<IroncladPotionPool>();
  }

  public override IEnumerable<CardModel> StartingDeck
  {
    get => (IEnumerable<CardModel>) Array.Empty<CardModel>();
  }

  public override IReadOnlyList<RelicModel> StartingRelics
  {
    get => (IReadOnlyList<RelicModel>) Array.Empty<RelicModel>();
  }

  public override float AttackAnimDelay => 0.0f;

  public override float CastAnimDelay => 0.0f;

  public override List<string> GetArchitectAttackVfx()
  {
    int capacity = 1;
    List<string> architectAttackVfx = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(architectAttackVfx, capacity);
    CollectionsMarshal.AsSpan<string>(architectAttackVfx)[0] = "vfx/vfx_attack_blunt";
    return architectAttackVfx;
  }

  public void ResetMockCardPool() => this._mockCardPool = (MockCardPool) null;

  public void AddToPool(CardModel card)
  {
    card.AssertCanonical();
    if (this._mockCardPool == null)
      this._mockCardPool = (MockCardPool) ModelDb.CardPool<MockCardPool>().ToMutable();
    this._mockCardPool.Add(card);
  }

  public override Color MapDrawingColor => new Color("462996");
}
