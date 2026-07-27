// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Orbs.OrbQueue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Orbs;

public class OrbQueue
{
  public const int maxCapacity = 10;
  private readonly Player _owner;
  private readonly List<OrbModel> _orbs = new List<OrbModel>();

  public IReadOnlyList<OrbModel> Orbs => (IReadOnlyList<OrbModel>) this._orbs;

  public int Capacity { get; private set; }

  public OrbQueue(Player owner) => this._owner = owner;

  public void Clear()
  {
    this._orbs.Clear();
    this.Capacity = 0;
  }

  public void AddCapacity(int capacity) => this.Capacity += capacity;

  public void RemoveCapacity(int capacity)
  {
    this.Capacity = Math.Max(0, this.Capacity - capacity);
    while (this.Orbs.Count > this.Capacity)
      this.Remove(this._orbs.Last<OrbModel>());
  }

  public async Task<bool> TryEnqueue(OrbModel orb)
  {
    if (this.Capacity == 0)
      return false;
    orb.AssertMutable();
    if (this.Orbs.Count >= this.Capacity)
      throw new InvalidOperationException("OrbQueue is full");
    this._orbs.Add(orb);
    await this.SmallWait();
    return true;
  }

  public bool Remove(OrbModel orb) => this._orbs.Remove(orb);

  public void Insert(int idx, OrbModel orb)
  {
    if (idx >= this.Capacity)
      throw new InvalidOperationException("idx cannot be greater than capacity");
    this._orbs.Insert(idx, orb);
  }

  public async Task BeforeTurnEnd(PlayerChoiceContext choiceContext)
  {
    foreach (OrbModel orbModel in this.Orbs.ToList<OrbModel>())
    {
      if (this._owner.Creature.CombatState == null)
        break;
      await orbModel.BeforeTurnEndOrbTrigger(choiceContext);
    }
  }

  public async Task AfterTurnStart(PlayerChoiceContext choiceContext)
  {
    foreach (OrbModel orbModel in this.Orbs.ToList<OrbModel>())
    {
      if (this._owner.Creature.CombatState == null)
        break;
      await orbModel.AfterTurnStartOrbTrigger(choiceContext);
    }
  }

  private async Task SmallWait()
  {
    if (LocalContext.IsMe(this._owner))
      await Cmd.CustomScaledWait(0.1f, 0.25f);
    else
      await Cmd.Wait(0.05f);
  }
}
