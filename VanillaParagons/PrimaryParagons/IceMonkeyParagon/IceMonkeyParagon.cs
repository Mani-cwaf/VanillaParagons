using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Unity;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;

namespace VanillaParagons.PrimaryParagons.IceMonkeyParagon
{
    public class IceMonkeyParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "IceMonkey-025";
    }
    public class IceMonkeyParagon : ModParagonUpgrade<IceMonkeyParagonBase>
    {
        public override string DisplayName => "Indefinate Negative";
        public override int Cost => 2000000;
        public override string Description => "ice colder than nothing itself, breaking the fabric of reality and piercing through any bloon.";
        public override void ApplyUpgrade(TowerModel tower)
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
            weapon.rate *= 0.125f;
            attackModel.range += 25;

            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
    }
} 