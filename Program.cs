using System;
using krypto.Models;
using krypto.Services;

namespace krypto
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            BankovniSystem banka = new BankovniSystem();
            bool bezi = true;

            CheckingAccount beznyUcet = new CheckingAccount("111", "Jindra", 1000m);
            SavingsAccount sporiciUcet = new SavingsAccount("222", "Jindra", 5000m);
            
            banka.PridatUcet(beznyUcet);
            banka.PridatUcet(sporiciUcet);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("    BANKOVNI SIMULATOR REPL v1.0     ");
            Console.WriteLine("-------------------------------------");
            Console.ResetColor();
            Console.WriteLine("Prikazy: list, info [cislo], vklad [cislo] [castka], vyber [cislo] [castka]");
            Console.WriteLine("         historie [cislo], prevod [z_cisla] [na_cislo] [castka], help, exit\n");

            while (bezi)
            {
                Console.Write("banka> ");
                string vstup = Console.ReadLine();

                if (string.IsNullOrEmpty(vstup)) continue;

                string[] casti = vstup.Split(' ');
                if (casti.Length == 0) continue;

                string prikaz = casti[0].ToLower();

                switch (prikaz)
                {
                    case "list":
                        banka.VypisVsechnyUcty();
                        break;

                    case "info":
                        if (casti.Length < 2)
                        {
                            Console.WriteLine("Chyba: Zadej cislo uctu!");
                            break;
                        }
                        Account u = banka.NajdiUcet(casti[1]);
                        if (u != null) u.VypisInfo();
                        else Console.WriteLine("Ucet nenalezen.");
                        break;

                    case "historie":
                        if (casti.Length < 2)
                        {
                            Console.WriteLine("Chyba: Zadej cislo uctu!");
                            break;
                        }
                        Account ucetHist = banka.NajdiUcet(casti[1]);
                        if (ucetHist != null) ucetHist.VypisHistorii();
                        else Console.WriteLine("Ucet nenalezen.");
                        break;

                    case "vklad":
                        if (casti.Length < 3)
                        {
                            Console.WriteLine("Chyba: Pouziti: vklad [cislo_uctu] [castka]");
                            break;
                        }
                        Account ucetProVklad = banka.NajdiUcet(casti[1]);
                        if (ucetProVklad != null)
                        {
                            if (decimal.TryParse(casti[2], out decimal castkaVklad))
                            {
                                ucetProVklad.Vlozit(castkaVklad);
                            }
                            else Console.WriteLine("Chyba: Neplatny format castky!");
                        }
                        else Console.WriteLine("Ucet nenalezen.");
                        break;

                    case "vyber":
                        if (casti.Length < 3)
                        {
                            Console.WriteLine("Chyba: Pouziti: vyber [cislo_uctu] [castka]");
                            break;
                        }
                        Account ucetProVyber = banka.NajdiUcet(casti[1]);
                        if (ucetProVyber != null)
                        {
                            if (decimal.TryParse(casti[2], out decimal castkaVyber))
                            {
                                try
                                {
                                    if (ucetProVyber is CheckingAccount bezny)
                                    {
                                        bezny.VyberSOverdraftem(castkaVyber);
                                    }
                                    else if (ucetProVyber is SavingsAccount sporici)
                                    {
                                        sporici.Vybrat(castkaVyber);
                                    }
                                }
                                catch (InsufficientFundsException ex)
                                {
                                    Console.WriteLine("Chyba pri vybehu: " + ex.Message);
                                }
                            }
                            else Console.WriteLine("Chyba: Neplatny format castky!");
                        }
                        else Console.WriteLine("Ucet nenalezen.");
                        break;

                    case "prevod":
                        if (casti.Length < 4)
                        {
                            Console.WriteLine("Chyba: Pouziti: prevod [z_uctu] [na_ucet] [castka]");
                            break;
                        }
                        if (decimal.TryParse(casti[3], out decimal castkaPrevod))
                        {
                            banka.PrevedPenize(casti[1], casti[2], castkaPrevod);
                        }
                        else Console.WriteLine("Chyba: Neplatny format castky!");
                        break;

                    case "help":
                        Console.WriteLine("\n--- NAPOVEDA ---");
                        Console.WriteLine("list                                 - Zobrazi vsechny ucty");
                        Console.WriteLine("info [cislo]                         - Detaily uctu");
                        Console.WriteLine("vklad [cislo] [kc]                   - Vlozi penize na ucet");
                        Console.WriteLine("vyber [cislo] [kc]                   - Vybere penize z uctu");
                        Console.WriteLine("historie [cislo]                     - Vypise historii transakci uctu");
                        Console.WriteLine("prevod [z_cisla] [na_cislo] [kc]     - Prevede penize mezi ucty");
                        Console.WriteLine("exit                                 - Ukonci program\n");
                        break;

                    case "exit":
                        bezi = false;
                        Console.WriteLine("Ukončuji simulator.");
                        break;

                    default:
                        Console.WriteLine("Neznamy prikaz. Napis 'help'.");
                        break;
                }
            }
        }
    }
}