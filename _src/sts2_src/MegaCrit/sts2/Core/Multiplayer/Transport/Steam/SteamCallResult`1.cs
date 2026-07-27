// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamCallResult`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Steamworks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;

public class SteamCallResult<T> : IDisposable where T : struct
{
  private readonly TaskCompletionSource<T> _completionSource = new TaskCompletionSource<T>();
  private readonly CallResult<T> _callResult;
  private readonly CancellationTokenRegistration? _cancelTokenRegistration;

  public System.Threading.Tasks.Task<T> Task => this._completionSource.Task;

  public SteamCallResult(SteamAPICall_t call, CancellationToken cancelToken = default (CancellationToken))
  {
    // ISSUE: method pointer
    this._callResult = CallResult<T>.Create(new CallResult<T>.APIDispatchDelegate((object) this, __methodptr(OnCallResult)));
    this._callResult.Set(call, (CallResult<T>.APIDispatchDelegate) null);
    if (!cancelToken.CanBeCanceled)
      return;
    this._cancelTokenRegistration = new CancellationTokenRegistration?(cancelToken.Register(new Action(this.Cancel)));
  }

  public void Cancel()
  {
    this._callResult.Cancel();
    this._completionSource.TrySetCanceled();
    ref readonly CancellationTokenRegistration? local = ref this._cancelTokenRegistration;
    if (!local.HasValue)
      return;
    local.GetValueOrDefault().Dispose();
  }

  private void OnCallResult(T result, bool ioError)
  {
    if (ioError)
      this._completionSource.SetException((Exception) new IOException($"Got IO failure from CallResult of type {typeof (T)}!"));
    else
      this._completionSource.SetResult(result);
    ref readonly CancellationTokenRegistration? local = ref this._cancelTokenRegistration;
    if (!local.HasValue)
      return;
    local.GetValueOrDefault().Dispose();
  }

  public void Dispose()
  {
    this._callResult.Dispose();
    ref readonly CancellationTokenRegistration? local = ref this._cancelTokenRegistration;
    if (!local.HasValue)
      return;
    local.GetValueOrDefault().Dispose();
  }
}
