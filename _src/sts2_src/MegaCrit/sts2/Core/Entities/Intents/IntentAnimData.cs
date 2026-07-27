// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Intents.IntentAnimData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Intents;

public static class IntentAnimData
{
  public const string attack1 = "attack_1";
  public const string attack2 = "attack_2";
  public const string attack3 = "attack_3";
  public const string attack4 = "attack_4";
  public const string attack5 = "attack_5";
  public const string buff = "buff";
  public const string cardDebuff = "card_debuff";
  public const string deathBlow = "death_blow";
  public const string debuff = "debuff";
  public const string defend = "defend";
  public const string escape = "escape";
  public const string heal = "heal";
  public const string hidden = "hidden";
  public const string sleep = "sleep";
  public const string status = "status";
  public const string stun = "stun";
  public const string summon = "summon";
  public const string unknown = "unknown";
  private static readonly Dictionary<string, IntentAnimData.InternalData> _data = new Dictionary<string, IntentAnimData.InternalData>()
  {
    {
      "attack_1",
      new IntentAnimData.InternalData("attack/intent_attack_1")
    },
    {
      "attack_2",
      new IntentAnimData.InternalData("attack/intent_attack_2")
    },
    {
      "attack_3",
      new IntentAnimData.InternalData("attack/intent_attack_3")
    },
    {
      "attack_4",
      new IntentAnimData.InternalData("attack/intent_attack_4")
    },
    {
      "attack_5",
      new IntentAnimData.InternalData("attack/intent_attack_5")
    },
    {
      nameof (buff),
      new IntentAnimData.InternalData("buff/intent_buff", 30)
    },
    {
      "card_debuff",
      new IntentAnimData.InternalData("card_debuff/intent_carddebuff", 15)
    },
    {
      "death_blow",
      new IntentAnimData.InternalData("intent_death_blow")
    },
    {
      nameof (debuff),
      new IntentAnimData.InternalData("debuff/intent_megadebuff", 11)
    },
    {
      nameof (defend),
      new IntentAnimData.InternalData("defend/intent_defend", 45)
    },
    {
      nameof (escape),
      new IntentAnimData.InternalData("escape/intent_escape", 40)
    },
    {
      nameof (heal),
      new IntentAnimData.InternalData("heal/intent_heal", 45)
    },
    {
      nameof (hidden),
      new IntentAnimData.InternalData("intent_hidden")
    },
    {
      nameof (sleep),
      new IntentAnimData.InternalData("sleep/intent_sleep", 16 /*0x10*/)
    },
    {
      nameof (status),
      new IntentAnimData.InternalData("status/intent_statuscard", 19)
    },
    {
      nameof (stun),
      new IntentAnimData.InternalData("stun/intent_stunned", 16 /*0x10*/)
    },
    {
      nameof (summon),
      new IntentAnimData.InternalData("summon/intent_summon", 25)
    },
    {
      nameof (unknown),
      new IntentAnimData.InternalData("unknown/intent_unknown", 30)
    }
  };

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return IntentAnimData._data.Values.SelectMany<IntentAnimData.InternalData, string>((Func<IntentAnimData.InternalData, IEnumerable<string>>) (v => (IEnumerable<string>) v.frames));
    }
  }

  public static string GetAnimationFrame(string animation, int frame)
  {
    return IntentAnimData._data[animation].frames[frame];
  }

  public static int GetAnimationFrameCount(string animation)
  {
    return IntentAnimData._data[animation].frames.Length;
  }

  private struct InternalData
  {
    public string[] frames;

    public InternalData(string prefix, int frameCount)
    {
      this.frames = new string[frameCount];
      for (int index = 0; index < frameCount; ++index)
        this.frames[index] = ImageHelper.GetImagePath($"atlases/intent_atlas.sprites/{prefix}_{index.ToString().PadLeft(2, '0')}.tres");
    }

    public InternalData(string singleFrameName)
    {
      this.frames = new string[1];
      this.frames[0] = ImageHelper.GetImagePath($"atlases/intent_atlas.sprites/{singleFrameName}.tres");
    }
  }
}
