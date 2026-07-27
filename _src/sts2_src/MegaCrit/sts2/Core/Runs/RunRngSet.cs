// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunRngSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class RunRngSet
{
  private static readonly RunRngSet _mockInstance = new RunRngSet(string.Empty);
  private readonly Dictionary<RunRngType, Rng> _rngs = new Dictionary<RunRngType, Rng>();

  public static RunRngSet GetMockInstance()
  {
    if (TestMode.IsOff)
      throw new InvalidOperationException("Cannot get RunRng when not in a run outside of tests!");
    return RunRngSet._mockInstance;
  }

  public string StringSeed { get; }

  public ulong Seed { get; }

  public Rng UpFront => this.GetRng(RunRngType.UpFront);

  public Rng Shuffle => this.GetRng(RunRngType.Shuffle);

  public Rng UnknownMapPoint => this.GetRng(RunRngType.UnknownMapPoint);

  public Rng CombatCardGeneration => this.GetRng(RunRngType.CombatCardGeneration);

  public Rng CombatPotionGeneration => this.GetRng(RunRngType.CombatPotionGeneration);

  public Rng CombatCardSelection => this.GetRng(RunRngType.CombatCardSelection);

  public Rng CombatEnergyCosts => this.GetRng(RunRngType.CombatEnergyCosts);

  public Rng CombatTargets => this.GetRng(RunRngType.CombatTargets);

  public Rng MonsterAi => this.GetRng(RunRngType.MonsterAi);

  public Rng Niche => this.GetRng(RunRngType.Niche);

  public Rng CombatOrbGeneration => this.GetRng(RunRngType.CombatOrbs);

  public Rng TreasureRoomRelics => this.GetRng(RunRngType.TreasureRoomRelics);

  public RunRngSet(string seed)
  {
    this.StringSeed = seed;
    if (seed.StartsWith("old"))
    {
      string stringSeed = this.StringSeed;
      int length = "old".Length;
      this.Seed = (ulong) (uint) StringHelper.GetDeterministicHashCodeOld(stringSeed.Substring(length, stringSeed.Length - length));
    }
    else
      this.Seed = StringHelper.GetDeterministicHashCode(seed);
    foreach (RunRngType runRngType in Enum.GetValues<RunRngType>())
      this._rngs[runRngType] = this.CreateRng(runRngType);
  }

  private Rng CreateRng(RunRngType rngType)
  {
    return new Rng(this.Seed, StringHelper.SnakeCase(rngType.ToString()));
  }

  public SerializableRunRngSet ToSerializable()
  {
    SerializableRunRngSet serializable = new SerializableRunRngSet()
    {
      Seed = this.StringSeed
    };
    foreach (KeyValuePair<RunRngType, Rng> rng1 in this._rngs)
    {
      RunRngType runRngType;
      Rng rng2;
      rng1.Deconstruct(ref runRngType, ref rng2);
      RunRngType key = runRngType;
      Rng rng3 = rng2;
      serializable.Rngs[key] = rng3.ToSerializable();
    }
    return serializable;
  }

  public static RunRngSet FromSave(SerializableRunRngSet save)
  {
    RunRngSet runRngSet = new RunRngSet(save.Seed);
    foreach (KeyValuePair<RunRngType, SerializableRng> rng in save.Rngs)
    {
      RunRngType runRngType;
      SerializableRng serializableRng;
      rng.Deconstruct(ref runRngType, ref serializableRng);
      RunRngType key = runRngType;
      SerializableRng serializable = serializableRng;
      runRngSet._rngs[key] = new Rng(serializable);
    }
    return runRngSet;
  }

  public void LoadFromSerializable(SerializableRunRngSet save)
  {
    if (this.StringSeed != save.Seed)
      throw new NotImplementedException("RngSet seed should not change during the run!");
    foreach (KeyValuePair<RunRngType, SerializableRng> rng in save.Rngs)
    {
      RunRngType key;
      SerializableRng serializable;
      rng.Deconstruct(ref key, ref serializable);
      this._rngs[key].LoadFromSerializable(serializable);
    }
  }

  public void MockRng(RunRngType rngType, ulong seed) => this._rngs[rngType] = new Rng(seed);

  public Rng GetRng(RunRngType rngType) => this._rngs[rngType];
}
