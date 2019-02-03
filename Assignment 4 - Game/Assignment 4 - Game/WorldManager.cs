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

/**
 * Manages all game locations, including movement between locations, drawing routines of locations, and music playback for locations.
 * Also handles boundary collisions by players and enemies.
**/
namespace Assignment_4___Game
{
    public class WorldManager
    {
        public enum Locations { START, TOWN, CROSSWAYS, SCREEN4, END };
        public enum NPCTypes { NPC1, NPC2 };
        public enum WorldState { TITLE, ADVENTURE, INTERACT, ENCOUNTER, GAME_OVER };
        private bool playerFlag;
        private SpriteFont worldDisplay;

        List<NPC> NPCs;

        Locations current_location;
        WorldState current_state;

        Texture2D grassyArea;
        Song overworldTheme;
        Texture2D townSign;
        private Vector2 townSign_position;

        private string currentInfo;

        Texture2D townArea;
        Song townTheme;
        Texture2D townHouse;
        Texture2D treesSpriteSheet;
        private Rectangle normalTree;

        Song battleTheme;

        Texture2D rockyArea;

        Game1 game;
        GraphicsDeviceManager graphics;

        private Random rng;
        private Encounter e;

        private double hitTimer;
        private const int HIT_RATE = 2;

        private double counterTimer;
        private const int COUNTER_RATE = 3;


        public WorldManager(Game1 gameInstance, GraphicsDeviceManager graphicsInstance)
        {
            game = gameInstance;
            graphics = graphicsInstance;
            rng = new Random();
        }

        public string CurrentInfo
        {
            get { return currentInfo; }
            set { currentInfo = value; }
        }
        public Locations CurrentLocation
        {
            get { return current_location; }
            set { current_location = value; }
        }

        public WorldState CurrentState
        {
            get { return current_state; }
            set { current_state = value; }
        }

        public bool PlayerFlag
        {
            get { return playerFlag; }
            set { playerFlag = value; }
        }

        public void Initialize()
        {
            current_location = Locations.START;
            current_state = WorldState.ADVENTURE;
            counterTimer = 0;
            playerFlag = false;
            NPCs = new List<NPC>();
            NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Hello World"));

            foreach (NPC n in NPCs)
            {
                n.Initialize();
            }
        }

        public void LoadContent()
        {
            worldDisplay = game.Content.Load<SpriteFont>("Arial");
            grassyArea = game.Content.Load<Texture2D>("grassyArea");
            overworldTheme = game.Content.Load<Song>("Oskar Schuster - Sneeuwland");
            townSign = game.Content.Load<Texture2D>("townSign");
            townSign_position = new Vector2(1150, 360);

            townArea = game.Content.Load<Texture2D>("townArea");
            townTheme = game.Content.Load<Song>("Jared C Balogh - Strides");
            townHouse = game.Content.Load<Texture2D>("townHouse");
            treesSpriteSheet = game.Content.Load<Texture2D>("treesSpriteSheet");
            normalTree = new Rectangle(159, 249, 81, 97);

            battleTheme = game.Content.Load<Song>("Deep Sky Blue");

            rockyArea = game.Content.Load<Texture2D>("rockyArea");

            foreach (NPC n in NPCs)
            {
                n.LoadContent();
            }
        }

        public void Update(Player playerInstance, GameTime gameTime)
        {
            musicManager(gameTime);

            if (current_state == WorldState.ADVENTURE)
            {
                checkPlayerGlobalCollisons(playerInstance);
                checkPlayerLocalCollisions(playerInstance);
                if (current_location != Locations.TOWN)
                    encounterManager(playerInstance, gameTime);
            }
            if (current_state != WorldState.ENCOUNTER && current_state != WorldState.GAME_OVER)
            {
                foreach (NPC n in NPCs)
                {
                    n.Update(gameTime);
                }
            }

            if (current_state == WorldState.ENCOUNTER)
            {
                encounterDriver(playerInstance, gameTime);
            }

            for (int i = NPCs.Count - 1; i > -1; i--)
            {
                for (int j = NPCs.Count - 1; j > -1; j--)
                {
                    if (NPCs[i].Equals(NPCs[j]))
                    {
                        continue;
                    }
                    else if (Vector2.Distance(NPCs[i].CurrentPosition + new Vector2(16, 16), NPCs[j].CurrentPosition + new Vector2(16, 16)) < 16 + 16)
                    {
                        NPCs[i].CurrentPosition = NPCs[i].OldPosition;
                        NPCs[j].CurrentPosition = NPCs[j].OldPosition;
                        return;
                    }
                    else if (Vector2.Distance(NPCs[i].CurrentPosition + new Vector2(16, 16), playerInstance.CurrentPosition + new Vector2(16, 16)) < 16 + 16)
                    {
                        NPCs[i].CurrentPosition = NPCs[i].OldPosition;
                        playerInstance.CurrentPosition = playerInstance.OldPosition;
                    }
                }

            }
           // if (hitTimer >= 1000 / HIT_RATE)
            //{
              //  hitTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            //}
        }


        //for (int i = enemyInstanceList.Count - 1; i > -1; i--)
        // {
        //find out which enemies are hitting bounds and which bound is hit
        //if (enemyInstanceList[i].CurrentPosition.X <= 16)
        // {
        //     enemyInstanceList.RemoveAt(i);
        //  }
        // else if (enemyInstanceList[i].CurrentPosition.X >= graphics.PreferredBackBufferWidth - 16)
        //  {
        //      enemyInstanceList.RemoveAt(i);
        //  }
        //   else if (enemyInstanceList[i].CurrentPosition.Y <= 16)
        //   {
        //       enemyInstanceList.RemoveAt(i);
        //  }
        //   else if (enemyInstanceList[i].CurrentPosition.Y >= graphics.PreferredBackBufferHeight - 16)
        //  {
        //     enemyInstanceList.RemoveAt(i);
        //  }
        // }


        public void Draw(SpriteBatch spriteBatch)
        {
            switch(current_state)
            {
            case WorldState.ADVENTURE:
            case WorldState.ENCOUNTER:
            //spriteBatch.Draw(rockyArea, new Vector2(0, 0), Color.White);
            switch (current_location)
            {
                case Locations.START:
                    spriteBatch.Draw(grassyArea, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(townSign, townSign_position, Color.White);
                    break;
                case Locations.TOWN:
                    spriteBatch.Draw(townArea, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(32, 32), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(162, 32), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(290, 32), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(950, 32), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(1078, 32), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(32, 500), Color.White);
                    spriteBatch.Draw(townHouse, new Vector2(162, 500), Color.White);
                    spriteBatch.Draw(treesSpriteSheet, new Vector2(475, 200), normalTree, Color.White, 0,
                        new Vector2(normalTree.Width / 2, normalTree.Height / 2), 1, SpriteEffects.None, 0);
                    spriteBatch.Draw(treesSpriteSheet, new Vector2(750, 600), normalTree, Color.White, 0,
                        new Vector2(normalTree.Width / 2, normalTree.Height / 2), 1, SpriteEffects.None, 0);
                    spriteBatch.Draw(treesSpriteSheet, new Vector2(1000, 500), normalTree, Color.White, 0,
                        new Vector2(normalTree.Width / 2, normalTree.Height / 2), 1, SpriteEffects.None, 0);
                    //spriteBatch.Draw
                    break;
                case Locations.CROSSWAYS:
                    spriteBatch.Draw(grassyArea, new Vector2(0, 0), Color.White);
                    break;
            }
            //spriteBatch.Draw(grassyArea, new Vector2(0, 0), Color.White);
            foreach (NPC n in NPCs)
            {
                n.Draw(spriteBatch);
            }

            if (current_state == WorldState.ENCOUNTER)
            {
                e.Draw(spriteBatch);
            }
            break;

            case WorldState.GAME_OVER: //not finished implementing
                game.GraphicsDevice.Clear(Color.Black);
                currentInfo = "Game Over! Press Enter to continue";
                spriteBatch.DrawString(worldDisplay, currentInfo, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);
                break;
        }
        }

        private void encounterManager(Player playerInstance, GameTime gameTime)
        {
            counterTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (counterTimer >= 1000 / COUNTER_RATE)
            {
                if (!playerInstance.CurrentPosition.Equals(playerInstance.OldPosition))
                {
                    switch (rng.Next(10))
                    {
                        case 0:
                            current_state = WorldState.ENCOUNTER;
                            playerInstance.InEncounter = true;
                            e = new Encounter(game, graphics, this, playerInstance, new Enemy(game, graphics));
                            e.Initialize();
                            e.LoadContent();
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            break;
                    }
                }
                counterTimer -= 1000 / COUNTER_RATE;
            }
        }
        private void encounterDriver(Player playerInstance, GameTime gameTime)
        {
            if (!PlayerFlag)
            {
                e.Update(gameTime);
            }

            if (e.current_state == Encounter.BattleState.ENEMY_WON || e.current_state == Encounter.BattleState.PLAYER_WON)
            {
                current_state = WorldState.ADVENTURE;
                playerInstance.InEncounter = false;
            }
        }
        private void checkPlayerGlobalCollisons(Player playerInstance)
        {
            if (playerInstance.HitBounds == true)
            {
                //find out which bound was hit
                if (playerInstance.CurrentPosition.X <= 16)
                {
                    switch (current_location)
                    {
                        case Locations.START:
                            playerInstance.CurrentPosition = new Vector2(16, playerInstance.CurrentPosition.Y);
                            break;
                        case Locations.TOWN:
                            current_location = Locations.START;
                            generateNPCs();
                            playerInstance.CurrentPosition = new Vector2(graphics.PreferredBackBufferWidth - 16, playerInstance.CurrentPosition.Y);
                            break;
                        case Locations.CROSSWAYS:
                            break;
                        case Locations.SCREEN4:
                            break;
                        case Locations.END:
                            break;
                    }
                }
                else if (playerInstance.CurrentPosition.X >= graphics.PreferredBackBufferWidth - 16)
                {
                    switch (current_location)
                    {
                        case Locations.START:
                            current_location = Locations.TOWN;
                            generateNPCs();
                            playerInstance.CurrentPosition = new Vector2(32, playerInstance.CurrentPosition.Y);
                            break;
                        case Locations.TOWN:
                            playerInstance.CurrentPosition = new Vector2(graphics.PreferredBackBufferWidth - 16, playerInstance.CurrentPosition.Y);
                            break;
                        case Locations.CROSSWAYS:
                            break;
                        case Locations.SCREEN4:
                            break;
                        case Locations.END:
                            break;
                    }
                }
                else if (playerInstance.CurrentPosition.Y <= 16)
                {
                    switch (current_location)
                    {
                        case Locations.START:
                            current_location = Locations.CROSSWAYS;
                            generateNPCs();
                            playerInstance.CurrentPosition = new Vector2(playerInstance.CurrentPosition.X, graphics.PreferredBackBufferHeight - 16);
                            break;
                        case Locations.TOWN:
                            break;
                        case Locations.CROSSWAYS:
                            break;
                        case Locations.SCREEN4:
                            break;
                        case Locations.END:
                            break;
                    }
                }
                else if (playerInstance.CurrentPosition.Y >= graphics.PreferredBackBufferHeight - 16)
                {
                    switch (current_location)
                    {
                        case Locations.START:
                            playerInstance.CurrentPosition = new Vector2(playerInstance.CurrentPosition.X, graphics.PreferredBackBufferHeight - 16);
                            break;
                        case Locations.TOWN:
                            playerInstance.CurrentPosition = new Vector2(playerInstance.CurrentPosition.X, graphics.PreferredBackBufferHeight - 16);
                            break;
                        case Locations.CROSSWAYS:
                            current_location = Locations.START;
                            generateNPCs();
                            playerInstance.CurrentPosition = new Vector2(playerInstance.CurrentPosition.X, 16);
                            break;
                        case Locations.SCREEN4:
                            break;
                        case Locations.END:
                            break;
                    }
                }
            }
        }
        private void checkPlayerLocalCollisions(Player playerInstance)
        {
            switch (current_location)
            {
                case Locations.START:
                    if (Vector2.Distance(playerInstance.CurrentCenter, new Vector2(townSign_position.X + 16, townSign_position.Y + 16)) <= 32)
                    {
                        playerInstance.CurrentPosition = playerInstance.OldPosition;
                    }
                    break;
                case Locations.TOWN:
                    //if (Vector2.Distance(playerInstance.CurrentCenter,
                    break;
            }
        }
        private void musicManager(GameTime gameTime)
        {
            if (current_state == WorldState.ADVENTURE || current_state == WorldState.INTERACT)
            {
                switch (current_location)
                {
                    case Locations.START:
                    case Locations.CROSSWAYS:
                        if (MediaPlayer.Queue.ActiveSong != overworldTheme)
                            MediaPlayer.Play(overworldTheme);
                        break;
                    case Locations.TOWN:
                        if (MediaPlayer.Queue.ActiveSong != townTheme)
                            MediaPlayer.Play(townTheme);
                        break;
                }
            }
            else
            {
                if (MediaPlayer.Queue.ActiveSong != battleTheme )
                  MediaPlayer.Play(battleTheme);
            }
        }
        private void generateNPCs()
        {
            NPCs.Clear();
            switch (current_location)
            {
                case Locations.START:
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Welcome to Giuseppe's Game POC!"));
                    break;
                case Locations.TOWN:
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "This is a town."));
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Head north, maybe you'll find something useful"));
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Look out for an important item somewhere in this world"));
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Enemies will attack you at random outside towns. Be careful!"));
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "There is a creature causing havok outside the city...I hope everyone will be OK."));
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Oh you seem injured. Let me heal your wounds.")); 
                    break;
                case Locations.CROSSWAYS:
                    NPCs.Add(new NPC(game, graphics, NPCTypes.NPC1, "Ahhck. I don't think I can go further than this")); 
                    break;
            }
            foreach (NPC n in NPCs)
            {
                n.Initialize();
                n.LoadContent();
            }
        }
        public void damageEnemy()
        {
            e.damageEnemy();
        }

        public void clearEnemyActionNotification()
        {
            e.clearEnemyActionNotification();
        }
    }
}
