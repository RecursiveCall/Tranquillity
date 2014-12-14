using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// Represents a 3D particle
	/// </summary>
    public interface IParticle
    {
        /// <summary>
        /// Gets or sets the position of the particle
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the color of the particle
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the angle of the particle
        /// </summary>
        float Angle { get; set; }

        /// <summary>
        /// Gets or sets the scale of the particle
        /// </summary>
        float Scale { get; set; }
    }
}
