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
namespace Assignment_4___Game
{
    class NPC
    {
        private Texture2D NPC1SpriteSheet;
        private int frameIndex;
        private double animationUpdateCounter;
        private double stancesUpdateCounter;
        private const double STANCE_CHANGE_RATE = 0.5;
        private const int ANIMATION_RATE = 3;
        private static Random rng;

        private Rectangle NPC1SpriteUp, NPC1SpriteDown, NPC1SpriteLeft, NPC1SpriteRight;

        public enum Stances { Up, Down, Left, Right };
        private Assignment_4___Game.WorldManager.NPCTypes NPC_type;
        private Stances current_stance;
        private Vector2 current_position;
        private Vector2 old_position;
        private string textBlurb;
        private bool hitBounds;
        private bool NPCRandomizer;

        private Game1 game;
        private GraphicsDeviceManager graphics;

        public NPC(Game1 gameInstance, GraphicsDeviceManager graphicsInstance, Assignment_4___Game.WorldManager.NPCTypes NPC, string text)
        {
            game = gameInstance;
            graphics = graphicsInstance;
            NPC_type = NPC;
            textBlurb = text;
            rng = new Random();
        }

        public Stances CurrentStance
        {
            get { return current_stance; }
            set { current_stance = value; }
        }


        public Vector2 CurrentPosition
        {
            get { return current_position; }
            set { current_position = value; }
        }

        public Vector2 OldPosition
        {
            get { return old_position; }
            set { old_position = value; }
        }

        public bool HitBounds
        {
            get { return hitBounds; }
            set { hitBounds = value; }
        }

        public void Initialize()
            {
            frameIndex = 0;
            animationUpdateCounter = 0;
            stancesUpdateCounter = 0;
            switch (rng.Next(4))
            {
                case 0:
                    current_stance = Stances.Up;
                    break;
                case 1:
                    current_stance = Stances.Down;
                    break;
                case 2:
                    current_stance = Stances.Left;
                    break;
                case 3:
                    current_stance = Stances.Right;
                    break;
            }

            current_position = new Vector2( (rng.Next(1280)) , (rng.Next(720)) );
            old_position = new Vector2((rng.Next(1280)), (rng.Next(720)));
            hitBounds = false;
            NPCRandomizer = false;
            }

        public void LoadContent()
            {
            NPC1SpriteSheet = game.Content.Load<Texture2D>("NPC1SpriteSheet");

            NPC1SpriteUp = new Rectangle(0, 96, 32, 32);
            NPC1SpriteDown = new Rectangle(0, 0, 32, 32);
            NPC1SpriteLeft = new Rectangle(0, 32, 32, 32);
            NPC1SpriteRight = new Rectangle(0, 64, 32, 32);
            }

        public void Update(GameTime gameTime)
        {
        stancesUpdateCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
        old_position = current_position;
        switch(rng.Next(0, 4))
            {
            case 0:
                if (stancesUpdateCounter >= 1000 / STANCE_CHANGE_RATE && NPCRandomizer)
                {   
                current_stance = Stances.Up;
                stancesUpdateCounter -= 1000 / STANCE_CHANGE_RATE;
                }
                NPCRandomizer = !NPCRandomizer;
                animate(gameTime);
                break;
            case 1:
                if (stancesUpdateCounter >= 1000 / STANCE_CHANGE_RATE && NPCRandomizer)
                {
                    current_stance = Stances.Down;
                    stancesUpdateCounter -= 1000 / STANCE_CHANGE_RATE;
                }
                NPCRandomizer = !NPCRandomizer;
                animate(gameTime);
                break;
            case 2:
                if (stancesUpdateCounter >= 1000 / STANCE_CHANGE_RATE && NPCRandomizer)
                {
                current_stance = Stances.Left;
                stancesUpdateCounter -= 1000 / STANCE_CHANGE_RATE;
                }
                NPCRandomizer = !NPCRandomizer;
                animate(gameTime);
                break;
            case 3:
                if (stancesUpdateCounter >= 1000 / STANCE_CHANGE_RATE && NPCRandomizer)
                {
                current_stance = Stances.Right;
                stancesUpdateCounter -= 1000 / STANCE_CHANGE_RATE;
                }
                NPCRandomizer = !NPCRandomizer;
                animate(gameTime);
                break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (NPC_type)
            {
                case Assignment_4___Game.WorldManager.NPCTypes.NPC1:
                    switch (current_stance)
                    {
                        case Stances.Down:
                            NPC1SpriteDown.Offset(32 * frameIndex, 0);
                            spriteBatch.Draw(NPC1SpriteSheet, current_position, NPC1SpriteDown, Color.White, 0,
                                new Vector2(NPC1SpriteDown.Width / 2, NPC1SpriteDown.Height / 2), 1, SpriteEffects.None, 0);
                            NPC1SpriteDown.Offset(-32 * frameIndex, 0);
                            break;
                        case Stances.Up:
                            NPC1SpriteUp.Offset(32 * frameIndex, 0);
                            spriteBatch.Draw(NPC1SpriteSheet, current_position, NPC1SpriteUp, Color.White, 0,
                                new Vector2(NPC1SpriteUp.Width / 2, NPC1SpriteUp.Height / 2), 1, SpriteEffects.None, 0);
                            NPC1SpriteUp.Offset(-32 * frameIndex, 0);
                            break;
                        case Stances.Right:
                            NPC1SpriteRight.Offset(32 * frameIndex, 0);
                            spriteBatch.Draw(NPC1SpriteSheet, current_position, NPC1SpriteRight, Color.White, 0,
                                new Vector2(NPC1SpriteRight.Width / 2, NPC1SpriteRight.Height / 2), 1, SpriteEffects.None, 0);
                            NPC1SpriteRight.Offset(-32 * frameIndex, 0);
                            break;
                        case Stances.Left:
                            NPC1SpriteLeft.Offset(32 * frameIndex, 0);
                            spriteBatch.Draw(NPC1SpriteSheet, current_position, NPC1SpriteLeft, Color.White, 0,
                                new Vector2(NPC1SpriteLeft.Width / 2, NPC1SpriteLeft.Height / 2), 1, SpriteEffects.None, 0);
                            NPC1SpriteLeft.Offset(-32 * frameIndex, 0);
                            break;
                        default:
                            //wtf???
                            break;
                    }
                    break;
                default:
                    //wtf
                    break;

            }
        }

        public void animate(GameTime gameTime)
        {
            animationUpdateCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationUpdateCounter >= 1000 / ANIMATION_RATE)
            {
                switch(current_stance)
                {
                    case Stances.Up:
                        if (current_position.Y <= 16)
                        {
                        current_position.Y = 16;
                        hitBounds = true;
                        }
                        else
                        {
                        current_position.Y -= 2;
                        hitBounds = false;
                        }
                        break;
                    case Stances.Down:
                        if (current_position.Y >= graphics.PreferredBackBufferHeight - 16)
                        {
                        current_position.Y = graphics.PreferredBackBufferHeight - 16;
                        hitBounds = true;
                        }
                        else
                        {
                            current_position.Y += 2;
                            hitBounds = false;
                        }
                        break;
                    case Stances.Left:
                        if (current_position.X <= 16)
                        {
                            current_position.X = 16;
                            hitBounds = true;
                        }
                        else
                        {
                            current_position.X -= 2;
                            hitBounds = false;
                        }
                        break;
                    case Stances.Right:
                        if (current_position.X >= graphics.PreferredBackBufferWidth - 16)
                        {
                            current_position.X = graphics.PreferredBackBufferWidth - 16;
                            hitBounds = true;
                        }
                        else
                        {
                            current_position.X += 2;
                            hitBounds = false;
                        }
                        break;
                }

                frameIndex++;
                animationUpdateCounter -= 1000 / ANIMATION_RATE;
            }

            if (frameIndex > 2)
                frameIndex = 0;
        }
    }
}
