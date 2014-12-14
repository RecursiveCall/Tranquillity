using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// Defines a particle with fixed properties
	/// </summary>
    public class StaticParticle : IParticle
    {
        /// <summary>
        /// Gets or sets the position of the particle
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the color of the particle
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the angle of the particle
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the scale of the particle
        /// </summary>
        public float Scale { get; set; }
    }
}
