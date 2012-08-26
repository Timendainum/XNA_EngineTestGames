using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TwoDWindowsGameLibrary.Sprites;

namespace SpriteGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class SpriteGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Dictionary<String, Texture2D> Textures = new Dictionary<String, Texture2D>();
		Sprite Tank;
		AnimatedSprite AnimatedTank;

		public SpriteGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			Texture2D newTexture = Content.Load<Texture2D>(@"MulticolorTanks");
			Textures.Add("MulticolorTanks", newTexture);
			newTexture = Content.Load<Texture2D>(@"PrincessCharacter");
			Textures.Add("PrincessCharacter", newTexture);

			//setup sprite 
			Tank = new Sprite(Vector2.Zero, Textures["MulticolorTanks"], new Rectangle(0, 0, 32, 32));
			AnimatedTank = new AnimatedSprite(new Vector2(32, 0), Textures["MulticolorTanks"], new Rectangle(0, 0, 32, 32), "animation", 8, 5.0f);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// TODO: Add your update logic here
			Tank.Update(gameTime);
			AnimatedTank.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			Tank.Draw(spriteBatch);
			AnimatedTank.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
