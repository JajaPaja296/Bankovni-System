using System;
using System.Collections.Generic;

namespace krypto.Models
{
    public interface IOverdraftable
    {
        void VyberSOverdraftem(decimal castka);
    }

    public abstract class Account
    {
        public string CisloUctu { get; set; }
        public string Majitel { get; set; }
        public decimal Zustatek { get; set; }
        public List<Transaction> Historie = new List<Transaction>();

        public Account(string cisloUctu, string majitel, decimal pocatecniVklad)
        {
            CisloUctu = cisloUctu;
            Majitel = majitel;
            Zustatek = pocatecniVklad;
        }

        public abstract void VypisInfo();

        public void Vlozit(decimal castka)
        {
            if (castka <= 0)
            {
                Console.WriteLine("Chyba: Nelze vlozit zapornou nebo nulovou castku!");
                return;
            }
            Zustatek += castka;
            PridatDoHistorie("Vklad", castka);
            Console.WriteLine("Uspesne vlozeno: $" + castka + ". Novy zustatek: $" + Zustatek);
        }

        public void PridatDoHistorie(string typ, decimal castka)
        {
            Transaction transakce = new Transaction();
            transakce.Datum = DateTime.Now;
            transakce.Typ = typ;
            transakce.Castka = castka;
            transakce.ZustatekPo = Zustatek;
            Historie.Add(transakce);
        }

        public void VypisHistorii()
        {
            Console.WriteLine("\n--- HISTORIE UCTU " + CisloUctu + " ---");
            if (Historie.Count == 0)
            {
                Console.WriteLine("Zadne transakce.");
            }
            else
            {
                foreach (Transaction t in Historie)
                {
                    Console.WriteLine(t.Datum + " | " + t.Typ + " | $" + t.Castka + " | zustatek: $" + t.ZustatekPo);
                }
            }
            Console.WriteLine();
        }
    }

    public class SavingsAccount : Account
    {
        public decimal UrokovaSazba { get; set; }

        public SavingsAccount(string cisloUctu, string majitel, decimal vklad) 
            : base(cisloUctu, majitel, vklad)
        {
            UrokovaSazba = 0.02m;
        }

        public override void VypisInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sporici ucet majitele " + Majitel + " (Cislo: " + CisloUctu + ")");
            Console.WriteLine("Zustatek: $" + Zustatek + " | Urok: " + (UrokovaSazba * 100) + "%");
            Console.ResetColor();
        }

        public void Vybrat(decimal castka)
        {
            if (castka <= 0)
            {
                Console.WriteLine("Chyba: Neplatna castka pro vyber!");
                return;
            }

            if (Zustatek - castka < 0)
            {
                throw new InsufficientFundsException("Na sporicim uctu nemuzete jit do minusu!");
            }

            Zustatek -= castka;
            PridatDoHistorie("Vyber", castka);
            Console.WriteLine("Uspesne vybrano: $" + castka + ". Novy zustatek: $" + Zustatek);
        }
    }

    public class CheckingAccount : Account, IOverdraftable
    {
        public decimal LimitPrecerpani { get; set; }

        public CheckingAccount(string cisloUctu, string majitel, decimal vklad) 
            : base(cisloUctu, majitel, vklad)
        {
            LimitPrecerpani = 500m;
        }

        public override void VypisInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Bezny ucet majitele " + Majitel + " (Cislo: " + CisloUctu + ")");
            Console.WriteLine("Zustatek: $" + Zustatek + " | Limit precerpani: $" + LimitPrecerpani);
            Console.ResetColor();
        }

        public void VyberSOverdraftem(decimal castka)
        {
            if (castka <= 0)
            {
                Console.WriteLine("Chyba: Neplatna castka pro vyber!");
                return;
            }

            if (Zustatek - castka < -LimitPrecerpani)
            {
                throw new InsufficientFundsException("Prekrocili byste limit kontokorentu.");
            }

            Zustatek -= castka;
            PridatDoHistorie("Vyber", castka);
            Console.WriteLine("Uspesne vybrano: $" + castka + ". Novy zustatek: $" + Zustatek);
        }
    }

    public class Transaction
    {
        public DateTime Datum { get; set; }
        public string Typ { get; set; }
        public decimal Castka { get; set; }
        public decimal ZustatekPo { get; set; }
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string zprava) : base(zprava)
        {
        }
    }
}