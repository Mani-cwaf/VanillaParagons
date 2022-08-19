using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Api.Towers;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using System.Linq;
using UnhollowerBaseLib;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;

namespace VanillaParagons.MilitaryParagons.MonkeyAceParagon
{
    public class MonkeyAceParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "MonkeyAce-502";
    }
    public class MonkeyAceParagon : ModParagonUpgrade<MonkeyAceParagonBase>
    {
        public override string DisplayName => "Rain of fire";
        public override int Cost => 967000;
        public override string Description => "why does this exist";
        //public override string Icon => "";
        //public override string Portrait => "";
        public override void ApplyUpgrade(TowerModel tower)
        {
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile;
            weapon.emission = new ArcEmissionModel("MonkeyAceParagonArcEmissionModel", 128, 180, 360, null, false);
            projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 110f, false, false));
            var seekingBehavior = new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 700, false, false);
            projectile.AddBehavior(seekingBehavior);
            projectile.GetDamageModel().damage += 80;
            projectile.pierce = 50;
            weapon.rate *= 0.5f;

            var missile = tower.GetBehaviors<AttackAirUnitModel>().First(a => a.name == "AttackAirUnitModel_Anti-MoabMissile_");

            missile.weapons[0].Rate *= 0.15f;
            missile.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 12500f, false, false));
            missile.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 1000.0f);
            missile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

            tower.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-220").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
            var attackModel2 = tower.GetBehaviors<AttackAirUnitModel>()[2];
            attackModel2.weapons[0].Rate *= 0.05f;
            attackModel2.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 2500.0f);
            attackModel2.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

            var groundzeroAbility = Game.instance.model.GetTower(TowerType.MonkeyAce, 0, 5).GetAbility().Duplicate();
            var goundzeroAbilityAttackModel = groundzeroAbility.GetBehavior<ActivateAttackModel>();

            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().maxDamage = 999999;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().CapDamage(999999);
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().damage = 200000;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.maxPierce = 999999;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.CapPierce(999999);
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.pierce = 250000;

            tower.AddBehavior(groundzeroAbility);

            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
    }
}