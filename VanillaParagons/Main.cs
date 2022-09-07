using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Utils;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using VanillaParagons;

[assembly: MelonInfo(typeof(VanillaParagons.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace VanillaParagons
{

    class Main : BloonsTD6Mod
    {
        public static ModSettingBool OPParagons = new ModSettingBool(false) { displayName = "Over Powered Paragons enabled? (Requires restart, does not effect previously places paragons.)" };
        public static ModSettingBool EnableParagons = new ModSettingBool(true) { displayName = "Paragons enabled? (Requires restart.)" };
        public static ModSettingBool EnableBuffableParagons = new ModSettingBool(true) { displayName = "Buffable Versions of Paragons enabled? (Requires restart.)" };

        [HarmonyPatch(typeof(TowerManager), nameof(TowerManager.IsParagonLocked))]
        internal static class TowerManager_IsParagonLocked
        {
            [HarmonyPostfix]
            private static void Postfix(Tower tower, ref bool __result)
            {
                if (EnableParagons == false)
                {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(ParagonTower), nameof(ParagonTower.UpdateDegree))]
        class UpdateDegreePatch
        {
            [HarmonyPostfix]
            internal static void Postfix(ParagonTower __instance)
            {
                var tower = __instance.tower;
                var towerModel = tower.towerModel;
                var degree = tower.GetTowerBehavior<ParagonTower>().GetCurrentDegree();               
                if (towerModel.baseId == "BananaFarm")
                {
                    if (degree % 5 == 0)
                    {
                        towerModel.GetBehavior<EmissionsPerRoundFilterModel>().count += degree / 5;
                    }
                    towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound += 950 * degree;
                }
                if (towerModel.baseId == "GlueGunner")
                {
                    if (degree >= 1)
                    {
                        if (degree >= 50)
                        {
                            towerModel.GetBehavior<SlowModel>().multiplier = 0.75f;
                        }
                        if (degree >= 75)
                        {
                            towerModel.GetBehavior<SlowModel>().multiplier = 0.7f;
                        }
                        if (degree >= 100)
                        {
                            towerModel.GetBehavior<SlowModel>().multiplier = 0.65f;
                        }
                    }
                }
                if (towerModel.baseId == "MonkeyVillage")
                {
                    if (degree >= 1)
                    {
                        towerModel.GetBehavior<RangeSupportModel>().additive += (float)degree / 4;
                        towerModel.GetBehavior<RateSupportModel>().multiplier *= (float)1 / (degree / 20);
                        towerModel.GetBehavior<DamageSupportModel>().increase += (float)degree / 4;
                        towerModel.GetBehavior<PierceSupportModel>().pierce += (float)degree / 2;

                        if (degree >= 75)
                        {
                            towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().incomeModifier = 1.75f;
                        }
                        if (degree >= 100)
                        {
                            towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().incomeModifier = 2;
                        }
                    }
                }
            }
        }
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
        }
        public override void OnGameModelLoaded(GameModel model)
        {
            base.OnGameModelLoaded(model);
        }
    }
}