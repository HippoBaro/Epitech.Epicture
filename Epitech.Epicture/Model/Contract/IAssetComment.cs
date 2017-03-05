namespace Epitech.Epicture.Model.Contract
{
    internal interface IAssetComment : IAsset
    {
        string Comment { get; }
        string Author { get; }
    }
}
