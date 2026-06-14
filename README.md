# Zavrecny projekt z PRG - Bankovni system

## Jak to funguje:

1. Tridy a dedicnost:
   Mame abstraktni zakladni tridu `Account` a dva potomky `CheckingAccount` a `SavingsAccount`. Na sporicim uctu nelze jit do minusu.

2. Rozhrani (Interface):
   Vyuzivam rozhrani `IOverdraftable` pro spravu uctu podporujici precerpani uctu .

3. Validace vstupu:
   Vsechny ciselne vstupy od uzivatele jsou bezpecne osetreny pomoci metody `decimal.TryParse`.

## Vyuzyti AI pri projektu:
Umela inteligence byla pouzita pouze jako konzultant. Pomahala mi pochopit teoreticke veci kolem OOP, jako je dedicnost trid a fungovani interface. Samotny kod jsem psal rucne.

## Ovladani
* list - Zobrazi prehled vsech uctu v systemu
* info [cislo_uctu] - Vypise podrobne informace o vybranem uctu
* vklad [cislo_uctu] [castka] - Pripise penize na ucet
* vyber [cislo_uctu] [castka] - Vybere penize (u bezneho uctu s moznosti jit do minusu
* exit - Ukonci program