// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Validation.DeserializationContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Validation;

public sealed class DeserializationContext
{
  private readonly List<ValidationError> _errors = new List<ValidationError>();
  private readonly Stack<string> _pathSegments = new Stack<string>();

  public void PushPath(string segment) => this._pathSegments.Push(segment);

  public void PopPath() => this._pathSegments.Pop();

  private string CurrentPath => string.Join(".", this._pathSegments.Reverse<string>());

  public void Warn(string message)
  {
    this._errors.Add(new ValidationError(ValidationSeverity.Warning, this.CurrentPath, message));
  }

  public void Fatal(string message)
  {
    this._errors.Add(new ValidationError(ValidationSeverity.Fatal, this.CurrentPath, message));
  }

  public IReadOnlyList<ValidationError> Errors => (IReadOnlyList<ValidationError>) this._errors;

  public bool HasFatal
  {
    get => this._errors.Any<ValidationError>((Func<ValidationError, bool>) (e => e.IsFatal));
  }

  public int WarningCount
  {
    get => this._errors.Count<ValidationError>((Func<ValidationError, bool>) (e => !e.IsFatal));
  }

  public int FatalCount
  {
    get => this._errors.Count<ValidationError>((Func<ValidationError, bool>) (e => e.IsFatal));
  }
}
