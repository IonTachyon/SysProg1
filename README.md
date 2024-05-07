# Sistemsko Programiranje Projekat 1 - Zadatak 23
Web server, koji pretrazuje root folder za ime fajla poslato preko GET requesta.
Vraca kao rezultat spisak fajlova koji se poklapaju sa tim imenom, i broj palindroma u svakom.

## Upotreba
Nakon pokretanja servera, server prikazuje poruku kada je spreman da prima zahteve:
`Listening to connectons on http://localhost:5050`
Za upotrebu servera, staviti ime trazenog fajla u URL: 
`http://localhost:5050/{imefajla}.txt`

Server pocinje pretragu iz direktorije u kojoj se nalazi executable file.
Rezultat se prikazuje kao HTML veb stranica.
U slucaju da fajl ne postoji, ili ako nema palindroma, server ce poslati poruku koja obavestava korisnika.


