using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Utils;
using Assets.Scripts.Models.Towers.Projectiles;
using System.Linq;
using static VanillaParagons.Main;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;

namespace VanillaParagons.PrimaryParagons.IceMonkeyParagon
{
    public class IceMonkeyParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "IceMonkey-013";
        public override string Name => "IceMonkey";
    }
    public class IceMonkeyParagon : ModParagonUpgrade<IceMonkeyParagonBase>
    {
        public override string DisplayName => "Indefinate Negative";
        public override int Cost => 400000;
        public override string Description => "ice colder than nothing itself, breaking the fabric of reality and piercing through any bloon.";
        public override void ApplyUpgrade(TowerModel tower)
        {
            var attackModel = tower.GetAttackModel();
            var weapon = tower.GetWeapon();
            var projectile = tower.GetDescendants<ProjectileModel>().ToIl2CppList()[0];
            var projectile1 = tower.GetDescendants<ProjectileModel>().ToIl2CppList()[1];
            projectile.RemoveBehavior<DamageModel>();
            projectile.AddBehavior(new DamageModel("IceMonkeyParagonDamageModel", 125f, 999999, true, true, true, BloonProperties.None, BloonProperties.None));
            projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("IceMonkeyParagonAddBonusDamagePerHitToBloonModel", "", 2, 15, 999999, true, false, false));
            projectile.pierce = 150;
            projectile.RemoveBehaviors<FreezeModel>();
            projectile.RemoveBehaviors<SlowModel>();
            projectile.RemoveBehavior<ProjectileFilterModel>();
            projectile.filters = null;
            projectile1.RemoveBehaviors<FreezeModel>();
            projectile1.RemoveBehaviors<SlowModel>();
            projectile1.RemoveBehavior<ProjectileFilterModel>();
            projectile1.filters = null;
            weapon.Rate *= 0.125f;

            tower.RemoveBehaviors<FilterBloonIfDamageTypeModel>();
            tower.AddBehavior(new SlowBloonsZoneModel("IceMonkeyParagonSlowBloonsZoneModel", 125, "", true, null, 0.4f, 0, false, 0, "", false, null));
            tower.range += 25;
            attackModel.range += 25;
            attackModel.RemoveBehavior<AttackFilterModel>();

            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            FileIOUtil.SaveObject("smh.json", tower);
            MelonLoader.MelonLogger.Msg(projectile.name);
            MelonLoader.MelonLogger.Msg(projectile1.name);
        }
    }
} 