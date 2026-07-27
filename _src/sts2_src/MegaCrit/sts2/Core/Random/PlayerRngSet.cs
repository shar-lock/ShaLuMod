// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Random.PlayerRngSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Random;

public class PlayerRngSet
{
  private readonly Dictionary<PlayerRngType, Rng> _rngs = new Dictionary<PlayerRngType, Rng>();

  public Rng Rewards => this.GetRng(PlayerRngType.Rewards);

  public Rng Shops => this.GetRng(PlayerRngType.Shops);

  public Rng Transformations => this.GetRng(PlayerRngType.Transformations);

  public ulong Seed { get; }

  public PlayerRngSet(ulong seed)
  {
    this.Seed = seed;
    foreach (PlayerRngType playerRngType in Enum.GetValues<PlayerRngType>())
      this._rngs[playerRngType] = this.CreateRng(playerRngType);
  }

  private Rng CreateRng(PlayerRngType rngType)
  {
    return new Rng(this.Seed, StringHelper.SnakeCase(rngType.ToString()));
  }

  public SerializablePlayerRngSet ToSerializable()
  {
    SerializablePlayerRngSet serializable = new SerializablePlayerRngSet()
    {
      Seed = this.Seed
    };
    foreach (KeyValuePair<PlayerRngType, Rng> rng1 in this._rngs)
    {
      PlayerRngType playerRngType;
      Rng rng2;
      rng1.Deconstruct(ref playerRngType, ref rng2);
      PlayerRngType key = playerRngType;
      Rng rng3 = rng2;
      serializable.Rngs[key] = rng3.ToSerializable();
    }
    return serializable;
  }

  public static PlayerRngSet FromSerializable(SerializablePlayerRngSet save)
  {
    PlayerRngSet playerRngSet = new PlayerRngSet(save.Seed);
    foreach (KeyValuePair<PlayerRngType, SerializableRng> rng in save.Rngs)
    {
      PlayerRngType playerRngType;
      SerializableRng serializableRng;
      rng.Deconstruct(ref playerRngType, ref serializableRng);
      PlayerRngType key = playerRngType;
      SerializableRng serializable = serializableRng;
      playerRngSet._rngs[key] = new Rng(serializable);
    }
    return playerRngSet;
  }

  public void LoadFromSerializable(SerializablePlayerRngSet save)
  {
    if ((long) this.Seed != (long) save.Seed)
      throw new NotImplementedException("RngSet seed should not change during the run!");
    foreach (KeyValuePair<PlayerRngType, SerializableRng> rng in save.Rngs)
    {
      PlayerRngType key;
      SerializableRng serializable;
      rng.Deconstruct(ref key, ref serializable);
      this._rngs[key].LoadFromSerializable(serializable);
    }
  }

  public Rng GetRng(PlayerRngType rngType) => this._rngs[rngType];
}
