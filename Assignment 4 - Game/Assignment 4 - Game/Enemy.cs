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

//Manages individual enemy creation, AI routines, drawing routines
namespace Assignment_4___Game
{
    public class Enemy
    {
        private Texture2D enemySpriteSheet;
        private Rectangle enemySprite1;

        public enum Enemies { Enemy1, Enemy2, Enemy3};

        private Enemies enemy_type;

        private int health;
        private static Random rng;
        private double enemyUpdateCounter;
        private string currentInfo;
        private const int UPDATE_RATE = 2;

        Game1 game;
        GraphicsDeviceManager graphics;

        private SpriteFont enemyDisplay;


        public Enemy(Game1 gameInstance, GraphicsDeviceManager graphicsInstance)
        {
            game = gameInstance;
            graphics = graphicsInstance;
            rng = new Random();
        }

        public Enemies EnemyType
        {
            get { return enemy_type; }
            set { enemy_type = value; }
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

        public void Initialize()
        {
            enemyUpdateCounter = 0;
            health = 100;
            currentInfo = "";

            switch (rng.Next(10))
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    enemy_type = Enemies.Enemy1;
                    health = rng.Next(0, 100) + 1;
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    enemy_type = Enemies.Enemy2;
                    health = rng.Next(50, 100) + 1;
                    break;
                case 9:
                    enemy_type = Enemies.Enemy3;
                    health = rng.Next(100, 150) + 1;
                    break;
                default:
                    break;
            }
            //enemy type should be random
            //current stance should be random but cannot be opposite of the player
            //current position should be random but should not spawn on top of the player
        }

        public void LoadContent()
        {
            enemySpriteSheet = game.Content.Load<Texture2D>("enemySpriteSheet");
            enemySprite1 = new Rectangle(0, 0, 50, 65);
            enemyDisplay = game.Content.Load<SpriteFont>("Arial");
            //define rectangles indicating the individual sprites and their positions
        }

        public void Update(Player playerInstance, WorldManager worldInstance, GameTime gameTime)
        {
            enemyUpdateCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (enemyUpdateCounter >= 1000 / UPDATE_RATE)
            {
                playerInstance.CurrentInfo = "";

                switch (rng.Next(10))
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        this.attack(playerInstance);
                        currentInfo = "Enemy attacks for 10 damage!";
                        break;
                    case 5:
                    case 6:
                    case 7:
                        this.heal();
                        currentInfo = "Enemy heals for 10 health!";
                        break;
                    case 8:
                    case 9:
                        break;
                }
                worldInstance.PlayerFlag = true;
                enemyUpdateCounter -= 1000 / UPDATE_RATE;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemySpriteSheet, new Vector2(650, 300), enemySprite1, Color.White, 0,
                        new Vector2(enemySprite1.Width / 2, enemySprite1.Height / 2), 1, SpriteEffects.None, 0); 
            spriteBatch.DrawString(enemyDisplay, "Enemy Health: " + this.Health, new Vector2(1100, 50), Color.White);
            spriteBatch.DrawString(enemyDisplay, currentInfo, new Vector2(600, 600), Color.White);
        }

        private void attack(Player playerInstance)
        {
            playerInstance.Health -= 10;
        }
        private void heal()
        {
            this.Health += 10;
        }
    }
}