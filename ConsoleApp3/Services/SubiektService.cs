using ADODB;
using InsERT;
using MyBaseLinkerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class SubiektService
{
    private Subiekt _subiekt;

    public void Init()
    {
        var insertGT = new GT
        {
            Produkt = ProduktEnum.gtaProduktSubiekt,
            Serwer = @"",
            Baza = "demo",
            Autentykacja = AutentykacjaEnum.gtaAutentykacjaMieszana,
            Uzytkownik = "sa",
            UzytkownikHaslo = "",
            Operator = "Szef",
            OperatorHaslo = ""
        };

        _subiekt = (Subiekt)insertGT.Uruchom((int)UruchomEnum.gtaUruchom);
        _subiekt.Okno.Widoczne = true;

    }


    public void AddOrUpdateContractor(string symbol, string name, string email, string address, string city, string postalCode, string country, string nip)
    {
        if (_subiekt == null)
            throw new Exception("Subiekt nie został zainicjalizowany.");

        var contractorMgr = _subiekt.KontrahenciManager;
        InsERT.Kontrahent contractor;

        if (contractorMgr.Istnieje(symbol))
        {
            contractor = contractorMgr.WczytajKontrahenta(symbol);
            Console.WriteLine($"Aktualizacja kontrahenta o symbolu {symbol}.");
        }
        else
        {
            contractor = contractorMgr.DodajKontrahenta();
            contractor.Symbol = symbol;

            Console.WriteLine($"Dodawanie nowego kontrahenta o symbolu {symbol}.");

        }

        contractor.Nazwa = name;
        contractor.Email = email;
        contractor.Ulica = address;
        contractor.Miejscowosc = city;
        contractor.KodPocztowy = postalCode;
        contractor.NIP = nip;

        var countries = CountriesCodes.LoadCountries(_subiekt);

        if (!string.IsNullOrWhiteSpace(country) && countries.TryGetValue(country, out int countryId))
        {
            contractor.Panstwo = countryId;
        }
        else
        {
            contractor.Panstwo = (int)SlownikEnum.gtaBrak;
        }

        contractor.Zapisz();
    }

    public void AddOrder(Order order, string symbol)
    {
        if (_subiekt == null)
            throw new Exception("Subiekt nie został zainicjalizowany.");

        if (!_subiekt.Kontrahenci.Istnieje(symbol))
            throw new Exception($"Kontrahent o symbolu {symbol} nie istnieje.");

        var vatRates = VatCodes.LoadVatRates(_subiekt);
        var contractor = _subiekt.Kontrahenci.Wczytaj(symbol);

        dynamic dokZamowienie = _subiekt.SuDokumentyManager.DodajZK();
        dokZamowienie.KontrahentId = contractor.Identyfikator;
        dokZamowienie.NumerOryginalny = $"BL-{order.order_id}";
        dokZamowienie.DataWystawienia = DateTime.Now;
        dokZamowienie.TerminRealizacji = DateTime.Now.AddDays(3);

        foreach (var product in order.products)
        {
            try
            {
                dynamic towar;

                if (_subiekt.TowaryManager.Istnieje(product.sku))
                {
                    towar = _subiekt.TowaryManager.WczytajTowar(product.sku);
                    Console.WriteLine($"Towar {product.sku} istnieje, wczytany.");
                }
                else
                {
                    Console.WriteLine($"Towar {product.sku} nie istnieje - dodaję nowy.");
                    towar = _subiekt.TowaryManager.DodajTowar();

                    towar.Symbol = product.sku;
                    towar.Nazwa = product.name;

                    dynamic price = towar.Ceny.Element(1);
                    price.Brutto = product.price_brutto;
                    price = towar.Ceny.Element(2);
                    price.Brutto = product.price_brutto;

                    if (vatRates.TryGetValue(product.tax_rate.ToString(), out int vatId))
                    {
                        towar.SprzedazVatId = vatId;
                        Console.WriteLine($"Ustawiono stawkę VAT {product.tax_rate}%");
                    }
                    else
                    {
                        Console.WriteLine($"Stawka VAT {product.tax_rate}% nie istnieje w systemie. Należy ją dodać ręcznie.");
                        return;
                    }



                    towar.Zapisz();
                    towar.Zamknij();

                    towar = _subiekt.TowaryManager.WczytajTowar(product.sku);
                }

                long idTowaru = towar.Identyfikator;

                dynamic pozycja = dokZamowienie.Pozycje.Dodaj(idTowaru);
                pozycja.IloscJm = product.quantity;

                dokZamowienie.Rezerwacja = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd dodawania produktu {product.sku}: {ex.Message}");
            }
        }

        dokZamowienie.Przelicz();
        dokZamowienie.Zapisz();

        Console.WriteLine($"Dodano zamówienie {order.order_id} do Subiekta jako ZK.");
    }
}




