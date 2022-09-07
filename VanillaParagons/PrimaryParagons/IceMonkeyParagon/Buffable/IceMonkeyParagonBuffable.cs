using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.TowerFilters;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using SubTowers.LordPhenix;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using VanillaParagons.SupportParagons.BananaFarmParagon.Displays;
using static VanillaParagons.Main;

namespace VanillaParagons.PrimaryParagons.IceMonkeyParagon.Buffable
{
    public class IceMonkeyParagonBuffable : ModTower
    {
        public override string DisplayName => "Buffable Indefinate Negative";
        public override string TowerSet => TowerSetType.Primary;
        public override bool DontAddToShop => !EnableBuffableParagons;
        public override string BaseTower => "IceMonkey-025";
        public override int Cost => 1257500;
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 1;
        public override int BottomPathUpgrades => 0;
        public override string Description => "Buffable version of the Indefinate Negative";
        public override void ModifyBaseTowerModel(TowerModel tower)
        {
            var attackModel = tower.GetAttackModel();
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile;
            projectile.RemoveBehavior<DamageModel>();
            projectile.RemoveBehavior<SlowModel>();
            projectile.AddBehavior(new DamageModel("IceMonkeyParagonDamageModel", 180000, 999999, true, true, true, BloonProperties.None, BloonProperties.None));
            projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("IceMonkeyParagonAddBonusDamagePerHitToBloonModel", "", 2, 400, 999999, true, false, false));
            projectile.RemoveBehaviors<SlowModifierForTagModel>();
            projectile.pierce = 150;
            tower.AddBehavior(new SlowBloonsZoneModel("IceMonkeyParagonSlowBloonsZoneModel", 125, "", true, null, 0.4f, 0, false, 0, "", false, null));
            tower.RemoveBehaviors<FilterMoabModel>();
            tower.RemoveBehaviors<FilterOutTagModel>();
            tower.RemoveBehaviors<FilterBloonIfDamageTypeModel>();
            tower.range += 25;
            weapon.Rate *= 0.15f;
            attackModel.range += 25;

            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.IceMonkey).towerIndex + 1;
        }
        public class IceMonkeyParagonBuffableDegree100 : ModUpgrade<IceMonkeyParagonBuffable>
        {
            public override string DisplayName => "Degree 100";
            public override string Description => "Degree 100";
            public override int Cost => 0;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel tower)
            {
                var weapon = tower.GetWeapon();
                var projectile = weapon.projectile;
                weapon.Rate *= 0.642857f;
                projectile.GetDamageModel().damage *= 2.5f;
                projectile.pierce *= 2.5f;
                tower.GetBehavior<SlowBloonsZoneModel>().speedScale = 0.1f;
            }
        }
    }
}
