﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    /// <summary>
    /// The Level2 Class instantiates a new Game,
    /// creating all the elements on the screen 
    /// necessary to play the game such as the Player and 
    /// enemies. It also creates all the patherns of the 
    /// enemies for level2 of the game.
    /// </summary>
    class Level2
    {
        Window window;
        Canvas canvas;

        int round = 1;
        static DispatcherTimer timerRandomShoot;
        int spaceX = 0;

        Player player;
        bool load;
        SpaceShip[] ships;
        SpaceShip[] ufos;
        Commander[] commanders;
        List<int> shipsNum = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> commandersNum = new List<int>() { 0, 1, 2, 3 };
        List<int> ufosNum = new List<int>() { 0, 1, 2, 3, 4 };
        bool exists1 = false;
        bool exists2 = false;
        bool exists3 = false;
        List<Enemies> enemies;
        Image lv2Pic;
        int level = 2;
        /// <summary>
        /// Level2 Class Constructor. It cosntructs the new window (screen),
        /// a new Canvas and the Player.
        /// </summary>
        /// <param name="window"> A new window to receive all elements fo the game and manage the screen</param>
        /// <param name="canvas">Area within the window which you can position all elements by using coordinates that are relative to the Canvas area.</param>
        /// <param name="player">The main player of the Game</param>
        public Level2(Window window, Canvas canvas, Player player)
        {
            this.window = window;
            this.canvas = canvas;
            this.player = player;
            this.player.updateCurrentLevel(2);
            enemies = new List<Enemies>();

        }
        public Level2(Window window, Canvas canvas, Player player, bool load)
        {
            this.load = load;
            if (load)
            {
                Level2.loadLevel2(canvas, window, player);
            }

            this.window = window;
            this.canvas = canvas;
            this.player = player;
            this.player.updateCurrentLevel(2);
            enemies = new List<Enemies>();
        }
        /// <summary>
        /// The static timerRandom method returns 
        /// a DispatcherTimer Object related to the random shooting.
        /// </summary>
        public static DispatcherTimer timerRandom
        {
            get { return timerRandomShoot; }
        }
       
        /// <summary>
        /// The DisplayLevel method displays an image on the canvas indicating the Level of the game.
        /// </summary>
        void DisplayLevel()
        {
            lv2Pic = new Image();
            lv2Pic.Height = 50;
            lv2Pic.Width = 140;
            this.canvas.Children.Add(lv2Pic);
            Canvas.SetTop(lv2Pic, 200);
            Canvas.SetLeft(lv2Pic, 350);
            lv2Pic.Source = UtilityMethods.LoadImage("pics/level2.png");
        }
        /// <summary>
        /// The Play Method creates and displays all enemies and the player on 
        /// the Canvas (screen).
        /// </summary>
        public async void Play()
        {
            Player.ColdDown = 0;

            if (this.round == 1)
            {
                DisplayLevel();
                await Task.Delay(2000);
                this.canvas.Children.Remove(lv2Pic);
            }
            else
            {
                this.shipsNum = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                this.commandersNum = new List<int>() { 0, 1, 2, 3 };
                this.ufosNum = new List<int>() { 0, 1, 2, 3, 4 };
                this.exists1 = false;
                this.exists2 = false;
                this.exists3 = false;
                this.spaceX = 0;

                await Task.Delay(1500);
            }

            lv2Pic = new Image();
            lv2Pic.Height = 40;
            lv2Pic.Width = 100;
            this.canvas.Children.Add(lv2Pic);
            lv2Pic.Source = UtilityMethods.LoadImage("pics/level2.png");

            //bee creation
            BitmapImage[] beeImages = { UtilityMethods.LoadImage("pics/bee0.png"),
                    UtilityMethods.LoadImage("pics/bee1.png") };
            Image[] beesPic = new Image[8];
            //int spaceX = 0;
            bool isDive = false;
            Bug[] bees = new Bug[beesPic.Length];
            for (int i = 0; i < beesPic.Length; i++)
            {
                beesPic[i] = new Image();
                beesPic[i].Width = 34;
                beesPic[i].Height = 26;
                canvas.Children.Add(beesPic[i]);
                Canvas.SetLeft(beesPic[i], 181 + spaceX);
                Canvas.SetTop(beesPic[i], 185);
                spaceX += 60;

                Point beePos = new Point();
                beePos.X = Canvas.GetLeft(beesPic[i]);
                beePos.Y = Canvas.GetTop(beesPic[i]);
                Animation beeAnimation = new Animation(beesPic[i], beeImages, true);
                Bug bee = new Bug(beePos, beesPic[i], canvas, beeAnimation);
                bees[i] = bee;
                enemies.Add(bees[i]);
                bees[i].setTarget(player);
                bees[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                bees[i].setDiveFrequency(8);
                bees[i].Fly(180);
            }

            //---------------------------------------------------------------------------
            //redbugs creation
            BitmapImage[] bugImages = { UtilityMethods.LoadImage("pics/redBug0.png"),
                    UtilityMethods.LoadImage("pics/redBug1.png") };
            Image[] bugsPic = new Image[9];
            Bug[] redbugs = new Bug[bugsPic.Length];
            spaceX = 0;
            isDive = false;
            for (int i = 0; i < bugsPic.Length; i++)
            {
                bugsPic[i] = new Image();
                bugsPic[i].Width = 34;
                bugsPic[i].Height = 26;
                canvas.Children.Add(bugsPic[i]);
                Canvas.SetLeft(bugsPic[i], 154 + spaceX);
                Canvas.SetTop(bugsPic[i], 145);
                spaceX += 60;

                Point bugPos = new Point();
                bugPos.X = Canvas.GetLeft(bugsPic[i]);
                bugPos.Y = Canvas.GetTop(bugsPic[i]);
                Animation bugAnimation = new Animation(bugsPic[i], bugImages, true);
                Bug bug = new Bug(bugPos, bugsPic[i], canvas, bugAnimation);
                redbugs[i] = bug;
                enemies.Add(redbugs[i]);
                redbugs[i].setTarget(player);
                redbugs[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                redbugs[i].setDiveFrequency(8);
                redbugs[i].setMoveCounter(2);
                redbugs[i].Fly(180);
            }

            //---------------------------------------------------------------------------
            //ships creation
            Image[] shipsPic = new Image[8];
            ships = new SpaceShip[shipsPic.Length];
            isDive = false;
            spaceX = 0;
            for (int i = 0; i < ships.Length; i++)
            {
                shipsPic[i] = new Image();
                shipsPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                shipsPic[i].Width = 34;
                shipsPic[i].Height = 26;
                canvas.Children.Add(shipsPic[i]);
                Canvas.SetLeft(shipsPic[i], 181 + spaceX);
                Canvas.SetTop(shipsPic[i], 105);
                spaceX += 60;

                Point shipPos = new Point();
                shipPos.X = Canvas.GetLeft(shipsPic[i]);
                shipPos.Y = Canvas.GetTop(shipsPic[i]);

                SpaceShip ship = new SpaceShip(shipPos, shipsPic[i], canvas);
                ships[i] = ship;
                enemies.Add(ships[i]);
                ships[i].setTarget(player);
                ships[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                ships[i].setDiveFrequency(8);
                ships[i].Fly(180);
                // ships[i].Shoot(200);
            }




            //---------------------------------------------------------------------------
            //UFO creation
            Image[] ufoPics = new Image[5];
            this.ufos = new SpaceShip[ufoPics.Length];
            spaceX = 0;
            for (int i = 0; i < ufoPics.Length; i++)
            {
                ufoPics[i] = new Image();
                ufoPics[i].Source = UtilityMethods.LoadImage("pics/UFO.png");
                ufoPics[i].Width = 34;
                ufoPics[i].Height = 26;
                canvas.Children.Add(ufoPics[i]);
                Canvas.SetLeft(ufoPics[i], 200 + spaceX);
                Canvas.SetTop(ufoPics[i], 65);
                spaceX += 60;

                Point ufoPos = new Point();
                ufoPos.X = Canvas.GetLeft(ufoPics[i]);
                ufoPos.Y = Canvas.GetTop(ufoPics[i]);

                SpaceShip ufo = new SpaceShip(ufoPos, ufoPics[i], canvas);
                ufos[i] = ufo;
                enemies.Add(ufos[i]);
                ufos[i].setTarget(player);
                ufos[i].setMoveCounter(2);
                ufos[i].Fly(180);
                // ships[i].Shoot(200);
            }



            //---------------------------------------------------------------------------
            //commanders creation
            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commander.png"),
                                        UtilityMethods.LoadImage("pics/commander2.png") };
            Image[] commanderPic = new Image[4];
            commanders = new Commander[commanderPic.Length];
            spaceX = 0;
            for (int i = 0; i < commanderPic.Length; i++)
            {
                commanderPic[i] = new Image();
                //commanderPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                commanderPic[i].Width = 34;
                commanderPic[i].Height = 26;
                canvas.Children.Add(commanderPic[i]);
                Canvas.SetLeft(commanderPic[i], 260 + spaceX);
                Canvas.SetTop(commanderPic[i], 30);
                spaceX += 90;

                Point commanderPos = new Point();
                commanderPos.X = Canvas.GetLeft(commanderPic[i]);
                commanderPos.Y = Canvas.GetTop(commanderPic[i]);
                Animation commanderAnimation = new Animation(commanderPic[i], commanderImages, true);
                Commander commander = new Commander(commanderPos, commanderPic[i], canvas, commanderAnimation);
                commanders[i] = commander;
                enemies.Add(commanders[i]);
                commanders[i].setTarget(player);
                commanders[i].Fly(180);
            }

            player.SetEnemyTarget(enemies);
            StartGame();
        }
        /// <summary>
        /// The StartGame method instantiates a DispatcherTimer Class which 
        /// will be used to control the interval of all enemies shooting execution 
        /// in the game of this specific level.
        /// </summary>
        void StartGame()
        {
            timerRandomShoot = new DispatcherTimer(DispatcherPriority.Normal);
            timerRandomShoot.Interval = TimeSpan.FromSeconds(1);
            timerRandomShoot.Tick += new EventHandler(ShootUpdate);
            timerRandomShoot.Start();
        }
        /// <summary>
        /// The shootUpdate method controls randomly the
        /// shooting of the enemies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShootUpdate(object sender, EventArgs e)
        {
            //-------------------------------
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i].IsDead() == true)
                {
                    shipsNum.Remove(i);
                }
            }

            for (int i = 0; i < commanders.Length; i++)
            {
                if (commanders[i].IsDead() == true)
                {
                    commandersNum.Remove(i);
                }
            }

            for (int i = 0; i < ufos.Length; i++)
            {
                if (ufos[i].IsDead() == true)
                {
                    ufosNum.Remove(i);
                }
            }

            //--------------------------------
            Random rand = new Random();
            int num1 = rand.Next(ships.Length);
            if (shipsNum.Contains(num1))
            {
                shipsNum.Remove(num1);
                exists1 = true;
            }
            if (exists1 == true)
            {
                if (ships[num1].IsDead() == true)
                {
                    shipsNum.Remove(num1);
                }
                else
                {
                    if (ships[num1].isShoot() == false)
                    {
                        ships[num1].Shoot(10);
                    }
                }

                exists1 = false;
            }

            //------------------------------------
            int num2 = rand.Next(commanders.Length);
            if (commandersNum.Contains(num2))
            {
                exists2 = true;
                commandersNum.Remove(num2);
            }
            if (exists2 == true)
            {
                if (commanders[num2].IsDead() == true)
                {
                    commandersNum.Remove(num2);
                }
                else
                {
                    if (commanders[num2].isShoot() == false)
                    {
                        commanders[num2].Shoot(15);
                    }
                }
                exists2 = false;
            }

            //------------------------------------
            int num3 = rand.Next(ufos.Length);
            if (ufosNum.Contains(num3))
            {
                exists3 = true;
                ufosNum.Remove(num3);
            }
            if (exists3 == true)
            {
                if (ufos[num3].IsDead() == true)
                {
                    ufosNum.Remove(num3);
                }
                else
                {
                    if (ufos[num3].isShoot() == false)
                    {
                        ufos[num3].Shoot(5);
                    }
                }
                exists3 = false;
            }

            //----------------------------------------
            if (this.enemies.Count == 0)
            {

                this.canvas.Children.Remove(lv2Pic);

                timerRandomShoot.Stop();
                if (this.round == 1)
                {
                    round++;
                    Play();
                }
                else
                {
                    Level3 lv3 = new Level3(this.window, this.canvas, this.player);
                    lv3.Play();
                }
            }
        }
        public void setRound(int round)
        {
            this.round = round;
        }
        public void setLoad(bool load)
        {
            this.load = load;
        }
        public static void saveLevel2(Player player, bool load)
        {
            String fileName = "GalagaSavedGame.bin";
            BinaryFormatter formatter = null;
            Stream stream = null;
            SerializeGameObj game = null;

            List<int> shipInt = new List<int>();
            List<int> commanderInt = new List<int>();
            List<int> bugInt = new List<int>();

            List<Point> shipPoint = new List<Point>();
            List<Point> commanderPoint = new List<Point>();
            List<Point> bugPoint = new List<Point>();

            List<String> shipPath = new List<string>();
            List<String> commanderPath = new List<string>();
            List<String> bugPath = new List<string>();
            List<double> minXShip = new List<double>();
            List<double> maxXShip = new List<double>();
            List<double> minXCom = new List<double>();
            List<double> maxXCom = new List<double>();
            List<double> minXBug = new List<double>();
            List<double> maxXBug = new List<double>();
            int coins;
            int lives;
            int level;
            if (load)
            {
                 coins = LoadLevels.getStaticPlayer().getCoins();
                 lives = LoadLevels.getStaticPlayer().GetLives();
                 level = LoadLevels.getStaticPlayer().getCurrentLevel();
            }
            else
            {
                 coins = player.getCoins();
                 lives = player.GetLives();
                 level = player.getCurrentLevel();
            }
           


            if (load == false)
            {
                for (int i = 0; i < player.getEnemiesList().Count; i++)
                {
                    if (player.getEnemiesList()[i].GetType() == typeof(SpaceShip))
                    {
                        shipInt.Add(0);
                        shipPoint.Add(player.getEnemiesList()[i].GetPoint());
                        shipPath.Add(player.getEnemiesList()[i].GetImage().Source.ToString());
                        maxXShip.Add(player.getEnemiesList()[i].getMaxX());
                        minXShip.Add(player.getEnemiesList()[i].getMinX());
                    }
                    else if (player.getEnemiesList()[i].GetType() == typeof(Commander))
                    {
                        commanderInt.Add(1);
                        commanderPoint.Add(player.getEnemiesList()[i].GetPoint());
                        commanderPath.Add(player.getEnemiesList()[i].GetImage().Source.ToString());
                        maxXCom.Add(player.getEnemiesList()[i].getMaxX());
                        minXCom.Add(player.getEnemiesList()[i].getMinX());
                    }
                    else if (player.getEnemiesList()[i].GetType() == typeof(Bug))
                    {
                        bugInt.Add(2);
                        bugPoint.Add(player.getEnemiesList()[i].GetPoint());
                        bugPath.Add(player.getEnemiesList()[i].GetImage().Source.ToString());
                        maxXBug.Add(player.getEnemiesList()[i].getMaxX());
                        minXBug.Add(player.getEnemiesList()[i].getMinX());
                    }
                }

            }
            else if (load)
            {
                for (int i = 0; i < LoadLevels.getStaticPlayer().getEnemiesList().Count; i++)
                {
                    if (LoadLevels.getStaticPlayer().getEnemiesList()[i].GetType() == typeof(SpaceShip))
                    {
                        shipInt.Add(0);
                        shipPoint.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetPoint());
                        shipPath.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetImage().Source.ToString());
                        maxXShip.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMaxX());
                        minXShip.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMinX());
                    }
                    else if (LoadLevels.getStaticPlayer().getEnemiesList()[i].GetType() == typeof(Commander))
                    {
                        commanderInt.Add(1);
                        commanderPoint.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetPoint());
                        commanderPath.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetImage().Source.ToString());
                        maxXCom.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMaxX());
                        minXCom.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMinX());
                    }

                    else if (LoadLevels.getStaticPlayer().getEnemiesList()[i].GetType() == typeof(Bug))
                    {
                        bugInt.Add(2);
                        bugPoint.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetPoint());
                        bugPath.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetImage().Source.ToString());
                        maxXBug.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMaxX());
                        minXBug.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMinX());

                    }
                }

            }


            game = new SerializeGameObj(shipInt, shipPoint, shipPath, commanderInt, commanderPoint, commanderPath,
                bugInt, bugPoint, bugPath, minXShip, maxXShip, minXCom, maxXCom, minXBug, maxXBug, coins, lives, level);

            try
            {
                formatter = new BinaryFormatter();
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, game);
                stream.Close();
                MessageBox.Show("Game has been saved");
            }
            catch (SerializationException e)
            {
                MessageBox.Show("An error occured and the current game was not able to be saved.");
                MessageBox.Show(e.Message);
            }
        }



        public static void loadLevel2(Canvas canvas, Window window, Player player)
        {
            String fileName = "GalagaSavedGame.bin";
            BinaryFormatter reader = null;
            Stream stream = null;
            SerializeGameObj game = null;
            List<int> enemieInt = new List<int>();
            List<Point> pointArr = new List<Point>();
            List<String> pathArr = new List<string>();
            List<double> minXShip = new List<double>();
            List<double> maxXShip = new List<double>();
            List<double> minXCom = new List<double>();
            List<double> maxXCom = new List<double>();
            List<double> minXBug = new List<double>();
            List<double> maxXBug = new List<double>();
            int coins = player.getCoins();
            int lives = player.GetLives();
            int level = player.getCurrentLevel();

            try
            {
                reader = new BinaryFormatter();
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                game = (SerializeGameObj)reader.Deserialize(stream);


                LoadLevels loadlvl1 = new LoadLevels(game.GetShipInt, game.GetShipPoint, game.GetShipPath, game.GetCommanderInt,
                       game.GetCommanderPoint, game.GetCommanderPath, game.GetBugInt, game.GetBugPoint, game.GetBugPath,
                        canvas, window, game.GetShipMin, game.GetShipMax, game.GetComMin, game.GetComMax, game.GetBugMin, game.GetBugMax
                        , game.GetCoins, game.GetLives, game.GetLevel);
                player = loadlvl1.getPlayer();
            }
            catch (SerializationException e)
            {
                MessageBox.Show("An error occured and the current game was not able to be LOADED.");
                MessageBox.Show(e.Message);
            }



        }
    }
}
