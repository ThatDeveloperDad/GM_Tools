using GameTools.Ruleset.Definitions;

namespace GameTools.RuleSet.DnD5eSRD;

public class Vocations5eData : VocationData
{
    public Vocations5eData():base() { }

    protected override void InitializeTemplates()
    {
        VocationTemplate alchemistTemplate = new VocationTemplate("Alchemist");
        RegisterVocation(alchemistTemplate);

        VocationTemplate brewerTemplate = new VocationTemplate("Brewer");
        RegisterVocation(brewerTemplate);

        VocationTemplate calligrapherTemplate = new VocationTemplate("Calligrapher");
        RegisterVocation(calligrapherTemplate);

        VocationTemplate carpenterTemplate = new VocationTemplate("Carpenter");
        RegisterVocation(carpenterTemplate);

        VocationTemplate cartographerTemplate = new VocationTemplate("Cartographer");
        RegisterVocation(cartographerTemplate);

        VocationTemplate cobblerTemplate = new VocationTemplate("Cobbler");
        RegisterVocation(cobblerTemplate);

        VocationTemplate cookTemplate = new VocationTemplate("Cook");
        RegisterVocation(cookTemplate);

        VocationTemplate glassblowerTemplate = new VocationTemplate("Glassblower");
        RegisterVocation(glassblowerTemplate);

        VocationTemplate jewelerTemplate = new VocationTemplate("Jeweler");
        RegisterVocation(jewelerTemplate);

        VocationTemplate leatherworkerTemplate = new VocationTemplate("Leatherworker");
        RegisterVocation(leatherworkerTemplate);

        VocationTemplate masonTemplate = new VocationTemplate("Mason");
        RegisterVocation(masonTemplate);

        VocationTemplate painterTemplate = new VocationTemplate("Painter");
        RegisterVocation(painterTemplate);

        VocationTemplate potterTemplate = new VocationTemplate("Potter");
        RegisterVocation(potterTemplate);

        VocationTemplate smithTemplate = new VocationTemplate("Smith");
        RegisterVocation(smithTemplate);

        VocationTemplate tinkerTemplate = new VocationTemplate("Tinker");
        RegisterVocation(tinkerTemplate);

        VocationTemplate weaverTemplate = new VocationTemplate("Weaver");
        RegisterVocation(weaverTemplate);

        VocationTemplate woodcarverTemplate = new VocationTemplate("Woodcarver");
        RegisterVocation(woodcarverTemplate);

        VocationTemplate blacksmithTemplate = new VocationTemplate("Blacksmith");
        RegisterVocation(blacksmithTemplate);

        VocationTemplate herbalistTemplate = new VocationTemplate("Herbalist");
        RegisterVocation(herbalistTemplate);

        VocationTemplate fletcherTemplate = new VocationTemplate("Fletcher");
        RegisterVocation(fletcherTemplate);

        VocationTemplate tailorTemplate = new VocationTemplate("Tailor");
        RegisterVocation(tailorTemplate);

        VocationTemplate scribeTemplate = new VocationTemplate("Scribe");
        RegisterVocation(scribeTemplate);

        VocationTemplate minerTemplate = new VocationTemplate("Miner");
        RegisterVocation(minerTemplate);

        VocationTemplate farmerTemplate = new VocationTemplate("Farmer");
        RegisterVocation(farmerTemplate);

        VocationTemplate hunterTemplate = new VocationTemplate("Hunter");
        RegisterVocation(hunterTemplate);

        VocationTemplate fisherTemplate = new VocationTemplate("Fisher");
        RegisterVocation(fisherTemplate);

        VocationTemplate merchantTemplate = new VocationTemplate("Merchant");
        RegisterVocation(merchantTemplate);

        VocationTemplate innkeeperTemplate = new VocationTemplate("Innkeeper");
        RegisterVocation(innkeeperTemplate);

        VocationTemplate bardTemplate = new VocationTemplate("Entertainer");
        RegisterVocation(bardTemplate);

        VocationTemplate healerTemplate = new VocationTemplate("Healer");
        RegisterVocation(healerTemplate);

        VocationTemplate navigatorTemplate = new VocationTemplate("Navigator");
        RegisterVocation(navigatorTemplate);

        VocationTemplate guardTemplate = new VocationTemplate("Guard");
        RegisterVocation(guardTemplate);

        VocationTemplate thiefTemplate = new VocationTemplate("Thief");
        RegisterVocation(thiefTemplate);

        VocationTemplate knightTemplate = new VocationTemplate("Knight");
        RegisterVocation(knightTemplate);

        VocationTemplate scholarTemplate = new VocationTemplate("Scholar");
        RegisterVocation(scholarTemplate);

        VocationTemplate explorerTemplate = new VocationTemplate("Explorer");
        RegisterVocation(explorerTemplate);

        VocationTemplate shopKeeper = new VocationTemplate("Shop Keeper");
        RegisterVocation(shopKeeper);

        VocationTemplate tavernKeeper = new VocationTemplate("Tavern Keeper");
        RegisterVocation(tavernKeeper);

        VocationTemplate Barkeep = new VocationTemplate("BarKeep");
        RegisterVocation(Barkeep);

        VocationTemplate Servant = new VocationTemplate("Servant");
        RegisterVocation(Servant);

    }
}
