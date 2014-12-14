using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Tranquillity
{
	/// <summary>
	/// Defines a particle with properties that change
	/// </summary>
    public class DynamicParticle : IParticle
    {
        /// <summary>
        /// Gets or sets the position of the particle
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets the initial position of the particle
        /// </summary>
        public Vector3 InitialPosition { get; internal set; }

        /// <summary>
        /// Gets or sets the color of the particle
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the initial color of the particle
        /// </summary>
        public Color InitialColor { get; internal set; }

        /// <summary>
        /// Gets or sets the angle of the particle
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets the initial angle of the particle
        /// </summary>
        public float InitialAngle { get; internal set; }

        /// <summary>
        /// Gets or sets the scale of the particle
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets the initial scale of the particle
        /// </summary>
        public float InitialScale { get; internal set; }

        /// <summary>
        /// Gets or sets the velocity of the particle
        /// </summary>
        public Vector3? Velocity { get; set; }

        /// <summary>
        /// Gets the initial velocity of the particle
        /// </summary>
        public Vector3? InitialVelocity { get; internal set; }

        /// <summary>
        /// Gets or sets the rotation of the particle
        /// </summary>
        public float? Rotation { get; set; }

        /// <summary>
        /// Gets the initial rotation of the particle
        /// </summary>
        public float? InitialRotation { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the particle is affected by particle affectors
        /// </summary>
        public bool IsAffectable { get; set; }

        TimeSpan? lifespan;
        /// <summary>
        /// Gets or sets the total lifespan of the particle
        /// </summary>
        public TimeSpan? Lifespan
        {
            get
            {
                return lifespan;
            }
            internal set
            {
                lifespan = value;
                RemainingLifetime = lifespan;
            }
        }

        /// <summary>
        /// Gets or sets the remaining lifetime of the particle
        /// </summary>
        public TimeSpan? RemainingLifetime { get; set; }

        /// <summary>
        /// Gets the age of the particle
        /// </summary>
        public float? Age
        {
            get
            {
                if (RemainingLifetime.HasValue && Lifespan.HasValue)
                {
                    return (float)RemainingLifetime.Value.TotalSeconds / (float)Lifespan.Value.TotalSeconds;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
