// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public abstract class PlayerChoiceContext
{
  private Stack<AbstractModel>? _modelStack;

  public IEnumerable<AbstractModel>? ModelStack => (IEnumerable<AbstractModel>) this._modelStack;

  public AbstractModel? LastInvolvedModel
  {
    get
    {
      Stack<AbstractModel> modelStack = this._modelStack;
      AbstractModel abstractModel;
      return (modelStack != null ? (modelStack.TryPeek(ref abstractModel) ? 1 : 0) : 0) == 0 ? (AbstractModel) null : abstractModel;
    }
  }

  public abstract ulong? OwnerId { get; }

  public void PushModel(AbstractModel model)
  {
    if (this._modelStack == null)
      this._modelStack = new Stack<AbstractModel>();
    this._modelStack.Push(model);
  }

  public void PopModel(AbstractModel model)
  {
    AbstractModel abstractModel = (AbstractModel) null;
    if (this._modelStack == null || !this._modelStack.TryPeek(ref abstractModel) || abstractModel != model)
      Log.Error($"Tried to pop model {model} from stack of player choice context {this} but {abstractModel} was on the top of the stack instead! (Stack size: {this._modelStack?.Count})");
    else
      this._modelStack.Pop();
  }

  public abstract Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options);

  public abstract Task SignalPlayerChoiceEnded();
}
