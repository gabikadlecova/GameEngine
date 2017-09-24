using Casting.RayCasting.Interfaces;

namespace Casting.Environment.Interfaces
{
    /// <summary>
    /// Specifies additional wall data
    /// </summary>
    public interface IWall : ICrossable
    {
        /// <summary>
        /// Texture on the X side
        /// </summary>
        ITextureWrapper TextureX { get; }

        /// <summary>
        /// Texture on the Y side
        /// </summary>
        ITextureWrapper TextureY { get; }
    }
}
