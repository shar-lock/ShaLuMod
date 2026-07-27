// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.Deprived
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

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Characters;

public sealed class Deprived : CharacterModel
{
  private MockCardPool? _mockCardPool;

  public override bool IsMock => true;

  public override bool IsPlayable => false;

  public override Color NameColor => StsColors.gold;

  public override CharacterGender Gender => CharacterGender.Neutral;

  protected override CharacterModel? UnlocksAfterRunAs => (CharacterModel) null;

  public override int StartingHp => 1000;

  public override int StartingGold => 99;

  public override int MaxEnergy => 100;

  public override CardPoolModel CardPool
  {
    get
    {
      return this._mockCardPool != null ? (CardPoolModel) this._mockCardPool : (CardPoolModel) ModelDb.CardPool<DeprivedCardPool>();
    }
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

  public override List<string> GetArchitectAttackVfx() => new List<string>();

  public void ResetMockCardPool() => this._mockCardPool = (MockCardPool) null;

  public void SetMockCardPool(IEnumerable<CardModel> cards)
  {
    this._mockCardPool = this._mockCardPool == null ? (MockCardPool) ModelDb.CardPool<MockCardPool>().ToMutable() : throw new InvalidOperationException("Mock card pool already initialized");
    foreach (CardModel card in cards)
      this._mockCardPool.Add(card);
  }

  public override Color MapDrawingColor => new Color("462996");
}
