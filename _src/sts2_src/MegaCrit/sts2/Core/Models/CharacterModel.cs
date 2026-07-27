// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CharacterModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class CharacterModel : AbstractModel
{
  public const string locTable = "characters";
  protected const string _relaxedTrigger = "Relaxed";
  protected const string _idleTrigger = "Idle";
  public const string relaxedAnim = "relaxed_loop";

  public virtual bool IsPlayable => true;

  public LocString Title => new LocString("characters", this.Id.Entry + ".title");

  public LocString TitleObject => new LocString("characters", this.Id.Entry + ".titleObject");

  public abstract Color NameColor { get; }

  public abstract CharacterGender Gender { get; }

  protected abstract CharacterModel? UnlocksAfterRunAs { get; }

  public LocString BestiarySeenQuote
  {
    get => new LocString("characters", this.Id.Entry + ".bestiaryQuote");
  }

  public LocString? BestiaryKillQuote
  {
    get => LocString.GetIfExists("characters", this.Id.Entry + ".bestiaryKillQuote");
  }

  public LocString PronounObject => new LocString("characters", this.Id.Entry + ".pronounObject");

  public LocString PossessiveAdjective
  {
    get => new LocString("characters", this.Id.Entry + ".possessiveAdjective");
  }

  public LocString PronounPossessive
  {
    get => new LocString("characters", this.Id.Entry + ".pronounPossessive");
  }

  public LocString PronounSubject => new LocString("characters", this.Id.Entry + ".pronounSubject");

  public LocString CardsModifierTitle
  {
    get => new LocString("characters", this.Id.Entry + ".cardsModifierTitle");
  }

  public LocString CardsModifierDescription
  {
    get => new LocString("characters", this.Id.Entry + ".cardsModifierDescription");
  }

  public LocString EventDeathPreventionLine
  {
    get => new LocString("characters", this.Id.Entry + ".eventDeathPrevention");
  }

  public string TrailPath
  {
    get => SceneHelper.GetScenePath("vfx/card_trail_" + this.Id.Entry.ToLowerInvariant());
  }

  public abstract int StartingHp { get; }

  public abstract int StartingGold { get; }

  public virtual int MaxEnergy => 3;

  public virtual Color EnergyLabelOutlineColor => new Color("0000000D");

  public virtual int BaseOrbSlotCount => 0;

  public virtual bool ShouldAlwaysShowStarCounter => false;

  public abstract CardPoolModel CardPool { get; }

  public abstract RelicPoolModel RelicPool { get; }

  public abstract PotionPoolModel PotionPool { get; }

  public abstract IEnumerable<CardModel> StartingDeck { get; }

  public abstract IReadOnlyList<RelicModel> StartingRelics { get; }

  public virtual IReadOnlyList<PotionModel> StartingPotions
  {
    get => (IReadOnlyList<PotionModel>) Array.Empty<PotionModel>();
  }

  private string VisualsPath
  {
    get => SceneHelper.GetScenePath("creature_visuals/" + this.Id.Entry.ToLowerInvariant());
  }

  public NCreatureVisuals CreateVisuals()
  {
    return PreloadManager.Cache.GetScene(this.VisualsPath).Instantiate<NCreatureVisuals>((PackedScene.GenEditState) 0L);
  }

  private string IconTexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/top_panel/character_icon_{this.Id.Entry.ToLowerInvariant()}.png");
    }
  }

  public Texture2D IconTexture => PreloadManager.Cache.GetTexture2D(this.IconTexturePath);

  private string IconOutlineTexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/top_panel/character_icon_{this.Id.Entry.ToLowerInvariant()}_outline.png");
    }
  }

  public Texture2D IconOutlineTexture
  {
    get => PreloadManager.Cache.GetTexture2D(this.IconOutlineTexturePath);
  }

  protected virtual string IconPath
  {
    get => SceneHelper.GetScenePath($"ui/character_icons/{this.Id.Entry.ToLowerInvariant()}_icon");
  }

  public Control Icon
  {
    get
    {
      return PreloadManager.Cache.GetScene(this.IconPath).Instantiate<Control>((PackedScene.GenEditState) 0L);
    }
  }

  public string EnergyCounterPath
  {
    get
    {
      return SceneHelper.GetScenePath($"combat/energy_counters/{this.Id.Entry.ToLowerInvariant()}_energy_counter");
    }
  }

  public string MerchantAnimPath
  {
    get
    {
      return SceneHelper.GetScenePath($"merchant/characters/{this.Id.Entry.ToLowerInvariant()}_merchant");
    }
  }

  public string RestSiteAnimPath
  {
    get
    {
      return SceneHelper.GetScenePath($"rest_site/characters/{this.Id.Entry.ToLowerInvariant()}_rest_site");
    }
  }

  private string ArmPointingTexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/hands/multiplayer_hand_{this.Id.Entry.ToLowerInvariant()}_point.png");
    }
  }

  public Texture2D ArmPointingTexture
  {
    get => PreloadManager.Cache.GetTexture2D(this.ArmPointingTexturePath);
  }

  private string ArmRockTexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/hands/multiplayer_hand_{this.Id.Entry.ToLowerInvariant()}_rock.png");
    }
  }

  public Texture2D ArmRockTexture => PreloadManager.Cache.GetTexture2D(this.ArmRockTexturePath);

  private string ArmPaperTexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/hands/multiplayer_hand_{this.Id.Entry.ToLowerInvariant()}_paper.png");
    }
  }

  public Texture2D ArmPaperTexture => PreloadManager.Cache.GetTexture2D(this.ArmPaperTexturePath);

  private string ArmScissorsTexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/hands/multiplayer_hand_{this.Id.Entry.ToLowerInvariant()}_scissors.png");
    }
  }

  public Texture2D ArmScissorsTexture
  {
    get => PreloadManager.Cache.GetTexture2D(this.ArmScissorsTexturePath);
  }

  public Achievement RunWonAchievement
  {
    get => Enum.Parse<Achievement>(StringExtensions.Capitalize(this.Id.Entry) + "Win");
  }

  protected virtual IEnumerable<string> ExtraAssetPaths
  {
    get => (IEnumerable<string>) Array.Empty<string>();
  }

  public string CharacterSelectTitle => this.Id.Entry.ToUpperInvariant() + ".title";

  public string CharacterSelectDesc => this.Id.Entry.ToUpperInvariant() + ".description";

  public string CharacterSelectBg
  {
    get
    {
      return SceneHelper.GetScenePath("screens/char_select/char_select_bg_" + this.Id.Entry.ToLowerInvariant());
    }
  }

  protected virtual string CharacterSelectIconPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/character_select/char_select_{this.Id.Entry.ToLowerInvariant()}.png");
    }
  }

  public CompressedTexture2D CharacterSelectIcon
  {
    get
    {
      return ResourceLoader.Load<CompressedTexture2D>(this.CharacterSelectIconPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  protected virtual string CharacterSelectLockedIconPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/character_select/char_select_{this.Id.Entry.ToLowerInvariant()}_locked.png");
    }
  }

  public CompressedTexture2D CharacterSelectLockedIcon
  {
    get
    {
      return ResourceLoader.Load<CompressedTexture2D>(this.CharacterSelectLockedIconPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public string CharacterSelectTransitionPath
  {
    get => $"res://materials/transitions/{this.Id.Entry.ToLowerInvariant()}_transition_mat.tres";
  }

  protected virtual string MapMarkerPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/map/icons/map_marker_{this.Id.Entry.ToLowerInvariant()}.png");
    }
  }

  public CompressedTexture2D MapMarker
  {
    get => PreloadManager.Cache.GetCompressedTexture2D(this.MapMarkerPath);
  }

  public virtual Color DialogueColor { get; } = new Color("28454f");

  public virtual VfxColor SpeechBubbleColor { get; } = VfxColor.Cyan;

  public virtual Color MapDrawingColor => Colors.Black;

  public virtual Color RemoteTargetingLineColor => Colors.Black;

  public virtual Color RemoteTargetingLineOutline => Colors.Black;

  public IEnumerable<string> AssetPathsCharacterSelect
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[5]
      {
        this.CharacterSelectBg,
        this.CharacterSelectIconPath,
        this.IconTexturePath,
        this.CharacterSelectLockedIconPath,
        this.CharacterSelectTransitionPath
      });
    }
  }

  public IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<string>) new string[9]
      {
        this.VisualsPath,
        this.IconTexturePath,
        this.IconPath,
        this.EnergyCounterPath,
        this.RestSiteAnimPath,
        this.MerchantAnimPath,
        this.CharacterSelectTransitionPath,
        this.MapMarkerPath,
        this.TrailPath
      }).Concat<string>(this.ExtraAssetPaths);
    }
  }

  public abstract float AttackAnimDelay { get; }

  public abstract float CastAnimDelay { get; }

  public virtual float PowerUpAnimDelay => this.CastAnimDelay;

  public abstract List<string> GetArchitectAttackVfx();

  public virtual string CharacterSelectSfx
  {
    get
    {
      return $"event:/sfx/characters/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_select";
    }
  }

  public string AttackSfx
  {
    get
    {
      return $"event:/sfx/characters/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_attack";
    }
  }

  public string CastSfx
  {
    get
    {
      return $"event:/sfx/characters/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_cast";
    }
  }

  public string PowerUpSfx => this.CastSfx;

  public string DeathSfx
  {
    get
    {
      return $"event:/sfx/characters/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_die";
    }
  }

  public virtual string CharacterTransitionSfx
  {
    get => "event:/sfx/ui/wipe_" + this.Id.Entry.ToLowerInvariant();
  }

  public virtual CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("relaxed_loop", true);
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state5.AddBranch("Idle", animState);
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("PowerUp", state1);
    animator.AddAnyState("Relaxed", state5);
    return animator;
  }

  public override bool ShouldReceiveCombatHooks => false;

  public void AddDetailsTo(LocString str)
  {
    str.Add("character", this.Title);
    str.Add("characterObject", this.TitleObject);
    str.Add("characterGender", this.Gender.ToString().ToLowerInvariant());
    str.Add("possessiveAdjective", this.PossessiveAdjective);
    str.Add("pronounObject", this.PronounObject);
    str.Add("pronounPossessive", this.PronounPossessive);
    str.Add("pronounSubject", this.PronounSubject);
  }

  public LocString GetUnlockText()
  {
    LocString unlockText = new LocString("characters", this.Id.Entry + ".unlockText");
    LocString locString = new LocString("characters", "LOCKED.title");
    LocString variable = this.UnlocksAfterRunAs != null ? (!SaveManager.Instance.GenerateUnlockStateFromProgress().Characters.Contains<CharacterModel>(this.UnlocksAfterRunAs) ? locString : this.UnlocksAfterRunAs.Title) : locString;
    unlockText.Add("Prerequisite", variable);
    return unlockText;
  }
}
