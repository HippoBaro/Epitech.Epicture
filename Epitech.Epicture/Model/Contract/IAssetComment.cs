namespace Epitech.Epicture.Model.Contract
{
    public interface IAssetComment : IAsset
    {
        string Comment { get; }

        /// <summary>
        /// The author of this comment
        /// </summary>
        string Author { get; }
    }
}
