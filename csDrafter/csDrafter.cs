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
            bool proceed = true;
            while (proceed)
            {
                d.Draft(0);
                d.ResetDraftedPlayers();
                d.FinalTeam = new List<Player>();
                d.deviation++;
            }
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
            //TestTeam.Add(new Player("Tim", 88));
            //TestTeam.Add(new Player("Josh", 88));
            //TestTeam.Add(new Player("Sam", 88));
            //TestTeam.Add(new Player("Zues", 88));
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
        public int completeTeams { get; set; }
        #endregion
        public Drafter(List<Player> playerList, int anyNumTeams)
        {
            deviation = 0;
            Players = playerList;
            numTeams = anyNumTeams;
            playersPerTeam = Players.Count / anyNumTeams;
            averageRank = GetTotalPlayersSkill(Players) / numTeams;
            FinalTeam = new List<Player>();
            completeTeams = 0;
        }

        #region Public Methods

        public List<Player> OrderFinalList(List<Player> playerList)
        {
            Dictionary<int,List<Player>> orderedList = new Dictionary<int,List<Player>>();  //playerList;   
            List<Player> subList = new List<Player>();
            if (playerList.Count > playersPerTeam)
            {
                for (int x = 0; x < FinalTeam.Count; x++)
                {
                    List<Player> subList = new List<Player>();
                    for (int y = 0; y < playersPerTeam; y++)
                    {
                        subList.Add(playerList[x]);
                        x++;
                    }
                    x--;
                    orderedList.Add(subList[0].skill, subList);
                }
            }

            SortedDictionary<int, List<Player>> sortedDict = new SortedDictionary<int, List<Player>>();
       
            playerList = playerList.OrderBy(x => x.skill).ThenBy(x => x.name).ToList<Player>();
            sortedDict.Add(playerList[0].skill, playerList);


            List<Player> finalList = new List<Player>();
            foreach(List<Player> sortedSubList in sortedDict.Values)
            {
                foreach(Player player in sortedSubList)
                {
                    finalList.Add(player);
                }
            }
            
           
            return finalList;
        }
        public List<Player> orderList(List<Player> playerList)
        {
            List<Player> orderedList = playerList;
           // playerList.OrderBy<Player.>
            return orderedList; 
        }

        public void Draft(int i)
        {
            iteration++;
            try
            {
                //i will be -1 when no available players are left, i will be >= count if fair team is impossible
                if (i >= 0 && i < Players.Count)
                {
                    if (isPromisingTeam(i))
                    {
                        //Assign player to team
                        Players[i].isDrafted = true;
                        FinalTeam.Add(Players[i]);

                        //Draft next available
                        Draft(lowestAvail());

                        //Drafting is complete 
                        if (FinalTeam.Count == Players.Count)
                        {
                            PrintTeams();
                        }

                        //Keep back tracking to get all possible combinations draft next
                        Players[i].isDrafted = false;
                        FinalTeam.Remove(FinalTeam[FinalTeam.Count - 1]);
                    }

                    //Player did not fit on team or we back tracked, draft next player 
                    Draft(i + 1);
                }
            }
            catch (Exception ex)
            {

            }
           
        }

        public bool isPromisingTeam(int i)
        {
            bool promising = false;
            try
            {
                //when i is out of bounds, we skip, opposed to non promising we draft next
                //if(i < 0 || i >= Players.Count)
                //{
                    if (!Players[i].isDrafted)
                    {
                        int teamSkill = TeamSkill(i);
                        if ((FinalTeam.Count + 1) % playersPerTeam == 0)
                        {
                            promising = teamSkill >= (averageRank - deviation) && teamSkill <= (averageRank + deviation);
                        }
                        else
                        {
                            promising = teamSkill <= (averageRank - deviation);
                        }
                    }
                //}
            }
            catch(Exception ex){

            }
 
            return promising;
        }


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
                if (!Players[x].isDrafted)
                {
                    lowestAvail = x;
                    break;
                }
            }
            return lowestAvail;
        }

        public void PrintTeams()
        {
            completeTeams++;
            int teamCount = 1;
            for (int i = 0; i < FinalTeam.Count; i++)
            {
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
            Console.WriteLine("Team Set: " + completeTeams);
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
                GetFairness -= Math.Abs(1 - (skillTotal / averageRank));
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
                int goBack = (FinalTeam.Count) % playersPerTeam;
     
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
