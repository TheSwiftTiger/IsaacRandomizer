using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;

namespace IsaacRandomizer
{

    public static class RandomNameGenerator
    {

        static string[] adjTxtFile = File.ReadAllLines(@"Rec\28K_adjectives.txt");

        static string[] txtFile = File.ReadAllLines(@"Rec\91K_nouns.txt");

        public static List<string> names = new List<string> { "Cricket", "Mom", "Tammy", "Bob", "Doctor", "Monstro", "Loki", "Dad", "Guppy", "Max", "Isaac", "Maggie", "Cain", "Judas", "???", "Eve", "Samson", "Azazel", "Lazarus", "Eden" };

        static string[] verbTxtFile = File.ReadAllLines(@"Rec\31K_verbs.txt");

        static string[] bossNouns = File.ReadAllLines(@"Rec\BossNouns.txt");

        static string[] bossAdjectives = File.ReadAllLines(@"Rec\BossAdjectives.txt");

        static string[] bossVerbs = File.ReadAllLines(@"Rec\BossVerbs.txt");

        static string[] consistentNouns = File.ReadAllLines(@"Rec\ItemNouns.txt");

        static string[] consistentAdjectives = File.ReadAllLines(@"Rec\ItemAdjectives.txt");

        static string[] biblicalNames = File.ReadAllLines(@"Rec\BiblicalNames.txt");

        public static string RandomNoun()
        {
            return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
        }
        public static string RandomAdjective()
        {
            return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)];
        }
        public static string RandomVerb()
        {
            return verbTxtFile[RandomNumberGenerator.RandomInt(0, verbTxtFile.Length - 1)];
        }
        public static string RandomItemName()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 100);

            if (rand < 20)
            {
                return names[RandomNumberGenerator.RandomInt(0, names.Count - 1)] + "'s " + adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 40)
            {
                return names[RandomNumberGenerator.RandomInt(0, names.Count - 1)] + "'s " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 60)
            {
                return "The " + adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 80)
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " of " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + "s";
            }

        }

        public static string RandomItemNameConsistent()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 100);

            if (rand < 20)
            {
                return names[RandomNumberGenerator.RandomInt(0, names.Count - 1)] + "'s " + consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)];
            }
            else if (rand < 40)
            {
                return names[RandomNumberGenerator.RandomInt(0, names.Count - 1)] + "'s " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)];
            }
            else if (rand < 60)
            {
                return "The " + consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)];
            }
            else if (rand < 80)
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)];
            }
            else
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " of " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + "s";
            }
        }

        public static string RandomFamiliarName()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 100);

            if (rand < 25)
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " Baby";
            }
            else if (rand < 50)
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " Friend";
            }
            else if (rand < 75)
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " Baby";
            }
            else
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " Friend";
            }
        }

        public static string RandomFamiliarNameConsistent()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 100);

            if (rand < 25)
            {
                return consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " Baby";
            }
            else if (rand < 50)
            {
                return consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " Friend";
            }
            else if (rand < 75)
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " Baby";
            }
            else
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " Friend";
            }
        }

        public static string RandomItemDescription()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 10);

            if (rand == 1)
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " down, " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " way up";
            }
            else if (rand <= 5)
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " up";
            }
            else if (rand <= 9)
            {
                return "You feel " + adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + "...";
            }
            else return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + "s";
        }

        public static string RandomItemDescriptionConsistent()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 10);

            if (rand == 1)
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " down, " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " way up";
            }
            else if (rand <= 5)
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " up";
            }
            else if (rand <= 9)
            {
                return "You feel " + consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + "...";
            }
            else return consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + "s";
        }

        public static string RandomActiveDescription()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 10);

            if (rand == 1)
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " at a cost";
            }
            else if (rand <= 5)
            {
                return "Reusable " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else
            {
                return verbTxtFile[RandomNumberGenerator.RandomInt(0, verbTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + "s";
            }
        }

        public static string RandomActiveDescriptionConsistent()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 10);

            if (rand == 1)
            {
                return consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " at a cost";
            }
            else if (rand <= 5)
            {
                return "Reusable " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)];
            }
            else
            {
                return verbTxtFile[RandomNumberGenerator.RandomInt(0, verbTxtFile.Length - 1)] + " " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + "s";
            }
        }

        public static string RandomFamiliarDescription()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 10);

            if (rand == 1)
            {
                return "He wants your " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand <= 5)
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " buddy";
            }
            else
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " buddy";
            }
        }

        public static string RandomFamiliarDescriptionConsistent()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 10);

            if (rand == 1)
            {
                return "He wants your " + consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)];
            }
            else if (rand <= 5)
            {
                return consistentAdjectives[RandomNumberGenerator.RandomInt(0, consistentAdjectives.Length - 1)] + " buddy";
            }
            else
            {
                return consistentNouns[RandomNumberGenerator.RandomInt(0, consistentNouns.Length - 1)] + " buddy";
            }
        }

        private static bool isEdgy(string word, string type)
        {
            switch (type)
            {
                case "noun":
                    return (Array.IndexOf(bossNouns, word) >= 0 && Array.IndexOf(bossNouns, word) <= 61);
                case "verb":
                    return (Array.IndexOf(bossVerbs, word) >= 0 && Array.IndexOf(bossVerbs, word) <= 16);
                case "adjective":
                    return (Array.IndexOf(bossAdjectives, word) >= 0 && Array.IndexOf(bossAdjectives, word) <= 61);
                default:
                    return false;
            }
        }

        private static bool isOriginal(string word, string type)
        {
            switch (type)
            {
                case "noun":
                    return (Array.IndexOf(bossNouns, word) > 61 && Array.IndexOf(bossNouns, word) <= 127);
                case "verb":
                    return false;
                case "adjective":
                    return (Array.IndexOf(bossAdjectives, word) > 61 && Array.IndexOf(bossAdjectives, word) <= 107);
                default:
                    return false;
            }
        }

        private static bool isJoke(string word, string type)
        {
            switch (type)
            {
                case "noun":
                    return (Array.IndexOf(bossNouns, word) > 127);
                case "verb":
                    return (Array.IndexOf(bossVerbs, word) > 16);
                case "adjective":
                    return (Array.IndexOf(bossAdjectives, word) > 107);
                default:
                    return false;
            }
        }

        public static bool isValid(string word, string type, bool joke, bool edgy, bool original)
        {
            if ((isEdgy(word, type) && !edgy) || (isOriginal(word, type) && !original) || ((isJoke(word, type) && !joke) || RandomNumberGenerator.RandomInt(0, 1) != 0))
            {
                return false;
            }
            return true;
        }

        public static string RandomBossAdjective()
        {
            int rng = RandomNumberGenerator.RandomInt(0, bossAdjectives.Length - 1);
            return bossAdjectives[rng];
        }

        public static string RandomBossNoun()
        {
            int rng = RandomNumberGenerator.RandomInt(0, bossNouns.Length - 1);

            return bossNouns[rng];
        }

        public static string RandomBossVerb()
        {
            int rng = RandomNumberGenerator.RandomInt(0, bossVerbs.Length - 1);
            return bossVerbs[rng];
        }

        public static string RandomName()
        {
            return names[RandomNumberGenerator.RandomInt(0, names.Count - 1)];
        }

        public static string RandomBossNameConsistent(bool joke, bool edgy, bool original)
        {
            
            if (!joke && !edgy && !original)
            {
                return "Did not work";
            }
            var rand = RandomNumberGenerator.RandomInt(1, 54);

            if (rand == 1)
            {
                var noun = RandomBossNoun();
                while (!isValid(noun, "noun", joke, edgy, original))
                {
                    noun = RandomBossNoun();
                }
                return "The " + noun;
            }
            else if (rand < 3)
            {
                var adj = RandomBossAdjective();
                var noun = RandomBossNoun();
                while (!isValid(adj, "adjective", joke, edgy, original))
                {
                    adj = RandomBossAdjective();
                }
                while (!isValid(noun, "noun", joke, edgy, original))
                {
                    noun = RandomBossNoun();
                }
                return "The " + adj + " " + noun;
            }
            else if (rand < 10)
            {
                var adj = RandomBossAdjective();
                var verb = RandomBossVerb();
                while (!isValid(adj, "adjective", joke, edgy, original))
                {
                    adj = RandomBossAdjective();
                }
                while (!isValid(verb, "verb", joke, edgy, original))
                {
                    verb = RandomBossVerb();
                }
                return adj + " " + verb;
            }
            else if (rand < 12)
            {
                var adj = RandomBossAdjective();
                while (!isValid(adj, "adjective", joke, edgy, original))
                {
                    adj = RandomBossAdjective();
                }
                return "The " + adj;
            }
            else if (rand < 15)
            {
                var noun1 = RandomBossNoun();
                var noun2 = RandomBossNoun();
                while (!isValid(noun1, "noun", joke, edgy, original))
                {
                    noun1 = RandomBossNoun();
                }
                while (!isValid(noun2, "noun", joke, edgy, original))
                {
                    noun2 = RandomBossNoun();
                }
                return noun1 + " of " + noun2;
            }
            else if (rand < 25)
            {
                var noun1 = RandomBossNoun();
                var noun2 = RandomBossNoun();
                while (!isValid(noun1, "noun", joke, edgy, original))
                {
                    noun1 = RandomBossNoun();
                }
                while (!isValid(noun2, "noun", joke, edgy, original))
                {
                    noun2 = RandomBossNoun();
                }
                return noun1 + "'s " + noun2;
            }
            else if (rand < 28)
            {
                var verb = RandomBossVerb();
                var noun = RandomBossNoun();
                while (!isValid(verb, "verb", joke, edgy, original))
                {
                    verb = RandomBossVerb();
                }
                while (!isValid(noun, "noun", joke, edgy, original))
                {
                    noun = RandomBossNoun();
                }
                return verb + " of " + noun;
            }
            else if (rand < 34)
            {
                var adj = RandomBossAdjective();
                var verb = RandomBossVerb();
                while (!isValid(adj, "adjective", joke, edgy, original))
                {
                    adj = RandomBossAdjective();
                }
                while (!isValid(verb, "verb", joke, edgy, original))
                {
                    verb = RandomBossVerb();
                }
                return adj + " " + verb;
            }
            else if (rand < 36)
            {
                var rand2 = RandomNumberGenerator.RandomInt(40, 44);
                if (rand2 < 40)
                {
                    var name = RandomName();
                    return "The " + name;
                }
                else if (rand2 < 41)
                {
                    var adj = RandomBossAdjective();
                    var noun = RandomName();
                    while (!isValid(adj, "adjective", joke, edgy, original))
                    {
                        adj = RandomBossAdjective();
                    }
                    return "The " + adj + " " + noun;
                }
                else if (rand2 < 42)
                {
                    var noun1 = RandomBossNoun();
                    var noun2 = RandomName();
                    while (!isValid(noun1, "noun", joke, edgy, original))
                    {
                        noun1 = RandomBossNoun();
                    }
                    return noun1 + " of " + noun2;
                }
                else if (rand2 < 43)
                {
                    var noun1 = RandomName();
                    var noun2 = RandomBossNoun();
                    while (!isValid(noun2, "noun", joke, edgy, original))
                    {
                        noun2 = RandomBossNoun();
                    }
                    return noun1 + "'s " + noun2;
                }
                else
                {
                    var verb = RandomBossVerb();
                    var noun = RandomName();
                    while (!isValid(verb, "verb", joke, edgy, original))
                    {
                        verb = RandomBossVerb();
                    }
                    return verb + " of " + noun;
                }
            }
            
            else
            {
                var adj = RandomBossAdjective();
                var noun1 = RandomBossNoun();
                var noun2 = RandomBossNoun();
                while (!isValid(adj, "adjective", joke, edgy, original))
                {
                    adj = RandomBossAdjective();
                }
                while (!isValid(noun1, "noun", joke, edgy, original))
                {
                    noun1 = RandomBossNoun();
                }
                while (!isValid(noun2, "noun", joke, edgy, original))
                {
                    noun2 = RandomBossNoun();
                }
                return adj + " " + noun1 + "-" + noun2;
            }
        }

        public static string RandomBossNameRandom()
        {
            var rand = RandomNumberGenerator.RandomInt(1, 54);

            if (rand == 1)
            {
                return "The " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 3)
            {
                return "The " + adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 10)
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 12)
            {
                return "The " + adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)];
            }
            else if (rand < 15)
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + " of " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 25)
            {
                return txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + "'s " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 28)
            {
                return verbTxtFile[RandomNumberGenerator.RandomInt(0, verbTxtFile.Length - 1)] + " of " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
            else if (rand < 34)
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + verbTxtFile[RandomNumberGenerator.RandomInt(0, verbTxtFile.Length - 1)];
            }
            else if (rand < 40)
            {
                return "The " + RandomName();
            }
            else if (rand < 41)
            {
                return "The " + RandomAdjective() + " " + RandomName();
            }
            else if (rand < 42)
            {
                return RandomNoun() + " of " + RandomName();
            }
            else if (rand < 43)
            {
                return RandomName() + "'s " + RandomNoun();
            }
            else if (rand < 44)
            {
                return RandomVerb() + " of " + RandomName();
            }
            else
            {
                return adjTxtFile[RandomNumberGenerator.RandomInt(0, adjTxtFile.Length - 1)] + " " + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)] + "-" + txtFile[RandomNumberGenerator.RandomInt(0, txtFile.Length - 1)];
            }
        }

        public static string RandomPill()
        { 
            var rnd = new Random();
            var nouns = new List<string>();
            foreach (var s in txtFile)
            {
                nouns.Add(s);
            }

            string noun = nouns[RandomNumberGenerator.RandomInt(0, nouns.Count - 1)];

            int choice2 = rnd.Next(0, 3);
            
            string modifier;
            if (choice2 == 0)
            {
                modifier = " Down!";
            }
            else if (choice2 == 1)
            {
                modifier = "!";
            }
            else
            {
                modifier = " Up!";
            }
            string complete = noun + modifier;
            return complete;


             
        }

        public static List<string> RandomCardName()
        {
            var cardNames = new List<String>();
            var numberTemplates = new List<String> { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI" };
            ShuffleClass.Shuffle(numberTemplates);
            for(int i=0 ; i<22 ; i++)
            {
                cardNames.Add(numberTemplates[i] + " - The " + RandomNoun());
            }
            return cardNames;
        }
            
        public static string RandomStageName()
        {
            var stageFile = File.ReadAllLines(@"Rec\stages.txt");
            var stageNames = new List<String>();

            foreach(var s in stageFile)
            {
                stageNames.Add(s);
            }

            return stageNames[RandomNumberGenerator.RandomInt(0, stageNames.Count-1)];
        }

        public static string RandomCharacterName()
        {
            return biblicalNames[RandomNumberGenerator.RandomInt(0, biblicalNames.Count() - 1)];
        }
        
        public static string RandomCurseName()
        {
            if (RandomNumberGenerator.RandomInt(0, 1) == 0)
            {
                return "Curse of the " + RandomNoun();
            }
            else
            {
                return "Curse of " + RandomNoun();
            }
        }

     }
}

