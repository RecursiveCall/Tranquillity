using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;


namespace Tranquillity
{
    /// <summary>
    /// Manages the logic and drawing of all particle systems.
    /// This is a DrawableGameComponent; simply add it to the game component collection of your game.
    /// </summary>
    public class ParticleManager
    {
        #region Fields
        

        private readonly Matrix InvertY = Matrix.Scaling(1, -1, 1);

        private readonly SpriteBatch SpriteBatch;
        private readonly BasicEffect BasicEffect;

        private readonly Dictionary<BlendState, List<IParticleSystem>> ParticleSystems = new Dictionary<BlendState, List<IParticleSystem>>();

        private readonly GraphicsDevice GraphicsDevice;


        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the camera view matrix
        /// </summary>
        Matrix View { get; set; }

        /// <summary>
        /// Gets or set the camera projection matrix
        /// </summary>
        Matrix Projection { get; set; }

        /// <summary>
        /// Gets the total current number of particles across all enabled particle systems
        /// </summary>
        public int ParticleCount
        {
            get
            {
                int total = 0;

                foreach (List<IParticleSystem> particleSystemBatch in ParticleSystems.Values)
                {
                    foreach (IParticleSystem particleSystem in particleSystemBatch)
                    {
                        if (particleSystem.Enabled)
                        {
                            total += particleSystem.ParticleCount;
                        }
                    }
                }

                return total;
            }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Initializes an instance of the particle manager
        /// </summary>
        /// <param name="game"></param>
        public ParticleManager(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            BasicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
                LightingEnabled = false,
                World = InvertY,
                View = Matrix.Identity
            };
        }


        #endregion

        #region Update


        /// <summary>
        /// Updates the properties of all particle systems
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (List<IParticleSystem> particleSystemBatch in ParticleSystems.Values)
            {
                foreach (IParticleSystem particleSystem in particleSystemBatch)
                {
                    if (particleSystem.Enabled)
                    {
                        DynamicParticleSystem dynamicParticleSystem = particleSystem as DynamicParticleSystem;

                        if (dynamicParticleSystem != null)
                        {
                            dynamicParticleSystem.Update(gameTime);
                        }
                    }
                }
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws all of the particle systems
        /// </summary>
        /// <remarks>
        /// An efficient SpriteBatch-based method by Shawn Hargreaves.
        /// Thoroughly documented at http://blogs.msdn.com/b/shawnhar/archive/2011/01/12/spritebatch-billboards-in-a-3d-world.aspx
        /// </remarks>
        public void Draw(GameTime gameTime)
        {
            BasicEffect.Projection = Projection;

            foreach (KeyValuePair<BlendState, List<IParticleSystem>> particleSystemBatch in ParticleSystems)
            {
                SpriteBatch.Begin(0, particleSystemBatch.Key, null, GraphicsDevice.DepthStencilStates.DepthRead, GraphicsDevice.RasterizerStates.CullNone, BasicEffect);

                foreach (IParticleSystem particleSystem in particleSystemBatch.Value)
                {
                    if (particleSystem.Enabled)
                    {
                        for (int i = 0; i < particleSystem.ParticleCount; i++)
                        {
                            IParticle particle = particleSystem[i];

                            Vector3 viewSpacePosition = (Vector3)Vector3.Transform(particle.Position, View);

                            SpriteBatch.Draw(particleSystem.Texture, new Vector2(viewSpacePosition.X, viewSpacePosition.Y), null, particle.Color, particle.Angle, particleSystem.TextureOrigin, particle.Scale, 0, viewSpacePosition.Z);
                        }
                    }
                }

                SpriteBatch.End();
            }
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Adds a particle system with default blend state settings
        /// </summary>
        public void AddParticleSystem(IParticleSystem particleSystem)
        {
            AddParticleSystem(particleSystem, GraphicsDevice.BlendStates.NonPremultiplied);
        }

        /// <summary>
        /// Adds a particle system with custom blend state settings
        /// </summary>
        public void AddParticleSystem(IParticleSystem particleSystem, BlendState blendState)
        {
            if (ParticleSystems.ContainsKey(blendState))
            {
                ParticleSystems[blendState].Add(particleSystem);
            }
            else
            {
                List<IParticleSystem> particleSystemBatch = new List<IParticleSystem>();
                particleSystemBatch.Add(particleSystem);

                ParticleSystems.Add(blendState, particleSystemBatch);
            }
        }

        /// <summary>
        /// Sets the view and projection matrices
        /// </summary>
        public void SetMatrices(Matrix view, Matrix projection)
        {
            View = view * InvertY;
            Projection = projection;
        }


        #endregion
    }
}
