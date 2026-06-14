using System;
using krypto.Models;
using krypto.Services;

namespace krypto
{
    class Program
    {
        static void Main()
        {
            // Oprava otazníků v konzoli
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            BankovniSystem banka = new BankovniSystem();
            bool bezi = true;

            CheckingAccount beznyUcet = new CheckingAccount("111", "Jindra", 1000m);
            SavingsAccount sporiciUcet = new SavingsAccount("222", "Jindra", 5000m);
            
            banka.PridatUcet(beznyUcet);
            banka.PridatUcet(sporiciUcet);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=====================================");
            Console.WriteLine("    BANKOVNI SIMULATOR REPL v1.0     ");
            Console.WriteLine("=====================================");
            Console.ResetColor();
            Console.WriteLine("Prikazy: list, info [cislo], vklad [cislo] [castka], vyber [cislo] [castka], help, exit\n");

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
                                if (ucetProVyber is CheckingAccount bezny)
                                {
                                    bezny.VyberSOverdraftem(castkaVyber);
                                }
                                else
                                {
                                    if (ucetProVyber.Zustatek - castkaVyber < 0)
                                    {
                                        Console.WriteLine("Chyba: Na sporicim uctu nemuzete jit do minusu!");
                                    }
                                    else
                                    {
                                        ucetProVyber.Zustatek -= castkaVyber;
                                        Console.WriteLine("Uspesne vybrano: $" + castkaVyber);
                                    }
                                }
                            }
                            else Console.WriteLine("Chyba: Neplatny format castky!");
                        }
                        else Console.WriteLine("Ucet nenalezen.");
                        break;

                    case "help":
                        Console.WriteLine("\n--- NAPOVEDA ---");
                        Console.WriteLine("list                 - Zobrazi vsechny ucty");
                        Console.WriteLine("info [cislo]         - Detaily uctu");
                        Console.WriteLine("vklad [cislo] [kc]   - Vlozi penize na ucet");
                        Console.WriteLine("vyber [cislo] [kc]   - Vybere penize z uctu");
                        Console.WriteLine("exit                 - Ukonci program\n");
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