# infsu_SOFS

# Fitness Studio Web

Web aplikacija za upravljanje paketima, članarinama i uplatama u fitness studiju.  
Projekt je izrađen u ASP.NET Core MVC tehnologiji, koristi PostgreSQL bazu podataka i organiziran je prema troslojnoj arhitekturi.

## Sadržaj

- [Opis projekta](#opis-projekta)
- [Tehnologije](#tehnologije)
- [Struktura projekta](#struktura-projekta)
- [Preduvjeti](#preduvjeti)
- [Preuzimanje projekta](#preuzimanje-projekta)
- [Potrebni NuGet paketi](#potrebni-nuget-paketi)
- [Postavljanje baze podataka](#postavljanje-baze-podataka)
- [Podešavanje connection stringa](#podešavanje-connection-stringa)
- [Pokretanje aplikacije](#pokretanje-aplikacije)
- [Dostupne stranice](#dostupne-stranice)
- [Trenutno implementirano](#trenutno-implementirano)
- [Napomene za lokalni rad u paru](#napomene-za-lokalni-rad-u-paru)

## Opis projekta

Projekt predstavlja dio informacijskog sustava za upravljanje fitness studijom.  
Trenutna verzija aplikacije implementira:

- šifrarnik paketa
- osnovu za prikaz članarina i uplata
- povezivanje s PostgreSQL bazom
- troslojnu arhitekturu:
  - prezentacijski sloj
  - poslovni sloj
  - sloj za pristup podacima

## Tehnologije

- ASP.NET Core MVC
- .NET SDK
- PostgreSQL
- Dapper
- Npgsql

## Struktura projekta

Projekt je organiziran kroz više projekata unutar jednog solutiona:

- `FitnessStudio.Web`  
  Prezentacijski sloj. Sadrži kontrolere, viewove, konfiguraciju i korisničko sučelje.

- `FitnessStudio.Business`  
  Poslovni sloj. Sadrži servise i poslovna pravila.

- `FitnessStudio.Data`  
  Sloj za pristup podacima. Sadrži repositoryje, SQL logiku i spajanje na bazu.

- `FitnessStudio.Domain`  
  Sadrži modele podataka koje koriste ostali slojevi.

- `FitnessStudio.Tests`  
  Projekt predviđen za testove. Trenutno nije dokumentiran u ovim uputama.

## Preduvjeti

Prije pokretanja projekta potrebno je imati instalirano:

- **.NET SDK**
- **PostgreSQL**
- **pgAdmin** ili drugi alat za rad s PostgreSQL bazom

Provjera je li .NET instaliran:

```bash
dotnet --version
```
## Preuzimanje projekta

Kloniranje repozitorija:

```bash
git clone <POVEZNICA_NA_REPOZITORIJ>
cd <IME_MAPE_PROJEKTA>
```
Ako se projekt preuzima kao ZIP arhiva, potrebno ga je raspakirati i otvoriti u željenom razvojnom okruženju.

Nakon preuzimanja projekta preporučuje se otvoriti solution ili root mapu projekta u razvojnom okruženju, primjerice Visual Studio ili Visual Studio Code.

## Potrebni NuGet paketi

Projekt koristi sljedeće NuGet pakete.

#### `Npgsql`

Službeni PostgreSQL provider za .NET.  

Koristi se za spajanje aplikacije na PostgreSQL bazu podataka.

#### `Dapper`

Lagani ORM za izvršavanje SQL upita i mapiranje rezultata na C# objekte.

#### `Microsoft.Extensions.Configuration.Abstractions`

Koristi se za dohvat connection stringa iz konfiguracije aplikacije.

Ako paketi nisu automatski instalirani, mogu se dodati naredbama:

```bash

dotnet add FitnessStudio.Data/FitnessStudio.Data.csproj package Npgsql

dotnet add FitnessStudio.Data/FitnessStudio.Data.csproj package Dapper

dotnet add FitnessStudio.Data/FitnessStudio.Data.csproj package Microsoft.Extensions.Configuration.Abstractions
```
## Postavljanje baze podataka

### 1. Kreiranje baze

Potrebno je lokalno kreirati PostgreSQL bazu podataka naziva:

- `fitness_studio`

To se može napraviti u pgAdminu ili SQL naredbom:

```sql
CREATE DATABASE fitness_studio;
```
### 2. Pokretanje SQL skripti

Nakon kreiranja baze potrebno je pokrenuti SQL skripte:

1. skriptu za kreiranje tablica  
2. skriptu za inicijalno punjenje baze podacima  

Redoslijed izvršavanja:

1. `create_tables.sql`
2. `insert_data.sql`

Skripte se mogu pokrenuti kroz:

- **pgAdmin Query Tool**
- `psql`
- ili drugi PostgreSQL alat

### 3. Očekivane tablice

Za trenutnu verziju aplikacije posebno su važne sljedeće tablice:

- `Clan`
- `Paket`
- `Clanarina`
- `Uplata`

Te tablice koriste se za:

- prikaz paketa
- odabir članova
- evidenciju članarina
- evidenciju uplata

## Podešavanje connection stringa

Aplikacija koristi connection string definiran u konfiguraciji.

Preporučuje se lokalno postaviti datoteku:

- `FitnessStudio.Web/appsettings.Development.json`

Primjer sadržaja:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fitness_studio;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  }
}
```

Potrebno je zamijeniti:

* YOUR_USERNAME stvarnim PostgreSQL korisničkim imenom
* YOUR_PASSWORD stvarnom lozinkom

primjer lokalnog connection stringa:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fitness_studio;Username=enesek;Password=moja_lozinka"
  }
}
```
## Pokretanje aplikacije

Aplikacija se pokreće iz web projekta naredbom:

```bash
dotnet run --project FitnessStudio.Web
```
Nakon pokretanja aplikacija će biti dostupna na lokalnoj adresi koju terminal ispiše, npr.:

* http://localhost:5287

Ako se aplikacija uspješno pokrene, moguće je otvoriti početnu stranicu i dalje navigirati prema ostalim dijelovima sustava.

## Dostupne stranice

Nakon uspješnog pokretanja aplikacije dostupne su sljedeće stranice:

- `/` – početna stranica
- `/Paket` – šifrarnik paketa
- `/Clanarina` – stranica za članarine i uplate

Početna stranica sadrži osnovnu navigaciju prema glavnim dijelovima sustava.

## Trenutno implementirano

Trenutna verzija aplikacije omogućuje:

- pokretanje lokalne ASP.NET Core MVC aplikacije
- spajanje na PostgreSQL bazu
- dohvat i prikaz paketa iz baze
- pretraživanje paketa po nazivu
- osnovnu početnu stranicu s navigacijom
- osnovu za daljnju implementaciju master-detail prikaza članarina i uplata

Do sada je implementiran prvi funkcionalni prolaz kroz sustav:

- web sloj prima zahtjev
- business sloj obrađuje zahtjev
- data sloj dohvaća podatke iz baze
- rezultat se prikazuje korisniku

## Napomene za lokalni rad u paru

Projekt je moguće razvijati u paru tako da obje osobe koriste isti izvorni kod, ali različite lokalne PostgreSQL postavke.

To znači da svaki član tima može imati:

- vlastiti PostgreSQL korisnički račun
- vlastitu lozinku
- vlastiti lokalni connection string

Važno je samo da:

- baza ima isti naziv: `fitness_studio`
- pokrenute su iste SQL skripte
- struktura tablica i podataka odgovara projektu

Dakle, kod aplikacije ostaje isti, a razlikuje se samo lokalni connection string.
