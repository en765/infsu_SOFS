CREATE TABLE Clan (
clan_id SERIAL PRIMARY KEY,
ime VARCHAR(50),
prezime VARCHAR(50),
email VARCHAR(100),
telefon VARCHAR(20)
);
CREATE TABLE Trener (
trener_id SERIAL PRIMARY KEY,
ime VARCHAR(50),
prezime VARCHAR(50),
specijalizacija VARCHAR(100)
);
CREATE TABLE Trening (
trening_id SERIAL PRIMARY KEY,
naziv VARCHAR(100),
tip VARCHAR(20),
kapacitet INT
);
CREATE TABLE Dvorana (
dvorana_id SERIAL PRIMARY KEY,
naziv VARCHAR(50),
kapacitet INT);
CREATE TABLE Termin (
termin_id SERIAL PRIMARY KEY,
datum DATE,
vrijeme TIME,
trajanje INT,
trening_id INT REFERENCES Trening(trening_id),
dvorana_id INT REFERENCES Dvorana(dvorana_id)
);
CREATE TABLE Rezervacija (
rezervacija_id SERIAL PRIMARY KEY,
clan_id INT REFERENCES Clan(clan_id),
termin_id INT REFERENCES Termin(termin_id),
status VARCHAR(20)
);
CREATE TABLE Paket (
paket_id SERIAL PRIMARY KEY,
naziv VARCHAR(100),
broj_treninga INT,
cijena DECIMAL
);
CREATE TABLE Clanarina (
clanarina_id SERIAL PRIMARY KEY,
clan_id INT REFERENCES Clan(clan_id),
paket_id INT REFERENCES Paket(paket_id) ON DELETE SET NULL,
datum_pocetka DATE,
datum_zavrsetka DATE,
status VARCHAR(20),
CONSTRAINT chk_clanarina_status
CHECK (status IN ('Aktivna', 'Istekla', 'Otkazana'))
);
CREATE TABLE Uplata (
uplata_id SERIAL PRIMARY KEY,
clanarina_id INT REFERENCES Clanarina(clanarina_id),
iznos DECIMAL,
datum DATE
);
CREATE TABLE Raspored (
raspored_id SERIAL PRIMARY KEY,
trener_id INT REFERENCES Trener(trener_id),
termin_id INT REFERENCES Termin(termin_id)
);