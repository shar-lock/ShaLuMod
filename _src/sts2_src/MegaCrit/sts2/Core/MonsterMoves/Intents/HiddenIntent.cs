// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.HiddenIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class HiddenIntent : AbstractIntent
{
  public override IntentType IntentType => IntentType.Hidden;

  protected override string IntentPrefix => "HIDDEN";

  protected override string? SpritePath => (string) null;

  public override bool HasIntentTip => false;
}
