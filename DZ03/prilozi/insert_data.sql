INSERT INTO Clan (ime, prezime, email, telefon) VALUES
('Ana', 'Anić', 'ana.anic@email.com', '0911111111'),
('Ivana', 'Ivić', 'ivana.ivic@email.com', '0912222222'),
('Luka', 'Lukić', 'luka.lukic@email.com', '0913333333'),
('Petra', 'Perić', 'petra.peric@email.com', '0914444444'),
('Marko', 'Marić', 'marko.maric@email.com', '0915555555');
INSERT INTO Trener (ime, prezime, specijalizacija) VALUES
('Ivan', 'Horvat', 'Funkcionalni trening'),
('Maja', 'Kovač', 'Pilates'),
('Tomislav', 'Babić', 'Kondicijski trening');
INSERT INTO Trening (naziv, tip, kapacitet) VALUES
('Jutarnji cardio', 'Grupni', 15),
('Pilates za početnike', 'Grupni', 12),
('HIIT trening', 'Grupni', 10),
('Individualni trening', 'Individualni', 1);
INSERT INTO Dvorana (naziv, kapacitet) VALUES
('Velika dvorana', 20),
('Mala dvorana', 10),
('Studio 1', 5);
INSERT INTO Termin (datum, vrijeme, trajanje, trening_id, dvorana_id) VALUES
('2026-06-10', '08:00:00', 60, 1, 1),
('2026-06-10', '10:00:00', 60, 2, 2),
('2026-06-11', '18:00:00', 45, 3, 1),
('2026-06-12', '16:00:00', 60, 4, 3);
INSERT INTO Rezervacija (clan_id, termin_id, status) VALUES
(1, 1, 'Potvrđena'),
(2, 1, 'Potvrđena'),
(3, 2, 'Na čekanju'),
(4, 3, 'Potvrđena'),
(5, 4, 'Potvrđena');
INSERT INTO Paket (naziv, broj_treninga, cijena) VALUES
('Mjesečni paket', 12, 45.00),
('Neograničeni paket', 999, 70.00),
('Individualni paket', 8, 100.00);
INSERT INTO Clanarina (clan_id, paket_id, datum_pocetka, datum_zavrsetka, status)
VALUES
(1, 1, '2026-04-01', '2026-04-30', 'Aktivna'),
(2, 1, '2026-04-05', '2026-05-04', 'Aktivna'),
(3, 3, '2026-04-01', '2026-04-30', 'Istekla'),
(4, 3, '2026-05-10', '2026-06-10', 'Aktivna'),
(5, 2, '2026-05-08', '2026-06-08', 'Otkazana');
INSERT INTO Uplata (clanarina_id, iznos, datum) VALUES
(1, 20.00, '2026-04-01'),
(1, 25.00, '2026-04-10'),
(2, 45.00, '2026-04-05'),
(3, 60.00, '2026-04-01'),
(3, 40.00, '2026-04-15'),
(4, 50.00, '2026-05-10'),
(5, 30.00, '2026-05-08');
INSERT INTO Raspored (trener_id, termin_id) VALUES
(1, 1),
(2, 2),
(1, 3),
(3, 4);