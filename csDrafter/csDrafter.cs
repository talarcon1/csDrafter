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
            //test ordering the list to remove dups


            bool proceed = true;
            while (proceed)
            {
                d.Draft(0);
                d.ResetDraftedPlayers();
                d.FinalTeam = new List<Player>();
                d.deviation++;
                if(d.completeTeams > 20)
                {
                    proceed = false;
                }
            }
        }
        static List<Player> TestTeam()
        {
            List<Player> TestTeam = new List<Player>();
            TestTeam.Add(new Player("Ric", 67, "a"));
            TestTeam.Add(new Player("Tico", 72, "b"));
            TestTeam.Add(new Player("Jr", 74, "c"));
            TestTeam.Add(new Player("Fur", 77,"d"));
            TestTeam.Add(new Player("Jamie", 86,"e"));
            TestTeam.Add(new Player("Danny", 76,"f"));
            TestTeam.Add(new Player("Gab", 79,"g"));
            TestTeam.Add(new Player("X", 86,"h"));
            TestTeam.Add(new Player("Tim", 87,"i"));
            TestTeam.Add(new Player("Josh", 88,"j"));
            TestTeam.Add(new Player("Sam", 91,"k"));
            TestTeam.Add(new Player("Zues", 84,"l"));
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

        public List<String> TeamIds { get; set; }
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
            TeamIds = new List<String>();
        }

        #region Public Methods

        public string GetTotalTeamId(List<Player> playerList)
        //public Dictionary<string, List<Player>> OrderFinalList(List<Player> playerList)
        {
            try
            {

           

            List<List<Player>> teamList = new List<List<Player>>();

            
           //Create sublists for each team
            if (playerList.Count > playersPerTeam)
            {
                for (int x = 0; x < playerList.Count; x++)
                {
                    List<Player> subList = new List<Player>();
                    for (int y = 0; y < playersPerTeam; y++)
                    {
                        subList.Add(playerList[x]);
                        x++;
                    }
                    x--;
                   teamList.Add(subList);
                }
            }

            //order each player in each team by their id
            Dictionary<string, List<Player>> orderedList = new Dictionary<string, List<Player>>();  //playerList;  
            foreach (List<Player> rawTeam in teamList)
            {
                List<Player> orderedTeam = new List<Player>();
                orderedTeam = rawTeam.OrderBy(x => x.id).ToList<Player>();//x.skill).ThenBy(x => x.name).ToList<Player>();

                //get total team id
                string teamId = "";
                foreach (Player player in orderedTeam)
                {
                    teamId += player.id;
                }
                orderedList.Add(teamId, orderedTeam);
            }

            //Get the total team ids and order them 
            List<string> keyList = new List<string>();
            foreach (string teamId in orderedList.Keys)
            {
                keyList.Add(teamId);
            }
            keyList = keyList.OrderBy(x => x).ToList<String>();

            //Order the teams by their team id
            //This step is unnecessary but it does order lists for printing
            // SortedDictionary<string, List<Player>> sortedDict = new SortedDictionary<string, List<Player>>();
            //foreach(string teamId in keyList)
            // {
            //     sortedDict.Add(teamId, orderedList[teamId]);
            // }

            //Take the sorted teams which are broken into a dictionary, and put into a list
            //List<Player> finalList = new List<Player>();
            //foreach(List<Player> sortedSubList in sortedDict.Values)
            //{
            //    foreach (Player player in sortedSubList)
            //    {
            //        finalList.Add(player);
            //    }
            //}

            string totalId = "";
            foreach(string teamId in keyList)
            {
                totalId += teamId;
            }
       
            //create a 1 entry dictionary with the final ordered list with the total team id as the key
            //Dictionary<string, List<Player>> finalDict = new Dictionary<string, List<Player>>();
            //finalDict.Add(,finalList);

            return totalId;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                            String teamId = (GetTotalTeamId(FinalTeam));
                            if (!TeamIds.Contains(teamId))
                            {
                                completeTeams++;
                                TeamIds.Add(teamId);
                                PrintTeams();
                            } 
                            

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

        public string id { get; set; }

        public Player(string anyName, int anySkill, string anyId)
            {
            this.name = anyName;
            this.skill = anySkill;
            this.isDrafted = false;
            this.id = anyId;
            }
    }
}
