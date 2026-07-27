// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.AbstractModelSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Achievements;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Afflictions.Mocks;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Cards.Mocks;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Enchantments.Mocks;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Encounters.Mocks;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Events.Mocks;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Monsters.Mocks;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Orbs.Mock;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Potions.Mocks;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Powers.Mocks;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Models.Singleton;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.Models;

public static class AbstractModelSubtypes
{
  [DynamicallyAccessedMembers]
  private static readonly Type _t0 = typeof (Play20CardsSingleTurnAchievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1 = typeof (SkillIronclad1Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t2 = typeof (SkillIronclad2Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t3 = typeof (SkillNecrobinder1Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t4 = typeof (SkillNecrobinder2Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t5 = typeof (SkillRegent1Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t6 = typeof (SkillRegent2Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t7 = typeof (SkillSilent1Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t8 = typeof (SkillSilent2Achievement);
  [DynamicallyAccessedMembers]
  private static readonly Type _t9 = typeof (DeprecatedAct);
  [DynamicallyAccessedMembers]
  private static readonly Type _t10 = typeof (Glory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t11 = typeof (Hive);
  [DynamicallyAccessedMembers]
  private static readonly Type _t12 = typeof (Overgrowth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t13 = typeof (Underdocks);
  [DynamicallyAccessedMembers]
  private static readonly Type _t14 = typeof (Bound);
  [DynamicallyAccessedMembers]
  private static readonly Type _t15 = typeof (Entangled);
  [DynamicallyAccessedMembers]
  private static readonly Type _t16 = typeof (Galvanized);
  [DynamicallyAccessedMembers]
  private static readonly Type _t17 = typeof (Hexed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t18 = typeof (MockNoUnplayableAffliction);
  [DynamicallyAccessedMembers]
  private static readonly Type _t19 = typeof (MockSelfDamageAffliction);
  [DynamicallyAccessedMembers]
  private static readonly Type _t20 = typeof (MockUselessAffliction);
  [DynamicallyAccessedMembers]
  private static readonly Type _t21 = typeof (Ringing);
  [DynamicallyAccessedMembers]
  private static readonly Type _t22 = typeof (Smog);
  [DynamicallyAccessedMembers]
  private static readonly Type _t23 = typeof (Tainted);
  [DynamicallyAccessedMembers]
  private static readonly Type _t24 = typeof (CccComboModel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t25 = typeof (DebufferModel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t26 = typeof (ColorlessCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t27 = typeof (CurseCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t28 = typeof (DefectCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t29 = typeof (DeprecatedCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t30 = typeof (DeprivedCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t31 = typeof (EventCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t32 = typeof (IroncladCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t33 = typeof (MockCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t34 = typeof (NecrobinderCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t35 = typeof (QuestCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t36 = typeof (RegentCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t37 = typeof (SilentCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t38 = typeof (StatusCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t39 = typeof (TokenCardPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t40 = typeof (Abrasive);
  [DynamicallyAccessedMembers]
  private static readonly Type _t41 = typeof (Abundance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t42 = typeof (Accelerant);
  [DynamicallyAccessedMembers]
  private static readonly Type _t43 = typeof (Accuracy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t44 = typeof (Acrobatics);
  [DynamicallyAccessedMembers]
  private static readonly Type _t45 = typeof (AdaptiveStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t46 = typeof (Adrenaline);
  [DynamicallyAccessedMembers]
  private static readonly Type _t47 = typeof (Afterimage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t48 = typeof (Afterlife);
  [DynamicallyAccessedMembers]
  private static readonly Type _t49 = typeof (Aggression);
  [DynamicallyAccessedMembers]
  private static readonly Type _t50 = typeof (Alchemize);
  [DynamicallyAccessedMembers]
  private static readonly Type _t51 = typeof (Alignment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t52 = typeof (AllForOne);
  [DynamicallyAccessedMembers]
  private static readonly Type _t53 = typeof (Anger);
  [DynamicallyAccessedMembers]
  private static readonly Type _t54 = typeof (Anointed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t55 = typeof (Anticipate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t56 = typeof (Apotheosis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t57 = typeof (Apparition);
  [DynamicallyAccessedMembers]
  private static readonly Type _t58 = typeof (Armaments);
  [DynamicallyAccessedMembers]
  private static readonly Type _t59 = typeof (Arsenal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t60 = typeof (AscendersBane);
  [DynamicallyAccessedMembers]
  private static readonly Type _t61 = typeof (AshenStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t62 = typeof (Assassinate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t63 = typeof (AstralPulse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t64 = typeof (Automation);
  [DynamicallyAccessedMembers]
  private static readonly Type _t65 = typeof (Backflip);
  [DynamicallyAccessedMembers]
  private static readonly Type _t66 = typeof (Backstab);
  [DynamicallyAccessedMembers]
  private static readonly Type _t67 = typeof (BadLuck);
  [DynamicallyAccessedMembers]
  private static readonly Type _t68 = typeof (BallLightning);
  [DynamicallyAccessedMembers]
  private static readonly Type _t69 = typeof (BansheesCry);
  [DynamicallyAccessedMembers]
  private static readonly Type _t70 = typeof (Barrage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t71 = typeof (Barricade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t72 = typeof (Bash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t73 = typeof (BattleTrance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t74 = typeof (BeaconOfHope);
  [DynamicallyAccessedMembers]
  private static readonly Type _t75 = typeof (BeamCell);
  [DynamicallyAccessedMembers]
  private static readonly Type _t76 = typeof (BeatDown);
  [DynamicallyAccessedMembers]
  private static readonly Type _t77 = typeof (BeatIntoShape);
  [DynamicallyAccessedMembers]
  private static readonly Type _t78 = typeof (Beckon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t79 = typeof (Begone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t80 = typeof (BelieveInYou);
  [DynamicallyAccessedMembers]
  private static readonly Type _t81 = typeof (BiasedCognition);
  [DynamicallyAccessedMembers]
  private static readonly Type _t82 = typeof (BigBang);
  [DynamicallyAccessedMembers]
  private static readonly Type _t83 = typeof (BlackHole);
  [DynamicallyAccessedMembers]
  private static readonly Type _t84 = typeof (BladeDance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t85 = typeof (BladeOfInk);
  [DynamicallyAccessedMembers]
  private static readonly Type _t86 = typeof (BladeSymphony);
  [DynamicallyAccessedMembers]
  private static readonly Type _t87 = typeof (Blaze);
  [DynamicallyAccessedMembers]
  private static readonly Type _t88 = typeof (BlightStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t89 = typeof (Bloodletting);
  [DynamicallyAccessedMembers]
  private static readonly Type _t90 = typeof (BloodWall);
  [DynamicallyAccessedMembers]
  private static readonly Type _t91 = typeof (Bludgeon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t92 = typeof (Blur);
  [DynamicallyAccessedMembers]
  private static readonly Type _t93 = typeof (Bodyguard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t94 = typeof (BodySlam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t95 = typeof (Bolas);
  [DynamicallyAccessedMembers]
  private static readonly Type _t96 = typeof (Bombardment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t97 = typeof (BoneShards);
  [DynamicallyAccessedMembers]
  private static readonly Type _t98 = typeof (BoostAway);
  [DynamicallyAccessedMembers]
  private static readonly Type _t99 = typeof (BootSequence);
  [DynamicallyAccessedMembers]
  private static readonly Type _t100 = typeof (BorrowedTime);
  [DynamicallyAccessedMembers]
  private static readonly Type _t101 = typeof (BouncingFlask);
  [DynamicallyAccessedMembers]
  private static readonly Type _t102 = typeof (Brand);
  [DynamicallyAccessedMembers]
  private static readonly Type _t103 = typeof (Break);
  [DynamicallyAccessedMembers]
  private static readonly Type _t104 = typeof (Breakthrough);
  [DynamicallyAccessedMembers]
  private static readonly Type _t105 = typeof (BrightestFlame);
  [DynamicallyAccessedMembers]
  private static readonly Type _t106 = typeof (BubbleBubble);
  [DynamicallyAccessedMembers]
  private static readonly Type _t107 = typeof (MegaCrit.Sts2.Core.Models.Cards.Buffer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t108 = typeof (BulkUp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t109 = typeof (BulletTime);
  [DynamicallyAccessedMembers]
  private static readonly Type _t110 = typeof (Bully);
  [DynamicallyAccessedMembers]
  private static readonly Type _t111 = typeof (Bulwark);
  [DynamicallyAccessedMembers]
  private static readonly Type _t112 = typeof (BundleOfJoy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t113 = typeof (Burn);
  [DynamicallyAccessedMembers]
  private static readonly Type _t114 = typeof (BurningPact);
  [DynamicallyAccessedMembers]
  private static readonly Type _t115 = typeof (Burst);
  [DynamicallyAccessedMembers]
  private static readonly Type _t116 = typeof (Bury);
  [DynamicallyAccessedMembers]
  private static readonly Type _t117 = typeof (ByrdonisEgg);
  [DynamicallyAccessedMembers]
  private static readonly Type _t118 = typeof (ByrdSwoop);
  [DynamicallyAccessedMembers]
  private static readonly Type _t119 = typeof (Cacophony);
  [DynamicallyAccessedMembers]
  private static readonly Type _t120 = typeof (Calamity);
  [DynamicallyAccessedMembers]
  private static readonly Type _t121 = typeof (Calcify);
  [DynamicallyAccessedMembers]
  private static readonly Type _t122 = typeof (CalculatedGamble);
  [DynamicallyAccessedMembers]
  private static readonly Type _t123 = typeof (CallOfTheVoid);
  [DynamicallyAccessedMembers]
  private static readonly Type _t124 = typeof (Caltrops);
  [DynamicallyAccessedMembers]
  private static readonly Type _t125 = typeof (Capacitor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t126 = typeof (CaptureSpirit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t127 = typeof (Cascade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t128 = typeof (Catastrophe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t129 = typeof (CelestialMight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t130 = typeof (Chaos);
  [DynamicallyAccessedMembers]
  private static readonly Type _t131 = typeof (Charge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t132 = typeof (ChargeBattery);
  [DynamicallyAccessedMembers]
  private static readonly Type _t133 = typeof (ChildOfTheStars);
  [DynamicallyAccessedMembers]
  private static readonly Type _t134 = typeof (Chill);
  [DynamicallyAccessedMembers]
  private static readonly Type _t135 = typeof (Cinder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t136 = typeof (Clash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t137 = typeof (Claw);
  [DynamicallyAccessedMembers]
  private static readonly Type _t138 = typeof (Cleanse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t139 = typeof (CloakAndDagger);
  [DynamicallyAccessedMembers]
  private static readonly Type _t140 = typeof (CloakOfStars);
  [DynamicallyAccessedMembers]
  private static readonly Type _t141 = typeof (Clumsy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t142 = typeof (ColdSnap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t143 = typeof (CollisionCourse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t144 = typeof (Colossus);
  [DynamicallyAccessedMembers]
  private static readonly Type _t145 = typeof (Comet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t146 = typeof (Compact);
  [DynamicallyAccessedMembers]
  private static readonly Type _t147 = typeof (CompileDriver);
  [DynamicallyAccessedMembers]
  private static readonly Type _t148 = typeof (Concoct);
  [DynamicallyAccessedMembers]
  private static readonly Type _t149 = typeof (Conflagration);
  [DynamicallyAccessedMembers]
  private static readonly Type _t150 = typeof (Conqueror);
  [DynamicallyAccessedMembers]
  private static readonly Type _t151 = typeof (Constellation);
  [DynamicallyAccessedMembers]
  private static readonly Type _t152 = typeof (ConsumingShadow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t153 = typeof (Convergence);
  [DynamicallyAccessedMembers]
  private static readonly Type _t154 = typeof (Coolant);
  [DynamicallyAccessedMembers]
  private static readonly Type _t155 = typeof (Coolheaded);
  [DynamicallyAccessedMembers]
  private static readonly Type _t156 = typeof (Coordinate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t157 = typeof (CorrosiveWave);
  [DynamicallyAccessedMembers]
  private static readonly Type _t158 = typeof (Corruption);
  [DynamicallyAccessedMembers]
  private static readonly Type _t159 = typeof (CosmicIndifference);
  [DynamicallyAccessedMembers]
  private static readonly Type _t160 = typeof (Countdown);
  [DynamicallyAccessedMembers]
  private static readonly Type _t161 = typeof (CrashLanding);
  [DynamicallyAccessedMembers]
  private static readonly Type _t162 = typeof (CreativeAi);
  [DynamicallyAccessedMembers]
  private static readonly Type _t163 = typeof (CrescentSpear);
  [DynamicallyAccessedMembers]
  private static readonly Type _t164 = typeof (CrimsonMantle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t165 = typeof (Cruelty);
  [DynamicallyAccessedMembers]
  private static readonly Type _t166 = typeof (CrushUnder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t167 = typeof (CurseOfTheBell);
  [DynamicallyAccessedMembers]
  private static readonly Type _t168 = typeof (DaggerSpray);
  [DynamicallyAccessedMembers]
  private static readonly Type _t169 = typeof (DaggerThrow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t170 = typeof (DanseMacabre);
  [DynamicallyAccessedMembers]
  private static readonly Type _t171 = typeof (DarkEmbrace);
  [DynamicallyAccessedMembers]
  private static readonly Type _t172 = typeof (Darkness);
  [DynamicallyAccessedMembers]
  private static readonly Type _t173 = typeof (DarkShackles);
  [DynamicallyAccessedMembers]
  private static readonly Type _t174 = typeof (Dash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t175 = typeof (Dazed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t176 = typeof (DeadlyPoison);
  [DynamicallyAccessedMembers]
  private static readonly Type _t177 = typeof (Deathbringer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t178 = typeof (DeathMarch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t179 = typeof (DeathsDoor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t180 = typeof (Debilitate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t181 = typeof (Debris);
  [DynamicallyAccessedMembers]
  private static readonly Type _t182 = typeof (Debt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t183 = typeof (Decay);
  [DynamicallyAccessedMembers]
  private static readonly Type _t184 = typeof (DecisionsDecisions);
  [DynamicallyAccessedMembers]
  private static readonly Type _t185 = typeof (DefendDefect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t186 = typeof (DefendIronclad);
  [DynamicallyAccessedMembers]
  private static readonly Type _t187 = typeof (DefendNecrobinder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t188 = typeof (DefendRegent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t189 = typeof (DefendSilent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t190 = typeof (Defile);
  [DynamicallyAccessedMembers]
  private static readonly Type _t191 = typeof (Deflect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t192 = typeof (Defragment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t193 = typeof (Defy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t194 = typeof (Delay);
  [DynamicallyAccessedMembers]
  private static readonly Type _t195 = typeof (Demesne);
  [DynamicallyAccessedMembers]
  private static readonly Type _t196 = typeof (DemonForm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t197 = typeof (DemonicShield);
  [DynamicallyAccessedMembers]
  private static readonly Type _t198 = typeof (DeprecatedCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t199 = typeof (Devastate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t200 = typeof (DevourLife);
  [DynamicallyAccessedMembers]
  private static readonly Type _t201 = typeof (Dirge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t202 = typeof (Discovery);
  [DynamicallyAccessedMembers]
  private static readonly Type _t203 = typeof (Disintegration);
  [DynamicallyAccessedMembers]
  private static readonly Type _t204 = typeof (Dismantle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t205 = typeof (Distraction);
  [DynamicallyAccessedMembers]
  private static readonly Type _t206 = typeof (DodgeAndRoll);
  [DynamicallyAccessedMembers]
  private static readonly Type _t207 = typeof (Dominate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t208 = typeof (DoubleEnergy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t209 = typeof (Doubt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t210 = typeof (Dowsing);
  [DynamicallyAccessedMembers]
  private static readonly Type _t211 = typeof (DrainPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t212 = typeof (DramaticEntrance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t213 = typeof (Dredge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t214 = typeof (DrumOfBattle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t215 = typeof (Dualcast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t216 = typeof (DualWield);
  [DynamicallyAccessedMembers]
  private static readonly Type _t217 = typeof (DyingStar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t218 = typeof (EchoForm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t219 = typeof (EchoingSlash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t220 = typeof (Eidolon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t221 = typeof (EndOfDays);
  [DynamicallyAccessedMembers]
  private static readonly Type _t222 = typeof (EnergySurge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t223 = typeof (EnfeeblingTouch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t224 = typeof (Enlightenment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t225 = typeof (Enthralled);
  [DynamicallyAccessedMembers]
  private static readonly Type _t226 = typeof (Entrench);
  [DynamicallyAccessedMembers]
  private static readonly Type _t227 = typeof (Entropy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t228 = typeof (Envenom);
  [DynamicallyAccessedMembers]
  private static readonly Type _t229 = typeof (Equilibrium);
  [DynamicallyAccessedMembers]
  private static readonly Type _t230 = typeof (Eradicate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t231 = typeof (EscapePlan);
  [DynamicallyAccessedMembers]
  private static readonly Type _t232 = typeof (EternalArmor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t233 = typeof (EvilEye);
  [DynamicallyAccessedMembers]
  private static readonly Type _t234 = typeof (ExpectAFight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t235 = typeof (Expertise);
  [DynamicallyAccessedMembers]
  private static readonly Type _t236 = typeof (Expose);
  [DynamicallyAccessedMembers]
  private static readonly Type _t237 = typeof (Exterminate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t238 = typeof (Fade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t239 = typeof (FallingStar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t240 = typeof (FanOfKnives);
  [DynamicallyAccessedMembers]
  private static readonly Type _t241 = typeof (Fasten);
  [DynamicallyAccessedMembers]
  private static readonly Type _t242 = typeof (Fear);
  [DynamicallyAccessedMembers]
  private static readonly Type _t243 = typeof (Feed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t244 = typeof (FeedingFrenzy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t245 = typeof (FeelNoPain);
  [DynamicallyAccessedMembers]
  private static readonly Type _t246 = typeof (Feral);
  [DynamicallyAccessedMembers]
  private static readonly Type _t247 = typeof (Fetch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t248 = typeof (FiendFire);
  [DynamicallyAccessedMembers]
  private static readonly Type _t249 = typeof (FightMe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t250 = typeof (FightThrough);
  [DynamicallyAccessedMembers]
  private static readonly Type _t251 = typeof (Finesse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t252 = typeof (Finisher);
  [DynamicallyAccessedMembers]
  private static readonly Type _t253 = typeof (Fisticuffs);
  [DynamicallyAccessedMembers]
  private static readonly Type _t254 = typeof (FlakCannon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t255 = typeof (FlameBarrier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t256 = typeof (Flanking);
  [DynamicallyAccessedMembers]
  private static readonly Type _t257 = typeof (FlashOfSteel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t258 = typeof (Flatten);
  [DynamicallyAccessedMembers]
  private static readonly Type _t259 = typeof (Flechettes);
  [DynamicallyAccessedMembers]
  private static readonly Type _t260 = typeof (FlickFlack);
  [DynamicallyAccessedMembers]
  private static readonly Type _t261 = typeof (FocusedStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t262 = typeof (Folly);
  [DynamicallyAccessedMembers]
  private static readonly Type _t263 = typeof (Footwork);
  [DynamicallyAccessedMembers]
  private static readonly Type _t264 = typeof (ForbiddenGrimoire);
  [DynamicallyAccessedMembers]
  private static readonly Type _t265 = typeof (ForegoneConclusion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t266 = typeof (ForgottenRitual);
  [DynamicallyAccessedMembers]
  private static readonly Type _t267 = typeof (FranticEscape);
  [DynamicallyAccessedMembers]
  private static readonly Type _t268 = typeof (Friendship);
  [DynamicallyAccessedMembers]
  private static readonly Type _t269 = typeof (Ftl);
  [DynamicallyAccessedMembers]
  private static readonly Type _t270 = typeof (Fuel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t271 = typeof (Furnace);
  [DynamicallyAccessedMembers]
  private static readonly Type _t272 = typeof (Fusion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t273 = typeof (GammaBlast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t274 = typeof (GangUp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t275 = typeof (GatherLight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t276 = typeof (Genesis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t277 = typeof (GeneticAlgorithm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t278 = typeof (GiantRock);
  [DynamicallyAccessedMembers]
  private static readonly Type _t279 = typeof (Glacier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t280 = typeof (Glasswork);
  [DynamicallyAccessedMembers]
  private static readonly Type _t281 = typeof (Glimmer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t282 = typeof (GlimpseBeyond);
  [DynamicallyAccessedMembers]
  private static readonly Type _t283 = typeof (Glitterstream);
  [DynamicallyAccessedMembers]
  private static readonly Type _t284 = typeof (Glow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t285 = typeof (GoForTheEyes);
  [DynamicallyAccessedMembers]
  private static readonly Type _t286 = typeof (GoldAxe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t287 = typeof (GrandFinale);
  [DynamicallyAccessedMembers]
  private static readonly Type _t288 = typeof (Graveblast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t289 = typeof (GraveWarden);
  [DynamicallyAccessedMembers]
  private static readonly Type _t290 = typeof (Greed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t291 = typeof (Guards);
  [DynamicallyAccessedMembers]
  private static readonly Type _t292 = typeof (GuidingStar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t293 = typeof (Guilty);
  [DynamicallyAccessedMembers]
  private static readonly Type _t294 = typeof (GunkUp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t295 = typeof (Hailstorm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t296 = typeof (HammerTime);
  [DynamicallyAccessedMembers]
  private static readonly Type _t297 = typeof (HandOfGreed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t298 = typeof (HandTrick);
  [DynamicallyAccessedMembers]
  private static readonly Type _t299 = typeof (Hang);
  [DynamicallyAccessedMembers]
  private static readonly Type _t300 = typeof (Haunt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t301 = typeof (Havoc);
  [DynamicallyAccessedMembers]
  private static readonly Type _t302 = typeof (Haze);
  [DynamicallyAccessedMembers]
  private static readonly Type _t303 = typeof (Headbutt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t304 = typeof (HeavenlyDrill);
  [DynamicallyAccessedMembers]
  private static readonly Type _t305 = typeof (Hegemony);
  [DynamicallyAccessedMembers]
  private static readonly Type _t306 = typeof (HeirloomHammer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t307 = typeof (HelixDrill);
  [DynamicallyAccessedMembers]
  private static readonly Type _t308 = typeof (HelloWorld);
  [DynamicallyAccessedMembers]
  private static readonly Type _t309 = typeof (Hellraiser);
  [DynamicallyAccessedMembers]
  private static readonly Type _t310 = typeof (Hemokinesis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t311 = typeof (Hibernate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t312 = typeof (HiddenCache);
  [DynamicallyAccessedMembers]
  private static readonly Type _t313 = typeof (HiddenDaggers);
  [DynamicallyAccessedMembers]
  private static readonly Type _t314 = typeof (HiddenGem);
  [DynamicallyAccessedMembers]
  private static readonly Type _t315 = typeof (HighFive);
  [DynamicallyAccessedMembers]
  private static readonly Type _t316 = typeof (Hologram);
  [DynamicallyAccessedMembers]
  private static readonly Type _t317 = typeof (Hotfix);
  [DynamicallyAccessedMembers]
  private static readonly Type _t318 = typeof (HowlFromBeyond);
  [DynamicallyAccessedMembers]
  private static readonly Type _t319 = typeof (HuddleUp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t320 = typeof (Hyperbeam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t321 = typeof (IAmInvincible);
  [DynamicallyAccessedMembers]
  private static readonly Type _t322 = typeof (IceLance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t323 = typeof (Ignition);
  [DynamicallyAccessedMembers]
  private static readonly Type _t324 = typeof (ImitationLearning);
  [DynamicallyAccessedMembers]
  private static readonly Type _t325 = typeof (Impatience);
  [DynamicallyAccessedMembers]
  private static readonly Type _t326 = typeof (Impervious);
  [DynamicallyAccessedMembers]
  private static readonly Type _t327 = typeof (Infection);
  [DynamicallyAccessedMembers]
  private static readonly Type _t328 = typeof (InfernalBlade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t329 = typeof (Inferno);
  [DynamicallyAccessedMembers]
  private static readonly Type _t330 = typeof (InfiniteBlades);
  [DynamicallyAccessedMembers]
  private static readonly Type _t331 = typeof (Inflame);
  [DynamicallyAccessedMembers]
  private static readonly Type _t332 = typeof (Injury);
  [DynamicallyAccessedMembers]
  private static readonly Type _t333 = typeof (Intercept);
  [DynamicallyAccessedMembers]
  private static readonly Type _t334 = typeof (Invoke);
  [DynamicallyAccessedMembers]
  private static readonly Type _t335 = typeof (IronWave);
  [DynamicallyAccessedMembers]
  private static readonly Type _t336 = typeof (Iteration);
  [DynamicallyAccessedMembers]
  private static readonly Type _t337 = typeof (JackOfAllTrades);
  [DynamicallyAccessedMembers]
  private static readonly Type _t338 = typeof (Jackpot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t339 = typeof (Juggernaut);
  [DynamicallyAccessedMembers]
  private static readonly Type _t340 = typeof (Juggling);
  [DynamicallyAccessedMembers]
  private static readonly Type _t341 = typeof (KinglyKick);
  [DynamicallyAccessedMembers]
  private static readonly Type _t342 = typeof (KinglyPunch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t343 = typeof (KnifeTrap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t344 = typeof (Knockdown);
  [DynamicallyAccessedMembers]
  private static readonly Type _t345 = typeof (KnockoutBlow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t346 = typeof (KnowThyPlace);
  [DynamicallyAccessedMembers]
  private static readonly Type _t347 = typeof (LanternKey);
  [DynamicallyAccessedMembers]
  private static readonly Type _t348 = typeof (Largesse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t349 = typeof (LeadingStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t350 = typeof (Leap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t351 = typeof (LegionOfBone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t352 = typeof (LegSweep);
  [DynamicallyAccessedMembers]
  private static readonly Type _t353 = typeof (Lethality);
  [DynamicallyAccessedMembers]
  private static readonly Type _t354 = typeof (Lift);
  [DynamicallyAccessedMembers]
  private static readonly Type _t355 = typeof (LightningRod);
  [DynamicallyAccessedMembers]
  private static readonly Type _t356 = typeof (Loop);
  [DynamicallyAccessedMembers]
  private static readonly Type _t357 = typeof (Luminesce);
  [DynamicallyAccessedMembers]
  private static readonly Type _t358 = typeof (LunarBlast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t359 = typeof (MachineLearning);
  [DynamicallyAccessedMembers]
  private static readonly Type _t360 = typeof (MadScience);
  [DynamicallyAccessedMembers]
  private static readonly Type _t361 = typeof (MakeItSo);
  [DynamicallyAccessedMembers]
  private static readonly Type _t362 = typeof (Malaise);
  [DynamicallyAccessedMembers]
  private static readonly Type _t363 = typeof (Mangle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t364 = typeof (ManifestAuthority);
  [DynamicallyAccessedMembers]
  private static readonly Type _t365 = typeof (MasterOfStrategy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t366 = typeof (MasterPlanner);
  [DynamicallyAccessedMembers]
  private static readonly Type _t367 = typeof (Maul);
  [DynamicallyAccessedMembers]
  private static readonly Type _t368 = typeof (Mayhem);
  [DynamicallyAccessedMembers]
  private static readonly Type _t369 = typeof (Melancholy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t370 = typeof (MementoMori);
  [DynamicallyAccessedMembers]
  private static readonly Type _t371 = typeof (Metamorphosis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t372 = typeof (MeteorShower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t373 = typeof (MeteorStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t374 = typeof (Midnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t375 = typeof (Mimic);
  [DynamicallyAccessedMembers]
  private static readonly Type _t376 = typeof (MindBlast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t377 = typeof (MindRot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t378 = typeof (MinionDiveBomb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t379 = typeof (MinionSacrifice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t380 = typeof (MinionStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t381 = typeof (Mirage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t382 = typeof (Misery);
  [DynamicallyAccessedMembers]
  private static readonly Type _t383 = typeof (MockAttackCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t384 = typeof (MockCurseCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t385 = typeof (MockPowerCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t386 = typeof (MockQuestCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t387 = typeof (MockSkillCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t388 = typeof (MockStatusCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t389 = typeof (Modded);
  [DynamicallyAccessedMembers]
  private static readonly Type _t390 = typeof (MoltenFist);
  [DynamicallyAccessedMembers]
  private static readonly Type _t391 = typeof (MomentumStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t392 = typeof (MonarchsGaze);
  [DynamicallyAccessedMembers]
  private static readonly Type _t393 = typeof (Monologue);
  [DynamicallyAccessedMembers]
  private static readonly Type _t394 = typeof (MultiCast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t395 = typeof (Murder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t396 = typeof (NecroMastery);
  [DynamicallyAccessedMembers]
  private static readonly Type _t397 = typeof (NegativePulse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t398 = typeof (NeowsFury);
  [DynamicallyAccessedMembers]
  private static readonly Type _t399 = typeof (Neurosurge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t400 = typeof (Neutralize);
  [DynamicallyAccessedMembers]
  private static readonly Type _t401 = typeof (NeutronAegis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t402 = typeof (Nightmare);
  [DynamicallyAccessedMembers]
  private static readonly Type _t403 = typeof (NoEscape);
  [DynamicallyAccessedMembers]
  private static readonly Type _t404 = typeof (Normality);
  [DynamicallyAccessedMembers]
  private static readonly Type _t405 = typeof (Nostalgia);
  [DynamicallyAccessedMembers]
  private static readonly Type _t406 = typeof (NotYet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t407 = typeof (NoxiousFumes);
  [DynamicallyAccessedMembers]
  private static readonly Type _t408 = typeof (Null);
  [DynamicallyAccessedMembers]
  private static readonly Type _t409 = typeof (Oblivion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t410 = typeof (Offering);
  [DynamicallyAccessedMembers]
  private static readonly Type _t411 = typeof (Omnislice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t412 = typeof (OneForAll);
  [DynamicallyAccessedMembers]
  private static readonly Type _t413 = typeof (OneTwoPunch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t414 = typeof (Orbit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t415 = typeof (Outbreak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t416 = typeof (Outmaneuver);
  [DynamicallyAccessedMembers]
  private static readonly Type _t417 = typeof (Outrage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t418 = typeof (Overclock);
  [DynamicallyAccessedMembers]
  private static readonly Type _t419 = typeof (PactsEnd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t420 = typeof (Pagestorm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t421 = typeof (PaleBlueDot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t422 = typeof (Panache);
  [DynamicallyAccessedMembers]
  private static readonly Type _t423 = typeof (PanicButton);
  [DynamicallyAccessedMembers]
  private static readonly Type _t424 = typeof (Parry);
  [DynamicallyAccessedMembers]
  private static readonly Type _t425 = typeof (Parse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t426 = typeof (ParticleWall);
  [DynamicallyAccessedMembers]
  private static readonly Type _t427 = typeof (Patter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t428 = typeof (Peck);
  [DynamicallyAccessedMembers]
  private static readonly Type _t429 = typeof (PerfectedStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t430 = typeof (PhantomBlades);
  [DynamicallyAccessedMembers]
  private static readonly Type _t431 = typeof (PhotonCut);
  [DynamicallyAccessedMembers]
  private static readonly Type _t432 = typeof (PiercingWail);
  [DynamicallyAccessedMembers]
  private static readonly Type _t433 = typeof (Pillage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t434 = typeof (PillarOfCreation);
  [DynamicallyAccessedMembers]
  private static readonly Type _t435 = typeof (Pinpoint);
  [DynamicallyAccessedMembers]
  private static readonly Type _t436 = typeof (Plot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t437 = typeof (PoisonedStab);
  [DynamicallyAccessedMembers]
  private static readonly Type _t438 = typeof (Poke);
  [DynamicallyAccessedMembers]
  private static readonly Type _t439 = typeof (PommelStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t440 = typeof (PoorSleep);
  [DynamicallyAccessedMembers]
  private static readonly Type _t441 = typeof (Pounce);
  [DynamicallyAccessedMembers]
  private static readonly Type _t442 = typeof (PreciseCut);
  [DynamicallyAccessedMembers]
  private static readonly Type _t443 = typeof (Predator);
  [DynamicallyAccessedMembers]
  private static readonly Type _t444 = typeof (Prepared);
  [DynamicallyAccessedMembers]
  private static readonly Type _t445 = typeof (PrepTime);
  [DynamicallyAccessedMembers]
  private static readonly Type _t446 = typeof (PrimalForce);
  [DynamicallyAccessedMembers]
  private static readonly Type _t447 = typeof (Production);
  [DynamicallyAccessedMembers]
  private static readonly Type _t448 = typeof (Prolong);
  [DynamicallyAccessedMembers]
  private static readonly Type _t449 = typeof (Prophesize);
  [DynamicallyAccessedMembers]
  private static readonly Type _t450 = typeof (Protector);
  [DynamicallyAccessedMembers]
  private static readonly Type _t451 = typeof (Prowess);
  [DynamicallyAccessedMembers]
  private static readonly Type _t452 = typeof (PullAggro);
  [DynamicallyAccessedMembers]
  private static readonly Type _t453 = typeof (PullFromBelow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t454 = typeof (Purity);
  [DynamicallyAccessedMembers]
  private static readonly Type _t455 = typeof (Putrefy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t456 = typeof (Pyre);
  [DynamicallyAccessedMembers]
  private static readonly Type _t457 = typeof (Quadcast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t458 = typeof (Quasar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t459 = typeof (Radiate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t460 = typeof (Rage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t461 = typeof (Rainbow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t462 = typeof (Rally);
  [DynamicallyAccessedMembers]
  private static readonly Type _t463 = typeof (Rampage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t464 = typeof (Rattle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t465 = typeof (Reanimate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t466 = typeof (Reap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t467 = typeof (ReaperForm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t468 = typeof (Reave);
  [DynamicallyAccessedMembers]
  private static readonly Type _t469 = typeof (Reboot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t470 = typeof (Rebound);
  [DynamicallyAccessedMembers]
  private static readonly Type _t471 = typeof (RefineBlade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t472 = typeof (Reflect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t473 = typeof (Reflex);
  [DynamicallyAccessedMembers]
  private static readonly Type _t474 = typeof (Refract);
  [DynamicallyAccessedMembers]
  private static readonly Type _t475 = typeof (Regret);
  [DynamicallyAccessedMembers]
  private static readonly Type _t476 = typeof (Relax);
  [DynamicallyAccessedMembers]
  private static readonly Type _t477 = typeof (Rend);
  [DynamicallyAccessedMembers]
  private static readonly Type _t478 = typeof (Resonance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t479 = typeof (Restlessness);
  [DynamicallyAccessedMembers]
  private static readonly Type _t480 = typeof (Ricochet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t481 = typeof (RightHandHand);
  [DynamicallyAccessedMembers]
  private static readonly Type _t482 = typeof (RipAndTear);
  [DynamicallyAccessedMembers]
  private static readonly Type _t483 = typeof (RocketPunch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t484 = typeof (RollingBoulder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t485 = typeof (RoyalGamble);
  [DynamicallyAccessedMembers]
  private static readonly Type _t486 = typeof (Royalties);
  [DynamicallyAccessedMembers]
  private static readonly Type _t487 = typeof (Rupture);
  [DynamicallyAccessedMembers]
  private static readonly Type _t488 = typeof (Sacrifice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t489 = typeof (Salvo);
  [DynamicallyAccessedMembers]
  private static readonly Type _t490 = typeof (Scare);
  [DynamicallyAccessedMembers]
  private static readonly Type _t491 = typeof (Scavenge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t492 = typeof (Scourge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t493 = typeof (Scrape);
  [DynamicallyAccessedMembers]
  private static readonly Type _t494 = typeof (Scrawl);
  [DynamicallyAccessedMembers]
  private static readonly Type _t495 = typeof (SculptingStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t496 = typeof (Seance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t497 = typeof (SecondWind);
  [DynamicallyAccessedMembers]
  private static readonly Type _t498 = typeof (SecretTechnique);
  [DynamicallyAccessedMembers]
  private static readonly Type _t499 = typeof (SecretWeapon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t500 = typeof (SeekerStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t501 = typeof (SeekingEdge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t502 = typeof (SentryMode);
  [DynamicallyAccessedMembers]
  private static readonly Type _t503 = typeof (SerpentForm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t504 = typeof (SetupStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t505 = typeof (SevenStars);
  [DynamicallyAccessedMembers]
  private static readonly Type _t506 = typeof (Severance);
  [DynamicallyAccessedMembers]
  private static readonly Type _t507 = typeof (Shadowmeld);
  [DynamicallyAccessedMembers]
  private static readonly Type _t508 = typeof (ShadowShield);
  [DynamicallyAccessedMembers]
  private static readonly Type _t509 = typeof (ShadowStep);
  [DynamicallyAccessedMembers]
  private static readonly Type _t510 = typeof (Shame);
  [DynamicallyAccessedMembers]
  private static readonly Type _t511 = typeof (SharedFate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t512 = typeof (Shatter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t513 = typeof (ShiningStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t514 = typeof (Shiv);
  [DynamicallyAccessedMembers]
  private static readonly Type _t515 = typeof (Shockwave);
  [DynamicallyAccessedMembers]
  private static readonly Type _t516 = typeof (Shroud);
  [DynamicallyAccessedMembers]
  private static readonly Type _t517 = typeof (ShrugItOff);
  [DynamicallyAccessedMembers]
  private static readonly Type _t518 = typeof (SicEm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t519 = typeof (SignalBoost);
  [DynamicallyAccessedMembers]
  private static readonly Type _t520 = typeof (Skewer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t521 = typeof (Skim);
  [DynamicallyAccessedMembers]
  private static readonly Type _t522 = typeof (SleightOfFlesh);
  [DynamicallyAccessedMembers]
  private static readonly Type _t523 = typeof (Slice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t524 = typeof (Slimed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t525 = typeof (Sloth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t526 = typeof (Smokestack);
  [DynamicallyAccessedMembers]
  private static readonly Type _t527 = typeof (Snakebite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t528 = typeof (Snap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t529 = typeof (Sneaky);
  [DynamicallyAccessedMembers]
  private static readonly Type _t530 = typeof (SolarStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t531 = typeof (Soot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t532 = typeof (Soul);
  [DynamicallyAccessedMembers]
  private static readonly Type _t533 = typeof (Soulbound);
  [DynamicallyAccessedMembers]
  private static readonly Type _t534 = typeof (SoulStorm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t535 = typeof (SovereignBlade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t536 = typeof (Sow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t537 = typeof (SpectrumShift);
  [DynamicallyAccessedMembers]
  private static readonly Type _t538 = typeof (Speedster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t539 = typeof (Spinner);
  [DynamicallyAccessedMembers]
  private static readonly Type _t540 = typeof (SpiritOfAsh);
  [DynamicallyAccessedMembers]
  private static readonly Type _t541 = typeof (Spite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t542 = typeof (Splash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t543 = typeof (SpoilsMap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t544 = typeof (SpoilsOfBattle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t545 = typeof (SporeMind);
  [DynamicallyAccessedMembers]
  private static readonly Type _t546 = typeof (Spur);
  [DynamicallyAccessedMembers]
  private static readonly Type _t547 = typeof (Squash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t548 = typeof (Squeeze);
  [DynamicallyAccessedMembers]
  private static readonly Type _t549 = typeof (Stack);
  [DynamicallyAccessedMembers]
  private static readonly Type _t550 = typeof (Stampede);
  [DynamicallyAccessedMembers]
  private static readonly Type _t551 = typeof (Stardust);
  [DynamicallyAccessedMembers]
  private static readonly Type _t552 = typeof (Stoke);
  [DynamicallyAccessedMembers]
  private static readonly Type _t553 = typeof (Stomp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t554 = typeof (StoneArmor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t555 = typeof (Storm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t556 = typeof (StormOfSteel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t557 = typeof (Strangle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t558 = typeof (Stratagem);
  [DynamicallyAccessedMembers]
  private static readonly Type _t559 = typeof (StrikeDefect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t560 = typeof (StrikeIronclad);
  [DynamicallyAccessedMembers]
  private static readonly Type _t561 = typeof (StrikeNecrobinder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t562 = typeof (StrikeRegent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t563 = typeof (StrikeSilent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t564 = typeof (Subroutine);
  [DynamicallyAccessedMembers]
  private static readonly Type _t565 = typeof (SuckerPunch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t566 = typeof (SummonForth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t567 = typeof (Sunder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t568 = typeof (Supercritical);
  [DynamicallyAccessedMembers]
  private static readonly Type _t569 = typeof (Supermassive);
  [DynamicallyAccessedMembers]
  private static readonly Type _t570 = typeof (Suppress);
  [DynamicallyAccessedMembers]
  private static readonly Type _t571 = typeof (Survivor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t572 = typeof (SweepingBeam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t573 = typeof (SweepingGaze);
  [DynamicallyAccessedMembers]
  private static readonly Type _t574 = typeof (SwordBoomerang);
  [DynamicallyAccessedMembers]
  private static readonly Type _t575 = typeof (SwordSage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t576 = typeof (Synchronize);
  [DynamicallyAccessedMembers]
  private static readonly Type _t577 = typeof (Synthesis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t578 = typeof (Tactician);
  [DynamicallyAccessedMembers]
  private static readonly Type _t579 = typeof (TagTeam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t580 = typeof (Tank);
  [DynamicallyAccessedMembers]
  private static readonly Type _t581 = typeof (Taunt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t582 = typeof (TearAsunder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t583 = typeof (Tempest);
  [DynamicallyAccessedMembers]
  private static readonly Type _t584 = typeof (Terraforming);
  [DynamicallyAccessedMembers]
  private static readonly Type _t585 = typeof (TeslaCoil);
  [DynamicallyAccessedMembers]
  private static readonly Type _t586 = typeof (TheBall);
  [DynamicallyAccessedMembers]
  private static readonly Type _t587 = typeof (TheBomb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t588 = typeof (TheGambit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t589 = typeof (TheHunt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t590 = typeof (TheScythe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t591 = typeof (TheSealedThrone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t592 = typeof (TheSmith);
  [DynamicallyAccessedMembers]
  private static readonly Type _t593 = typeof (ThinkingAhead);
  [DynamicallyAccessedMembers]
  private static readonly Type _t594 = typeof (Thrash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t595 = typeof (ThrummingHatchet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t596 = typeof (Thunder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t597 = typeof (Thunderclap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t598 = typeof (TimesUp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t599 = typeof (ToolsOfTheTrade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t600 = typeof (ToricToughness);
  [DynamicallyAccessedMembers]
  private static readonly Type _t601 = typeof (Toxic);
  [DynamicallyAccessedMembers]
  private static readonly Type _t602 = typeof (Tracking);
  [DynamicallyAccessedMembers]
  private static readonly Type _t603 = typeof (Transfigure);
  [DynamicallyAccessedMembers]
  private static readonly Type _t604 = typeof (TrashToTreasure);
  [DynamicallyAccessedMembers]
  private static readonly Type _t605 = typeof (Tremble);
  [DynamicallyAccessedMembers]
  private static readonly Type _t606 = typeof (TrueGrit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t607 = typeof (Turbo);
  [DynamicallyAccessedMembers]
  private static readonly Type _t608 = typeof (Tutor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t609 = typeof (TwinStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t610 = typeof (Tyranny);
  [DynamicallyAccessedMembers]
  private static readonly Type _t611 = typeof (UltimateDefend);
  [DynamicallyAccessedMembers]
  private static readonly Type _t612 = typeof (UltimateStrike);
  [DynamicallyAccessedMembers]
  private static readonly Type _t613 = typeof (Undeath);
  [DynamicallyAccessedMembers]
  private static readonly Type _t614 = typeof (Underworld);
  [DynamicallyAccessedMembers]
  private static readonly Type _t615 = typeof (Unleash);
  [DynamicallyAccessedMembers]
  private static readonly Type _t616 = typeof (Unmovable);
  [DynamicallyAccessedMembers]
  private static readonly Type _t617 = typeof (Unrelenting);
  [DynamicallyAccessedMembers]
  private static readonly Type _t618 = typeof (Untouchable);
  [DynamicallyAccessedMembers]
  private static readonly Type _t619 = typeof (UpMySleeve);
  [DynamicallyAccessedMembers]
  private static readonly Type _t620 = typeof (Uppercut);
  [DynamicallyAccessedMembers]
  private static readonly Type _t621 = typeof (Uproar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t622 = typeof (Veilpiercer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t623 = typeof (Venerate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t624 = typeof (Vicious);
  [DynamicallyAccessedMembers]
  private static readonly Type _t625 = typeof (MegaCrit.Sts2.Core.Models.Cards.Void);
  [DynamicallyAccessedMembers]
  private static readonly Type _t626 = typeof (VoidForm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t627 = typeof (Volley);
  [DynamicallyAccessedMembers]
  private static readonly Type _t628 = typeof (Voltaic);
  [DynamicallyAccessedMembers]
  private static readonly Type _t629 = typeof (WasteAway);
  [DynamicallyAccessedMembers]
  private static readonly Type _t630 = typeof (WellLaidPlans);
  [DynamicallyAccessedMembers]
  private static readonly Type _t631 = typeof (Whirlwind);
  [DynamicallyAccessedMembers]
  private static readonly Type _t632 = typeof (Whistle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t633 = typeof (WhiteNoise);
  [DynamicallyAccessedMembers]
  private static readonly Type _t634 = typeof (Wish);
  [DynamicallyAccessedMembers]
  private static readonly Type _t635 = typeof (Wisp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t636 = typeof (Wither);
  [DynamicallyAccessedMembers]
  private static readonly Type _t637 = typeof (Wound);
  [DynamicallyAccessedMembers]
  private static readonly Type _t638 = typeof (WraithForm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t639 = typeof (Writhe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t640 = typeof (WroughtInWar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t641 = typeof (Zap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t642 = typeof (Defect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t643 = typeof (DeprecatedCharacter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t644 = typeof (Deprived);
  [DynamicallyAccessedMembers]
  private static readonly Type _t645 = typeof (Ironclad);
  [DynamicallyAccessedMembers]
  private static readonly Type _t646 = typeof (Necrobinder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t647 = typeof (RandomCharacter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t648 = typeof (Regent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t649 = typeof (Silent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t650 = typeof (Adroit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t651 = typeof (Clone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t652 = typeof (Corrupted);
  [DynamicallyAccessedMembers]
  private static readonly Type _t653 = typeof (DeprecatedEnchantment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t654 = typeof (Glam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t655 = typeof (Goopy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t656 = typeof (Imbued);
  [DynamicallyAccessedMembers]
  private static readonly Type _t657 = typeof (Inky);
  [DynamicallyAccessedMembers]
  private static readonly Type _t658 = typeof (Instinct);
  [DynamicallyAccessedMembers]
  private static readonly Type _t659 = typeof (MockFreeEnchantment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t660 = typeof (Momentum);
  [DynamicallyAccessedMembers]
  private static readonly Type _t661 = typeof (Nimble);
  [DynamicallyAccessedMembers]
  private static readonly Type _t662 = typeof (PerfectFit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t663 = typeof (RoyallyApproved);
  [DynamicallyAccessedMembers]
  private static readonly Type _t664 = typeof (Sharp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t665 = typeof (Slither);
  [DynamicallyAccessedMembers]
  private static readonly Type _t666 = typeof (SlumberingEssence);
  [DynamicallyAccessedMembers]
  private static readonly Type _t667 = typeof (SoulsPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t668 = typeof (Sown);
  [DynamicallyAccessedMembers]
  private static readonly Type _t669 = typeof (Spiral);
  [DynamicallyAccessedMembers]
  private static readonly Type _t670 = typeof (Steady);
  [DynamicallyAccessedMembers]
  private static readonly Type _t671 = typeof (Swift);
  [DynamicallyAccessedMembers]
  private static readonly Type _t672 = typeof (TezcatarasEmber);
  [DynamicallyAccessedMembers]
  private static readonly Type _t673 = typeof (Vigorous);
  [DynamicallyAccessedMembers]
  private static readonly Type _t674 = typeof (AeonglassBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t675 = typeof (AxebotsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t676 = typeof (BattlewornDummyEventV1Encounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t677 = typeof (BattlewornDummyEventV2Encounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t678 = typeof (BattlewornDummyEventV3Encounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t679 = typeof (BowlbugsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t680 = typeof (BowlbugsWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t681 = typeof (BygoneEffigyElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t682 = typeof (ByrdonisElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t683 = typeof (CeremonialBeastBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t684 = typeof (ChompersNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t685 = typeof (ConstructMenagerieNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t686 = typeof (CorpseSlugsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t687 = typeof (CorpseSlugsWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t688 = typeof (CubexConstructNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t689 = typeof (CultistsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t690 = typeof (DecimillipedeElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t691 = typeof (DenseVegetationEventEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t692 = typeof (DeprecatedEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t693 = typeof (DevotedSculptorWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t694 = typeof (EntomancerElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t695 = typeof (ExoskeletonsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t696 = typeof (ExoskeletonsWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t697 = typeof (FabricatorNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t698 = typeof (FakeMerchantEventEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t699 = typeof (FlyconidNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t700 = typeof (FogmogNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t701 = typeof (FossilStalkerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t702 = typeof (FrogKnightNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t703 = typeof (FuzzyWurmCrawlerWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t704 = typeof (GlobeHeadNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t705 = typeof (GremlinMercNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t706 = typeof (HauntedShipNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t707 = typeof (HunterKillerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t708 = typeof (InfestedPrismsElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t709 = typeof (InkletsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t710 = typeof (KaiserCrabBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t711 = typeof (KnightsElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t712 = typeof (KnowledgeDemonBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t713 = typeof (LagavulinMatriarchBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t714 = typeof (LivingFogNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t715 = typeof (LouseProgenitorNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t716 = typeof (MawlerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t717 = typeof (MechaKnightElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t718 = typeof (MockArtifactEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t719 = typeof (MockAttackAndSummonEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t720 = typeof (MockBossEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t721 = typeof (MockEliteEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t722 = typeof (MockMonsterEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t723 = typeof (MockNoRewardsEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t724 = typeof (MockPlatingEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t725 = typeof (MockTwoMonsterEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t726 = typeof (MysteriousKnightEventEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t727 = typeof (MytesNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t728 = typeof (NibbitsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t729 = typeof (NibbitsWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t730 = typeof (OvergrowthCrawlers);
  [DynamicallyAccessedMembers]
  private static readonly Type _t731 = typeof (OvicopterNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t732 = typeof (OwlMagistrateNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t733 = typeof (PhantasmalGardenersElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t734 = typeof (PhrogParasiteElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t735 = typeof (PunchConstructNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t736 = typeof (PunchOffEventEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t737 = typeof (QueenBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t738 = typeof (RubyRaidersNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t739 = typeof (ScrollsOfBitingNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t740 = typeof (ScrollsOfBitingWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t741 = typeof (SeapunkNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t742 = typeof (SeapunkWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t743 = typeof (SewerClamNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t744 = typeof (ShrinkerBeetleWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t745 = typeof (SkulkingColonyElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t746 = typeof (SlimedBerserkerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t747 = typeof (SlimesNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t748 = typeof (SlimesWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t749 = typeof (SlitheringStranglerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t750 = typeof (SludgeSpinnerWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t751 = typeof (SlumberingBeetleNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t752 = typeof (SnappingJaxfruitNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t753 = typeof (SoulFyshBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t754 = typeof (SoulNexusElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t755 = typeof (SpinyToadNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t756 = typeof (TerrorEelElite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t757 = typeof (TestSubjectBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t758 = typeof (TheArchitectEventEncounter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t759 = typeof (TheInsatiableBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t760 = typeof (TheKinBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t761 = typeof (TheLostAndForgottenNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t762 = typeof (TheObscuraNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t763 = typeof (ThievingHopperWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t764 = typeof (ToadpolesWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t765 = typeof (TunnelerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t766 = typeof (TunnelerWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t767 = typeof (TurretOperatorWeak);
  [DynamicallyAccessedMembers]
  private static readonly Type _t768 = typeof (TwoTailedRatsNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t769 = typeof (VantomBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t770 = typeof (VineShamblerNormal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t771 = typeof (WaterfallGiantBoss);
  [DynamicallyAccessedMembers]
  private static readonly Type _t772 = typeof (AbyssalBaths);
  [DynamicallyAccessedMembers]
  private static readonly Type _t773 = typeof (Amalgamator);
  [DynamicallyAccessedMembers]
  private static readonly Type _t774 = typeof (AromaOfChaos);
  [DynamicallyAccessedMembers]
  private static readonly Type _t775 = typeof (BattlewornDummy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t776 = typeof (BrainLeech);
  [DynamicallyAccessedMembers]
  private static readonly Type _t777 = typeof (Bugslayer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t778 = typeof (ByrdonisNest);
  [DynamicallyAccessedMembers]
  private static readonly Type _t779 = typeof (ColorfulPhilosophers);
  [DynamicallyAccessedMembers]
  private static readonly Type _t780 = typeof (ColossalFlower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t781 = typeof (CrystalSphere);
  [DynamicallyAccessedMembers]
  private static readonly Type _t782 = typeof (Darv);
  [DynamicallyAccessedMembers]
  private static readonly Type _t783 = typeof (DenseVegetation);
  [DynamicallyAccessedMembers]
  private static readonly Type _t784 = typeof (DeprecatedAncientEvent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t785 = typeof (DeprecatedEvent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t786 = typeof (DollRoom);
  [DynamicallyAccessedMembers]
  private static readonly Type _t787 = typeof (DoorsOfLightAndDark);
  [DynamicallyAccessedMembers]
  private static readonly Type _t788 = typeof (DrowningBeacon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t789 = typeof (EndlessConveyor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t790 = typeof (FakeMerchant);
  [DynamicallyAccessedMembers]
  private static readonly Type _t791 = typeof (FieldOfManSizedHoles);
  [DynamicallyAccessedMembers]
  private static readonly Type _t792 = typeof (GraveOfTheForgotten);
  [DynamicallyAccessedMembers]
  private static readonly Type _t793 = typeof (HungryForMushrooms);
  [DynamicallyAccessedMembers]
  private static readonly Type _t794 = typeof (InfestedAutomaton);
  [DynamicallyAccessedMembers]
  private static readonly Type _t795 = typeof (JungleMazeAdventure);
  [DynamicallyAccessedMembers]
  private static readonly Type _t796 = typeof (MegaCrit.Sts2.Core.Models.Events.LostWisp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t797 = typeof (LuminousChoir);
  [DynamicallyAccessedMembers]
  private static readonly Type _t798 = typeof (MockEventModel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t799 = typeof (MorphicGrove);
  [DynamicallyAccessedMembers]
  private static readonly Type _t800 = typeof (Neow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t801 = typeof (Nonupeipe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t802 = typeof (Orobas);
  [DynamicallyAccessedMembers]
  private static readonly Type _t803 = typeof (Pael);
  [DynamicallyAccessedMembers]
  private static readonly Type _t804 = typeof (PotionCourier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t805 = typeof (PunchOff);
  [DynamicallyAccessedMembers]
  private static readonly Type _t806 = typeof (RanwidTheElder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t807 = typeof (Reflections);
  [DynamicallyAccessedMembers]
  private static readonly Type _t808 = typeof (RelicTrader);
  [DynamicallyAccessedMembers]
  private static readonly Type _t809 = typeof (RoomFullOfCheese);
  [DynamicallyAccessedMembers]
  private static readonly Type _t810 = typeof (RoundTeaParty);
  [DynamicallyAccessedMembers]
  private static readonly Type _t811 = typeof (SapphireSeed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t812 = typeof (SelfHelpBook);
  [DynamicallyAccessedMembers]
  private static readonly Type _t813 = typeof (SlipperyBridge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t814 = typeof (SpiralingWhirlpool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t815 = typeof (SpiritGrafter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t816 = typeof (StoneOfAllTime);
  [DynamicallyAccessedMembers]
  private static readonly Type _t817 = typeof (SunkenStatue);
  [DynamicallyAccessedMembers]
  private static readonly Type _t818 = typeof (SunkenTreasury);
  [DynamicallyAccessedMembers]
  private static readonly Type _t819 = typeof (Symbiote);
  [DynamicallyAccessedMembers]
  private static readonly Type _t820 = typeof (TabletOfTruth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t821 = typeof (Tanx);
  [DynamicallyAccessedMembers]
  private static readonly Type _t822 = typeof (TeaMaster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t823 = typeof (Tezcatara);
  [DynamicallyAccessedMembers]
  private static readonly Type _t824 = typeof (TheArchitect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t825 = typeof (TheFutureOfPotions);
  [DynamicallyAccessedMembers]
  private static readonly Type _t826 = typeof (TheLanternKey);
  [DynamicallyAccessedMembers]
  private static readonly Type _t827 = typeof (TheLegendsWereTrue);
  [DynamicallyAccessedMembers]
  private static readonly Type _t828 = typeof (ThisOrThat);
  [DynamicallyAccessedMembers]
  private static readonly Type _t829 = typeof (TinkerTime);
  [DynamicallyAccessedMembers]
  private static readonly Type _t830 = typeof (TrashHeap);
  [DynamicallyAccessedMembers]
  private static readonly Type _t831 = typeof (Trial);
  [DynamicallyAccessedMembers]
  private static readonly Type _t832 = typeof (UnrestSite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t833 = typeof (Vakuu);
  [DynamicallyAccessedMembers]
  private static readonly Type _t834 = typeof (WarHistorianRepy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t835 = typeof (WaterloggedScriptorium);
  [DynamicallyAccessedMembers]
  private static readonly Type _t836 = typeof (WelcomeToWongos);
  [DynamicallyAccessedMembers]
  private static readonly Type _t837 = typeof (Wellspring);
  [DynamicallyAccessedMembers]
  private static readonly Type _t838 = typeof (WhisperingHollow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t839 = typeof (WoodCarvings);
  [DynamicallyAccessedMembers]
  private static readonly Type _t840 = typeof (ZenWeaver);
  [DynamicallyAccessedMembers]
  private static readonly Type _t841 = typeof (AllStar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t842 = typeof (BigGameHunter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t843 = typeof (CharacterCards);
  [DynamicallyAccessedMembers]
  private static readonly Type _t844 = typeof (CursedRun);
  [DynamicallyAccessedMembers]
  private static readonly Type _t845 = typeof (DeadlyEvents);
  [DynamicallyAccessedMembers]
  private static readonly Type _t846 = typeof (DeprecatedModifier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t847 = typeof (Draft);
  [DynamicallyAccessedMembers]
  private static readonly Type _t848 = typeof (Flight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t849 = typeof (Hoarder);
  [DynamicallyAccessedMembers]
  private static readonly Type _t850 = typeof (Insanity);
  [DynamicallyAccessedMembers]
  private static readonly Type _t851 = typeof (Midas);
  [DynamicallyAccessedMembers]
  private static readonly Type _t852 = typeof (Murderous);
  [DynamicallyAccessedMembers]
  private static readonly Type _t853 = typeof (NightTerrors);
  [DynamicallyAccessedMembers]
  private static readonly Type _t854 = typeof (SealedDeck);
  [DynamicallyAccessedMembers]
  private static readonly Type _t855 = typeof (Specialized);
  [DynamicallyAccessedMembers]
  private static readonly Type _t856 = typeof (Terminal);
  [DynamicallyAccessedMembers]
  private static readonly Type _t857 = typeof (Vintage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t858 = typeof (Aeonglass);
  [DynamicallyAccessedMembers]
  private static readonly Type _t859 = typeof (Architect);
  [DynamicallyAccessedMembers]
  private static readonly Type _t860 = typeof (AssassinRubyRaider);
  [DynamicallyAccessedMembers]
  private static readonly Type _t861 = typeof (Axebot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t862 = typeof (AxeRubyRaider);
  [DynamicallyAccessedMembers]
  private static readonly Type _t863 = typeof (BattleFriendV1);
  [DynamicallyAccessedMembers]
  private static readonly Type _t864 = typeof (BattleFriendV2);
  [DynamicallyAccessedMembers]
  private static readonly Type _t865 = typeof (BattleFriendV3);
  [DynamicallyAccessedMembers]
  private static readonly Type _t866 = typeof (BigDummy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t867 = typeof (BowlbugEgg);
  [DynamicallyAccessedMembers]
  private static readonly Type _t868 = typeof (BowlbugNectar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t869 = typeof (BowlbugRock);
  [DynamicallyAccessedMembers]
  private static readonly Type _t870 = typeof (BowlbugSilk);
  [DynamicallyAccessedMembers]
  private static readonly Type _t871 = typeof (BruteRubyRaider);
  [DynamicallyAccessedMembers]
  private static readonly Type _t872 = typeof (BygoneEffigy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t873 = typeof (Byrdonis);
  [DynamicallyAccessedMembers]
  private static readonly Type _t874 = typeof (MegaCrit.Sts2.Core.Models.Monsters.Byrdpip);
  [DynamicallyAccessedMembers]
  private static readonly Type _t875 = typeof (CalcifiedCultist);
  [DynamicallyAccessedMembers]
  private static readonly Type _t876 = typeof (CeremonialBeast);
  [DynamicallyAccessedMembers]
  private static readonly Type _t877 = typeof (Chomper);
  [DynamicallyAccessedMembers]
  private static readonly Type _t878 = typeof (CorpseSlug);
  [DynamicallyAccessedMembers]
  private static readonly Type _t879 = typeof (CrossbowRubyRaider);
  [DynamicallyAccessedMembers]
  private static readonly Type _t880 = typeof (Crusher);
  [DynamicallyAccessedMembers]
  private static readonly Type _t881 = typeof (CubexConstruct);
  [DynamicallyAccessedMembers]
  private static readonly Type _t882 = typeof (DampCultist);
  [DynamicallyAccessedMembers]
  private static readonly Type _t883 = typeof (DecimillipedeSegmentBack);
  [DynamicallyAccessedMembers]
  private static readonly Type _t884 = typeof (DecimillipedeSegmentFront);
  [DynamicallyAccessedMembers]
  private static readonly Type _t885 = typeof (DecimillipedeSegmentMiddle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t886 = typeof (DeprecatedMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t887 = typeof (DevotedSculptor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t888 = typeof (Entomancer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t889 = typeof (Exoskeleton);
  [DynamicallyAccessedMembers]
  private static readonly Type _t890 = typeof (EyeWithTeeth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t891 = typeof (Fabricator);
  [DynamicallyAccessedMembers]
  private static readonly Type _t892 = typeof (FakeMerchantMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t893 = typeof (FatGremlin);
  [DynamicallyAccessedMembers]
  private static readonly Type _t894 = typeof (FlailKnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t895 = typeof (Flyconid);
  [DynamicallyAccessedMembers]
  private static readonly Type _t896 = typeof (Fogmog);
  [DynamicallyAccessedMembers]
  private static readonly Type _t897 = typeof (FossilStalker);
  [DynamicallyAccessedMembers]
  private static readonly Type _t898 = typeof (FrogKnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t899 = typeof (FuzzyWurmCrawler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t900 = typeof (GasBomb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t901 = typeof (GlobeHead);
  [DynamicallyAccessedMembers]
  private static readonly Type _t902 = typeof (GremlinMerc);
  [DynamicallyAccessedMembers]
  private static readonly Type _t903 = typeof (Guardbot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t904 = typeof (HauntedShip);
  [DynamicallyAccessedMembers]
  private static readonly Type _t905 = typeof (HunterKiller);
  [DynamicallyAccessedMembers]
  private static readonly Type _t906 = typeof (InfestedPrism);
  [DynamicallyAccessedMembers]
  private static readonly Type _t907 = typeof (Inklet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t908 = typeof (KinFollower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t909 = typeof (KinPriest);
  [DynamicallyAccessedMembers]
  private static readonly Type _t910 = typeof (KnowledgeDemon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t911 = typeof (LagavulinMatriarch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t912 = typeof (LeafSlimeM);
  [DynamicallyAccessedMembers]
  private static readonly Type _t913 = typeof (LeafSlimeS);
  [DynamicallyAccessedMembers]
  private static readonly Type _t914 = typeof (LivingFog);
  [DynamicallyAccessedMembers]
  private static readonly Type _t915 = typeof (LivingShield);
  [DynamicallyAccessedMembers]
  private static readonly Type _t916 = typeof (LouseProgenitor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t917 = typeof (MagiKnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t918 = typeof (Mawler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t919 = typeof (MechaKnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t920 = typeof (MockArtifactMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t921 = typeof (MockAttackAndSummonMinionMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t922 = typeof (MockAttackMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t923 = typeof (MockIntangibleMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t924 = typeof (MockPlatingMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t925 = typeof (MockReattachMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t926 = typeof (MultiAttackMoveMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t927 = typeof (MysteriousKnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t928 = typeof (Myte);
  [DynamicallyAccessedMembers]
  private static readonly Type _t929 = typeof (Nibbit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t930 = typeof (Noisebot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t931 = typeof (OneHpMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t932 = typeof (Osty);
  [DynamicallyAccessedMembers]
  private static readonly Type _t933 = typeof (Ovicopter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t934 = typeof (OwlMagistrate);
  [DynamicallyAccessedMembers]
  private static readonly Type _t935 = typeof (MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t936 = typeof (Parafright);
  [DynamicallyAccessedMembers]
  private static readonly Type _t937 = typeof (PhantasmalGardener);
  [DynamicallyAccessedMembers]
  private static readonly Type _t938 = typeof (PhrogParasite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t939 = typeof (PunchConstruct);
  [DynamicallyAccessedMembers]
  private static readonly Type _t940 = typeof (Queen);
  [DynamicallyAccessedMembers]
  private static readonly Type _t941 = typeof (Rocket);
  [DynamicallyAccessedMembers]
  private static readonly Type _t942 = typeof (ScrollOfBiting);
  [DynamicallyAccessedMembers]
  private static readonly Type _t943 = typeof (Seapunk);
  [DynamicallyAccessedMembers]
  private static readonly Type _t944 = typeof (SewerClam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t945 = typeof (ShrinkerBeetle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t946 = typeof (SingleAttackMoveMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t947 = typeof (SkulkingColony);
  [DynamicallyAccessedMembers]
  private static readonly Type _t948 = typeof (SlimedBerserker);
  [DynamicallyAccessedMembers]
  private static readonly Type _t949 = typeof (SlitheringStrangler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t950 = typeof (SludgeSpinner);
  [DynamicallyAccessedMembers]
  private static readonly Type _t951 = typeof (SlumberingBeetle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t952 = typeof (SnappingJaxfruit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t953 = typeof (SneakyGremlin);
  [DynamicallyAccessedMembers]
  private static readonly Type _t954 = typeof (SoulFysh);
  [DynamicallyAccessedMembers]
  private static readonly Type _t955 = typeof (SoulNexus);
  [DynamicallyAccessedMembers]
  private static readonly Type _t956 = typeof (SpectralKnight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t957 = typeof (SpinyToad);
  [DynamicallyAccessedMembers]
  private static readonly Type _t958 = typeof (Stabbot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t959 = typeof (TenHpMonster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t960 = typeof (TerrorEel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t961 = typeof (TestSubject);
  [DynamicallyAccessedMembers]
  private static readonly Type _t962 = typeof (TheAdversaryMkOne);
  [DynamicallyAccessedMembers]
  private static readonly Type _t963 = typeof (TheAdversaryMkThree);
  [DynamicallyAccessedMembers]
  private static readonly Type _t964 = typeof (TheAdversaryMkTwo);
  [DynamicallyAccessedMembers]
  private static readonly Type _t965 = typeof (TheForgotten);
  [DynamicallyAccessedMembers]
  private static readonly Type _t966 = typeof (TheInsatiable);
  [DynamicallyAccessedMembers]
  private static readonly Type _t967 = typeof (TheLost);
  [DynamicallyAccessedMembers]
  private static readonly Type _t968 = typeof (TheObscura);
  [DynamicallyAccessedMembers]
  private static readonly Type _t969 = typeof (ThievingHopper);
  [DynamicallyAccessedMembers]
  private static readonly Type _t970 = typeof (Toadpole);
  [DynamicallyAccessedMembers]
  private static readonly Type _t971 = typeof (TorchHeadAmalgam);
  [DynamicallyAccessedMembers]
  private static readonly Type _t972 = typeof (ToughEgg);
  [DynamicallyAccessedMembers]
  private static readonly Type _t973 = typeof (TrackerRubyRaider);
  [DynamicallyAccessedMembers]
  private static readonly Type _t974 = typeof (Tunneler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t975 = typeof (TurretOperator);
  [DynamicallyAccessedMembers]
  private static readonly Type _t976 = typeof (TwigSlimeM);
  [DynamicallyAccessedMembers]
  private static readonly Type _t977 = typeof (TwigSlimeS);
  [DynamicallyAccessedMembers]
  private static readonly Type _t978 = typeof (TwoTailedRat);
  [DynamicallyAccessedMembers]
  private static readonly Type _t979 = typeof (Vantom);
  [DynamicallyAccessedMembers]
  private static readonly Type _t980 = typeof (VineShambler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t981 = typeof (WaterfallGiant);
  [DynamicallyAccessedMembers]
  private static readonly Type _t982 = typeof (Wriggler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t983 = typeof (Zapbot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t984 = typeof (DarkOrb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t985 = typeof (FrostOrb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t986 = typeof (GlassOrb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t987 = typeof (LightningOrb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t988 = typeof (MockCombatCleanupOrb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t989 = typeof (PlasmaOrb);
  [DynamicallyAccessedMembers]
  private static readonly Type _t990 = typeof (DefectPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t991 = typeof (DeprecatedPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t992 = typeof (EventPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t993 = typeof (IroncladPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t994 = typeof (MockPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t995 = typeof (NecrobinderPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t996 = typeof (RegentPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t997 = typeof (SharedPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t998 = typeof (SilentPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t999 = typeof (TokenPotionPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1000 = typeof (Ambergris);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1001 = typeof (Ashwater);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1002 = typeof (AttackPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1003 = typeof (BeetleJuice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1004 = typeof (BlessingOfTheForge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1005 = typeof (BlockPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1006 = typeof (BloodPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1007 = typeof (BoneBrew);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1008 = typeof (BottledPotential);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1009 = typeof (Clarity);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1010 = typeof (ColorlessPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1011 = typeof (CosmicConcoction);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1012 = typeof (CunningPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1013 = typeof (CureAll);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1014 = typeof (DeprecatedPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1015 = typeof (DexterityPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1016 = typeof (DistilledChaos);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1017 = typeof (DropletOfPrecognition);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1018 = typeof (Duplicator);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1019 = typeof (EnergyPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1020 = typeof (EntropicBrew);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1021 = typeof (EssenceOfDarkness);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1022 = typeof (ExplosiveAmpoule);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1023 = typeof (FairyInABottle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1024 = typeof (FirePotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1025 = typeof (FlexPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1026 = typeof (FocusPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1027 = typeof (Fortifier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1028 = typeof (FoulPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1029 = typeof (FruitJuice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1030 = typeof (FyshOil);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1031 = typeof (GamblersBrew);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1032 = typeof (GhostInAJar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1033 = typeof (GigantificationPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1034 = typeof (GlowwaterPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1035 = typeof (HeartOfIron);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1036 = typeof (KingsCourage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1037 = typeof (LiquidBronze);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1038 = typeof (LiquidMemories);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1039 = typeof (LuckyTonic);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1040 = typeof (MazalethsGift);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1041 = typeof (MockDiscardAndAddShivsPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1042 = typeof (OrobicAcid);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1043 = typeof (PoisonPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1044 = typeof (PotionOfBinding);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1045 = typeof (PotionOfCapacity);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1046 = typeof (PotionOfDoom);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1047 = typeof (PotionShapedRock);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1048 = typeof (PotOfGhouls);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1049 = typeof (PowderedDemise);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1050 = typeof (PowerPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1051 = typeof (RadiantTincture);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1052 = typeof (RegenPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1053 = typeof (ShacklingPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1054 = typeof (ShipInABottle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1055 = typeof (SkillPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1056 = typeof (SneckoOil);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1057 = typeof (SoldiersStew);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1058 = typeof (SpeedPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1059 = typeof (StableSerum);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1060 = typeof (StarPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1061 = typeof (StrengthPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1062 = typeof (SwiftPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1063 = typeof (TouchOfInsanity);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1064 = typeof (VulnerablePotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1065 = typeof (WeakPotion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1066 = typeof (AccelerantPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1067 = typeof (AccuracyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1068 = typeof (AdaptablePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1069 = typeof (AfterimagePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1070 = typeof (AggressionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1071 = typeof (AmbergrisPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1072 = typeof (AnticipatePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1073 = typeof (ArsenalPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1074 = typeof (ArtifactPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1075 = typeof (AsleepPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1076 = typeof (AutomationPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1077 = typeof (BackAttackLeftPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1078 = typeof (BackAttackRightPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1079 = typeof (BarricadePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1080 = typeof (BattlewornDummyTimeLimitPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1081 = typeof (BeaconOfHopePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1082 = typeof (BiasedCognitionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1083 = typeof (BlackHolePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1084 = typeof (BlockNextTurnPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1085 = typeof (BlurPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1086 = typeof (BorrowedTimePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1087 = typeof (BufferPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1088 = typeof (BurrowedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1089 = typeof (BurstPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1090 = typeof (CacophonyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1091 = typeof (CalamityPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1092 = typeof (CalcifyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1093 = typeof (CallOfTheVoidPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1094 = typeof (ChainsOfBindingPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1095 = typeof (ChildOfTheStarsPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1096 = typeof (ClarityPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1097 = typeof (ColossusPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1098 = typeof (ConcoctPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1099 = typeof (ConfusedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1100 = typeof (ConquerorPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1101 = typeof (ConstrictPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1102 = typeof (ConsumingShadowPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1103 = typeof (CoolantPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1104 = typeof (CoordinatePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1105 = typeof (CorrosiveWavePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1106 = typeof (CorruptionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1107 = typeof (CountdownPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1108 = typeof (CoveredPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1109 = typeof (CrabRagePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1110 = typeof (CreativeAiPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1111 = typeof (CrimsonMantlePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1112 = typeof (CrueltyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1113 = typeof (CrushUnderPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1114 = typeof (CuriousPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1115 = typeof (CurlUpPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1116 = typeof (DampenPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1117 = typeof (DanseMacabrePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1118 = typeof (DarkEmbracePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1119 = typeof (DarkShacklesPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1120 = typeof (DebilitatePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1121 = typeof (DemesnePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1122 = typeof (DemisePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1123 = typeof (DemonFormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1124 = typeof (DevourLifePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1125 = typeof (DexterityPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1126 = typeof (DieForYouPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1127 = typeof (DisintegrationPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1128 = typeof (DoomPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1129 = typeof (DoubleDamagePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1130 = typeof (DrawCardsNextTurnPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1131 = typeof (DuplicationPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1132 = typeof (DyingStarPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1133 = typeof (EchoFormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1134 = typeof (EnergyNextTurnPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1135 = typeof (EnfeeblingTouchPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1136 = typeof (EnragePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1137 = typeof (EntropyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1138 = typeof (EnvenomPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1139 = typeof (EscapeArtistPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1140 = typeof (FadePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1141 = typeof (FanOfKnivesPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1142 = typeof (FastenPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1143 = typeof (FeedingFrenzyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1144 = typeof (FeelNoPainPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1145 = typeof (FeralPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1146 = typeof (FlameBarrierPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1147 = typeof (FlankingPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1148 = typeof (FlexPotionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1149 = typeof (FlutterPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1150 = typeof (FocusedStrikePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1151 = typeof (FocusPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1152 = typeof (ForbiddenGrimoirePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1153 = typeof (ForegoneConclusionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1154 = typeof (FrailPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1155 = typeof (FreeAttackPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1156 = typeof (FreePowerPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1157 = typeof (FreeSkillPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1158 = typeof (FriendshipPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1159 = typeof (FurnacePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1160 = typeof (GalvanicPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1161 = typeof (GenesisPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1162 = typeof (GigantificationPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1163 = typeof (GravityPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1164 = typeof (GuardedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1165 = typeof (HailstormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1166 = typeof (HammerTimePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1167 = typeof (HangPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1168 = typeof (HardenedShellPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1169 = typeof (HardToKillPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1170 = typeof (HatchPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1171 = typeof (HauntPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1172 = typeof (HeistPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1173 = typeof (HelicalDartPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1174 = typeof (HelloWorldPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1175 = typeof (HellraiserPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1176 = typeof (HexPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1177 = typeof (HibernatePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1178 = typeof (HighVoltagePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1179 = typeof (HotfixPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1180 = typeof (IllusionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1181 = typeof (ImbalancedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1182 = typeof (ImitationLearningPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1183 = typeof (ImprovementPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1184 = typeof (InfernoPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1185 = typeof (InfestedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1186 = typeof (InfiniteBladesPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1187 = typeof (IntangiblePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1188 = typeof (InterceptPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1189 = typeof (IterationPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1190 = typeof (JuggernautPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1191 = typeof (JugglingPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1192 = typeof (KnockdownPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1193 = typeof (LeadershipPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1194 = typeof (LethalityPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1195 = typeof (LightningRodPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1196 = typeof (LoopPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1197 = typeof (MachineLearningPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1198 = typeof (MagicBombPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1199 = typeof (ManglePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1200 = typeof (MasterPlannerPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1201 = typeof (MayhemPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1202 = typeof (MindRotPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1203 = typeof (MinionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1204 = typeof (MockCloneCardsOnPlayPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1205 = typeof (MockDoNotScaleInMultiplayerPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1206 = typeof (MockExtraTurnPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1207 = typeof (MockFreeCardsPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1208 = typeof (MockGainBlockOnAttackPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1209 = typeof (MockInvincibleOnDeathPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1210 = typeof (MockModifyEnergyCostPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1211 = typeof (MockModifyStarCostPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1212 = typeof (MockPhaseObserverPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1213 = typeof (MockPreventDeathPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1214 = typeof (MockRemoveDrawnCardsFromCombatPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1215 = typeof (MockResetCombatOnShufflePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1216 = typeof (MockRevivePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1217 = typeof (MockScaleInMultiplayerPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1218 = typeof (MockTemporaryStrengthLossPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1219 = typeof (MockUnhittablePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1220 = typeof (MonarchsGazePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1221 = typeof (MonarchsGazeStrengthDownPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1222 = typeof (MonologuePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1223 = typeof (NecroMasteryPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1224 = typeof (NemesisPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1225 = typeof (NeurosurgePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1226 = typeof (NightmarePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1227 = typeof (NoBlockPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1228 = typeof (NoDrawPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1229 = typeof (NoEnergyGainPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1230 = typeof (NostalgiaPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1231 = typeof (NoxiousFumesPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1232 = typeof (OblivionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1233 = typeof (OneForAllPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1234 = typeof (OneTwoPunchPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1235 = typeof (OrbitPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1236 = typeof (OutbreakPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1237 = typeof (PagestormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1238 = typeof (PainfulStabsPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1239 = typeof (PaleBlueDotPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1240 = typeof (PanachePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1241 = typeof (PaperCutsPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1242 = typeof (ParryPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1243 = typeof (PersonalHivePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1244 = typeof (PhantomBladesPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1245 = typeof (PiercingWailPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1246 = typeof (PillarOfCreationPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1247 = typeof (PlatingPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1248 = typeof (PlowPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1249 = typeof (PoisonPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1250 = typeof (PossessSpeedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1251 = typeof (PossessStrengthPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1252 = typeof (PrepTimePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1253 = typeof (PyrePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1254 = typeof (RadiancePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1255 = typeof (RagePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1256 = typeof (RampartPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1257 = typeof (RavenousPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1258 = typeof (ReaperFormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1259 = typeof (ReattachPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1260 = typeof (ReboundPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1261 = typeof (ReflectPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1262 = typeof (RegenPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1263 = typeof (ReptileTrinketPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1264 = typeof (RetainHandPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1265 = typeof (RingingPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1266 = typeof (RitualPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1267 = typeof (RollingBoulderPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1268 = typeof (RoyaltiesPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1269 = typeof (RupturePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1270 = typeof (SandpitPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1271 = typeof (SeekingEdgePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1272 = typeof (SelfFormingClayPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1273 = typeof (SentryModePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1274 = typeof (SerpentFormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1275 = typeof (SetupStrikePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1276 = typeof (ShacklingPotionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1277 = typeof (ShadowmeldPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1278 = typeof (ShadowStepPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1279 = typeof (ShriekPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1280 = typeof (ShrinkPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1281 = typeof (ShroudPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1282 = typeof (SicEmPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1283 = typeof (SignalBoostPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1284 = typeof (SkittishPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1285 = typeof (SleightOfFleshPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1286 = typeof (SlipperyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1287 = typeof (SlothPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1288 = typeof (SlowPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1289 = typeof (SlumberPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1290 = typeof (SmoggyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1291 = typeof (SmokestackPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1292 = typeof (SneakyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1293 = typeof (SoarPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1294 = typeof (SoulboundPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1295 = typeof (SpectrumShiftPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1296 = typeof (SpeedPotionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1297 = typeof (SpeedsterPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1298 = typeof (SpinnerPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1299 = typeof (SpiritOfAshPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1300 = typeof (StampedePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1301 = typeof (StarNextTurnPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1302 = typeof (SteamEruptionPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1303 = typeof (StockPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1304 = typeof (StormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1305 = typeof (StranglePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1306 = typeof (StratagemPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1307 = typeof (StrengthPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1308 = typeof (SubroutinePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1309 = typeof (SuckPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1310 = typeof (SummonNextTurnPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1311 = typeof (SurprisePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1312 = typeof (SurroundedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1313 = typeof (SwipePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1314 = typeof (SwordSagePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1315 = typeof (SynchronizePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1316 = typeof (TagTeamPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1317 = typeof (TaintedPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1318 = typeof (TangledPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1319 = typeof (TankPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1320 = typeof (TenderPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1321 = typeof (TerritorialPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1322 = typeof (TheBombPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1323 = typeof (TheGambitPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1324 = typeof (TheHuntPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1325 = typeof (TheSealedThronePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1326 = typeof (ThieveryPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1327 = typeof (ThornsPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1328 = typeof (ThunderPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1329 = typeof (ToolsOfTheTradePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1330 = typeof (ToricToughnessPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1331 = typeof (TrackingPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1332 = typeof (TrashToTreasurePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1333 = typeof (TyrannyPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1334 = typeof (UnderworldPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1335 = typeof (UnmovablePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1336 = typeof (VeilpiercerPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1337 = typeof (ViciousPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1338 = typeof (VigorPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1339 = typeof (VitalSparkPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1340 = typeof (VoidFormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1341 = typeof (VulnerablePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1342 = typeof (WasteAwayPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1343 = typeof (WeakPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1344 = typeof (WellLaidPlansPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1345 = typeof (WitheringPresencePower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1346 = typeof (WraithFormPower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1347 = typeof (DefectRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1348 = typeof (DeprecatedRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1349 = typeof (EventRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1350 = typeof (FallbackRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1351 = typeof (IroncladRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1352 = typeof (NecrobinderRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1353 = typeof (RegentRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1354 = typeof (SharedRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1355 = typeof (SilentRelicPool);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1356 = typeof (Akabeko);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1357 = typeof (AlchemicalCoffer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1358 = typeof (AmethystAubergine);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1359 = typeof (Anchor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1360 = typeof (ArcaneScroll);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1361 = typeof (ArchaicTooth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1362 = typeof (ArtOfWar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1363 = typeof (Astrolabe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1364 = typeof (BagOfMarbles);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1365 = typeof (BagOfPreparation);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1366 = typeof (BeatingRemnant);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1367 = typeof (BeautifulBracelet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1368 = typeof (Bellows);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1369 = typeof (BeltBuckle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1370 = typeof (BigHat);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1371 = typeof (BigMushroom);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1372 = typeof (BiiigHug);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1373 = typeof (BingBong);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1374 = typeof (BlackBlood);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1375 = typeof (BlackStar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1376 = typeof (BlessedAntler);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1377 = typeof (BloodSoakedRose);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1378 = typeof (BloodVial);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1379 = typeof (BoneFlute);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1380 = typeof (BoneTea);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1381 = typeof (Bookmark);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1382 = typeof (BookOfFiveRings);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1383 = typeof (BookRepairKnife);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1384 = typeof (BoomingConch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1385 = typeof (BoundPhylactery);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1386 = typeof (BowlerHat);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1387 = typeof (Bread);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1388 = typeof (BrilliantScarf);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1389 = typeof (Brimstone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1390 = typeof (BronzeScales);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1391 = typeof (BurningBlood);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1392 = typeof (BurningSticks);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1393 = typeof (MegaCrit.Sts2.Core.Models.Relics.Byrdpip);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1394 = typeof (CallingBell);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1395 = typeof (Candelabra);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1396 = typeof (CaptainsWheel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1397 = typeof (Cauldron);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1398 = typeof (CentennialPuzzle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1399 = typeof (Chandelier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1400 = typeof (CharonsAshes);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1401 = typeof (ChemicalX);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1402 = typeof (ChoicesParadox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1403 = typeof (ChosenCheese);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1404 = typeof (Circlet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1405 = typeof (Claws);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1406 = typeof (CloakClasp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1407 = typeof (CrackedCore);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1408 = typeof (Crossbow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1409 = typeof (CursedPearl);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1410 = typeof (DarkstonePeriapt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1411 = typeof (DataDisk);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1412 = typeof (DaughterOfTheWind);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1413 = typeof (DelicateFrond);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1414 = typeof (DemonTongue);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1415 = typeof (DeprecatedRelic);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1416 = typeof (DiamondDiadem);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1417 = typeof (DingyRug);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1418 = typeof (DistinguishedCape);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1419 = typeof (DivineDestiny);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1420 = typeof (DivineRight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1421 = typeof (DollysMirror);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1422 = typeof (DowsingRod);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1423 = typeof (DragonFruit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1424 = typeof (DreamCatcher);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1425 = typeof (Driftwood);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1426 = typeof (DustyTome);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1427 = typeof (Ectoplasm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1428 = typeof (ElectricShrymp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1429 = typeof (EmberTea);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1430 = typeof (EmotionChip);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1431 = typeof (EmptyCage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1432 = typeof (EternalFeather);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1433 = typeof (FakeAnchor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1434 = typeof (FakeBloodVial);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1435 = typeof (FakeHappyFlower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1436 = typeof (FakeLeesWaffle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1437 = typeof (FakeMango);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1438 = typeof (FakeMerchantsRug);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1439 = typeof (FakeOrichalcum);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1440 = typeof (FakeSneckoEye);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1441 = typeof (FakeStrikeDummy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1442 = typeof (FakeVenerableTeaSet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1443 = typeof (FencingManual);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1444 = typeof (FestivePopper);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1445 = typeof (Fiddle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1446 = typeof (FishingRod);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1447 = typeof (ForgottenSoul);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1448 = typeof (FragrantMushroom);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1449 = typeof (FresnelLens);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1450 = typeof (FrozenEgg);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1451 = typeof (FuneraryMask);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1452 = typeof (FurCoat);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1453 = typeof (GalacticDust);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1454 = typeof (GamblingChip);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1455 = typeof (GamePiece);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1456 = typeof (GhostSeed);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1457 = typeof (Girya);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1458 = typeof (GlassEye);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1459 = typeof (Glitter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1460 = typeof (GnarledHammer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1461 = typeof (GoldenCompass);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1462 = typeof (GoldenPearl);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1463 = typeof (GoldPlatedCables);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1464 = typeof (Gorget);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1465 = typeof (GremlinHorn);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1466 = typeof (HandDrill);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1467 = typeof (HappyFlower);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1468 = typeof (HeftyTablet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1469 = typeof (HelicalDart);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1470 = typeof (HistoryCourse);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1471 = typeof (HornCleat);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1472 = typeof (IceCream);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1473 = typeof (InfusedCore);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1474 = typeof (IntimidatingHelmet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1475 = typeof (IronClub);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1476 = typeof (IvoryTile);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1477 = typeof (JeweledMask);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1478 = typeof (JewelryBox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1479 = typeof (JossPaper);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1480 = typeof (JuzuBracelet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1481 = typeof (Kaleidoscope);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1482 = typeof (Kifuda);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1483 = typeof (Kunai);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1484 = typeof (Kusarigama);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1485 = typeof (Lantern);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1486 = typeof (LargeCapsule);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1487 = typeof (LastingCandy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1488 = typeof (LavaLamp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1489 = typeof (LavaRock);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1490 = typeof (LeadPaperweight);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1491 = typeof (LeafyPoultice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1492 = typeof (LeesWaffle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1493 = typeof (LetterOpener);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1494 = typeof (LizardTail);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1495 = typeof (LoomingFruit);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1496 = typeof (LordsParasol);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1497 = typeof (LostCoffer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1498 = typeof (MegaCrit.Sts2.Core.Models.Relics.LostWisp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1499 = typeof (LuckyFysh);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1500 = typeof (LunarPastry);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1501 = typeof (Mango);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1502 = typeof (MassiveScroll);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1503 = typeof (MawBank);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1504 = typeof (MealTicket);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1505 = typeof (MeatCleaver);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1506 = typeof (MeatOnTheBone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1507 = typeof (MembershipCard);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1508 = typeof (MercuryHourglass);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1509 = typeof (Metronome);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1510 = typeof (MiniatureCannon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1511 = typeof (MiniatureTent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1512 = typeof (MiniRegent);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1513 = typeof (MoltenEgg);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1514 = typeof (MrStruggles);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1515 = typeof (MummifiedHand);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1516 = typeof (MusicBox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1517 = typeof (MysticLighter);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1518 = typeof (NeowsBones);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1519 = typeof (NeowsSacrifice);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1520 = typeof (NeowsTalisman);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1521 = typeof (NeowsTorment);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1522 = typeof (NewLeaf);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1523 = typeof (NinjaScroll);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1524 = typeof (Nunchaku);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1525 = typeof (NutritiousOyster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1526 = typeof (NutritiousSoup);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1527 = typeof (OddlySmoothStone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1528 = typeof (OldCoin);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1529 = typeof (OrangeDough);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1530 = typeof (Orichalcum);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1531 = typeof (OrnamentalFan);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1532 = typeof (Orrery);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1533 = typeof (PaelsBlood);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1534 = typeof (PaelsClaw);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1535 = typeof (PaelsEye);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1536 = typeof (PaelsFlesh);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1537 = typeof (PaelsGrowth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1538 = typeof (PaelsHorn);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1539 = typeof (MegaCrit.Sts2.Core.Models.Relics.PaelsLegion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1540 = typeof (PaelsTears);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1541 = typeof (PaelsTooth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1542 = typeof (PaelsWing);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1543 = typeof (PandorasBox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1544 = typeof (Pantograph);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1545 = typeof (PaperKrane);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1546 = typeof (PaperPhrog);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1547 = typeof (ParryingShield);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1548 = typeof (Pear);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1549 = typeof (Pendulum);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1550 = typeof (PenNib);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1551 = typeof (Permafrost);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1552 = typeof (PetrifiedToad);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1553 = typeof (PhialHolster);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1554 = typeof (PhilosophersStone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1555 = typeof (PhylacteryUnbound);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1556 = typeof (Planisphere);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1557 = typeof (Pocketwatch);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1558 = typeof (PollinousCore);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1559 = typeof (Pomander);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1560 = typeof (PotionBelt);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1561 = typeof (PowerCell);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1562 = typeof (PrayerWheel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1563 = typeof (PrecariousShears);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1564 = typeof (PreciseScissors);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1565 = typeof (PreservedFog);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1566 = typeof (PrismaticGem);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1567 = typeof (PumpkinCandle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1568 = typeof (PunchDagger);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1569 = typeof (RadiantPearl);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1570 = typeof (RainbowRing);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1571 = typeof (RazorTooth);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1572 = typeof (RedMask);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1573 = typeof (RedSkull);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1574 = typeof (Regalite);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1575 = typeof (RegalPillow);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1576 = typeof (ReptileTrinket);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1577 = typeof (RingingTriangle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1578 = typeof (RingOfTheDrake);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1579 = typeof (RingOfTheSnake);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1580 = typeof (RippleBasin);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1581 = typeof (RoyalPoison);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1582 = typeof (RoyalStamp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1583 = typeof (RuinedHelmet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1584 = typeof (RunicCapacitor);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1585 = typeof (RunicPyramid);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1586 = typeof (Sai);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1587 = typeof (SandCastle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1588 = typeof (ScreamingFlagon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1589 = typeof (ScrollBoxes);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1590 = typeof (SeaGlass);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1591 = typeof (SealOfGold);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1592 = typeof (SelfFormingClay);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1593 = typeof (SereTalon);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1594 = typeof (Shovel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1595 = typeof (Shuriken);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1596 = typeof (SignetRing);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1597 = typeof (SilkenTress);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1598 = typeof (SilverCrucible);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1599 = typeof (SlingOfCourage);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1600 = typeof (SmallCapsule);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1601 = typeof (SneckoEye);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1602 = typeof (SneckoSkull);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1603 = typeof (Sozu);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1604 = typeof (SparklingRouge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1605 = typeof (SpikedGauntlets);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1606 = typeof (StoneCalendar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1607 = typeof (StoneCracker);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1608 = typeof (StoneHumidifier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1609 = typeof (Storybook);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1610 = typeof (Strawberry);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1611 = typeof (StrikeDummy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1612 = typeof (SturdyClamp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1613 = typeof (SwordOfJade);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1614 = typeof (SwordOfStone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1615 = typeof (SymbioticVirus);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1616 = typeof (TanxsWhistle);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1617 = typeof (TeaOfDiscourtesy);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1618 = typeof (TheAbacus);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1619 = typeof (TheBoot);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1620 = typeof (TheCourier);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1621 = typeof (ThrowingAxe);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1622 = typeof (Tingsha);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1623 = typeof (TinyMailbox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1624 = typeof (ToastyMittens);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1625 = typeof (Toolbox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1626 = typeof (TouchOfOrobas);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1627 = typeof (ToughBandages);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1628 = typeof (ToxicEgg);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1629 = typeof (ToyBox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1630 = typeof (TriBoomerang);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1631 = typeof (TungstenRod);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1632 = typeof (TuningFork);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1633 = typeof (TwistedFunnel);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1634 = typeof (UnceasingTop);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1635 = typeof (UndyingSigil);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1636 = typeof (UnsettlingLamp);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1637 = typeof (Vajra);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1638 = typeof (Vambrace);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1639 = typeof (VelvetChoker);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1640 = typeof (VenerableTeaSet);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1641 = typeof (VeryHotCocoa);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1642 = typeof (VexingPuzzlebox);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1643 = typeof (VitruvianMinion);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1644 = typeof (WarHammer);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1645 = typeof (WarPaint);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1646 = typeof (Whetstone);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1647 = typeof (WhisperingEarring);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1648 = typeof (WhiteBeastStatue);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1649 = typeof (WhiteStar);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1650 = typeof (WingCharm);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1651 = typeof (WingedBoots);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1652 = typeof (WongoCustomerAppreciationBadge);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1653 = typeof (WongosMysteryTicket);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1654 = typeof (YummyCookie);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1655 = typeof (MultiplayerScalingModel);
  private static readonly Type[] _subtypes = new Type[1656]
  {
    AbstractModelSubtypes._t0,
    AbstractModelSubtypes._t1,
    AbstractModelSubtypes._t2,
    AbstractModelSubtypes._t3,
    AbstractModelSubtypes._t4,
    AbstractModelSubtypes._t5,
    AbstractModelSubtypes._t6,
    AbstractModelSubtypes._t7,
    AbstractModelSubtypes._t8,
    AbstractModelSubtypes._t9,
    AbstractModelSubtypes._t10,
    AbstractModelSubtypes._t11,
    AbstractModelSubtypes._t12,
    AbstractModelSubtypes._t13,
    AbstractModelSubtypes._t14,
    AbstractModelSubtypes._t15,
    AbstractModelSubtypes._t16,
    AbstractModelSubtypes._t17,
    AbstractModelSubtypes._t18,
    AbstractModelSubtypes._t19,
    AbstractModelSubtypes._t20,
    AbstractModelSubtypes._t21,
    AbstractModelSubtypes._t22,
    AbstractModelSubtypes._t23,
    AbstractModelSubtypes._t24,
    AbstractModelSubtypes._t25,
    AbstractModelSubtypes._t26,
    AbstractModelSubtypes._t27,
    AbstractModelSubtypes._t28,
    AbstractModelSubtypes._t29,
    AbstractModelSubtypes._t30,
    AbstractModelSubtypes._t31,
    AbstractModelSubtypes._t32,
    AbstractModelSubtypes._t33,
    AbstractModelSubtypes._t34,
    AbstractModelSubtypes._t35,
    AbstractModelSubtypes._t36,
    AbstractModelSubtypes._t37,
    AbstractModelSubtypes._t38,
    AbstractModelSubtypes._t39,
    AbstractModelSubtypes._t40,
    AbstractModelSubtypes._t41,
    AbstractModelSubtypes._t42,
    AbstractModelSubtypes._t43,
    AbstractModelSubtypes._t44,
    AbstractModelSubtypes._t45,
    AbstractModelSubtypes._t46,
    AbstractModelSubtypes._t47,
    AbstractModelSubtypes._t48,
    AbstractModelSubtypes._t49,
    AbstractModelSubtypes._t50,
    AbstractModelSubtypes._t51,
    AbstractModelSubtypes._t52,
    AbstractModelSubtypes._t53,
    AbstractModelSubtypes._t54,
    AbstractModelSubtypes._t55,
    AbstractModelSubtypes._t56,
    AbstractModelSubtypes._t57,
    AbstractModelSubtypes._t58,
    AbstractModelSubtypes._t59,
    AbstractModelSubtypes._t60,
    AbstractModelSubtypes._t61,
    AbstractModelSubtypes._t62,
    AbstractModelSubtypes._t63,
    AbstractModelSubtypes._t64,
    AbstractModelSubtypes._t65,
    AbstractModelSubtypes._t66,
    AbstractModelSubtypes._t67,
    AbstractModelSubtypes._t68,
    AbstractModelSubtypes._t69,
    AbstractModelSubtypes._t70,
    AbstractModelSubtypes._t71,
    AbstractModelSubtypes._t72,
    AbstractModelSubtypes._t73,
    AbstractModelSubtypes._t74,
    AbstractModelSubtypes._t75,
    AbstractModelSubtypes._t76,
    AbstractModelSubtypes._t77,
    AbstractModelSubtypes._t78,
    AbstractModelSubtypes._t79,
    AbstractModelSubtypes._t80,
    AbstractModelSubtypes._t81,
    AbstractModelSubtypes._t82,
    AbstractModelSubtypes._t83,
    AbstractModelSubtypes._t84,
    AbstractModelSubtypes._t85,
    AbstractModelSubtypes._t86,
    AbstractModelSubtypes._t87,
    AbstractModelSubtypes._t88,
    AbstractModelSubtypes._t89,
    AbstractModelSubtypes._t90,
    AbstractModelSubtypes._t91,
    AbstractModelSubtypes._t92,
    AbstractModelSubtypes._t93,
    AbstractModelSubtypes._t94,
    AbstractModelSubtypes._t95,
    AbstractModelSubtypes._t96,
    AbstractModelSubtypes._t97,
    AbstractModelSubtypes._t98,
    AbstractModelSubtypes._t99,
    AbstractModelSubtypes._t100,
    AbstractModelSubtypes._t101,
    AbstractModelSubtypes._t102,
    AbstractModelSubtypes._t103,
    AbstractModelSubtypes._t104,
    AbstractModelSubtypes._t105,
    AbstractModelSubtypes._t106,
    AbstractModelSubtypes._t107,
    AbstractModelSubtypes._t108,
    AbstractModelSubtypes._t109,
    AbstractModelSubtypes._t110,
    AbstractModelSubtypes._t111,
    AbstractModelSubtypes._t112,
    AbstractModelSubtypes._t113,
    AbstractModelSubtypes._t114,
    AbstractModelSubtypes._t115,
    AbstractModelSubtypes._t116,
    AbstractModelSubtypes._t117,
    AbstractModelSubtypes._t118,
    AbstractModelSubtypes._t119,
    AbstractModelSubtypes._t120,
    AbstractModelSubtypes._t121,
    AbstractModelSubtypes._t122,
    AbstractModelSubtypes._t123,
    AbstractModelSubtypes._t124,
    AbstractModelSubtypes._t125,
    AbstractModelSubtypes._t126,
    AbstractModelSubtypes._t127,
    AbstractModelSubtypes._t128,
    AbstractModelSubtypes._t129,
    AbstractModelSubtypes._t130,
    AbstractModelSubtypes._t131,
    AbstractModelSubtypes._t132,
    AbstractModelSubtypes._t133,
    AbstractModelSubtypes._t134,
    AbstractModelSubtypes._t135,
    AbstractModelSubtypes._t136,
    AbstractModelSubtypes._t137,
    AbstractModelSubtypes._t138,
    AbstractModelSubtypes._t139,
    AbstractModelSubtypes._t140,
    AbstractModelSubtypes._t141,
    AbstractModelSubtypes._t142,
    AbstractModelSubtypes._t143,
    AbstractModelSubtypes._t144,
    AbstractModelSubtypes._t145,
    AbstractModelSubtypes._t146,
    AbstractModelSubtypes._t147,
    AbstractModelSubtypes._t148,
    AbstractModelSubtypes._t149,
    AbstractModelSubtypes._t150,
    AbstractModelSubtypes._t151,
    AbstractModelSubtypes._t152,
    AbstractModelSubtypes._t153,
    AbstractModelSubtypes._t154,
    AbstractModelSubtypes._t155,
    AbstractModelSubtypes._t156,
    AbstractModelSubtypes._t157,
    AbstractModelSubtypes._t158,
    AbstractModelSubtypes._t159,
    AbstractModelSubtypes._t160,
    AbstractModelSubtypes._t161,
    AbstractModelSubtypes._t162,
    AbstractModelSubtypes._t163,
    AbstractModelSubtypes._t164,
    AbstractModelSubtypes._t165,
    AbstractModelSubtypes._t166,
    AbstractModelSubtypes._t167,
    AbstractModelSubtypes._t168,
    AbstractModelSubtypes._t169,
    AbstractModelSubtypes._t170,
    AbstractModelSubtypes._t171,
    AbstractModelSubtypes._t172,
    AbstractModelSubtypes._t173,
    AbstractModelSubtypes._t174,
    AbstractModelSubtypes._t175,
    AbstractModelSubtypes._t176,
    AbstractModelSubtypes._t177,
    AbstractModelSubtypes._t178,
    AbstractModelSubtypes._t179,
    AbstractModelSubtypes._t180,
    AbstractModelSubtypes._t181,
    AbstractModelSubtypes._t182,
    AbstractModelSubtypes._t183,
    AbstractModelSubtypes._t184,
    AbstractModelSubtypes._t185,
    AbstractModelSubtypes._t186,
    AbstractModelSubtypes._t187,
    AbstractModelSubtypes._t188,
    AbstractModelSubtypes._t189,
    AbstractModelSubtypes._t190,
    AbstractModelSubtypes._t191,
    AbstractModelSubtypes._t192,
    AbstractModelSubtypes._t193,
    AbstractModelSubtypes._t194,
    AbstractModelSubtypes._t195,
    AbstractModelSubtypes._t196,
    AbstractModelSubtypes._t197,
    AbstractModelSubtypes._t198,
    AbstractModelSubtypes._t199,
    AbstractModelSubtypes._t200,
    AbstractModelSubtypes._t201,
    AbstractModelSubtypes._t202,
    AbstractModelSubtypes._t203,
    AbstractModelSubtypes._t204,
    AbstractModelSubtypes._t205,
    AbstractModelSubtypes._t206,
    AbstractModelSubtypes._t207,
    AbstractModelSubtypes._t208,
    AbstractModelSubtypes._t209,
    AbstractModelSubtypes._t210,
    AbstractModelSubtypes._t211,
    AbstractModelSubtypes._t212,
    AbstractModelSubtypes._t213,
    AbstractModelSubtypes._t214,
    AbstractModelSubtypes._t215,
    AbstractModelSubtypes._t216,
    AbstractModelSubtypes._t217,
    AbstractModelSubtypes._t218,
    AbstractModelSubtypes._t219,
    AbstractModelSubtypes._t220,
    AbstractModelSubtypes._t221,
    AbstractModelSubtypes._t222,
    AbstractModelSubtypes._t223,
    AbstractModelSubtypes._t224,
    AbstractModelSubtypes._t225,
    AbstractModelSubtypes._t226,
    AbstractModelSubtypes._t227,
    AbstractModelSubtypes._t228,
    AbstractModelSubtypes._t229,
    AbstractModelSubtypes._t230,
    AbstractModelSubtypes._t231,
    AbstractModelSubtypes._t232,
    AbstractModelSubtypes._t233,
    AbstractModelSubtypes._t234,
    AbstractModelSubtypes._t235,
    AbstractModelSubtypes._t236,
    AbstractModelSubtypes._t237,
    AbstractModelSubtypes._t238,
    AbstractModelSubtypes._t239,
    AbstractModelSubtypes._t240,
    AbstractModelSubtypes._t241,
    AbstractModelSubtypes._t242,
    AbstractModelSubtypes._t243,
    AbstractModelSubtypes._t244,
    AbstractModelSubtypes._t245,
    AbstractModelSubtypes._t246,
    AbstractModelSubtypes._t247,
    AbstractModelSubtypes._t248,
    AbstractModelSubtypes._t249,
    AbstractModelSubtypes._t250,
    AbstractModelSubtypes._t251,
    AbstractModelSubtypes._t252,
    AbstractModelSubtypes._t253,
    AbstractModelSubtypes._t254,
    AbstractModelSubtypes._t255,
    AbstractModelSubtypes._t256,
    AbstractModelSubtypes._t257,
    AbstractModelSubtypes._t258,
    AbstractModelSubtypes._t259,
    AbstractModelSubtypes._t260,
    AbstractModelSubtypes._t261,
    AbstractModelSubtypes._t262,
    AbstractModelSubtypes._t263,
    AbstractModelSubtypes._t264,
    AbstractModelSubtypes._t265,
    AbstractModelSubtypes._t266,
    AbstractModelSubtypes._t267,
    AbstractModelSubtypes._t268,
    AbstractModelSubtypes._t269,
    AbstractModelSubtypes._t270,
    AbstractModelSubtypes._t271,
    AbstractModelSubtypes._t272,
    AbstractModelSubtypes._t273,
    AbstractModelSubtypes._t274,
    AbstractModelSubtypes._t275,
    AbstractModelSubtypes._t276,
    AbstractModelSubtypes._t277,
    AbstractModelSubtypes._t278,
    AbstractModelSubtypes._t279,
    AbstractModelSubtypes._t280,
    AbstractModelSubtypes._t281,
    AbstractModelSubtypes._t282,
    AbstractModelSubtypes._t283,
    AbstractModelSubtypes._t284,
    AbstractModelSubtypes._t285,
    AbstractModelSubtypes._t286,
    AbstractModelSubtypes._t287,
    AbstractModelSubtypes._t288,
    AbstractModelSubtypes._t289,
    AbstractModelSubtypes._t290,
    AbstractModelSubtypes._t291,
    AbstractModelSubtypes._t292,
    AbstractModelSubtypes._t293,
    AbstractModelSubtypes._t294,
    AbstractModelSubtypes._t295,
    AbstractModelSubtypes._t296,
    AbstractModelSubtypes._t297,
    AbstractModelSubtypes._t298,
    AbstractModelSubtypes._t299,
    AbstractModelSubtypes._t300,
    AbstractModelSubtypes._t301,
    AbstractModelSubtypes._t302,
    AbstractModelSubtypes._t303,
    AbstractModelSubtypes._t304,
    AbstractModelSubtypes._t305,
    AbstractModelSubtypes._t306,
    AbstractModelSubtypes._t307,
    AbstractModelSubtypes._t308,
    AbstractModelSubtypes._t309,
    AbstractModelSubtypes._t310,
    AbstractModelSubtypes._t311,
    AbstractModelSubtypes._t312,
    AbstractModelSubtypes._t313,
    AbstractModelSubtypes._t314,
    AbstractModelSubtypes._t315,
    AbstractModelSubtypes._t316,
    AbstractModelSubtypes._t317,
    AbstractModelSubtypes._t318,
    AbstractModelSubtypes._t319,
    AbstractModelSubtypes._t320,
    AbstractModelSubtypes._t321,
    AbstractModelSubtypes._t322,
    AbstractModelSubtypes._t323,
    AbstractModelSubtypes._t324,
    AbstractModelSubtypes._t325,
    AbstractModelSubtypes._t326,
    AbstractModelSubtypes._t327,
    AbstractModelSubtypes._t328,
    AbstractModelSubtypes._t329,
    AbstractModelSubtypes._t330,
    AbstractModelSubtypes._t331,
    AbstractModelSubtypes._t332,
    AbstractModelSubtypes._t333,
    AbstractModelSubtypes._t334,
    AbstractModelSubtypes._t335,
    AbstractModelSubtypes._t336,
    AbstractModelSubtypes._t337,
    AbstractModelSubtypes._t338,
    AbstractModelSubtypes._t339,
    AbstractModelSubtypes._t340,
    AbstractModelSubtypes._t341,
    AbstractModelSubtypes._t342,
    AbstractModelSubtypes._t343,
    AbstractModelSubtypes._t344,
    AbstractModelSubtypes._t345,
    AbstractModelSubtypes._t346,
    AbstractModelSubtypes._t347,
    AbstractModelSubtypes._t348,
    AbstractModelSubtypes._t349,
    AbstractModelSubtypes._t350,
    AbstractModelSubtypes._t351,
    AbstractModelSubtypes._t352,
    AbstractModelSubtypes._t353,
    AbstractModelSubtypes._t354,
    AbstractModelSubtypes._t355,
    AbstractModelSubtypes._t356,
    AbstractModelSubtypes._t357,
    AbstractModelSubtypes._t358,
    AbstractModelSubtypes._t359,
    AbstractModelSubtypes._t360,
    AbstractModelSubtypes._t361,
    AbstractModelSubtypes._t362,
    AbstractModelSubtypes._t363,
    AbstractModelSubtypes._t364,
    AbstractModelSubtypes._t365,
    AbstractModelSubtypes._t366,
    AbstractModelSubtypes._t367,
    AbstractModelSubtypes._t368,
    AbstractModelSubtypes._t369,
    AbstractModelSubtypes._t370,
    AbstractModelSubtypes._t371,
    AbstractModelSubtypes._t372,
    AbstractModelSubtypes._t373,
    AbstractModelSubtypes._t374,
    AbstractModelSubtypes._t375,
    AbstractModelSubtypes._t376,
    AbstractModelSubtypes._t377,
    AbstractModelSubtypes._t378,
    AbstractModelSubtypes._t379,
    AbstractModelSubtypes._t380,
    AbstractModelSubtypes._t381,
    AbstractModelSubtypes._t382,
    AbstractModelSubtypes._t383,
    AbstractModelSubtypes._t384,
    AbstractModelSubtypes._t385,
    AbstractModelSubtypes._t386,
    AbstractModelSubtypes._t387,
    AbstractModelSubtypes._t388,
    AbstractModelSubtypes._t389,
    AbstractModelSubtypes._t390,
    AbstractModelSubtypes._t391,
    AbstractModelSubtypes._t392,
    AbstractModelSubtypes._t393,
    AbstractModelSubtypes._t394,
    AbstractModelSubtypes._t395,
    AbstractModelSubtypes._t396,
    AbstractModelSubtypes._t397,
    AbstractModelSubtypes._t398,
    AbstractModelSubtypes._t399,
    AbstractModelSubtypes._t400,
    AbstractModelSubtypes._t401,
    AbstractModelSubtypes._t402,
    AbstractModelSubtypes._t403,
    AbstractModelSubtypes._t404,
    AbstractModelSubtypes._t405,
    AbstractModelSubtypes._t406,
    AbstractModelSubtypes._t407,
    AbstractModelSubtypes._t408,
    AbstractModelSubtypes._t409,
    AbstractModelSubtypes._t410,
    AbstractModelSubtypes._t411,
    AbstractModelSubtypes._t412,
    AbstractModelSubtypes._t413,
    AbstractModelSubtypes._t414,
    AbstractModelSubtypes._t415,
    AbstractModelSubtypes._t416,
    AbstractModelSubtypes._t417,
    AbstractModelSubtypes._t418,
    AbstractModelSubtypes._t419,
    AbstractModelSubtypes._t420,
    AbstractModelSubtypes._t421,
    AbstractModelSubtypes._t422,
    AbstractModelSubtypes._t423,
    AbstractModelSubtypes._t424,
    AbstractModelSubtypes._t425,
    AbstractModelSubtypes._t426,
    AbstractModelSubtypes._t427,
    AbstractModelSubtypes._t428,
    AbstractModelSubtypes._t429,
    AbstractModelSubtypes._t430,
    AbstractModelSubtypes._t431,
    AbstractModelSubtypes._t432,
    AbstractModelSubtypes._t433,
    AbstractModelSubtypes._t434,
    AbstractModelSubtypes._t435,
    AbstractModelSubtypes._t436,
    AbstractModelSubtypes._t437,
    AbstractModelSubtypes._t438,
    AbstractModelSubtypes._t439,
    AbstractModelSubtypes._t440,
    AbstractModelSubtypes._t441,
    AbstractModelSubtypes._t442,
    AbstractModelSubtypes._t443,
    AbstractModelSubtypes._t444,
    AbstractModelSubtypes._t445,
    AbstractModelSubtypes._t446,
    AbstractModelSubtypes._t447,
    AbstractModelSubtypes._t448,
    AbstractModelSubtypes._t449,
    AbstractModelSubtypes._t450,
    AbstractModelSubtypes._t451,
    AbstractModelSubtypes._t452,
    AbstractModelSubtypes._t453,
    AbstractModelSubtypes._t454,
    AbstractModelSubtypes._t455,
    AbstractModelSubtypes._t456,
    AbstractModelSubtypes._t457,
    AbstractModelSubtypes._t458,
    AbstractModelSubtypes._t459,
    AbstractModelSubtypes._t460,
    AbstractModelSubtypes._t461,
    AbstractModelSubtypes._t462,
    AbstractModelSubtypes._t463,
    AbstractModelSubtypes._t464,
    AbstractModelSubtypes._t465,
    AbstractModelSubtypes._t466,
    AbstractModelSubtypes._t467,
    AbstractModelSubtypes._t468,
    AbstractModelSubtypes._t469,
    AbstractModelSubtypes._t470,
    AbstractModelSubtypes._t471,
    AbstractModelSubtypes._t472,
    AbstractModelSubtypes._t473,
    AbstractModelSubtypes._t474,
    AbstractModelSubtypes._t475,
    AbstractModelSubtypes._t476,
    AbstractModelSubtypes._t477,
    AbstractModelSubtypes._t478,
    AbstractModelSubtypes._t479,
    AbstractModelSubtypes._t480,
    AbstractModelSubtypes._t481,
    AbstractModelSubtypes._t482,
    AbstractModelSubtypes._t483,
    AbstractModelSubtypes._t484,
    AbstractModelSubtypes._t485,
    AbstractModelSubtypes._t486,
    AbstractModelSubtypes._t487,
    AbstractModelSubtypes._t488,
    AbstractModelSubtypes._t489,
    AbstractModelSubtypes._t490,
    AbstractModelSubtypes._t491,
    AbstractModelSubtypes._t492,
    AbstractModelSubtypes._t493,
    AbstractModelSubtypes._t494,
    AbstractModelSubtypes._t495,
    AbstractModelSubtypes._t496,
    AbstractModelSubtypes._t497,
    AbstractModelSubtypes._t498,
    AbstractModelSubtypes._t499,
    AbstractModelSubtypes._t500,
    AbstractModelSubtypes._t501,
    AbstractModelSubtypes._t502,
    AbstractModelSubtypes._t503,
    AbstractModelSubtypes._t504,
    AbstractModelSubtypes._t505,
    AbstractModelSubtypes._t506,
    AbstractModelSubtypes._t507,
    AbstractModelSubtypes._t508,
    AbstractModelSubtypes._t509,
    AbstractModelSubtypes._t510,
    AbstractModelSubtypes._t511,
    AbstractModelSubtypes._t512,
    AbstractModelSubtypes._t513,
    AbstractModelSubtypes._t514,
    AbstractModelSubtypes._t515,
    AbstractModelSubtypes._t516,
    AbstractModelSubtypes._t517,
    AbstractModelSubtypes._t518,
    AbstractModelSubtypes._t519,
    AbstractModelSubtypes._t520,
    AbstractModelSubtypes._t521,
    AbstractModelSubtypes._t522,
    AbstractModelSubtypes._t523,
    AbstractModelSubtypes._t524,
    AbstractModelSubtypes._t525,
    AbstractModelSubtypes._t526,
    AbstractModelSubtypes._t527,
    AbstractModelSubtypes._t528,
    AbstractModelSubtypes._t529,
    AbstractModelSubtypes._t530,
    AbstractModelSubtypes._t531,
    AbstractModelSubtypes._t532,
    AbstractModelSubtypes._t533,
    AbstractModelSubtypes._t534,
    AbstractModelSubtypes._t535,
    AbstractModelSubtypes._t536,
    AbstractModelSubtypes._t537,
    AbstractModelSubtypes._t538,
    AbstractModelSubtypes._t539,
    AbstractModelSubtypes._t540,
    AbstractModelSubtypes._t541,
    AbstractModelSubtypes._t542,
    AbstractModelSubtypes._t543,
    AbstractModelSubtypes._t544,
    AbstractModelSubtypes._t545,
    AbstractModelSubtypes._t546,
    AbstractModelSubtypes._t547,
    AbstractModelSubtypes._t548,
    AbstractModelSubtypes._t549,
    AbstractModelSubtypes._t550,
    AbstractModelSubtypes._t551,
    AbstractModelSubtypes._t552,
    AbstractModelSubtypes._t553,
    AbstractModelSubtypes._t554,
    AbstractModelSubtypes._t555,
    AbstractModelSubtypes._t556,
    AbstractModelSubtypes._t557,
    AbstractModelSubtypes._t558,
    AbstractModelSubtypes._t559,
    AbstractModelSubtypes._t560,
    AbstractModelSubtypes._t561,
    AbstractModelSubtypes._t562,
    AbstractModelSubtypes._t563,
    AbstractModelSubtypes._t564,
    AbstractModelSubtypes._t565,
    AbstractModelSubtypes._t566,
    AbstractModelSubtypes._t567,
    AbstractModelSubtypes._t568,
    AbstractModelSubtypes._t569,
    AbstractModelSubtypes._t570,
    AbstractModelSubtypes._t571,
    AbstractModelSubtypes._t572,
    AbstractModelSubtypes._t573,
    AbstractModelSubtypes._t574,
    AbstractModelSubtypes._t575,
    AbstractModelSubtypes._t576,
    AbstractModelSubtypes._t577,
    AbstractModelSubtypes._t578,
    AbstractModelSubtypes._t579,
    AbstractModelSubtypes._t580,
    AbstractModelSubtypes._t581,
    AbstractModelSubtypes._t582,
    AbstractModelSubtypes._t583,
    AbstractModelSubtypes._t584,
    AbstractModelSubtypes._t585,
    AbstractModelSubtypes._t586,
    AbstractModelSubtypes._t587,
    AbstractModelSubtypes._t588,
    AbstractModelSubtypes._t589,
    AbstractModelSubtypes._t590,
    AbstractModelSubtypes._t591,
    AbstractModelSubtypes._t592,
    AbstractModelSubtypes._t593,
    AbstractModelSubtypes._t594,
    AbstractModelSubtypes._t595,
    AbstractModelSubtypes._t596,
    AbstractModelSubtypes._t597,
    AbstractModelSubtypes._t598,
    AbstractModelSubtypes._t599,
    AbstractModelSubtypes._t600,
    AbstractModelSubtypes._t601,
    AbstractModelSubtypes._t602,
    AbstractModelSubtypes._t603,
    AbstractModelSubtypes._t604,
    AbstractModelSubtypes._t605,
    AbstractModelSubtypes._t606,
    AbstractModelSubtypes._t607,
    AbstractModelSubtypes._t608,
    AbstractModelSubtypes._t609,
    AbstractModelSubtypes._t610,
    AbstractModelSubtypes._t611,
    AbstractModelSubtypes._t612,
    AbstractModelSubtypes._t613,
    AbstractModelSubtypes._t614,
    AbstractModelSubtypes._t615,
    AbstractModelSubtypes._t616,
    AbstractModelSubtypes._t617,
    AbstractModelSubtypes._t618,
    AbstractModelSubtypes._t619,
    AbstractModelSubtypes._t620,
    AbstractModelSubtypes._t621,
    AbstractModelSubtypes._t622,
    AbstractModelSubtypes._t623,
    AbstractModelSubtypes._t624,
    AbstractModelSubtypes._t625,
    AbstractModelSubtypes._t626,
    AbstractModelSubtypes._t627,
    AbstractModelSubtypes._t628,
    AbstractModelSubtypes._t629,
    AbstractModelSubtypes._t630,
    AbstractModelSubtypes._t631,
    AbstractModelSubtypes._t632,
    AbstractModelSubtypes._t633,
    AbstractModelSubtypes._t634,
    AbstractModelSubtypes._t635,
    AbstractModelSubtypes._t636,
    AbstractModelSubtypes._t637,
    AbstractModelSubtypes._t638,
    AbstractModelSubtypes._t639,
    AbstractModelSubtypes._t640,
    AbstractModelSubtypes._t641,
    AbstractModelSubtypes._t642,
    AbstractModelSubtypes._t643,
    AbstractModelSubtypes._t644,
    AbstractModelSubtypes._t645,
    AbstractModelSubtypes._t646,
    AbstractModelSubtypes._t647,
    AbstractModelSubtypes._t648,
    AbstractModelSubtypes._t649,
    AbstractModelSubtypes._t650,
    AbstractModelSubtypes._t651,
    AbstractModelSubtypes._t652,
    AbstractModelSubtypes._t653,
    AbstractModelSubtypes._t654,
    AbstractModelSubtypes._t655,
    AbstractModelSubtypes._t656,
    AbstractModelSubtypes._t657,
    AbstractModelSubtypes._t658,
    AbstractModelSubtypes._t659,
    AbstractModelSubtypes._t660,
    AbstractModelSubtypes._t661,
    AbstractModelSubtypes._t662,
    AbstractModelSubtypes._t663,
    AbstractModelSubtypes._t664,
    AbstractModelSubtypes._t665,
    AbstractModelSubtypes._t666,
    AbstractModelSubtypes._t667,
    AbstractModelSubtypes._t668,
    AbstractModelSubtypes._t669,
    AbstractModelSubtypes._t670,
    AbstractModelSubtypes._t671,
    AbstractModelSubtypes._t672,
    AbstractModelSubtypes._t673,
    AbstractModelSubtypes._t674,
    AbstractModelSubtypes._t675,
    AbstractModelSubtypes._t676,
    AbstractModelSubtypes._t677,
    AbstractModelSubtypes._t678,
    AbstractModelSubtypes._t679,
    AbstractModelSubtypes._t680,
    AbstractModelSubtypes._t681,
    AbstractModelSubtypes._t682,
    AbstractModelSubtypes._t683,
    AbstractModelSubtypes._t684,
    AbstractModelSubtypes._t685,
    AbstractModelSubtypes._t686,
    AbstractModelSubtypes._t687,
    AbstractModelSubtypes._t688,
    AbstractModelSubtypes._t689,
    AbstractModelSubtypes._t690,
    AbstractModelSubtypes._t691,
    AbstractModelSubtypes._t692,
    AbstractModelSubtypes._t693,
    AbstractModelSubtypes._t694,
    AbstractModelSubtypes._t695,
    AbstractModelSubtypes._t696,
    AbstractModelSubtypes._t697,
    AbstractModelSubtypes._t698,
    AbstractModelSubtypes._t699,
    AbstractModelSubtypes._t700,
    AbstractModelSubtypes._t701,
    AbstractModelSubtypes._t702,
    AbstractModelSubtypes._t703,
    AbstractModelSubtypes._t704,
    AbstractModelSubtypes._t705,
    AbstractModelSubtypes._t706,
    AbstractModelSubtypes._t707,
    AbstractModelSubtypes._t708,
    AbstractModelSubtypes._t709,
    AbstractModelSubtypes._t710,
    AbstractModelSubtypes._t711,
    AbstractModelSubtypes._t712,
    AbstractModelSubtypes._t713,
    AbstractModelSubtypes._t714,
    AbstractModelSubtypes._t715,
    AbstractModelSubtypes._t716,
    AbstractModelSubtypes._t717,
    AbstractModelSubtypes._t718,
    AbstractModelSubtypes._t719,
    AbstractModelSubtypes._t720,
    AbstractModelSubtypes._t721,
    AbstractModelSubtypes._t722,
    AbstractModelSubtypes._t723,
    AbstractModelSubtypes._t724,
    AbstractModelSubtypes._t725,
    AbstractModelSubtypes._t726,
    AbstractModelSubtypes._t727,
    AbstractModelSubtypes._t728,
    AbstractModelSubtypes._t729,
    AbstractModelSubtypes._t730,
    AbstractModelSubtypes._t731,
    AbstractModelSubtypes._t732,
    AbstractModelSubtypes._t733,
    AbstractModelSubtypes._t734,
    AbstractModelSubtypes._t735,
    AbstractModelSubtypes._t736,
    AbstractModelSubtypes._t737,
    AbstractModelSubtypes._t738,
    AbstractModelSubtypes._t739,
    AbstractModelSubtypes._t740,
    AbstractModelSubtypes._t741,
    AbstractModelSubtypes._t742,
    AbstractModelSubtypes._t743,
    AbstractModelSubtypes._t744,
    AbstractModelSubtypes._t745,
    AbstractModelSubtypes._t746,
    AbstractModelSubtypes._t747,
    AbstractModelSubtypes._t748,
    AbstractModelSubtypes._t749,
    AbstractModelSubtypes._t750,
    AbstractModelSubtypes._t751,
    AbstractModelSubtypes._t752,
    AbstractModelSubtypes._t753,
    AbstractModelSubtypes._t754,
    AbstractModelSubtypes._t755,
    AbstractModelSubtypes._t756,
    AbstractModelSubtypes._t757,
    AbstractModelSubtypes._t758,
    AbstractModelSubtypes._t759,
    AbstractModelSubtypes._t760,
    AbstractModelSubtypes._t761,
    AbstractModelSubtypes._t762,
    AbstractModelSubtypes._t763,
    AbstractModelSubtypes._t764,
    AbstractModelSubtypes._t765,
    AbstractModelSubtypes._t766,
    AbstractModelSubtypes._t767,
    AbstractModelSubtypes._t768,
    AbstractModelSubtypes._t769,
    AbstractModelSubtypes._t770,
    AbstractModelSubtypes._t771,
    AbstractModelSubtypes._t772,
    AbstractModelSubtypes._t773,
    AbstractModelSubtypes._t774,
    AbstractModelSubtypes._t775,
    AbstractModelSubtypes._t776,
    AbstractModelSubtypes._t777,
    AbstractModelSubtypes._t778,
    AbstractModelSubtypes._t779,
    AbstractModelSubtypes._t780,
    AbstractModelSubtypes._t781,
    AbstractModelSubtypes._t782,
    AbstractModelSubtypes._t783,
    AbstractModelSubtypes._t784,
    AbstractModelSubtypes._t785,
    AbstractModelSubtypes._t786,
    AbstractModelSubtypes._t787,
    AbstractModelSubtypes._t788,
    AbstractModelSubtypes._t789,
    AbstractModelSubtypes._t790,
    AbstractModelSubtypes._t791,
    AbstractModelSubtypes._t792,
    AbstractModelSubtypes._t793,
    AbstractModelSubtypes._t794,
    AbstractModelSubtypes._t795,
    AbstractModelSubtypes._t796,
    AbstractModelSubtypes._t797,
    AbstractModelSubtypes._t798,
    AbstractModelSubtypes._t799,
    AbstractModelSubtypes._t800,
    AbstractModelSubtypes._t801,
    AbstractModelSubtypes._t802,
    AbstractModelSubtypes._t803,
    AbstractModelSubtypes._t804,
    AbstractModelSubtypes._t805,
    AbstractModelSubtypes._t806,
    AbstractModelSubtypes._t807,
    AbstractModelSubtypes._t808,
    AbstractModelSubtypes._t809,
    AbstractModelSubtypes._t810,
    AbstractModelSubtypes._t811,
    AbstractModelSubtypes._t812,
    AbstractModelSubtypes._t813,
    AbstractModelSubtypes._t814,
    AbstractModelSubtypes._t815,
    AbstractModelSubtypes._t816,
    AbstractModelSubtypes._t817,
    AbstractModelSubtypes._t818,
    AbstractModelSubtypes._t819,
    AbstractModelSubtypes._t820,
    AbstractModelSubtypes._t821,
    AbstractModelSubtypes._t822,
    AbstractModelSubtypes._t823,
    AbstractModelSubtypes._t824,
    AbstractModelSubtypes._t825,
    AbstractModelSubtypes._t826,
    AbstractModelSubtypes._t827,
    AbstractModelSubtypes._t828,
    AbstractModelSubtypes._t829,
    AbstractModelSubtypes._t830,
    AbstractModelSubtypes._t831,
    AbstractModelSubtypes._t832,
    AbstractModelSubtypes._t833,
    AbstractModelSubtypes._t834,
    AbstractModelSubtypes._t835,
    AbstractModelSubtypes._t836,
    AbstractModelSubtypes._t837,
    AbstractModelSubtypes._t838,
    AbstractModelSubtypes._t839,
    AbstractModelSubtypes._t840,
    AbstractModelSubtypes._t841,
    AbstractModelSubtypes._t842,
    AbstractModelSubtypes._t843,
    AbstractModelSubtypes._t844,
    AbstractModelSubtypes._t845,
    AbstractModelSubtypes._t846,
    AbstractModelSubtypes._t847,
    AbstractModelSubtypes._t848,
    AbstractModelSubtypes._t849,
    AbstractModelSubtypes._t850,
    AbstractModelSubtypes._t851,
    AbstractModelSubtypes._t852,
    AbstractModelSubtypes._t853,
    AbstractModelSubtypes._t854,
    AbstractModelSubtypes._t855,
    AbstractModelSubtypes._t856,
    AbstractModelSubtypes._t857,
    AbstractModelSubtypes._t858,
    AbstractModelSubtypes._t859,
    AbstractModelSubtypes._t860,
    AbstractModelSubtypes._t861,
    AbstractModelSubtypes._t862,
    AbstractModelSubtypes._t863,
    AbstractModelSubtypes._t864,
    AbstractModelSubtypes._t865,
    AbstractModelSubtypes._t866,
    AbstractModelSubtypes._t867,
    AbstractModelSubtypes._t868,
    AbstractModelSubtypes._t869,
    AbstractModelSubtypes._t870,
    AbstractModelSubtypes._t871,
    AbstractModelSubtypes._t872,
    AbstractModelSubtypes._t873,
    AbstractModelSubtypes._t874,
    AbstractModelSubtypes._t875,
    AbstractModelSubtypes._t876,
    AbstractModelSubtypes._t877,
    AbstractModelSubtypes._t878,
    AbstractModelSubtypes._t879,
    AbstractModelSubtypes._t880,
    AbstractModelSubtypes._t881,
    AbstractModelSubtypes._t882,
    AbstractModelSubtypes._t883,
    AbstractModelSubtypes._t884,
    AbstractModelSubtypes._t885,
    AbstractModelSubtypes._t886,
    AbstractModelSubtypes._t887,
    AbstractModelSubtypes._t888,
    AbstractModelSubtypes._t889,
    AbstractModelSubtypes._t890,
    AbstractModelSubtypes._t891,
    AbstractModelSubtypes._t892,
    AbstractModelSubtypes._t893,
    AbstractModelSubtypes._t894,
    AbstractModelSubtypes._t895,
    AbstractModelSubtypes._t896,
    AbstractModelSubtypes._t897,
    AbstractModelSubtypes._t898,
    AbstractModelSubtypes._t899,
    AbstractModelSubtypes._t900,
    AbstractModelSubtypes._t901,
    AbstractModelSubtypes._t902,
    AbstractModelSubtypes._t903,
    AbstractModelSubtypes._t904,
    AbstractModelSubtypes._t905,
    AbstractModelSubtypes._t906,
    AbstractModelSubtypes._t907,
    AbstractModelSubtypes._t908,
    AbstractModelSubtypes._t909,
    AbstractModelSubtypes._t910,
    AbstractModelSubtypes._t911,
    AbstractModelSubtypes._t912,
    AbstractModelSubtypes._t913,
    AbstractModelSubtypes._t914,
    AbstractModelSubtypes._t915,
    AbstractModelSubtypes._t916,
    AbstractModelSubtypes._t917,
    AbstractModelSubtypes._t918,
    AbstractModelSubtypes._t919,
    AbstractModelSubtypes._t920,
    AbstractModelSubtypes._t921,
    AbstractModelSubtypes._t922,
    AbstractModelSubtypes._t923,
    AbstractModelSubtypes._t924,
    AbstractModelSubtypes._t925,
    AbstractModelSubtypes._t926,
    AbstractModelSubtypes._t927,
    AbstractModelSubtypes._t928,
    AbstractModelSubtypes._t929,
    AbstractModelSubtypes._t930,
    AbstractModelSubtypes._t931,
    AbstractModelSubtypes._t932,
    AbstractModelSubtypes._t933,
    AbstractModelSubtypes._t934,
    AbstractModelSubtypes._t935,
    AbstractModelSubtypes._t936,
    AbstractModelSubtypes._t937,
    AbstractModelSubtypes._t938,
    AbstractModelSubtypes._t939,
    AbstractModelSubtypes._t940,
    AbstractModelSubtypes._t941,
    AbstractModelSubtypes._t942,
    AbstractModelSubtypes._t943,
    AbstractModelSubtypes._t944,
    AbstractModelSubtypes._t945,
    AbstractModelSubtypes._t946,
    AbstractModelSubtypes._t947,
    AbstractModelSubtypes._t948,
    AbstractModelSubtypes._t949,
    AbstractModelSubtypes._t950,
    AbstractModelSubtypes._t951,
    AbstractModelSubtypes._t952,
    AbstractModelSubtypes._t953,
    AbstractModelSubtypes._t954,
    AbstractModelSubtypes._t955,
    AbstractModelSubtypes._t956,
    AbstractModelSubtypes._t957,
    AbstractModelSubtypes._t958,
    AbstractModelSubtypes._t959,
    AbstractModelSubtypes._t960,
    AbstractModelSubtypes._t961,
    AbstractModelSubtypes._t962,
    AbstractModelSubtypes._t963,
    AbstractModelSubtypes._t964,
    AbstractModelSubtypes._t965,
    AbstractModelSubtypes._t966,
    AbstractModelSubtypes._t967,
    AbstractModelSubtypes._t968,
    AbstractModelSubtypes._t969,
    AbstractModelSubtypes._t970,
    AbstractModelSubtypes._t971,
    AbstractModelSubtypes._t972,
    AbstractModelSubtypes._t973,
    AbstractModelSubtypes._t974,
    AbstractModelSubtypes._t975,
    AbstractModelSubtypes._t976,
    AbstractModelSubtypes._t977,
    AbstractModelSubtypes._t978,
    AbstractModelSubtypes._t979,
    AbstractModelSubtypes._t980,
    AbstractModelSubtypes._t981,
    AbstractModelSubtypes._t982,
    AbstractModelSubtypes._t983,
    AbstractModelSubtypes._t984,
    AbstractModelSubtypes._t985,
    AbstractModelSubtypes._t986,
    AbstractModelSubtypes._t987,
    AbstractModelSubtypes._t988,
    AbstractModelSubtypes._t989,
    AbstractModelSubtypes._t990,
    AbstractModelSubtypes._t991,
    AbstractModelSubtypes._t992,
    AbstractModelSubtypes._t993,
    AbstractModelSubtypes._t994,
    AbstractModelSubtypes._t995,
    AbstractModelSubtypes._t996,
    AbstractModelSubtypes._t997,
    AbstractModelSubtypes._t998,
    AbstractModelSubtypes._t999,
    AbstractModelSubtypes._t1000,
    AbstractModelSubtypes._t1001,
    AbstractModelSubtypes._t1002,
    AbstractModelSubtypes._t1003,
    AbstractModelSubtypes._t1004,
    AbstractModelSubtypes._t1005,
    AbstractModelSubtypes._t1006,
    AbstractModelSubtypes._t1007,
    AbstractModelSubtypes._t1008,
    AbstractModelSubtypes._t1009,
    AbstractModelSubtypes._t1010,
    AbstractModelSubtypes._t1011,
    AbstractModelSubtypes._t1012,
    AbstractModelSubtypes._t1013,
    AbstractModelSubtypes._t1014,
    AbstractModelSubtypes._t1015,
    AbstractModelSubtypes._t1016,
    AbstractModelSubtypes._t1017,
    AbstractModelSubtypes._t1018,
    AbstractModelSubtypes._t1019,
    AbstractModelSubtypes._t1020,
    AbstractModelSubtypes._t1021,
    AbstractModelSubtypes._t1022,
    AbstractModelSubtypes._t1023,
    AbstractModelSubtypes._t1024,
    AbstractModelSubtypes._t1025,
    AbstractModelSubtypes._t1026,
    AbstractModelSubtypes._t1027,
    AbstractModelSubtypes._t1028,
    AbstractModelSubtypes._t1029,
    AbstractModelSubtypes._t1030,
    AbstractModelSubtypes._t1031,
    AbstractModelSubtypes._t1032,
    AbstractModelSubtypes._t1033,
    AbstractModelSubtypes._t1034,
    AbstractModelSubtypes._t1035,
    AbstractModelSubtypes._t1036,
    AbstractModelSubtypes._t1037,
    AbstractModelSubtypes._t1038,
    AbstractModelSubtypes._t1039,
    AbstractModelSubtypes._t1040,
    AbstractModelSubtypes._t1041,
    AbstractModelSubtypes._t1042,
    AbstractModelSubtypes._t1043,
    AbstractModelSubtypes._t1044,
    AbstractModelSubtypes._t1045,
    AbstractModelSubtypes._t1046,
    AbstractModelSubtypes._t1047,
    AbstractModelSubtypes._t1048,
    AbstractModelSubtypes._t1049,
    AbstractModelSubtypes._t1050,
    AbstractModelSubtypes._t1051,
    AbstractModelSubtypes._t1052,
    AbstractModelSubtypes._t1053,
    AbstractModelSubtypes._t1054,
    AbstractModelSubtypes._t1055,
    AbstractModelSubtypes._t1056,
    AbstractModelSubtypes._t1057,
    AbstractModelSubtypes._t1058,
    AbstractModelSubtypes._t1059,
    AbstractModelSubtypes._t1060,
    AbstractModelSubtypes._t1061,
    AbstractModelSubtypes._t1062,
    AbstractModelSubtypes._t1063,
    AbstractModelSubtypes._t1064,
    AbstractModelSubtypes._t1065,
    AbstractModelSubtypes._t1066,
    AbstractModelSubtypes._t1067,
    AbstractModelSubtypes._t1068,
    AbstractModelSubtypes._t1069,
    AbstractModelSubtypes._t1070,
    AbstractModelSubtypes._t1071,
    AbstractModelSubtypes._t1072,
    AbstractModelSubtypes._t1073,
    AbstractModelSubtypes._t1074,
    AbstractModelSubtypes._t1075,
    AbstractModelSubtypes._t1076,
    AbstractModelSubtypes._t1077,
    AbstractModelSubtypes._t1078,
    AbstractModelSubtypes._t1079,
    AbstractModelSubtypes._t1080,
    AbstractModelSubtypes._t1081,
    AbstractModelSubtypes._t1082,
    AbstractModelSubtypes._t1083,
    AbstractModelSubtypes._t1084,
    AbstractModelSubtypes._t1085,
    AbstractModelSubtypes._t1086,
    AbstractModelSubtypes._t1087,
    AbstractModelSubtypes._t1088,
    AbstractModelSubtypes._t1089,
    AbstractModelSubtypes._t1090,
    AbstractModelSubtypes._t1091,
    AbstractModelSubtypes._t1092,
    AbstractModelSubtypes._t1093,
    AbstractModelSubtypes._t1094,
    AbstractModelSubtypes._t1095,
    AbstractModelSubtypes._t1096,
    AbstractModelSubtypes._t1097,
    AbstractModelSubtypes._t1098,
    AbstractModelSubtypes._t1099,
    AbstractModelSubtypes._t1100,
    AbstractModelSubtypes._t1101,
    AbstractModelSubtypes._t1102,
    AbstractModelSubtypes._t1103,
    AbstractModelSubtypes._t1104,
    AbstractModelSubtypes._t1105,
    AbstractModelSubtypes._t1106,
    AbstractModelSubtypes._t1107,
    AbstractModelSubtypes._t1108,
    AbstractModelSubtypes._t1109,
    AbstractModelSubtypes._t1110,
    AbstractModelSubtypes._t1111,
    AbstractModelSubtypes._t1112,
    AbstractModelSubtypes._t1113,
    AbstractModelSubtypes._t1114,
    AbstractModelSubtypes._t1115,
    AbstractModelSubtypes._t1116,
    AbstractModelSubtypes._t1117,
    AbstractModelSubtypes._t1118,
    AbstractModelSubtypes._t1119,
    AbstractModelSubtypes._t1120,
    AbstractModelSubtypes._t1121,
    AbstractModelSubtypes._t1122,
    AbstractModelSubtypes._t1123,
    AbstractModelSubtypes._t1124,
    AbstractModelSubtypes._t1125,
    AbstractModelSubtypes._t1126,
    AbstractModelSubtypes._t1127,
    AbstractModelSubtypes._t1128,
    AbstractModelSubtypes._t1129,
    AbstractModelSubtypes._t1130,
    AbstractModelSubtypes._t1131,
    AbstractModelSubtypes._t1132,
    AbstractModelSubtypes._t1133,
    AbstractModelSubtypes._t1134,
    AbstractModelSubtypes._t1135,
    AbstractModelSubtypes._t1136,
    AbstractModelSubtypes._t1137,
    AbstractModelSubtypes._t1138,
    AbstractModelSubtypes._t1139,
    AbstractModelSubtypes._t1140,
    AbstractModelSubtypes._t1141,
    AbstractModelSubtypes._t1142,
    AbstractModelSubtypes._t1143,
    AbstractModelSubtypes._t1144,
    AbstractModelSubtypes._t1145,
    AbstractModelSubtypes._t1146,
    AbstractModelSubtypes._t1147,
    AbstractModelSubtypes._t1148,
    AbstractModelSubtypes._t1149,
    AbstractModelSubtypes._t1150,
    AbstractModelSubtypes._t1151,
    AbstractModelSubtypes._t1152,
    AbstractModelSubtypes._t1153,
    AbstractModelSubtypes._t1154,
    AbstractModelSubtypes._t1155,
    AbstractModelSubtypes._t1156,
    AbstractModelSubtypes._t1157,
    AbstractModelSubtypes._t1158,
    AbstractModelSubtypes._t1159,
    AbstractModelSubtypes._t1160,
    AbstractModelSubtypes._t1161,
    AbstractModelSubtypes._t1162,
    AbstractModelSubtypes._t1163,
    AbstractModelSubtypes._t1164,
    AbstractModelSubtypes._t1165,
    AbstractModelSubtypes._t1166,
    AbstractModelSubtypes._t1167,
    AbstractModelSubtypes._t1168,
    AbstractModelSubtypes._t1169,
    AbstractModelSubtypes._t1170,
    AbstractModelSubtypes._t1171,
    AbstractModelSubtypes._t1172,
    AbstractModelSubtypes._t1173,
    AbstractModelSubtypes._t1174,
    AbstractModelSubtypes._t1175,
    AbstractModelSubtypes._t1176,
    AbstractModelSubtypes._t1177,
    AbstractModelSubtypes._t1178,
    AbstractModelSubtypes._t1179,
    AbstractModelSubtypes._t1180,
    AbstractModelSubtypes._t1181,
    AbstractModelSubtypes._t1182,
    AbstractModelSubtypes._t1183,
    AbstractModelSubtypes._t1184,
    AbstractModelSubtypes._t1185,
    AbstractModelSubtypes._t1186,
    AbstractModelSubtypes._t1187,
    AbstractModelSubtypes._t1188,
    AbstractModelSubtypes._t1189,
    AbstractModelSubtypes._t1190,
    AbstractModelSubtypes._t1191,
    AbstractModelSubtypes._t1192,
    AbstractModelSubtypes._t1193,
    AbstractModelSubtypes._t1194,
    AbstractModelSubtypes._t1195,
    AbstractModelSubtypes._t1196,
    AbstractModelSubtypes._t1197,
    AbstractModelSubtypes._t1198,
    AbstractModelSubtypes._t1199,
    AbstractModelSubtypes._t1200,
    AbstractModelSubtypes._t1201,
    AbstractModelSubtypes._t1202,
    AbstractModelSubtypes._t1203,
    AbstractModelSubtypes._t1204,
    AbstractModelSubtypes._t1205,
    AbstractModelSubtypes._t1206,
    AbstractModelSubtypes._t1207,
    AbstractModelSubtypes._t1208,
    AbstractModelSubtypes._t1209,
    AbstractModelSubtypes._t1210,
    AbstractModelSubtypes._t1211,
    AbstractModelSubtypes._t1212,
    AbstractModelSubtypes._t1213,
    AbstractModelSubtypes._t1214,
    AbstractModelSubtypes._t1215,
    AbstractModelSubtypes._t1216,
    AbstractModelSubtypes._t1217,
    AbstractModelSubtypes._t1218,
    AbstractModelSubtypes._t1219,
    AbstractModelSubtypes._t1220,
    AbstractModelSubtypes._t1221,
    AbstractModelSubtypes._t1222,
    AbstractModelSubtypes._t1223,
    AbstractModelSubtypes._t1224,
    AbstractModelSubtypes._t1225,
    AbstractModelSubtypes._t1226,
    AbstractModelSubtypes._t1227,
    AbstractModelSubtypes._t1228,
    AbstractModelSubtypes._t1229,
    AbstractModelSubtypes._t1230,
    AbstractModelSubtypes._t1231,
    AbstractModelSubtypes._t1232,
    AbstractModelSubtypes._t1233,
    AbstractModelSubtypes._t1234,
    AbstractModelSubtypes._t1235,
    AbstractModelSubtypes._t1236,
    AbstractModelSubtypes._t1237,
    AbstractModelSubtypes._t1238,
    AbstractModelSubtypes._t1239,
    AbstractModelSubtypes._t1240,
    AbstractModelSubtypes._t1241,
    AbstractModelSubtypes._t1242,
    AbstractModelSubtypes._t1243,
    AbstractModelSubtypes._t1244,
    AbstractModelSubtypes._t1245,
    AbstractModelSubtypes._t1246,
    AbstractModelSubtypes._t1247,
    AbstractModelSubtypes._t1248,
    AbstractModelSubtypes._t1249,
    AbstractModelSubtypes._t1250,
    AbstractModelSubtypes._t1251,
    AbstractModelSubtypes._t1252,
    AbstractModelSubtypes._t1253,
    AbstractModelSubtypes._t1254,
    AbstractModelSubtypes._t1255,
    AbstractModelSubtypes._t1256,
    AbstractModelSubtypes._t1257,
    AbstractModelSubtypes._t1258,
    AbstractModelSubtypes._t1259,
    AbstractModelSubtypes._t1260,
    AbstractModelSubtypes._t1261,
    AbstractModelSubtypes._t1262,
    AbstractModelSubtypes._t1263,
    AbstractModelSubtypes._t1264,
    AbstractModelSubtypes._t1265,
    AbstractModelSubtypes._t1266,
    AbstractModelSubtypes._t1267,
    AbstractModelSubtypes._t1268,
    AbstractModelSubtypes._t1269,
    AbstractModelSubtypes._t1270,
    AbstractModelSubtypes._t1271,
    AbstractModelSubtypes._t1272,
    AbstractModelSubtypes._t1273,
    AbstractModelSubtypes._t1274,
    AbstractModelSubtypes._t1275,
    AbstractModelSubtypes._t1276,
    AbstractModelSubtypes._t1277,
    AbstractModelSubtypes._t1278,
    AbstractModelSubtypes._t1279,
    AbstractModelSubtypes._t1280,
    AbstractModelSubtypes._t1281,
    AbstractModelSubtypes._t1282,
    AbstractModelSubtypes._t1283,
    AbstractModelSubtypes._t1284,
    AbstractModelSubtypes._t1285,
    AbstractModelSubtypes._t1286,
    AbstractModelSubtypes._t1287,
    AbstractModelSubtypes._t1288,
    AbstractModelSubtypes._t1289,
    AbstractModelSubtypes._t1290,
    AbstractModelSubtypes._t1291,
    AbstractModelSubtypes._t1292,
    AbstractModelSubtypes._t1293,
    AbstractModelSubtypes._t1294,
    AbstractModelSubtypes._t1295,
    AbstractModelSubtypes._t1296,
    AbstractModelSubtypes._t1297,
    AbstractModelSubtypes._t1298,
    AbstractModelSubtypes._t1299,
    AbstractModelSubtypes._t1300,
    AbstractModelSubtypes._t1301,
    AbstractModelSubtypes._t1302,
    AbstractModelSubtypes._t1303,
    AbstractModelSubtypes._t1304,
    AbstractModelSubtypes._t1305,
    AbstractModelSubtypes._t1306,
    AbstractModelSubtypes._t1307,
    AbstractModelSubtypes._t1308,
    AbstractModelSubtypes._t1309,
    AbstractModelSubtypes._t1310,
    AbstractModelSubtypes._t1311,
    AbstractModelSubtypes._t1312,
    AbstractModelSubtypes._t1313,
    AbstractModelSubtypes._t1314,
    AbstractModelSubtypes._t1315,
    AbstractModelSubtypes._t1316,
    AbstractModelSubtypes._t1317,
    AbstractModelSubtypes._t1318,
    AbstractModelSubtypes._t1319,
    AbstractModelSubtypes._t1320,
    AbstractModelSubtypes._t1321,
    AbstractModelSubtypes._t1322,
    AbstractModelSubtypes._t1323,
    AbstractModelSubtypes._t1324,
    AbstractModelSubtypes._t1325,
    AbstractModelSubtypes._t1326,
    AbstractModelSubtypes._t1327,
    AbstractModelSubtypes._t1328,
    AbstractModelSubtypes._t1329,
    AbstractModelSubtypes._t1330,
    AbstractModelSubtypes._t1331,
    AbstractModelSubtypes._t1332,
    AbstractModelSubtypes._t1333,
    AbstractModelSubtypes._t1334,
    AbstractModelSubtypes._t1335,
    AbstractModelSubtypes._t1336,
    AbstractModelSubtypes._t1337,
    AbstractModelSubtypes._t1338,
    AbstractModelSubtypes._t1339,
    AbstractModelSubtypes._t1340,
    AbstractModelSubtypes._t1341,
    AbstractModelSubtypes._t1342,
    AbstractModelSubtypes._t1343,
    AbstractModelSubtypes._t1344,
    AbstractModelSubtypes._t1345,
    AbstractModelSubtypes._t1346,
    AbstractModelSubtypes._t1347,
    AbstractModelSubtypes._t1348,
    AbstractModelSubtypes._t1349,
    AbstractModelSubtypes._t1350,
    AbstractModelSubtypes._t1351,
    AbstractModelSubtypes._t1352,
    AbstractModelSubtypes._t1353,
    AbstractModelSubtypes._t1354,
    AbstractModelSubtypes._t1355,
    AbstractModelSubtypes._t1356,
    AbstractModelSubtypes._t1357,
    AbstractModelSubtypes._t1358,
    AbstractModelSubtypes._t1359,
    AbstractModelSubtypes._t1360,
    AbstractModelSubtypes._t1361,
    AbstractModelSubtypes._t1362,
    AbstractModelSubtypes._t1363,
    AbstractModelSubtypes._t1364,
    AbstractModelSubtypes._t1365,
    AbstractModelSubtypes._t1366,
    AbstractModelSubtypes._t1367,
    AbstractModelSubtypes._t1368,
    AbstractModelSubtypes._t1369,
    AbstractModelSubtypes._t1370,
    AbstractModelSubtypes._t1371,
    AbstractModelSubtypes._t1372,
    AbstractModelSubtypes._t1373,
    AbstractModelSubtypes._t1374,
    AbstractModelSubtypes._t1375,
    AbstractModelSubtypes._t1376,
    AbstractModelSubtypes._t1377,
    AbstractModelSubtypes._t1378,
    AbstractModelSubtypes._t1379,
    AbstractModelSubtypes._t1380,
    AbstractModelSubtypes._t1381,
    AbstractModelSubtypes._t1382,
    AbstractModelSubtypes._t1383,
    AbstractModelSubtypes._t1384,
    AbstractModelSubtypes._t1385,
    AbstractModelSubtypes._t1386,
    AbstractModelSubtypes._t1387,
    AbstractModelSubtypes._t1388,
    AbstractModelSubtypes._t1389,
    AbstractModelSubtypes._t1390,
    AbstractModelSubtypes._t1391,
    AbstractModelSubtypes._t1392,
    AbstractModelSubtypes._t1393,
    AbstractModelSubtypes._t1394,
    AbstractModelSubtypes._t1395,
    AbstractModelSubtypes._t1396,
    AbstractModelSubtypes._t1397,
    AbstractModelSubtypes._t1398,
    AbstractModelSubtypes._t1399,
    AbstractModelSubtypes._t1400,
    AbstractModelSubtypes._t1401,
    AbstractModelSubtypes._t1402,
    AbstractModelSubtypes._t1403,
    AbstractModelSubtypes._t1404,
    AbstractModelSubtypes._t1405,
    AbstractModelSubtypes._t1406,
    AbstractModelSubtypes._t1407,
    AbstractModelSubtypes._t1408,
    AbstractModelSubtypes._t1409,
    AbstractModelSubtypes._t1410,
    AbstractModelSubtypes._t1411,
    AbstractModelSubtypes._t1412,
    AbstractModelSubtypes._t1413,
    AbstractModelSubtypes._t1414,
    AbstractModelSubtypes._t1415,
    AbstractModelSubtypes._t1416,
    AbstractModelSubtypes._t1417,
    AbstractModelSubtypes._t1418,
    AbstractModelSubtypes._t1419,
    AbstractModelSubtypes._t1420,
    AbstractModelSubtypes._t1421,
    AbstractModelSubtypes._t1422,
    AbstractModelSubtypes._t1423,
    AbstractModelSubtypes._t1424,
    AbstractModelSubtypes._t1425,
    AbstractModelSubtypes._t1426,
    AbstractModelSubtypes._t1427,
    AbstractModelSubtypes._t1428,
    AbstractModelSubtypes._t1429,
    AbstractModelSubtypes._t1430,
    AbstractModelSubtypes._t1431,
    AbstractModelSubtypes._t1432,
    AbstractModelSubtypes._t1433,
    AbstractModelSubtypes._t1434,
    AbstractModelSubtypes._t1435,
    AbstractModelSubtypes._t1436,
    AbstractModelSubtypes._t1437,
    AbstractModelSubtypes._t1438,
    AbstractModelSubtypes._t1439,
    AbstractModelSubtypes._t1440,
    AbstractModelSubtypes._t1441,
    AbstractModelSubtypes._t1442,
    AbstractModelSubtypes._t1443,
    AbstractModelSubtypes._t1444,
    AbstractModelSubtypes._t1445,
    AbstractModelSubtypes._t1446,
    AbstractModelSubtypes._t1447,
    AbstractModelSubtypes._t1448,
    AbstractModelSubtypes._t1449,
    AbstractModelSubtypes._t1450,
    AbstractModelSubtypes._t1451,
    AbstractModelSubtypes._t1452,
    AbstractModelSubtypes._t1453,
    AbstractModelSubtypes._t1454,
    AbstractModelSubtypes._t1455,
    AbstractModelSubtypes._t1456,
    AbstractModelSubtypes._t1457,
    AbstractModelSubtypes._t1458,
    AbstractModelSubtypes._t1459,
    AbstractModelSubtypes._t1460,
    AbstractModelSubtypes._t1461,
    AbstractModelSubtypes._t1462,
    AbstractModelSubtypes._t1463,
    AbstractModelSubtypes._t1464,
    AbstractModelSubtypes._t1465,
    AbstractModelSubtypes._t1466,
    AbstractModelSubtypes._t1467,
    AbstractModelSubtypes._t1468,
    AbstractModelSubtypes._t1469,
    AbstractModelSubtypes._t1470,
    AbstractModelSubtypes._t1471,
    AbstractModelSubtypes._t1472,
    AbstractModelSubtypes._t1473,
    AbstractModelSubtypes._t1474,
    AbstractModelSubtypes._t1475,
    AbstractModelSubtypes._t1476,
    AbstractModelSubtypes._t1477,
    AbstractModelSubtypes._t1478,
    AbstractModelSubtypes._t1479,
    AbstractModelSubtypes._t1480,
    AbstractModelSubtypes._t1481,
    AbstractModelSubtypes._t1482,
    AbstractModelSubtypes._t1483,
    AbstractModelSubtypes._t1484,
    AbstractModelSubtypes._t1485,
    AbstractModelSubtypes._t1486,
    AbstractModelSubtypes._t1487,
    AbstractModelSubtypes._t1488,
    AbstractModelSubtypes._t1489,
    AbstractModelSubtypes._t1490,
    AbstractModelSubtypes._t1491,
    AbstractModelSubtypes._t1492,
    AbstractModelSubtypes._t1493,
    AbstractModelSubtypes._t1494,
    AbstractModelSubtypes._t1495,
    AbstractModelSubtypes._t1496,
    AbstractModelSubtypes._t1497,
    AbstractModelSubtypes._t1498,
    AbstractModelSubtypes._t1499,
    AbstractModelSubtypes._t1500,
    AbstractModelSubtypes._t1501,
    AbstractModelSubtypes._t1502,
    AbstractModelSubtypes._t1503,
    AbstractModelSubtypes._t1504,
    AbstractModelSubtypes._t1505,
    AbstractModelSubtypes._t1506,
    AbstractModelSubtypes._t1507,
    AbstractModelSubtypes._t1508,
    AbstractModelSubtypes._t1509,
    AbstractModelSubtypes._t1510,
    AbstractModelSubtypes._t1511,
    AbstractModelSubtypes._t1512,
    AbstractModelSubtypes._t1513,
    AbstractModelSubtypes._t1514,
    AbstractModelSubtypes._t1515,
    AbstractModelSubtypes._t1516,
    AbstractModelSubtypes._t1517,
    AbstractModelSubtypes._t1518,
    AbstractModelSubtypes._t1519,
    AbstractModelSubtypes._t1520,
    AbstractModelSubtypes._t1521,
    AbstractModelSubtypes._t1522,
    AbstractModelSubtypes._t1523,
    AbstractModelSubtypes._t1524,
    AbstractModelSubtypes._t1525,
    AbstractModelSubtypes._t1526,
    AbstractModelSubtypes._t1527,
    AbstractModelSubtypes._t1528,
    AbstractModelSubtypes._t1529,
    AbstractModelSubtypes._t1530,
    AbstractModelSubtypes._t1531,
    AbstractModelSubtypes._t1532,
    AbstractModelSubtypes._t1533,
    AbstractModelSubtypes._t1534,
    AbstractModelSubtypes._t1535,
    AbstractModelSubtypes._t1536,
    AbstractModelSubtypes._t1537,
    AbstractModelSubtypes._t1538,
    AbstractModelSubtypes._t1539,
    AbstractModelSubtypes._t1540,
    AbstractModelSubtypes._t1541,
    AbstractModelSubtypes._t1542,
    AbstractModelSubtypes._t1543,
    AbstractModelSubtypes._t1544,
    AbstractModelSubtypes._t1545,
    AbstractModelSubtypes._t1546,
    AbstractModelSubtypes._t1547,
    AbstractModelSubtypes._t1548,
    AbstractModelSubtypes._t1549,
    AbstractModelSubtypes._t1550,
    AbstractModelSubtypes._t1551,
    AbstractModelSubtypes._t1552,
    AbstractModelSubtypes._t1553,
    AbstractModelSubtypes._t1554,
    AbstractModelSubtypes._t1555,
    AbstractModelSubtypes._t1556,
    AbstractModelSubtypes._t1557,
    AbstractModelSubtypes._t1558,
    AbstractModelSubtypes._t1559,
    AbstractModelSubtypes._t1560,
    AbstractModelSubtypes._t1561,
    AbstractModelSubtypes._t1562,
    AbstractModelSubtypes._t1563,
    AbstractModelSubtypes._t1564,
    AbstractModelSubtypes._t1565,
    AbstractModelSubtypes._t1566,
    AbstractModelSubtypes._t1567,
    AbstractModelSubtypes._t1568,
    AbstractModelSubtypes._t1569,
    AbstractModelSubtypes._t1570,
    AbstractModelSubtypes._t1571,
    AbstractModelSubtypes._t1572,
    AbstractModelSubtypes._t1573,
    AbstractModelSubtypes._t1574,
    AbstractModelSubtypes._t1575,
    AbstractModelSubtypes._t1576,
    AbstractModelSubtypes._t1577,
    AbstractModelSubtypes._t1578,
    AbstractModelSubtypes._t1579,
    AbstractModelSubtypes._t1580,
    AbstractModelSubtypes._t1581,
    AbstractModelSubtypes._t1582,
    AbstractModelSubtypes._t1583,
    AbstractModelSubtypes._t1584,
    AbstractModelSubtypes._t1585,
    AbstractModelSubtypes._t1586,
    AbstractModelSubtypes._t1587,
    AbstractModelSubtypes._t1588,
    AbstractModelSubtypes._t1589,
    AbstractModelSubtypes._t1590,
    AbstractModelSubtypes._t1591,
    AbstractModelSubtypes._t1592,
    AbstractModelSubtypes._t1593,
    AbstractModelSubtypes._t1594,
    AbstractModelSubtypes._t1595,
    AbstractModelSubtypes._t1596,
    AbstractModelSubtypes._t1597,
    AbstractModelSubtypes._t1598,
    AbstractModelSubtypes._t1599,
    AbstractModelSubtypes._t1600,
    AbstractModelSubtypes._t1601,
    AbstractModelSubtypes._t1602,
    AbstractModelSubtypes._t1603,
    AbstractModelSubtypes._t1604,
    AbstractModelSubtypes._t1605,
    AbstractModelSubtypes._t1606,
    AbstractModelSubtypes._t1607,
    AbstractModelSubtypes._t1608,
    AbstractModelSubtypes._t1609,
    AbstractModelSubtypes._t1610,
    AbstractModelSubtypes._t1611,
    AbstractModelSubtypes._t1612,
    AbstractModelSubtypes._t1613,
    AbstractModelSubtypes._t1614,
    AbstractModelSubtypes._t1615,
    AbstractModelSubtypes._t1616,
    AbstractModelSubtypes._t1617,
    AbstractModelSubtypes._t1618,
    AbstractModelSubtypes._t1619,
    AbstractModelSubtypes._t1620,
    AbstractModelSubtypes._t1621,
    AbstractModelSubtypes._t1622,
    AbstractModelSubtypes._t1623,
    AbstractModelSubtypes._t1624,
    AbstractModelSubtypes._t1625,
    AbstractModelSubtypes._t1626,
    AbstractModelSubtypes._t1627,
    AbstractModelSubtypes._t1628,
    AbstractModelSubtypes._t1629,
    AbstractModelSubtypes._t1630,
    AbstractModelSubtypes._t1631,
    AbstractModelSubtypes._t1632,
    AbstractModelSubtypes._t1633,
    AbstractModelSubtypes._t1634,
    AbstractModelSubtypes._t1635,
    AbstractModelSubtypes._t1636,
    AbstractModelSubtypes._t1637,
    AbstractModelSubtypes._t1638,
    AbstractModelSubtypes._t1639,
    AbstractModelSubtypes._t1640,
    AbstractModelSubtypes._t1641,
    AbstractModelSubtypes._t1642,
    AbstractModelSubtypes._t1643,
    AbstractModelSubtypes._t1644,
    AbstractModelSubtypes._t1645,
    AbstractModelSubtypes._t1646,
    AbstractModelSubtypes._t1647,
    AbstractModelSubtypes._t1648,
    AbstractModelSubtypes._t1649,
    AbstractModelSubtypes._t1650,
    AbstractModelSubtypes._t1651,
    AbstractModelSubtypes._t1652,
    AbstractModelSubtypes._t1653,
    AbstractModelSubtypes._t1654,
    AbstractModelSubtypes._t1655
  };

  public static int Count => 1656;

  public static IReadOnlyList<Type> All => (IReadOnlyList<Type>) AbstractModelSubtypes._subtypes;

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  [return: DynamicallyAccessedMembers]
  public static Type Get(int i) => AbstractModelSubtypes._subtypes[i];
}
