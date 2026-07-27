// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.StunIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class StunIntent : AbstractIntent
{
  private const string _intentPrefix = "STUN";
  private const string _atlasName = "intent_atlas";
  private const string _spriteName = "intent_stun";
  private const string _spritePath = "atlases/intent_atlas.sprites/intent_stun.tres";

  public override IntentType IntentType => IntentType.Stun;

  protected override string IntentPrefix => "STUN";

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_stun.tres";

  public static HoverTip GetStaticHoverTip()
  {
    if (!AtlasManager.IsAtlasLoaded("intent_atlas"))
      AtlasManager.LoadAtlas("intent_atlas");
    return new HoverTip(new LocString("intents", "STUN.title"), new LocString("intents", "STUN.description"), (Texture2D) AtlasManager.GetSprite("intent_atlas", "intent_stun"));
  }
}
