// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSpineBinding
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public abstract class MegaSpineBinding : IDisposable
{
  public void Dispose()
  {
    this.BoundObject.Dispose();
    GC.SuppressFinalize((object) this);
  }

  public GodotObject BoundObject { get; private set; }

  protected abstract string SpineClassName { get; }

  protected virtual IEnumerable<string> SpineMethods => (IEnumerable<string>) Array.Empty<string>();

  protected virtual IEnumerable<string> SpineSignals => (IEnumerable<string>) Array.Empty<string>();

  protected MegaSpineBinding(Variant native)
  {
    if (((Variant) ref native).VariantType != 24L)
      throw new InvalidOperationException($"Expected a GodotObject but was {((Variant) ref native).VariantType}!");
    this.BoundObject = ((Variant) ref native).AsGodotObject();
    this.ValidateBoundObject();
  }

  protected Error Connect(string signalName, Callable callable)
  {
    return this.BoundObject.Connect(StringName.op_Implicit(signalName), callable, 0U);
  }

  protected void Disconnect(string signalName, Callable callable)
  {
    this.BoundObject.Disconnect(StringName.op_Implicit(signalName), callable);
  }

  protected Variant Call(string methodName, params Variant[] args)
  {
    if (!this.SpineMethods.Contains<string>(methodName))
      throw new InvalidOperationException($"You must add {methodName} to {this.GetType().Name}.SpineMethods before calling it!");
    Variant variant = this.BoundObject.Call(StringName.op_Implicit(methodName), args);
    GC.KeepAlive((object) this.BoundObject);
    GC.KeepAlive((object) args);
    return variant;
  }

  protected Variant? CallNullable(string methodName, params Variant[] args)
  {
    Variant variant = this.Call(methodName, args);
    return new Variant?(((Variant) ref variant).VariantType == null ? new Variant() : variant);
  }

  private void ValidateBoundObject()
  {
    if (this.BoundObject == null)
      return;
    if (this.BoundObject.GetClass() != this.SpineClassName)
      throw new InvalidOperationException($"Expected {"BoundObject"} to be a {this.SpineClassName}, but it is a {this.BoundObject.GetClass()}!");
    foreach (string spineMethod in this.SpineMethods)
    {
      if (!this.BoundObject.HasMethod(StringName.op_Implicit(spineMethod)))
        throw new InvalidOperationException($"{this.SpineClassName} does not have method {spineMethod}!");
    }
    foreach (string spineSignal in this.SpineSignals)
    {
      if (!this.BoundObject.HasSignal(StringName.op_Implicit(spineSignal)))
        throw new InvalidOperationException($"{this.SpineClassName} does not have signal {spineSignal}!");
    }
  }
}
