// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.HoverTips.HoverTip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

#nullable enable
namespace MegaCrit.Sts2.Core.HoverTips;

public record struct HoverTip : IHoverTip
{
  public string? Title { get; private set; }

  public Texture2D? Icon { get; private set; }

  public string Description { get; private set; }

  public string Id { get; set; }

  public bool IsSmart { get; set; }

  public bool IsDebuff { get; set; }

  public bool IsInstanced { get; set; }

  public AbstractModel? CanonicalModel { get; private set; }

  public bool ShouldOverrideTextOverflow { get; set; }

  public HoverTip(LocString description, Texture2D? icon = null)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CTitle\u003Ek__BackingField = (string) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsSmart\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsDebuff\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsInstanced\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CCanonicalModel\u003Ek__BackingField = (AbstractModel) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CShouldOverrideTextOverflow\u003Ek__BackingField = false;
    this.Id = $"LocString with Description={description.LocTable}.{description.LocEntryKey}";
    this.Description = description.GetFormattedText();
    this.Icon = icon;
  }

  public HoverTip(LocString title, LocString description, Texture2D? icon = null)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsSmart\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsDebuff\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsInstanced\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CCanonicalModel\u003Ek__BackingField = (AbstractModel) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CShouldOverrideTextOverflow\u003Ek__BackingField = false;
    this.Id = $"LocString with Title={title.LocTable}.{title.LocEntryKey} and Description={description.LocTable}.{description.LocEntryKey}";
    this.Title = title.GetFormattedText();
    this.Description = description.GetFormattedText();
    this.Icon = icon;
  }

  public HoverTip(LocString title, string description, Texture2D? icon = null)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsSmart\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsDebuff\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsInstanced\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CCanonicalModel\u003Ek__BackingField = (AbstractModel) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CShouldOverrideTextOverflow\u003Ek__BackingField = false;
    this.Id = $"LocString with Title={title.LocTable}.{title.LocEntryKey}";
    this.Title = title.GetFormattedText();
    this.Description = description;
    this.Icon = icon;
  }

  public HoverTip(AfflictionModel affliction, LocString description)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsSmart\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsInstanced\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CCanonicalModel\u003Ek__BackingField = (AbstractModel) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CShouldOverrideTextOverflow\u003Ek__BackingField = false;
    this.Id = affliction.Id.ToString();
    this.Title = affliction.Title.GetFormattedText();
    this.Description = description.GetFormattedText();
    this.Icon = (Texture2D) null;
    this.IsDebuff = true;
  }

  public HoverTip(OrbModel orb, LocString description)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsSmart\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsDebuff\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CIsInstanced\u003Ek__BackingField = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003CCanonicalModel\u003Ek__BackingField = (AbstractModel) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CShouldOverrideTextOverflow\u003Ek__BackingField = false;
    this.Id = orb.Id.ToString();
    this.Title = orb.Title.GetFormattedText();
    this.Description = description.GetFormattedText();
    this.Icon = (Texture2D) orb.Icon;
  }

  public HoverTip(PowerModel power, string description, bool isSmart)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CCanonicalModel\u003Ek__BackingField = (AbstractModel) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003CShouldOverrideTextOverflow\u003Ek__BackingField = false;
    this.Id = power.Id.ToString();
    this.Title = power.Title.GetFormattedText();
    this.Description = description;
    this.Icon = power.Icon;
    this.IsDebuff = power.Type == PowerType.Debuff;
    this.IsInstanced = power.InstanceType != 0;
    this.IsSmart = isSmart;
  }

  public void SetCanonicalModel(AbstractModel model)
  {
    model.AssertCanonical();
    this.CanonicalModel = model;
  }

  public static HoverTipAlignment GetHoverTipAlignment(Node2D node, float threshold = 0.75f)
  {
    double x = (double) node.GlobalPosition.X;
    Rect2 visibleRect = ((Node) node).GetViewport().GetVisibleRect();
    double num = (double) ((Rect2) ref visibleRect).Size.X * (double) threshold;
    return x <= num ? HoverTipAlignment.Right : HoverTipAlignment.Left;
  }

  public static HoverTipAlignment GetHoverTipAlignment(Control node, float threshold = 0.75f)
  {
    double x = (double) node.GlobalPosition.X;
    Rect2 visibleRect = ((Node) node).GetViewport().GetVisibleRect();
    double num = (double) ((Rect2) ref visibleRect).Size.X * (double) threshold;
    return x <= num ? HoverTipAlignment.Right : HoverTipAlignment.Left;
  }
}
