using GameTools.Ruleset.Definitions;

namespace GameTools.RuleSet.DnD5eSRD;

public class Backgrounds5eData : BackgroundData
{
    public Backgrounds5eData():base()
    {
    }

    protected override void InitializeTemplates()
    {
        BackgroundTemplate acolyteTemplate = new BackgroundTemplate("Acolyte");
        RegisterBackground(acolyteTemplate);

        BackgroundTemplate charlatanTemplate = new BackgroundTemplate("Charlatan");
        RegisterBackground(charlatanTemplate);

        BackgroundTemplate criminalTemplate = new BackgroundTemplate("Criminal");
        RegisterBackground(criminalTemplate);

        BackgroundTemplate entertainerTemplate = new BackgroundTemplate("Entertainer");
        RegisterBackground(entertainerTemplate);

        BackgroundTemplate folkHeroTemplate = new BackgroundTemplate("Folk Hero");
        RegisterBackground(folkHeroTemplate);

        BackgroundTemplate guildArtisanTemplate = new BackgroundTemplate("Guild Artisan");
        RegisterBackground(guildArtisanTemplate);

        BackgroundTemplate hermitTemplate = new BackgroundTemplate("Hermit");
        RegisterBackground(hermitTemplate);

        BackgroundTemplate nobleTemplate = new BackgroundTemplate("Noble");
        RegisterBackground(nobleTemplate);

        BackgroundTemplate outlanderTemplate = new BackgroundTemplate("Outlander");
        RegisterBackground(outlanderTemplate);

        BackgroundTemplate sageTemplate = new BackgroundTemplate("Sage");
        RegisterBackground(sageTemplate);

        BackgroundTemplate sailorTemplate = new BackgroundTemplate("Sailor");
        RegisterBackground(sailorTemplate);

        BackgroundTemplate soldierTemplate = new BackgroundTemplate("Soldier");
        RegisterBackground(soldierTemplate);

        BackgroundTemplate urchinTemplate = new BackgroundTemplate("Urchin");
        RegisterBackground(urchinTemplate);

    }
}
