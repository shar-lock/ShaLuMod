// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Reaction.NReaction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Reaction;

[ScriptPath("res://src/Core/Nodes/Reaction/NReaction.cs")]
public class NReaction : TextureRect
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/reaction");
  private const string _exclamationPath = "res://images/ui/emote/exclaim.png";
  private const string _skullPath = "res://images/ui/emote/skull.png";
  private const string _thumbDownPath = "res://images/ui/emote/thumb_down.png";
  private const string _sadSlimePath = "res://images/ui/emote/slime_sad.png";
  private const string _questionMarkPath = "res://images/ui/emote/question.png";
  private const string _heartPath = "res://images/ui/emote/heart.png";
  private const string _thumbUpPath = "res://images/ui/emote/thumb_up.png";
  private const string _happyCultistPath = "res://images/ui/emote/happy_cultist.png";

  public ReactionType Type => NReaction.TextureToType(this.Texture);

  public static NReaction Create(Texture2D reactionTexture)
  {
    NReaction nreaction = PreloadManager.Cache.GetScene(NReaction._scenePath).Instantiate<NReaction>((PackedScene.GenEditState) 0L);
    nreaction.Texture = reactionTexture;
    return nreaction;
  }

  public static NReaction Create(ReactionType type)
  {
    return NReaction.Create(NReaction.TypeToTexture(type));
  }

  public void BeginAnim() => TaskHelper.RunSafely(this.DoAnim());

  private async Task DoAnim()
  {
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    ((CanvasItem) this).Modulate = modulate;
    float num1 = Rng.Chaotic.NextFloat(40f, 60f);
    float num2 = Rng.Chaotic.NextFloat(-30f, 30f);
    Vector2 position = ((Control) this).Position;
    Vector2 up = Vector2.Up;
    Vector2 vector2_1 = Vector2.op_Multiply(((Vector2) ref up).Rotated(Mathf.DegToRad(num2)), num1);
    Vector2 vector2_2 = Vector2.op_Addition(position, vector2_1);
    Tween tween = ((Node) this).CreateTween();
    tween.SetParallel(true);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(vector2_2), 0.30000001192092896).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.30000001192092896).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween.SetParallel(false);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.20000000298023224).SetDelay(0.60000002384185791).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L);
    bool flag = await tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  private static Texture2D TypeToTexture(ReactionType type)
  {
    AssetCache cache = PreloadManager.Cache;
    string path;
    switch (type)
    {
      case ReactionType.Exclamation:
        path = "res://images/ui/emote/exclaim.png";
        break;
      case ReactionType.Skull:
        path = "res://images/ui/emote/skull.png";
        break;
      case ReactionType.ThumbDown:
        path = "res://images/ui/emote/thumb_down.png";
        break;
      case ReactionType.SadSlime:
        path = "res://images/ui/emote/slime_sad.png";
        break;
      case ReactionType.QuestionMark:
        path = "res://images/ui/emote/question.png";
        break;
      case ReactionType.Heart:
        path = "res://images/ui/emote/heart.png";
        break;
      case ReactionType.ThumbUp:
        path = "res://images/ui/emote/thumb_up.png";
        break;
      case ReactionType.HappyCultist:
        path = "res://images/ui/emote/happy_cultist.png";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
    return cache.GetTexture2D(path);
  }

  private static ReactionType TextureToType(Texture2D texture)
  {
    string resourcePath = ((Resource) texture).ResourcePath;
    if (resourcePath != null)
    {
      switch (resourcePath.Length)
      {
        case 31 /*0x1F*/:
          switch (resourcePath[22])
          {
            case 'h':
              if (resourcePath == "res://images/ui/emote/heart.png")
                return ReactionType.Heart;
              break;
            case 's':
              if (resourcePath == "res://images/ui/emote/skull.png")
                return ReactionType.Skull;
              break;
          }
          break;
        case 33:
          if (resourcePath == "res://images/ui/emote/exclaim.png")
            return ReactionType.Exclamation;
          break;
        case 34:
          switch (resourcePath[22])
          {
            case 'q':
              if (resourcePath == "res://images/ui/emote/question.png")
                return ReactionType.QuestionMark;
              break;
            case 't':
              if (resourcePath == "res://images/ui/emote/thumb_up.png")
                return ReactionType.ThumbUp;
              break;
          }
          break;
        case 35:
          if (resourcePath == "res://images/ui/emote/slime_sad.png")
            return ReactionType.SadSlime;
          break;
        case 36:
          if (resourcePath == "res://images/ui/emote/thumb_down.png")
            return ReactionType.ThumbDown;
          break;
        case 39:
          if (resourcePath == "res://images/ui/emote/happy_cultist.png")
            return ReactionType.HappyCultist;
          break;
      }
    }
    throw new ArgumentOutOfRangeException(nameof (texture), (object) texture, (string) null);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NReaction.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("TextureRect"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("reactionTexture"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NReaction.MethodName.BeginAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReaction.MethodName.TypeToTexture, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("type"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NReaction.MethodName.TextureToType, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("texture"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NReaction.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NReaction nreaction = NReaction.Create(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NReaction>(ref nreaction);
      return true;
    }
    if (StringName.op_Equality(ref method, NReaction.MethodName.BeginAnim) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeginAnim();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReaction.MethodName.TypeToTexture) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Texture2D texture = NReaction.TypeToTexture(VariantUtils.ConvertTo<ReactionType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref texture);
      return true;
    }
    if (!StringName.op_Equality(ref method, NReaction.MethodName.TextureToType) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ReactionType type = NReaction.TextureToType(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<ReactionType>(ref type);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NReaction.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NReaction nreaction = NReaction.Create(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NReaction>(ref nreaction);
      return true;
    }
    if (StringName.op_Equality(ref method, NReaction.MethodName.TypeToTexture) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Texture2D texture = NReaction.TypeToTexture(VariantUtils.ConvertTo<ReactionType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref texture);
      return true;
    }
    if (StringName.op_Equality(ref method, NReaction.MethodName.TextureToType) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ReactionType type = NReaction.TextureToType(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<ReactionType>(ref type);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NReaction.MethodName.Create) || StringName.op_Equality(ref method, NReaction.MethodName.BeginAnim) || StringName.op_Equality(ref method, NReaction.MethodName.TypeToTexture) || StringName.op_Equality(ref method, NReaction.MethodName.TextureToType) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NReaction.PropertyName.Type))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    ref godot_variant local = ref value;
    ReactionType type = this.Type;
    godot_variant from = VariantUtils.CreateFrom<ReactionType>(ref type);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NReaction.PropertyName.Type, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName BeginAnim = StringName.op_Implicit(nameof (BeginAnim));
    public static readonly StringName TypeToTexture = StringName.op_Implicit(nameof (TypeToTexture));
    public static readonly StringName TextureToType = StringName.op_Implicit(nameof (TextureToType));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName Type = StringName.op_Implicit(nameof (Type));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
