// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NFeedbackScreenOpener
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NFeedbackScreenOpener.cs")]
public class NFeedbackScreenOpener : Node
{
  public static NFeedbackScreenOpener Instance { get; private set; }

  public override void _EnterTree() => NFeedbackScreenOpener.Instance = this;

  public override void _ExitTree() => NFeedbackScreenOpener.Instance = (NFeedbackScreenOpener) null;

  public override void _Input(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventKey inputEventKey) || !inputEventKey.Pressed || inputEventKey.Keycode != 4194333L || ((CanvasItem) NGame.Instance.GetOrCreateFeedbackScreen()).Visible || NCapstoneContainer.Instance?.CurrentCapstoneScreen is NCapstoneSubmenuStack currentCapstoneScreen && currentCapstoneScreen.ScreenType == NetScreenType.Feedback)
      return;
    TaskHelper.RunSafely(this.OpenFeedbackScreen());
  }

  public async Task OpenFeedbackScreen()
  {
    Image screenshot = ((Texture2D) this.GetViewport().GetTexture()).GetImage();
    double num = (double) await this.AwaitProcessFrame();
    NGame.Instance.GetInspectCardScreen().Close();
    NGame.Instance.GetInspectRelicScreen().Close();
    NSendFeedbackScreen feedbackScreen = NGame.Instance.GetOrCreateFeedbackScreen();
    feedbackScreen.SetScreenshot(screenshot);
    feedbackScreen.Open();
    screenshot = (Image) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NFeedbackScreenOpener.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFeedbackScreenOpener.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFeedbackScreenOpener.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFeedbackScreenOpener.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFeedbackScreenOpener.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFeedbackScreenOpener.MethodName._Input) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFeedbackScreenOpener.MethodName._EnterTree) || StringName.op_Equality(ref method, NFeedbackScreenOpener.MethodName._ExitTree) || StringName.op_Equality(ref method, NFeedbackScreenOpener.MethodName._Input) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
  }

  public class PropertyName : Node.PropertyName
  {
  }

  public class SignalName : Node.SignalName
  {
  }
}
