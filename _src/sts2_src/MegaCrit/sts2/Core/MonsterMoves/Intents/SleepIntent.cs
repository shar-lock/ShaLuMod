// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.SleepIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class SleepIntent : AbstractIntent
{
  public override IntentType IntentType => IntentType.Sleep;

  protected override string IntentPrefix => "SLEEP";

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_sleep.tres";
}
