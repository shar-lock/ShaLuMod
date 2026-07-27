// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.HealIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class HealIntent : AbstractIntent
{
  public override IntentType IntentType => IntentType.Heal;

  protected override string IntentPrefix => "HEAL";

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_heal.tres";
}
