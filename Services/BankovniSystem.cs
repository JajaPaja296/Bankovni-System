using System;
using System.Collections.Generic;
using krypto.Models;

namespace krypto.Services
{
    public class BankovniSystem
    {
        private List<Account> seznamUctu = new List<Account>();

        public void PridatUcet(Account ucet)
        {
            seznamUctu.Add(ucet);
        }

        public Account NajdiUcet(string cisloUctu)
        {
            foreach (Account ucet in seznamUctu)
            {
                if (ucet.CisloUctu == cisloUctu)
                {
                    return ucet;
                }
            }
            return null;
        }

        public void PrevedPenize(string zCisla, string naCislo, decimal castka)
        {
            Account zdroj = NajdiUcet(zCisla);
            Account cil = NajdiUcet(naCislo);

            if (zdroj == null || cil == null)
            {
                Console.WriteLine("Chyba: Jeden nebo oba ucty neexistuji!");
                return;
            }

            if (castka <= 0)
            {
                Console.WriteLine("Chyba: Castka pro prevod musi byt vetsi nez 0!");
                return;
            }

            // Pouziti try-catch bloku ke zpracovani vlastni vyjimky ze zadani
            try
            {
                if (zdroj is CheckingAccount bezny)
                {
                    bezny.VyberSOverdraftem(castka);
                }
                else if (zdroj is SavingsAccount sporici)
                {
                    sporici.Vybrat(castka);
                }

                cil.Zustatek += castka;
                cil.PridatDoHistorie("Prichozi prevod", castka);
                Console.WriteLine("Prevod " + castka + " z uctu " + zCisla + " na ucet " + naCislo + " probehl uspesne.");
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine("Chyba pri prevodu: " + ex.Message);
            }
        }

        public void VypisVsechnyUcty()
        {
            Console.WriteLine("\n--- SEZNAM VSECH UCTU V BANCOVCE ---");
            foreach (Account ucet in seznamUctu)
            {
                ucet.VypisInfo();
            }
            Console.WriteLine();
        }
    }
}