// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.DebuffIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class DebuffIntent : AbstractIntent
{
  private readonly bool _strong;

  public override IntentType IntentType
  {
    get => !this._strong ? IntentType.Debuff : IntentType.DebuffStrong;
  }

  protected override string IntentPrefix => "DEBUFF";

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_debuff.tres";

  public DebuffIntent(bool strong = false) => this._strong = strong;
}
