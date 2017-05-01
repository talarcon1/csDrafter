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
            Drafter d = new Drafter(p,2);


            d.sentinel = 100;


            int percent = 50;
            
            
            int deviation = d.averageRank * percent / 100;
            if (deviation == 0)
            {
               deviation = 1;
            }else if(deviation < 0)
            {
                deviation = int.MaxValue - d.averageRank -1;
            }

            d.maxDeviation = deviation;


            //d.deviation = 0;// deviation;
            ////@TA TODO what find the max amount of teams
            //while (d.completeTeams < d.sentinel && d.deviation <= d.maxDeviation)
            //{
            //    d.Draft(0);
            //    d.deviation++;
            //}

            d.deviation =  deviation; 
            d.Draft(0);

            //because we start with deviation 0 and increase, 
            d.CompletedSets = d.CompletedSets.OrderByDescending(x => x.totalFairness).ToList<TeamSet>();
            d.finalPrint(d.CompletedSets);



            //d.ResetDraftedPlayers();
            //    d.FinalTeam = new List<Player>();
            //d.deviation += deviation;
            //}
            Console.WriteLine("done");
            Console.ReadLine();
        }
        static List<Player> TestTeam()
        {
            List<Player> TestTeam = new List<Player>();
            //TestTeam.Add(new Player("Ric", 67, "a"));
            //TestTeam.Add(new Player("Tico", 72, "b"));
            //TestTeam.Add(new Player("Jr", 74, "c"));
            //TestTeam.Add(new Player("Fur", 77, "d"));
            //TestTeam.Add(new Player("Jamie", 86, "e"));
            //TestTeam.Add(new Player("Danny", 76, "f"));
            //TestTeam.Add(new Player("Gab", 79, "g"));
            //TestTeam.Add(new Player("X", 86, "h"));
            //TestTeam.Add(new Player("Tim", 87, "i"));
            //TestTeam.Add(new Player("Josh", 88, "j"));
            //TestTeam.Add(new Player("Sam", 91, "k"));
            //TestTeam.Add(new Player("Zues", 84, "l"));


            TestTeam.Add(new Player("Ric", 1, "a"));
            TestTeam.Add(new Player("Tico", 1, "b"));
            TestTeam.Add(new Player("Jr", 1, "c"));
            TestTeam.Add(new Player("Fur", 3, "d"));
            TestTeam.Add(new Player("Jamie", 3, "e"));
            TestTeam.Add(new Player("Danny", 3, "f"));
            //TestTeam.Add(new Player("Gab", 7, "g"));
            //TestTeam.Add(new Player("X", 5, "h"));
            //TestTeam.Add(new Player("Tim", 11, "i"));
            //TestTeam.Add(new Player("Josh", 6, "j"));
            //TestTeam.Add(new Player("Sam", 8, "k"));
            //TestTeam.Add(new Player("Zues", 10, "l"));


            return TestTeam;
        }
    }

    class Drafter
    {
        #region Fields
        public List<TeamSet> CompletedSets { get; set; }
        //public Dictionary<string,TeamSet> CompletedSets { get; set; }
        public int deviation { get; set; }
        public int maxDeviation { get; set; }
        public int numPlayers { get; set; }
        public int numTeams { get; set; }
        public int averageRank { get; set; }
        public List<Player> Players { get; set; }
        public List<Player> FinalTeam { get; set; }
        public int playersPerTeam { get; set; }
        public int iteration { get; set; }
        public int completeTeams { get; set; }

        public List<String> TeamIds { get; set; }
        public int sentinel { get; set; }
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
            sentinel = 0;
            CompletedSets = new List<TeamSet>();
            //CompletedSets = new Dictionary<string, TeamSet>();
        }

        #region Public Methods

        //public string GetTotalTeamId(List<Team> teamList)
        ////public Dictionary<string, List<Player>> OrderFinalList(List<Player> playerList)
        //{
        //    try
        //    {


        //        //order each player in each team by their id
        //        Dictionary<string, List<Player>> orderedList = new Dictionary<string, List<Player>>();  //playerList;  
        //        foreach (Team rawTeam in teamList)
        //        {
        //            List<Player> orderedTeam = new List<Player>();
        //            orderedTeam = rawTeam.PlayerList.OrderBy(x => x.id).ToList<Player>();//x.skill).ThenBy(x => x.name).ToList<Player>();

        //            //get total team id
        //            string teamId = "";
        //            foreach (Player player in orderedTeam)
        //            {
        //                teamId += player.id;
        //            }
        //            orderedList.Add(teamId, orderedTeam);
        //        }

        //        //Get the total team ids and order them 
        //        List<string> keyList = new List<string>();
        //        foreach (string teamId in orderedList.Keys)
        //        {
        //            keyList.Add(teamId);
        //        }
        //        keyList = keyList.OrderBy(x => x).ToList<String>();

        //        //Order the teams by their team id
        //        //This step is unnecessary but it does order lists for printing
        //        // SortedDictionary<string, List<Player>> sortedDict = new SortedDictionary<string, List<Player>>();
        //        //foreach(string teamId in keyList)
        //        // {
        //        //     sortedDict.Add(teamId, orderedList[teamId]);
        //        // }

        //        //Take the sorted teams which are broken into a dictionary, and put into a list
        //        //List<Player> finalList = new List<Player>();
        //        //foreach(List<Player> sortedSubList in sortedDict.Values)
        //        //{
        //        //    foreach (Player player in sortedSubList)
        //        //    {
        //        //        finalList.Add(player);
        //        //    }
        //        //}

        //        string totalId = "";
        //        foreach (string teamId in keyList)
        //        {
        //            totalId += teamId;
        //        }

        //        //create a 1 entry dictionary with the final ordered list with the total team id as the key
        //        //Dictionary<string, List<Player>> finalDict = new Dictionary<string, List<Player>>();
        //        //finalDict.Add(,finalList);

        //        return totalId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

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
                foreach (string teamId in keyList)
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
                            if (completeTeams >= sentinel || deviation > maxDeviation )
                            {
                                return;
                            }
                            String teamId = GetTotalTeamId(FinalTeam);
                            if (!TeamIds.Contains(teamId))
                            {
                                //TeamSet finalSet =  new TeamSet(FinalTeam, playersPerTeam, averageRank, teamId);
                                //String teamId = GetTotalTeamId(finalSet.TeamList);
                                //finalSet.teamId = teamId;

                                try
                                {
                                    TeamIds.Add(teamId);
                                    //@TA NOTE using a dictionary slows the process exponentially
                                    //team id is key, this will not allow duplicates
                                    CompletedSets.Add(new TeamSet(FinalTeam, playersPerTeam, averageRank, teamId));
                                    //CompletedSets.Add(teamId, finalSet);
                                    completeTeams++;
                                   // PrintTeams();
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                          
                                //PrintTeams();
                                //PrintTeamsOdds();
                            
                            

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
                            promising = teamSkill <= (averageRank + deviation) ;
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
        public void finalPrint(List<TeamSet> teamList)
        {
            TeamSet teamSet = null;
            for (int h = 0; h < teamList.Count || h > sentinel; h++)
            {
                teamSet = teamList[h];
                for (int i = 0; i < teamSet.TeamList.Count; i++)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("  TEAM " + (i + 1) + " " + teamSet.TeamList[i].odds);
                    Console.WriteLine("--------------------");
                    for (int j = 0; j < teamSet.TeamList[i].PlayerList.Count; j++)
                    {
                        Console.WriteLine(teamSet.TeamList[i].PlayerList[j].name); //+ " " + teamSet.TeamList[i].PlayerList[j].skill);
                    }
                    //Console.WriteLine();
                    //Console.WriteLine("Odds: " + teamSet.TeamList[i].odds);
                    Console.WriteLine();
                   
                }
                Console.WriteLine("Team Set: " + (h + 1));
                Console.WriteLine("Fairness: " + teamSet.totalFairness.ToString("#.00") + "%");
                Console.WriteLine();
                Console.WriteLine(".....................................");
                Console.WriteLine();
            }
            
        }
        public void PrintTeams()
        {
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
            Console.WriteLine();
            Console.WriteLine("Team Set: " + completeTeams);
            //Console.WriteLine()


            // Console.WriteLine("Fairness: " + GetFairness() + "%");
            ////bPrintedTeams = True
        }

        public void PrintTeamsOdds()
        {
            //int teamCount = 1;
            //for (int i = 0; i < FinalTeam.Count; i++)
            //{
            //    if ((i % playersPerTeam) == 0)
            //    {
            //        Console.WriteLine();
            //        Console.WriteLine("-----------");
            //        Console.WriteLine("TEAM " + teamCount);
            //        Console.WriteLine("-----------");
            //        teamCount++;
            //    }
            //    Console.WriteLine(FinalTeam[i].name);
            //}
            //Console.WriteLine();
            //Console.WriteLine("Team Set: " + completeTeams);
            ////Console.WriteLine()

            ////bPrintedTeams = True

            //List<int> allTeamsSkills = new List<int>();
            //for (int x = 0; x < FinalTeam.Count; x++)
            //{
            //    int teamSkill = 0;
            //    for (int y = 0; y < playersPerTeam; y++)
            //    {
            //        teamSkill += FinalTeam[x].skill;
            //        x++;
            //    }

            //    x--;
            //    allTeamsSkills.Add(teamSkill);
            //}
            //for(int y = 0; y < allTeamsSkills.Count; y++)
            //{
            //    Console.WriteLine("Team " + (y+1) + ": " + GetOdds(allTeamsSkills[y]));
            //}
           
        }
        //public String GetFairness()
        //{
        //    decimal GetFairness = 100;
        //    List<int> allTeamsSkills = new List<int>();
        //    for (int x = 0; x < FinalTeam.Count; x++)
        //    {
        //        int teamSkill = 0;
        //        for (int y = 0; y < playersPerTeam; y++)
        //        {
        //            teamSkill += FinalTeam[x].skill;
        //            x++;
        //        }
        //        x--;
        //        allTeamsSkills.Add(teamSkill);
        //    }
        //    //Total percent of fairness
        //    foreach (int skillTotal in allTeamsSkills)
        //    {
        //        GetFairness -= (Math.Abs(1 - (decimal)skillTotal / averageRank)) * 100;
        //    }
        //    return GetFairness.ToString("#.00");
        //}

        //public string GetOdds(int skillTotal)
        //{
        //        decimal odds;
           
        //        odds = (decimal)skillTotal / averageRank;
        //        if (odds >= 1)
        //        {
        //            odds *= 100;
        //           return odds.ToString("-###.00");
        //        }
        //        else
        //        {
        //            odds = 100 + (1 - odds);
        //            return odds.ToString("+###.00");

        //        }
            
        //}
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

    class TeamSet
    {
        //should this be dictionary of team list with fairness attached
        // public Dictionary<int,List<Player>> TeamDict{ get; set; }
        public List<Team> TeamList { get; set; }
        public string teamId { get; set; }

        public List<Player> FinalList { get; set; }

        public decimal totalFairness { get; set; }

        public int playersPerTeam { get; set; }

        public int averageRank { get; set; }

        public TeamSet(List<Player> finalList, int playersPerTeam, int averageRank, string teamId)
        {

            this.FinalList = finalList;
            this.playersPerTeam = playersPerTeam;
            this.averageRank = averageRank;
            this.teamId = teamId;
            //key - fairness , value - break final list into teams


            //separate final list into teams
            //this.TeamDict = CreateTeamDict(FinalList);
            this.TeamList = CreateTeamList(finalList);
            SetTeamOdds(this.TeamList);
            this.totalFairness = GetFairness(this.TeamList);

        }

        public List<Team> CreateTeamList(List<Player> finalTeam)
        {
            List<Team> finalList = new List<Team>();

            try
            {
                //int totalSkill;
                List<Player> team;
                for (int x = 0; x < finalTeam.Count; x++)
                {
                    team = new List<Player>();
                    //totalSkill = 0;
                    for (int y = 0; y < playersPerTeam; y++)
                    {
                        //totalSkill += finalTeam[x].skill;
                        team.Add(finalTeam[x]);
                        x++;
                    }
                    x--;
                   
                   team = team.OrderByDescending(y => y.skill).ToList<Player>();
                   finalList.Add(new Team(team, averageRank));
                }
            }
            catch (Exception ex)
            {
               
            }
            return finalList;
        }
    



        public void SetTeamOdds(List<Team> teams)
        {
            foreach(Team team in teams)
            {
                team.odds = GetOdds(team.totalSkill, averageRank);
            }
        }
        public String GetOdds(int skillTotal, int averageRank)
        {
            //@TA TODO odd calculator should be team/team not team/average
            decimal odds;

            odds = (decimal)skillTotal / averageRank;
            if (odds >= 1)
            {
                odds *= 100;
                return odds.ToString("-###.00");
            }
            else
            {
                odds = 100 + ((1 - odds) * 100);
                return odds.ToString("+###.00");

            }

        }
        public decimal GetFairness(List<Team> teams)
        {
            decimal GetFairness = 100;
            
            //Total percent of fairness
            foreach (Team team in teams)
            {
                decimal test = (decimal)team.totalSkill / averageRank;
                test = 1 - test;
                GetFairness -= (Math.Abs(1 - (decimal)team.totalSkill / averageRank)) * 100;
            }
            return GetFairness;
        }

    }

    class Team
    {
        public List<Player> PlayerList {get; set;}
        public int totalSkill { get; set; }
        public String odds { get; set; }
        public int averageRank { get; set; }

        public int playersPerTeam { get; set; }

        public Team(List<Player> PlayerList , int averageRank)
        {
            this.PlayerList = PlayerList;
            this.averageRank = averageRank;
            this.playersPerTeam = PlayerList.Count;
            this.totalSkill = GetTotalSkill(PlayerList);

        }

        public int GetTotalSkill(List<Player> PlayerList)
        {
            int finalTotal = 0;
            foreach(Player player in PlayerList)
            {
                finalTotal += player.skill;
            }
            return finalTotal;
        }

        public void Add(Player player)
        {
            this.PlayerList.Add(player);
        }

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
