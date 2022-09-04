using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using SubTowers.LordPhenix;
using System.Collections.Generic;
using System.Linq;
using static VanillaParagons.Main;

namespace VanillaParagons.MilitaryParagons.MonkeyAceParagon.Buffable
{
    public class MonkeyAceParagonBuffable : ModTower
    {
        public override string DisplayName => "Buffable Rain of Fire";
        public override string TowerSet => TowerSetType.Military;
        public override bool DontAddToShop => !EnableBuffableParagons;
        public override string BaseTower => "MonkeyAce-502"; // the base bahaviors I'll build off of, basically needed cuz I'm not coding in all that monkey ace flying stuff.
        public override int Cost => 900000;
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 1;
        public override int BottomPathUpgrades => 0;
        public override string Description => "Buffable version of the Rain of Fire";
        public override void ModifyBaseTowerModel(TowerModel tower) //modifying the behaviors, and btd mod helper supplies the paremiter of TowerModel (makes the tower and adds a variable so I can edit the tower).
        {
            // created variables to use later, using the tower inserted into the tower parimeter.
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile;

            //projectile count.
            weapon.emission = new ArcEmissionModel("MonkeyAceParagonArcEmissionModel", 128, 180, 360, null, false);
            //damage.
            projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 220, false, false));
            projectile.GetDamageModel().damage += 160;
            projectile.pierce = 50;
            //seeking.
            var seekingBehavior = new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 700, false, false);
            projectile.AddBehavior(seekingBehavior);

            //using the tower perimeter to get the missles, since the 5-0-2 already has them.
            var missile = tower.GetBehaviors<AttackAirUnitModel>().First(a => a.name == "AttackAirUnitModel_Anti-MoabMissile_");

            //missle attack speed
            missile.weapons[0].Rate *= 0.1f;
            //missle damage
            missile.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 12500f, false, false));
            missile.GetDescendants<DamageModel>().ForEach(damageModel => damageModel.damage = 1000.0f);
            missile.GetDescendants<DamageModel>().ForEach(damageModel => damageModel.immuneBloonProperties = BloonProperties.None);

            //found pineapples from an existing tower and duplicating them into my tower.
            tower.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-220").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
            var pineapples = tower.GetBehaviors<AttackAirUnitModel>()[2];
            //pineapple attack speed
            pineapples.weapons[0].Rate *= 0.05f;
            //pineapple damage
            pineapples.GetDescendants<DamageModel>().ForEach(damageModel => damageModel.damage = 3500.0f);
            pineapples.GetDescendants<DamageModel>().ForEach(damageModel => damageModel.immuneBloonProperties = BloonProperties.None);

            //found ability model from an existing tower and duplicating it into my tower, and also getting the activate attack model (a model containing attack models exclusive to when the ability is active).
            var groundzeroAbility = Game.instance.model.GetTowerFromId("MonkeyAce-050").GetAbility().Duplicate();
            var goundzeroAbilityAttackModel = groundzeroAbility.GetBehavior<ActivateAttackModel>();
            
            //getting rid of the caps and changing the damage.
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().maxDamage = 999999;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().CapDamage(999999);
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().damage = 250000;
            //getting rid of the caps and changing the pierce.
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.maxPierce = 999999;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.CapPierce(999999);
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.pierce = 250000;

            tower.AddBehavior(groundzeroAbility);

            //making the tower hit camo by removing the invisible filters in some of my projectiles, and making the tower see camo like the ninja.
            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
        //making my buffable tower come right after the monkey ace
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.MonkeyAce).towerIndex + 1;
        }
        public class MonkeyAceParagonBuffableDegree100 : ModUpgrade<MonkeyAceParagonBuffable>
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
                weapon.rate *= 0.642857f;
                projectile.GetDamageModel().damage *= 2.5f;
                projectile.pierce *= 2.5f;
            }
        }
    }
}
