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

//Manages the player characters input, movement, drawing routines, and toggles a flag when the player hits a screen boundary.
namespace Assignment_4___Game
{
    public class Player
    {
        private Texture2D playerSpriteSheet;
        private int frameIndex;
        private double animationUpdateCounter;
        private const int ANIMATION_RATE = 3;
        private double encounterUpdateCounter;
        private const int UPDATE_RATE = 5;
        private bool inEncounter;

        private Rectangle playerSpriteUp;
        private Rectangle playerSpriteDown;
        private Rectangle playerSpriteLeft;
        private Rectangle playerSpriteRight;


        public enum Stances { Up, Down, Left, Right }
        private Stances current_stance;
        private Vector2 current_position;
        private Vector2 current_center;
        private Vector2 old_position;
        private Vector2 old_center;
        private int score;
        private int health;
        private int level;
        private Boolean hitBounds;
        private string currentInfo;

        private Game1 game;
        private GraphicsDeviceManager graphics;
        private SpriteFont playerDisplay;

        public Player(Game1 gameInstance, GraphicsDeviceManager graphicsInstance)
        {
            game = gameInstance;
            graphics = graphicsInstance;
        }

        public bool InEncounter
        {
            get { return inEncounter; }
            set { inEncounter = value; }
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

        public Vector2 CurrentCenter
        {
            get { return current_center; }
            set { current_center = value; }
        }

        public Vector2 OldCenter
        {
            get { return old_center; }
            set { old_center = value; }
        }

        public Vector2 OldPosition
        {
            get { return old_position; }
            set { old_position = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int Health
        {
            get { return health; }
            set
            {
                if (value > 0)
                    health = value;
                else
                    health = 0;
            }
        }

        public string CurrentInfo
        {
            get { return currentInfo; }
            set { currentInfo = value; }
        }

        public int Level
        {
            get { return level; }
            set { if (value > level) level = value; }
        }

        public Boolean HitBounds
        {
            get { return hitBounds; }
            set { hitBounds = value; }
        }

        public void Initialize()
            {
            frameIndex = 0;
            animationUpdateCounter = 0;
            current_stance = Stances.Down;
            current_center = new Vector2(current_position.X + 16, current_position.Y + 17);
            current_position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            old_position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            old_center = new Vector2(old_position.X + 16, old_position.Y + 17);
            currentInfo = "";
            score = 0;
            health = 100;
            level = 1;
            hitBounds = false;
            inEncounter = false;
            }

        public void LoadContent()
            {
            playerSpriteSheet = game.Content.Load<Texture2D>("playerSpriteSheet");

            playerSpriteUp = new Rectangle(0, 102, 32, 34);
            playerSpriteDown = new Rectangle(0, 0, 32, 34);
            playerSpriteLeft = new Rectangle(0, 34, 32, 34);
            playerSpriteRight = new Rectangle(0, 68, 32, 34);

            playerDisplay = game.Content.Load<SpriteFont>("Arial");
            }

        public void Update(GameTime gameTime, WorldManager worldInstance)
            {
                old_position = current_position;
                old_center = current_center;
                if (worldInstance.CurrentState != WorldManager.WorldState.ENCOUNTER)
                    handleWorldInput(gameTime);
                else
                    handleEncounterInput(gameTime, worldInstance);   
            }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!inEncounter)
            {
                switch (current_stance)
                {
                    case Stances.Down:
                        playerSpriteDown.Offset(32 * frameIndex, 0);
                        spriteBatch.Draw(playerSpriteSheet, current_position, playerSpriteDown, Color.White, 0,
                            new Vector2(playerSpriteDown.Width / 2, playerSpriteDown.Height / 2), 1, SpriteEffects.None, 0);
                        playerSpriteDown.Offset(-32 * frameIndex, 0);
                        break;
                    case Stances.Up:
                        playerSpriteUp.Offset(32 * frameIndex, 0);
                        spriteBatch.Draw(playerSpriteSheet, current_position, playerSpriteUp, Color.White, 0,
                            new Vector2(playerSpriteUp.Width / 2, playerSpriteUp.Height / 2), 1, SpriteEffects.None, 0);
                        playerSpriteUp.Offset(-32 * frameIndex, 0);
                        break;
                    case Stances.Right:
                        playerSpriteRight.Offset(32 * frameIndex, 0);
                        spriteBatch.Draw(playerSpriteSheet, current_position, playerSpriteRight, Color.White, 0,
                            new Vector2(playerSpriteRight.Width / 2, playerSpriteRight.Height / 2), 1, SpriteEffects.None, 0);
                        playerSpriteRight.Offset(-32 * frameIndex, 0);
                        break;
                    case Stances.Left:
                        playerSpriteLeft.Offset(32 * frameIndex, 0);
                        spriteBatch.Draw(playerSpriteSheet, current_position, playerSpriteLeft, Color.White, 0,
                            new Vector2(playerSpriteLeft.Width / 2, playerSpriteLeft.Height / 2), 1, SpriteEffects.None, 0);
                        playerSpriteLeft.Offset(-32 * frameIndex, 0);
                        break;
                    default:
                        //wtf???
                        break;
                }
            }

            spriteBatch.DrawString(playerDisplay, currentInfo, new Vector2(600, 600), Color.White);
        }

        private void animate(GameTime gameTime)
        {
            animationUpdateCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationUpdateCounter >= 1000 / ANIMATION_RATE)
            {
                frameIndex++;
                animationUpdateCounter -= 1000 / ANIMATION_RATE;
            }

            if (frameIndex > 2)
                frameIndex = 0;
        }
        private void handleWorldInput(GameTime gameTime)
        {
             if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    {
                        current_stance = Stances.Up;

                        if (current_position.Y <= 16)
                        {
                            hitBounds = true;
                        }
                        else
                        {
                            current_position.Y -= 2;
                            hitBounds = false;
                        }

                        animate(gameTime);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    {
                        current_stance = Stances.Down;

                        if (current_position.Y >= graphics.PreferredBackBufferHeight - 16)
                        {
                            hitBounds = true;
                        }
                        else
                        {
                            current_position.Y += 2;
                            hitBounds = false;
                        }

                        animate(gameTime);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    {
                        current_stance = Stances.Left;

                        if (current_position.X <= 16)
                        {
                            hitBounds = true;
                        }
                        else
                        {
                            current_position.X -= 2;
                            hitBounds = false;
                        }

                        animate(gameTime);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    {
                        current_stance = Stances.Right;

                        if (current_position.X >= graphics.PreferredBackBufferWidth - 16)
                        {
                            hitBounds = true;
                        }
                        else
                        {
                            current_position.X += 2;
                            hitBounds = false;
                        }

                        animate(gameTime);
                    }
                    current_center = new Vector2(current_position.X + 16, current_position.Y + 17);
                }
        private void handleEncounterInput(GameTime gameTime, WorldManager worldInstance)
        {
            encounterUpdateCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (encounterUpdateCounter >= 1000 / UPDATE_RATE)
            {
                
                if (worldInstance.PlayerFlag)
                {
                    //handle player stuff
                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        worldInstance.damageEnemy();
                        worldInstance.clearEnemyActionNotification();
                        currentInfo = "Player damaged enemy for 10 health!";
                        worldInstance.PlayerFlag = false;
                    }
                }
                encounterUpdateCounter -= 1000 / UPDATE_RATE;
            }
        }
        }
    }
