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
            List<Player> p = TestTeam();
            Drafter d = new Drafter(p, 4);
            
        }
        static List<Player> TestTeam()
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
    }

    class Drafter
    {
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

        public void ResetDraftedPlayers()
        {
            foreach (Player player in Players)
            {
                player.isDrafted = false;
            }
        }

        public int lowestAvail()
        {
            int lowestAvail = -1;
            for (int x = 0; x < Players.Count; x++)
            {
                if (Players[x].isDrafted)
                {
                    lowestAvail = x;
                    break;
                }
            }
            return lowestAvail;
        }

        public void PrintTeams()
        {
            for(int i = 0; i < FinalTeam.Count; i++)
            {
                int teamCount = 1;
                if ((i % playersPerTeam) == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("-----------");
                    Console.WriteLine("TEAM " + teamCount);
                    Console.WriteLine("-----------");
                    teamCount++;
                }
                Console.WriteLine(FinalTeam[i].name);
            }
            //Console.WriteLine()
            //Console.WriteLine("Fairness: " & GetFairness(FinalTeam, PlayersPerTeam) & "%")
            //bPrintedTeams = True
        }
        public decimal GetFairness()
        {
           decimal GetFairness = 100;
           List<int> allTeamsSkills = new List<int>();
            for (int x = 0; x < FinalTeam.Count; x++)
            {
                int teamSkill = 0;
                for (int y = 0; y < playersPerTeam; y++)
                {
                    teamSkill += FinalTeam[x].skill;
                    x++;
                }
                x--;
                allTeamsSkills.Add(teamSkill);
            }
            foreach (int skillTotal in allTeamsSkills)
            {
                GetFairness -= Math.Abs(1 - (skillTotal / averageRank)) * 100;
            } 
           return GetFairness;
        }
        public int TeamSkill(int i)
        {
            //Player is not on final team yet
            int TeamSkill = Players[i].skill;
            try
            {
                //minus 1 because current player is already accounted for 
                int goBack = (FinalTeam.Count - 1) % playersPerTeam;
     
                //Why do we need this
                //if (goBack < 0) //Player is first
                //{
                //    goBack = playersPerTeam - 1;
                //}
                for(int j = 0; j < goBack; j++)
                {
                    TeamSkill += FinalTeam[FinalTeam.Count - 1 - j].skill;
                }
     
            }
            catch (Exception ex)
            {

            }
            return TeamSkill;
        }

      
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
