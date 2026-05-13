# infsu_SOFS

# Fitness Studio Web

Web aplikacija za upravljanje paketima, članarinama i uplatama u fitness studiju.  
Projekt je izrađen u ASP.NET Core MVC tehnologiji, koristi PostgreSQL bazu podataka i organiziran je prema troslojnoj arhitekturi.

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
  Sadrži testove za poslovnu logiku sustava (paketi, članarine, uplate) i integracijski test za provjeru rada cijelog sustava.

## Preduvjeti

Prije pokretanja projekta potrebno je imati instalirano:

- **.NET SDK**
- **PostgreSQL**
- **pgAdmin** ili drugi alat za rad s PostgreSQL bazom

Provjera je li .NET instaliran:

```bash
dotnet --version
```

Provjera Git instalacije

```bash
git --version
```

## Preuzimanje projekta

Kloniranje repozitorija:

```bash
git clone <POVEZNICA_NA_REPOZITORIJ>
cd <IME_MAPE_PROJEKTA>
```
Ako se projekt preuzima kao ZIP arhiva, potrebno ga je raspakirati i otvoriti u željenom razvojnom okruženju.

Nakon preuzimanja projekta preporučuje se otvoriti solution ili root mapu projekta u razvojnom okruženju, primjerice Visual Studio ili Visual Studio Code.

Nakon preuzimanja projekta potrebno je obnoviti NuGet pakete:

```bash
dotnet restore
```

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

Za pokretanje aplikacije potrebno je koristiti aktualnu verziju baze podataka priloženu uz ovu zadaću.

> **Napomena:** Za pokretanje i testiranje ove verzije aplikacije potrebno je koristiti aktualne SQL skripte priložene uz ovu zadaću. Tijekom implementacije uvedene su određene izmjene u odnosu na raniju verziju baze podataka, pa starije skripte nisu predviđene za ovu verziju rješenja.

Potrebno je lokalno kreirati PostgreSQL bazu podataka naziva:

- `fitness_studio`

To se može napraviti u pgAdminu ili SQL naredbom:

```sql
CREATE DATABASE fitness_studio;
```
### 2. Pokretanje SQL skripti

Potrebne su dvije skripte:

* create_tables.sql — skripta za kreiranje baze podataka i tablica
* insert_data.sql — skripta za punjenje baze oglednim podacima

Koraci postavljanja baze

1. Pokrenuti PostgreSQL server.
2. U pgAdminu ili drugom PostgreSQL alatu kreirati novu bazu podataka, npr. fitness_studio.
3. Otvoriti Query Tool nad tom bazom.
4. Pokrenuti skriptu create_tables.sql.
5. Nakon toga pokrenuti skriptu insert_data.sql.

Moguće je i kopirati tekst iz create_tables.pdf i insert_data.pdf koji su priloženi u predaji 3. domaće zadaće te ih pokrenuti u PostgreSQL Query Toolu.

## Podešavanje connection stringa

Aplikacija koristi connection string definiran u konfiguraciji (appsettings.json)

Preporučuje se lokalno postaviti datoteku:

- `FitnessStudio.Web/appsettings.json`

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
* Port stvarnim portom koji koristi server vaše baze podataka

primjer lokalnog connection stringa:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fitness_studio;Username=enesek;Password=moja_lozinka"
  }
}
```
## Pokretanje aplikacije

Prije pokretanja preporučuje se provjeriti da se projekt uspješno prevodi:

```bash
dotnet build
```

Aplikacija se pokreće iz root mape projekta naredbom:

```bash
dotnet run --project FitnessStudio.Web
```
Nakon pokretanja aplikacija će biti dostupna na lokalnoj adresi koju terminal ispiše, npr.:

* http://localhost:5287

Ako se aplikacija uspješno pokrene, moguće je otvoriti početnu stranicu i dalje navigirati prema ostalim dijelovima sustava.

## Funkcionalnosti implementirane u ovoj verziji

U ovoj verziji sustava implementirano je:

- šifrarnik paketa
- master-detail prikaz članarina i uplata
- CRUD operacije nad paketima
- CRUD operacije nad članarinama
- CRUD operacije nad uplatama
- pretraživanje podataka
- validacija poslovnih pravila
- rad s povezanim podacima putem padajućih izbornika

## Dostupne stranice

Nakon uspješnog pokretanja aplikacije dostupne su sljedeće stranice:

- `/` – početna stranica
- `/Paket` – šifrarnik paketa
- `/Clanarina` – stranica za članarine i uplate

Početna stranica sadrži osnovnu navigaciju prema glavnim dijelovima sustava.

