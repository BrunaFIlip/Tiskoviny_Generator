using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;

namespace ukol_tiskoviny
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = Generator();

            var prvnichDesetCernoBilich = generator.PrvnichDesetCernoBilich();
            var poslednichDesetCernoBilich = generator.PoslednichDesetCernoBilich();

            var prvnichDesetBarevnych = generator.PrvnichDesetBarevnych();
            var poslednichDesetBarevnych = generator.PoslednichDesetBarevnych();

            var nejvetsiObjem = generator.NejvetsiObjem();
            var nejmensiObjem = generator.NejmensiObjem();

            var prvniVybraneBarevneTiskoviny = generator.PrvniBarevneTiskoviny("Noviny", 5);
            var prvniPodleSebe = generator.PrvniDleVlastnihoVyberu("Časopis", "černo-bílá", 5);

            var nejstarsi = generator.Nejstarsi();
            var nejmladsi = generator.Nejmladsi();



            foreach (var item in nejstarsi)
            {
                item.Vypis();
            }

            Console.ReadKey();
        }


        static IEnumerable<Tiskoviny> Generator()
        {
            string[] druhTiskoviny = { "Noviny", "Časopis", "Kniha", "Jiné" };
            string[] barvaObalky = { "černo-bílá", "barevná" };
            Random rnd = new Random();
            for(int i = 0; i < 1000; i++)
            {
                yield return new Tiskoviny(druhTiskoviny, barvaObalky, rnd);
            }
        }

    }



    class Tiskoviny
    {
        public Tiskoviny(string[] druhtiskoviny, string[] barvaobalky, Random rnd)
        {
            BarvaTisku = barvaobalky[rnd.Next() % barvaobalky.Length];
            Platforma = druhtiskoviny[rnd.Next() % druhtiskoviny.Length];
            Datum = RandomDatum();

            Rozmery = new Rozmer(20,100);
        }


        public string BarvaTisku { get; set; }
        public string Platforma { get; set; }
        public DateTime Datum { get; set; }



        public void Vypis()
        {
            Console.WriteLine("===============================================================================");
            Console.WriteLine(Platforma);
            Console.WriteLine("Barva hlavní stránky: {0}", BarvaTisku);
            Console.WriteLine("Datum vydání: {0}", Datum.ToShortDateString());
            Console.WriteLine("Objem: {0}", Rozmery.Objem());
            Console.WriteLine("===============================================================================");
        }


        public DateTime RandomDatum()
        {
            DateTime start = new DateTime(1900, 1, 1);
            Random rnd = new Random();
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            DateTime konecneDatum = start.AddDays(rnd.Next(range));
            return konecneDatum;
        }

        public Rozmer Rozmery {get; set;}

    }

    public struct Rozmer
    {
        public int Vyska { get; private set; }
        public int Sirka { get; private set; }
        public int Hloubka { get; private set; }


        public Rozmer(int minRozmer, int maxRozmer)
        {
            Random rnd = new Random();

            Vyska = rnd.Next(minRozmer, maxRozmer);
            Sirka = rnd.Next(minRozmer, maxRozmer);
            Hloubka = rnd.Next(minRozmer, maxRozmer);
        }

        public int Objem()
        {
            return Vyska * Sirka * Hloubka;
        }
    }


    static class LinqMetody
    {
        public static IEnumerable<Tiskoviny> PrvnichDesetCernoBilich(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.Where(x => x.BarvaTisku == "černo-bílá").Take(10);
        }
        public static IEnumerable<Tiskoviny> PoslednichDesetCernoBilich(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.Where(x => x.BarvaTisku == "černo-bílá").TakeLast(10);
        }
        public static IEnumerable<Tiskoviny> PrvnichDesetBarevnych(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.Where(x => x.BarvaTisku == "barevná").Take(10);
        }
        public static IEnumerable<Tiskoviny> PoslednichDesetBarevnych(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.Where(x => x.BarvaTisku == "barevná").TakeLast(10);
        }
        public static IEnumerable<Tiskoviny> NejvetsiObjem(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.OrderBy(x => x.Rozmery.Objem()).TakeLast(5).Reverse();
        }
        public static IEnumerable<Tiskoviny> NejmensiObjem(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.OrderBy(x => x.Rozmery.Objem()).Take(5);
        }
        public static IEnumerable<Tiskoviny> PrvniBarevneTiskoviny(this IEnumerable<Tiskoviny> tisk, string kteryDruh, int kolik)
        {
            return tisk.Where(x => x.Platforma == kteryDruh && x.BarvaTisku == "barevná").Take(kolik);
        }
        public static IEnumerable<Tiskoviny> PrvniDleVlastnihoVyberu(this IEnumerable<Tiskoviny> tisk, string kteryDruh, string barva, int kolik)
        {
            return tisk.Where(x => x.Platforma == kteryDruh && x.BarvaTisku == barva).Take(kolik);
        }
        public static IEnumerable<Tiskoviny> Nejstarsi(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.OrderBy(x => x.Datum).Take(5);
        }
        public static IEnumerable<Tiskoviny> Nejmladsi(this IEnumerable<Tiskoviny> tisk)
        {
            return tisk.OrderBy(x => x.Datum).TakeLast(5).Reverse();
        }
    }
}
