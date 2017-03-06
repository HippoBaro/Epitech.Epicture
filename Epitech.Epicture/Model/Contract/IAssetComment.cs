namespace Epitech.Epicture.Model.Contract
{
    public interface IAssetComment : IAsset
    {
        string Comment { get; }
        string Author { get; }
    }
}
