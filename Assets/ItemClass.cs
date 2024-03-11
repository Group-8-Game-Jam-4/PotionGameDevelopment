public class ItemClass
{
    // names
    // name used internally
    public string className = "";
    // name shown in uis
    public string displayName = "";


    // item properties
    // name of the image that will be displayed without extension
    public string imageName = "";
    // how many of the item per stack
    public int stackSize = 16;
    // common = 0, uncommon = 1, rare = 2, epic = 3
    public string rarity;
    public int quantity = 0;


    // spawn locations
    // grassland = 0, volcano = 2, snowyMountains = 3
    public string spawnBiome1;
    public string spawnBiome2;


    // economy values
    public int sellPrice;
    public int storePrice;
    public int goblinPrice;
}
