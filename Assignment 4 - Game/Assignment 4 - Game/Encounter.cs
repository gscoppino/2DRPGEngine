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
    class Encounter
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        private WorldManager world;
        private Player player;
        private Enemy enemy;

        private Texture2D grassyBattlePlain;

        public enum BattleState { ACTIVE, PLAYER_WON, ENEMY_WON, PLAYER_FLED, ENEMY_FLED }
        public BattleState current_state;

        public Encounter(Game1 gameInstance, GraphicsDeviceManager graphicsInstance, WorldManager worldInstance, Player playerInstance, Enemy enemy)
        {
            game = gameInstance;
            graphics = graphicsInstance;
            world = worldInstance;
            player = playerInstance;
            this.enemy = enemy;
        }

        public void Initialize()
        {
            current_state = BattleState.ACTIVE;
            enemy.Initialize();
        }

        public void LoadContent()
        {

            switch (world.CurrentLocation)
            {
                case WorldManager.Locations.START:
                    grassyBattlePlain = game.Content.Load<Texture2D>("grassyBattlePlain");
                    break;
                case WorldManager.Locations.TOWN:
                    break;
                case WorldManager.Locations.CROSSWAYS:
                    break;
                case WorldManager.Locations.SCREEN4:
                    break;
                case WorldManager.Locations.END:
                    break;
            }

            enemy.LoadContent();

        }

        public void Update(GameTime gameTime)
        {
            switch (current_state)
            {
                case BattleState.ACTIVE:
                    if (enemy.Health == 0)
                    {
                        current_state = BattleState.PLAYER_WON;
                        player.CurrentInfo = "";
                        enemy.CurrentInfo = "";
                    }
                    else if (player.Health == 0)
                    {
                        current_state = BattleState.ENEMY_WON;
                        player.CurrentInfo = "";
                        enemy.CurrentInfo = "";
                    }
                    else
                        enemy.Update(player, world, gameTime);
                    break;
                case BattleState.ENEMY_FLED:
                    break;
                case BattleState.ENEMY_WON:
                    break;
                case BattleState.PLAYER_FLED:
                    break;
                case BattleState.PLAYER_WON:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (world.CurrentLocation)
            {
                case WorldManager.Locations.START:
                    spriteBatch.Draw(grassyBattlePlain, new Vector2(240, 60), Color.White);
                    break;
            }
            enemy.Draw(spriteBatch);
        }

        public void damageEnemy()
        {
            enemy.Health -= 10;
        }

        public void clearEnemyActionNotification()
        {
            enemy.CurrentInfo = "";
        }
    }
}
