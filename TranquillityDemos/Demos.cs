using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

// Tranquillity
using Tranquillity;
using Windows.UI.Core;
using SharpDX.Toolkit.Input;

namespace TranquillityDemos
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Demos : Game
    {
        #region Fields


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ParticleManager particleManager;

        string[] effects =
        {
            "Explosions",
            "Smoke Plume",
            "Ring of fire"
        };

        int currentState = 0;

        Matrix view;
        Matrix projection;

        Model grid;
        BasicEffect basicEffect;

        SpriteFont arial;

        Texture2D explosion;
        Texture2D fire;
        Texture2D smoke;

        // Camera state.
        float cameraArc = -5;
        float cameraRotation = 0;
        float cameraDistance = 300;

        float pointerPositionX = 0.0f;
        float pointerPositionY = 0.0f;

        FireParticleSystem fireParticleSystem;
        SmokeParticleSystem smokePlumeParticleSystem;

        SmokeRingEmitter smokeRingEmitter;
        SmokePlumeEmitter smokePlumeEmitter;

        ExplosionParticleSystem explosionParticleSystem;
        ExplosionSmokeParticleSystem explosionSmokeParticleSystem;
        ProjectileTrailParticleSystem projectileTrailParticleSystem;

        List<Projectile> projectiles = new List<Projectile>();

        TimeSpan timeToNextProjectile = TimeSpan.Zero;

        const int smokePlumeParticles = 50;
        const int smokeRingParticles = 50;
        const int fireRingSystemParticles = 250;

        public CoreWindow CoreWindow
        {
            get
            {
                return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow;
            }
        }


        #endregion

        #region Initialization


        public Demos()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>("Fonts/Font");

            grid = Content.Load<Model>("Models/grid");

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                Texture = Content.Load<Texture2D>(@"Models/checker"),
                Sampler = GraphicsDevice.SamplerStates.LinearWrap
            };

            explosion = Content.Load<Texture2D>("Textures/explosion");
            fire = Content.Load<Texture2D>("Textures/fire");
            smoke = Content.Load<Texture2D>("Textures/smoke");

            particleManager = new ParticleManager(GraphicsDevice);

            // Ring
            fireParticleSystem = new FireParticleSystem(500, fire);
            smokePlumeParticleSystem = new SmokeParticleSystem(500, smoke);

            smokePlumeEmitter = new SmokePlumeEmitter(Vector3.Zero, 0);
            smokeRingEmitter = new SmokeRingEmitter(Vector3.Zero, 0);

            smokePlumeParticleSystem.AddEmitter(smokePlumeEmitter);
            smokePlumeParticleSystem.AddEmitter(smokeRingEmitter);

            particleManager.AddParticleSystem(smokePlumeParticleSystem, GraphicsDevice.BlendStates.NonPremultiplied);
            particleManager.AddParticleSystem(fireParticleSystem, GraphicsDevice.BlendStates.Additive);

            // Explosions
            explosionParticleSystem = new ExplosionParticleSystem(100, explosion);
            explosionParticleSystem.AddAffector(new VelocityAffector(Vector3.Down));

            explosionSmokeParticleSystem = new ExplosionSmokeParticleSystem(100, smoke);
            explosionSmokeParticleSystem.AddAffector(new VelocityAffector(Vector3.Down));

            projectileTrailParticleSystem = new ProjectileTrailParticleSystem(500, smoke);

            particleManager.AddParticleSystem(explosionSmokeParticleSystem, GraphicsDevice.BlendStates.NonPremultiplied);
            particleManager.AddParticleSystem(projectileTrailParticleSystem, GraphicsDevice.BlendStates.NonPremultiplied);
            particleManager.AddParticleSystem(explosionParticleSystem, GraphicsDevice.BlendStates.Additive);

            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.ApplyChanges();

            CoreWindow.PointerPressed += CoreWindow_PointerPressed;
            CoreWindow.PointerMoved += CoreWindow_PointerMoved;
            CoreWindow.PointerReleased += CoreWindow_PointerReleased;
        }


        #endregion

        #region Update


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (currentState == 0)
            {
                UpdateExplosions(gameTime);
            }

            UpdateProjectiles(gameTime);

            particleManager.SetMatrices(view, projection);

            particleManager.Update(gameTime);

            base.Update(gameTime);
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Compute camera matrices.
            float aspectRatio = (float)GraphicsDevice.Viewport.Width /
                                (float)GraphicsDevice.Viewport.Height;

            view = Matrix.Translation(0, -25, 0) *
                Matrix.RotationY(MathUtil.DegreesToRadians(cameraRotation)) *
                Matrix.RotationX(MathUtil.DegreesToRadians(cameraArc)) *
                Matrix.LookAtRH(new Vector3(0, 0, -cameraDistance),
                                    new Vector3(0, 0, 0), Vector3.Up);

            projection = Matrix.PerspectiveFovRH(MathUtil.PiOverFour, aspectRatio, 1, 10000);

            DrawGrid(view, projection);

            DrawMessage();

            particleManager.Draw(gameTime);

            base.Draw(gameTime);
        }


        #endregion

        #region Private Methods


        /// <summary>
        /// Helper for updating the explosions effect.
        /// </summary>
        void UpdateExplosions(GameTime gameTime)
        {
            timeToNextProjectile -= gameTime.ElapsedGameTime;

            if (timeToNextProjectile <= TimeSpan.Zero)
            {
                // Create a new projectile once per second. The real work of moving
                // and creating particles is handled inside the Projectile class.
                projectiles.Add(new Projectile(explosionParticleSystem, explosionSmokeParticleSystem, projectileTrailParticleSystem));

                timeToNextProjectile += TimeSpan.FromSeconds(1);
            }
        }

        /// <summary>
        /// Helper for updating the list of active projectiles.
        /// </summary>
        void UpdateProjectiles(GameTime gameTime)
        {
            int i = 0;

            while (i < projectiles.Count)
            {
                if (!projectiles[i].Update(gameTime))
                {
                    projectileTrailParticleSystem.RemoveEmitter(projectiles[i].TrailEmitter);

                    // Remove projectiles at the end of their life.
                    projectiles.RemoveAt(i);
                }
                else
                {
                    // Advance to the next projectile.
                    i++;
                }
            }
        }

        void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            pointerPositionX = (float)args.CurrentPoint.Position.X;
            pointerPositionY = (float)args.CurrentPoint.Position.Y;
        }

        void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.IsInContact)
            {
                cameraArc += (pointerPositionY - (float)args.CurrentPoint.Position.Y) * 0.1f;
                cameraRotation -= (pointerPositionX - (float)args.CurrentPoint.Position.X) * 0.1f;
            }

            // Limit the arc movement.
            if (cameraArc > 90.0f)
                cameraArc = 90.0f;
            else if (cameraArc < -90.0f)
                cameraArc = -90.0f;

            pointerPositionX = (float)args.CurrentPoint.Position.X;
            pointerPositionY = (float)args.CurrentPoint.Position.Y;
        }

        void CoreWindow_PointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            currentState++;

            if (currentState > effects.Length - 1)
                currentState = 0;

            fireParticleSystem.EmissionRate = 0;
            smokeRingEmitter.EmissionRate = 0;
            smokePlumeEmitter.EmissionRate = 0;

            switch (currentState)
            {
                case 1:
                    smokePlumeEmitter.EmissionRate = smokePlumeParticles;

                    break;

                case 2:
                    fireParticleSystem.EmissionRate = fireRingSystemParticles;
                    smokeRingEmitter.EmissionRate = smokeRingParticles;

                    break;
            }

            pointerPositionX = 0.0f;
            pointerPositionY = 0.0f;
        }

        /// <summary>
        /// Helper for drawing the background grid model.
        /// </summary>
        void DrawGrid(Matrix view, Matrix projection)
        {
            grid.Draw(GraphicsDevice, Matrix.Identity, view, projection, basicEffect);
        }

        /// <summary>
        /// Helper for drawing our message text.
        /// </summary>
        void DrawMessage()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(arial, "Current effect: ", new Vector2(50, 50), Color.White);
            spriteBatch.DrawString(arial, effects[(int)currentState], new Vector2(220, 50), Color.White);
            spriteBatch.DrawString(arial, "Tap to switch demo", new Vector2(50, 70), Color.White);
            spriteBatch.End();
        }


        #endregion
    }
}
