// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NSceneContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NSceneContainer.cs")]
public class NSceneContainer : Control
{
  private Control? _currentScene;

  public Control? CurrentScene
  {
    get
    {
      if (this._currentScene == null)
        return (Control) null;
      if (!GodotObject.IsInstanceValid((GodotObject) this._currentScene))
        return (Control) null;
      return ((GodotObject) this._currentScene).IsQueuedForDeletion() ? (Control) null : this._currentScene;
    }
    private set => this._currentScene = value;
  }

  public void SetCurrentScene(Control node)
  {
    foreach (Node child in ((Node) this).GetChildren(false))
    {
      ((Node) this).RemoveChildSafely(child);
      child.QueueFreeSafely();
    }
    this.CurrentScene = node;
    if (((Node) node).GetParent() == null)
      ((Node) this).AddChildSafely((Node) node);
    else
      ((Node) node).Reparent((Node) this, true);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NSceneContainer.MethodName.SetCurrentScene, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NSceneContainer.MethodName.SetCurrentScene) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetCurrentScene(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSceneContainer.MethodName.SetCurrentScene) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSceneContainer.PropertyName.CurrentScene))
    {
      this.CurrentScene = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSceneContainer.PropertyName._currentScene))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentScene = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSceneContainer.PropertyName.CurrentScene))
    {
      ref godot_variant local = ref value;
      Control currentScene = this.CurrentScene;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref currentScene);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NSceneContainer.PropertyName._currentScene))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._currentScene);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSceneContainer.PropertyName._currentScene, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSceneContainer.PropertyName.CurrentScene, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName currentScene1 = NSceneContainer.PropertyName.CurrentScene;
    Control currentScene2 = this.CurrentScene;
    Variant variant = Variant.From<Control>(ref currentScene2);
    serializationInfo.AddProperty(currentScene1, variant);
    info.AddProperty(NSceneContainer.PropertyName._currentScene, Variant.From<Control>(ref this._currentScene));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSceneContainer.PropertyName.CurrentScene, ref variant1))
      this.CurrentScene = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (!info.TryGetProperty(NSceneContainer.PropertyName._currentScene, ref variant2))
      return;
    this._currentScene = ((Variant) ref variant2).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName SetCurrentScene = StringName.op_Implicit(nameof (SetCurrentScene));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CurrentScene = StringName.op_Implicit(nameof (CurrentScene));
    public static readonly StringName _currentScene = StringName.op_Implicit(nameof (_currentScene));
  }

  public class SignalName : Control.SignalName
  {
  }
}
