// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.PotionReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class PotionReward : Reward
{
  private bool _wasTaken;
  private Control? _icon;

  protected override RewardType RewardType => RewardType.Potion;

  public override int RewardsSetIndex => 2;

  public PotionModel? Potion { get; private set; }

  public override Vector2 IconPosition => new Vector2(0.0f, -2f);

  public override LocString Description => this.Potion.Title;

  public PotionModel? ClaimedPotion { get; private set; }

  protected override IEnumerable<IHoverTip> ExtraHoverTips => this.Potion.HoverTips;

  public PotionReward(Player player)
    : base(player)
  {
  }

  public PotionReward(PotionModel potion, Player player)
    : base(player)
  {
    potion.AssertMutable();
    this.Potion = potion;
  }

  public override bool IsPopulated => this.Potion != null;

  public override void Populate()
  {
    Rng rng = this._rngOverride ?? this.Player.PlayerRng.Rewards;
    if (this.Potion != null)
      return;
    this.Potion = PotionFactory.CreateRandomPotionOutOfCombat(this.Player, rng).ToMutable();
  }

  public override Control? CreateIcon()
  {
    if (TestMode.IsOn)
      return (Control) null;
    if (this._icon == null)
      this._icon = (Control) NPotion.Create(this.Potion);
    return this._icon;
  }

  protected override async Task<bool> OnSelect()
  {
    PotionProcureResult procure = await PotionCmd.TryToProcure(this.Potion, this.Player);
    if (procure.success)
    {
      Log.Info($"Player {this.Player.NetId} obtained {procure.potion.Id} from potion reward");
      this.ClaimedPotion = this.Potion;
      this._wasTaken = true;
      return true;
    }
    if (procure.failureReason == PotionProcureFailureReason.TooFull)
      return false;
    this.ClaimedPotion = this.Potion;
    this._wasTaken = true;
    return true;
  }

  public override void OnSkipped()
  {
    if (this._wasTaken)
      return;
    this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).PotionChoices.Add(new ModelChoiceHistoryEntry(this.Potion.Id, false));
  }

  public override void MarkContentAsSeen() => SaveManager.Instance.MarkPotionAsSeen(this.Potion);
}
