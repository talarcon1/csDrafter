using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csDrafter
{
    class csDrafter
    {
        static void Main(string[] args)
        {

        }
    }

    class Drafter
    {
        public Drafter(List<Player> playerList, int anyNumTeams)
        {
            deviation = 0;
            Players = playerList;
            numTeams = anyNumTeams;
            playersPerTeam = Players.Count / anyNumTeams;
            averageRank = GetTotalPlayersSkill(Players) / numTeams;
        }

        #region Public Methods
        public int GetTotalPlayersSkill(List<Player> playerList)
        {
            int totalSkill = 0;
            foreach (Player player in playerList)
            {
                totalSkill += player.skill;
            } 
            return totalSkill;
        }

        public List<Player> TestTeam()
        {
            List<Player> TestTeam = new List<Player>();
            TestTeam.Add(new Player("Ric", 67));
            TestTeam.Add(new Player("Tico", 72));
            TestTeam.Add(new Player("Jr", 74));
            TestTeam.Add(new Player("Fur", 77));
            TestTeam.Add(new Player("Jamie", 86));
            TestTeam.Add(new Player("Danny", 76));
            TestTeam.Add(new Player("Gab", 79));
            TestTeam.Add(new Player("X", 86));
            TestTeam.Add(new Player("Tim", 87));
            TestTeam.Add(new Player("Josh", 88));
            TestTeam.Add(new Player("Sam", 91));
            TestTeam.Add(new Player("Zues", 84));
            return TestTeam;
        }
        #endregion

        #region Fields
        public int deviation { get; set; }
        public int numPlayers { get; set; }
        public int numTeams { get; set; }
        public int averageRank { get; set; }
        public List<Player> Players { get; set; }
        public List<Player> FinalTeam { get; set; }
        public int playersPerTeam { get; set; }
        public int iteration { get; set; }
        #endregion
     
    }

    class Player
    {
        public bool isDrafted { get; set; }
        public int skill { get; set; }
        public string name { get; set; }

        public Player(string anyName, int anySkill)
            {
            this.name = anyName;
            this.skill = anySkill;
            this.isDrafted = false;
            }
    }
}
