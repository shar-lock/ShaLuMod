// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NSearchBar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NSearchBar.cs")]
public class NSearchBar : Control
{
  private LineEdit _textArea;
  private NButton _clearButton;
  private 
  #nullable disable
  NSearchBar.QueryChangedEventHandler backing_QueryChanged;
  private NSearchBar.QuerySubmittedEventHandler backing_QuerySubmitted;

  [GeneratedRegex("[\\t\\r\\n]")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static 
  #nullable enable
  Regex NonSpaceWhitespaceCharacters()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5.Instance;
  }

  [GeneratedRegex("\\s{2,}")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex ConsecutiveSpaces()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6.Instance;
  }

  [GeneratedRegex("<.*?>")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex HtmlTags()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7.Instance;
  }

  public string Text => this._textArea.Text;

  public LineEdit TextArea => this._textArea;

  public override void _Ready()
  {
    this._textArea = ((Node) this).GetNode<LineEdit>(NodePath.op_Implicit("TextArea"));
    ((GodotObject) this._textArea).Connect(LineEdit.SignalName.TextChanged, Callable.From<string>(new Action<string>(this.TextUpdated)), 0U);
    ((GodotObject) this._textArea).Connect(LineEdit.SignalName.TextSubmitted, Callable.From<string>(new Action<string>(this.TextSubmitted)), 0U);
    this._textArea.SetPlaceholder(new LocString("card_library", "SEARCH_PLACEHOLDER").GetRawText());
    this._clearButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("ClearButton"));
    ((GodotObject) this._clearButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ClearText)), 0U);
  }

  private void TextUpdated(string _)
  {
    ((GodotObject) this).EmitSignal(NSearchBar.SignalName.QueryChanged, new Variant[1]
    {
      Variant.op_Implicit(this._textArea.Text)
    });
  }

  private void TextSubmitted(string _)
  {
    ((GodotObject) this).EmitSignal(NSearchBar.SignalName.QuerySubmitted, new Variant[1]
    {
      Variant.op_Implicit(this._textArea.Text)
    });
  }

  private void ClearText(NButton _) => this.ClearText();

  public void ClearText()
  {
    ((Control) this._textArea).TryGrabFocus();
    if (string.IsNullOrWhiteSpace(this._textArea.Text))
      return;
    this._textArea.Text = "";
    ((GodotObject) this).EmitSignal(NSearchBar.SignalName.QueryChanged, new Variant[1]
    {
      Variant.op_Implicit(this._textArea.Text)
    });
  }

  public static string Normalize(string text)
  {
    string input = NSearchBar.NonSpaceWhitespaceCharacters().Replace(text.Trim(), " ");
    return NSearchBar.ConsecutiveSpaces().Replace(input, " ").ToLowerInvariant();
  }

  public static string RemoveHtmlTags(string text)
  {
    return NSearchBar.HtmlTags().Replace(text, string.Empty);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NSearchBar.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSearchBar.MethodName.TextUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSearchBar.MethodName.TextSubmitted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSearchBar.MethodName.ClearText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSearchBar.MethodName.ClearText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSearchBar.MethodName.Normalize, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSearchBar.MethodName.RemoveHtmlTags, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSearchBar.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.TextUpdated) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TextUpdated(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.TextSubmitted) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TextSubmitted(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.ClearText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ClearText(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.ClearText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.Normalize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NSearchBar.Normalize(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (!StringName.op_Equality(ref method, NSearchBar.MethodName.RemoveHtmlTags) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    string str1 = NSearchBar.RemoveHtmlTags(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref str1);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.Normalize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NSearchBar.Normalize(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NSearchBar.MethodName.RemoveHtmlTags) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NSearchBar.RemoveHtmlTags(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSearchBar.MethodName._Ready) || StringName.op_Equality(ref method, NSearchBar.MethodName.TextUpdated) || StringName.op_Equality(ref method, NSearchBar.MethodName.TextSubmitted) || StringName.op_Equality(ref method, NSearchBar.MethodName.ClearText) || StringName.op_Equality(ref method, NSearchBar.MethodName.Normalize) || StringName.op_Equality(ref method, NSearchBar.MethodName.RemoveHtmlTags) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSearchBar.PropertyName._textArea))
    {
      this._textArea = VariantUtils.ConvertTo<LineEdit>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSearchBar.PropertyName._clearButton))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._clearButton = VariantUtils.ConvertTo<NButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSearchBar.PropertyName.Text))
    {
      ref godot_variant local = ref value;
      string text = this.Text;
      godot_variant from = VariantUtils.CreateFrom<string>(ref text);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSearchBar.PropertyName.TextArea))
    {
      ref godot_variant local = ref value;
      LineEdit textArea = this.TextArea;
      godot_variant from = VariantUtils.CreateFrom<LineEdit>(ref textArea);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSearchBar.PropertyName._textArea))
    {
      value = VariantUtils.CreateFrom<LineEdit>(ref this._textArea);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSearchBar.PropertyName._clearButton))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NButton>(ref this._clearButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSearchBar.PropertyName._textArea, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSearchBar.PropertyName._clearButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NSearchBar.PropertyName.Text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSearchBar.PropertyName.TextArea, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSearchBar.PropertyName._textArea, Variant.From<LineEdit>(ref this._textArea));
    info.AddProperty(NSearchBar.PropertyName._clearButton, Variant.From<NButton>(ref this._clearButton));
    info.AddSignalEventDelegate(NSearchBar.SignalName.QueryChanged, (Delegate) this.backing_QueryChanged);
    info.AddSignalEventDelegate(NSearchBar.SignalName.QuerySubmitted, (Delegate) this.backing_QuerySubmitted);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSearchBar.PropertyName._textArea, ref variant1))
      this._textArea = ((Variant) ref variant1).As<LineEdit>();
    Variant variant2;
    if (info.TryGetProperty(NSearchBar.PropertyName._clearButton, ref variant2))
      this._clearButton = ((Variant) ref variant2).As<NButton>();
    NSearchBar.QueryChangedEventHandler changedEventHandler;
    if (info.TryGetSignalEventDelegate<NSearchBar.QueryChangedEventHandler>(NSearchBar.SignalName.QueryChanged, ref changedEventHandler))
      this.backing_QueryChanged = changedEventHandler;
    NSearchBar.QuerySubmittedEventHandler submittedEventHandler;
    if (!info.TryGetSignalEventDelegate<NSearchBar.QuerySubmittedEventHandler>(NSearchBar.SignalName.QuerySubmitted, ref submittedEventHandler))
      return;
    this.backing_QuerySubmitted = submittedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NSearchBar.SignalName.QueryChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("query"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSearchBar.SignalName.QuerySubmitted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("query"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  public event NSearchBar.QueryChangedEventHandler QueryChanged
  {
    add => this.backing_QueryChanged += value;
    remove => this.backing_QueryChanged -= value;
  }

  protected void EmitSignalQueryChanged(string query)
  {
    ((GodotObject) this).EmitSignal(NSearchBar.SignalName.QueryChanged, new Variant[1]
    {
      Variant.op_Implicit(query)
    });
  }

  public event NSearchBar.QuerySubmittedEventHandler QuerySubmitted
  {
    add => this.backing_QuerySubmitted += value;
    remove => this.backing_QuerySubmitted -= value;
  }

  protected void EmitSignalQuerySubmitted(string query)
  {
    ((GodotObject) this).EmitSignal(NSearchBar.SignalName.QuerySubmitted, new Variant[1]
    {
      Variant.op_Implicit(query)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NSearchBar.SignalName.QueryChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSearchBar.QueryChangedEventHandler backingQueryChanged = this.backing_QueryChanged;
      if (backingQueryChanged == null)
        return;
      backingQueryChanged(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NSearchBar.SignalName.QuerySubmitted) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSearchBar.QuerySubmittedEventHandler backingQuerySubmitted = this.backing_QuerySubmitted;
      if (backingQuerySubmitted == null)
        return;
      backingQuerySubmitted(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NSearchBar.SignalName.QueryChanged) || StringName.op_Equality(ref signal, NSearchBar.SignalName.QuerySubmitted) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void QueryChangedEventHandler(
  #nullable enable
  string query);

  [Signal]
  public delegate void QuerySubmittedEventHandler(string query);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName TextUpdated = StringName.op_Implicit(nameof (TextUpdated));
    public static readonly StringName TextSubmitted = StringName.op_Implicit(nameof (TextSubmitted));
    public static readonly StringName ClearText = StringName.op_Implicit(nameof (ClearText));
    public static readonly StringName Normalize = StringName.op_Implicit(nameof (Normalize));
    public static readonly StringName RemoveHtmlTags = StringName.op_Implicit(nameof (RemoveHtmlTags));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Text = StringName.op_Implicit(nameof (Text));
    public static readonly StringName TextArea = StringName.op_Implicit(nameof (TextArea));
    public static readonly StringName _textArea = StringName.op_Implicit(nameof (_textArea));
    public static readonly StringName _clearButton = StringName.op_Implicit(nameof (_clearButton));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName QueryChanged = StringName.op_Implicit(nameof (QueryChanged));
    public static readonly StringName QuerySubmitted = StringName.op_Implicit(nameof (QuerySubmitted));
  }
}
