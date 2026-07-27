// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.HoverTips.NHoverTipCardContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.HoverTips;

[ScriptPath("res://src/Core/Nodes/HoverTips/NHoverTipCardContainer.cs")]
public class NHoverTipCardContainer : Control
{
  private const string _cardHoverTipScenePath = "res://scenes/ui/card_hover_tip.tscn";
  private const float _padding = 4f;

  private IEnumerable<Control> Tips
  {
    get => ((IEnumerable) ((Node) this).GetChildren(false)).OfType<Control>();
  }

  public void Add(CardHoverTip cardTip)
  {
    Control child = PreloadManager.Cache.GetScene("res://scenes/ui/card_hover_tip.tscn").Instantiate<Control>((PackedScene.GenEditState) 0L);
    ((Node) this).AddChildSafely((Node) child);
    NCard node = ((Node) child).GetNode<NCard>(NodePath.op_Implicit("%Card"));
    node.Model = cardTip.Card;
    node.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
  }

  public void LayoutResizeAndReposition(Vector2 globalStartLocation, HoverTipAlignment alignment)
  {
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    Vector2 size = ((Rect2) ref viewportRect).Size;
    Vector2 vector2_1 = Vector2.Zero;
    Vector2 vector2_2 = Vector2.Zero;
    float num = 0.0f;
    foreach (Control tip in this.Tips)
    {
      tip.Position = vector2_2;
      vector2_1 = new Vector2(Mathf.Max(vector2_2.X + tip.Size.X, vector2_1.X), Mathf.Max(vector2_2.Y + tip.Size.Y, vector2_1.Y));
      vector2_2 = Vector2.op_Addition(vector2_2, Vector2.op_Multiply(Vector2.Down, tip.Size.Y + 4f));
      num = Mathf.Max(tip.Size.X, num);
    }
    switch (alignment)
    {
      case HoverTipAlignment.Left:
        this.GlobalPosition = Vector2.op_Addition(globalStartLocation, Vector2.op_Multiply(Vector2.Left, vector2_1.X));
        break;
      case HoverTipAlignment.Right:
        this.GlobalPosition = globalStartLocation;
        break;
    }
    this.Size = vector2_1;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NHoverTipCardContainer.MethodName.LayoutResizeAndReposition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("globalStartLocation"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("alignment"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NHoverTipCardContainer.MethodName.LayoutResizeAndReposition) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.LayoutResizeAndReposition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<HoverTipAlignment>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHoverTipCardContainer.MethodName.LayoutResizeAndReposition) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : Control.MethodName
  {
    public static readonly StringName LayoutResizeAndReposition = StringName.op_Implicit(nameof (LayoutResizeAndReposition));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
