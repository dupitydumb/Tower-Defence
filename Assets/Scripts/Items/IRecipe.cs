public interface IRecipe
{
    public RecipeData[] ItemsRecipe { get; set; }
    public float CraftTime { get; set; }
}

[System.Serializable]
public class RecipeData
{
    public ItemsData item;
    public int amount;
}