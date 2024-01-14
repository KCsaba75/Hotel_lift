using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace Lift
{
    class Program
    {
        struct LiftAdat
        {
            public DateTime Idopont;
            public int KartyaSzam;
            public int InduloEmelet;
            public int ErkezoEmelet;
        }

        static void Main(string[] args)
        {
            List<LiftAdat> ListaAdatok = new List<LiftAdat>();

            string[] sorok = File.ReadAllLines("lift.txt");

            for (int sorszam = 0; sorszam < Math.Min(sorok.Length, 1001); sorszam++)
            {
                if (sorok[sorszam].Length > 0)
                {
                    string[] szavak = sorok[sorszam].Split(' ');

                    LiftAdat ujAdat = new LiftAdat
                    {
                        KartyaSzam = Convert.ToInt32(szavak[1]),
                        InduloEmelet = Convert.ToInt32(szavak[2]),
                        ErkezoEmelet = Convert.ToInt32(szavak[3]),
                        Idopont = Convert.ToDateTime(szavak[0])
                    };

                    ListaAdatok.Add(ujAdat);
                }
            }

            int osszesHasznalat = 0;

            for (int i = 0; i < ListaAdatok.Count; i++)
            {
                int utazasokSzama = ListaAdatok[i].InduloEmelet + ListaAdatok[i].ErkezoEmelet;
                if (utazasokSzama > 0) osszesHasznalat++;
            }

            DateTime legkorabbiIdopont = ListaAdatok[0].Idopont;
            DateTime legkesobbiIdopont = ListaAdatok[osszesHasznalat - 1].Idopont;

            Console.WriteLine($"3. Feladat: Összes lifthasználat: {osszesHasznalat}");
            Console.WriteLine($"4. Feladat: Időszak: {legkorabbiIdopont} - {legkesobbiIdopont}");

            int legmagasabbErkezoEmeletIndex = 0;
            int legnagyobbKartyaSzamIndex = 0;

            for (int i = 0; i < ListaAdatok.Count; i++)
            {
                if (ListaAdatok[i].ErkezoEmelet > ListaAdatok[legmagasabbErkezoEmeletIndex].ErkezoEmelet)
                    legmagasabbErkezoEmeletIndex = i;

                if (ListaAdatok[i].KartyaSzam > ListaAdatok[legnagyobbKartyaSzamIndex].KartyaSzam)
                    legnagyobbKartyaSzamIndex = i;
            }

            int legnagyobbKartyaSzam = ListaAdatok[legnagyobbKartyaSzamIndex].KartyaSzam;
            int legmagasabbErkezoEmelet = ListaAdatok[legmagasabbErkezoEmeletIndex].ErkezoEmelet;

            Console.WriteLine($"5. Feladat: A legnagyobb cél szint: {legmagasabbErkezoEmelet}");

            int kartyaSzam;
            int celEmelet;

            Console.WriteLine("6. Feladat");

            do
            {
                Console.Write("   Kérek egy kártyaszámot: ");
                kartyaSzam = Convert.ToInt32(Console.ReadLine());
            } while (kartyaSzam > legnagyobbKartyaSzam || kartyaSzam <= 0);

            do
            {
                Console.Write("   Kérek egy cél emeletet: ");
                celEmelet = Convert.ToInt32(Console.ReadLine());
            } while (celEmelet <= 0 || celEmelet > 5);

            int utazasokSzamaKartyaval = ListaAdatok.Count(adat =>
                adat.KartyaSzam == kartyaSzam && adat.ErkezoEmelet == celEmelet);

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(utazasokSzamaKartyaval > 0
                ? $"7. feladat: Utaztak a {kartyaSzam}. kártyával a megadott {celEmelet}. emeletre."
                : $"7. feladat: Nem utaztak a {kartyaSzam}. kártyával a megadott {celEmelet}. emeletre.");

            Console.Write("8. Feladat");

            int maxNap = legkesobbiIdopont.Day + 1;
            int minNap = legkorabbiIdopont.Day;

            int[] hasznalatStat = new int[maxNap];
            DateTime[] hasznalatNapok = new DateTime[maxNap];

            for (int t = 0; t < ListaAdatok.Count; t++)
            {
                int hasznalat = ListaAdatok[t].InduloEmelet + ListaAdatok[t].ErkezoEmelet;
                int aktNap = ListaAdatok[t].Idopont.Day;

                if (aktNap < maxNap && hasznalat > 0)
                {
                    hasznalatStat[aktNap]++;
                    hasznalatNapok[aktNap] = ListaAdatok[t].Idopont;
                }
            }

            Console.WriteLine("   Statisztika:");

            for (int idoIndex = minNap; idoIndex < maxNap; idoIndex++)
            {
                Console.WriteLine($"{hasznalatNapok[idoIndex].ToString("yyyy-MM-dd")} - {hasznalatStat[idoIndex]}X");
            }

            Console.ReadKey();
        }
    }
}