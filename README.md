# Szakdolgozat - Kiértékelő rendszer fejlesztése programozási feladatok számára .NET környezetben

A rendszer egy szerver-kliens megvalósítás, amely a konzolos alkalmazásokat tudja kiértékelni, majd az eredményt eltárolni. Megoldásilag a "Test Driven Development" fejlesztési stílushoz alkalmazásához lehetne legjobban hasonlítani.  

A kiértékelés egy előre megadott teszteset soron alapul (, amely formátuma kötött), amelyet előzőleg már felvittek a szerverre. A tesztekhez leíró dokumentumok tartoznak, ebben szerepel az adott feladat leírása (akár egy mintával együtt). Ezek a teszt esetek és a leíró fájl tesztekbe / feladatokba vannak, míg a tesztek tantárgyakba vannak rendezve.

Ezt a hieracrhikus szerkezetet egy SQLite adatbázis tárolja a szerver oldalon, amelyet csak a szerver oldali alkalmazás ér el.

Az alkalmazás használatához jogosultság szükséges, azaz bejelentkezésre van szükség. Emiatt felhasználói adatok vannak, amely szintén az adatbázisban van eltárolva (a fájljai pedig egy mappa struktúrában).

A különböző funkciók jogosultsági szintek alapján használhatóak. **3 féle jogosultság** van

 - user (kiértékelés, eredmény megtekintés, beállítások oldalak),
 - teacher (user + feladat kiosztás, tesztek kezelése, felhasználói eredmény megtekintés oldalak),
 - admin (teacher + tantárgy és felhasználó kezelés oldalak),

ahol az admin a legtágabb, a user a legszűkebb. Ezek alapján különböző számú funkciók érhetőek el.

Az egyes elemekhez tartozó fájlok pedig ennek a hierarchiának megfelelően egy gyökérjegyzékben tárolódnak:
 - Gyökér_jegyzék 
   - Users
     - Username
       - Teszt1_result
   - Subjects
     - Teszt1
       - Desc_FileName
       - TesztCase1

<br/>

## A fejlesztés során felhasznált technológiák:

 * .NET Framework
 * WCF
 * SQLite
 * Sockets (régebbi TCP kliens-szerver kommunikációra, és a UDP alapú Multicasting-ra)

## Telepítés

Le kell generálni a telepítőket: A két setup project lefordítása után létrejön mindkét alkalmazáshoz két-két telepítő fájl (egy .msi és egy .exe). Telepítés a .exe fájllal ajánlott, mert az a függőségeket is telepíti, amennyiben hiányoznának. A szerver futtatásához és telepítéséhez adminisztrátori jog szükséges.

<br/>
<br/>

Készítette: Hunkó Dániel

<br/>
<br/>
<br/>
<br/>

# Thesis - "Console application evaluator system" development in .NET enviroment

This system consists of two application: a client and a server. It's main purpose is to evaluate console applications, and store the generated result files. This system can best be compared to the "Test Driven Development" development style.

The evaluation is based on a series of predefined test cases (, the test format is fixed), which were previously uploaded to the server. The tests are accompanied by descriptive documents, which include a description of the given task (even with a sample). These test cases and the description file are organized into tests / tasks, while the tests are organized into subjects.

This hierarchical structure is stored in a SQLite database on the server side, which is only accessible by the server-side application.

Authorization is required to use the application, i.e. login is required. Because of this, there is user data, which is also stored in the database (and its files in a folder structure).

Different pages can be reached based on the authorization levels. There are **3 types of authorization**

 - user (evaluation, result viewing, settings pages),
 - teacher (user + assignment and test management, user result viewing pages),
 - admin (teacher + subject and user management pages),

where admin is the broadest, user is the narrowest. Based on these, the number of available pages are different.

The files belonging to each element are stored in a root directory according to this hierarchy:
 - Root_folder
   - Users
     - Username
       - Teszt1_result
   - Subjects
     - Teszt1
       - Desc_FileName
       - TesztCase1

<br/>

## Technologies used in development:

 * .NET Framework
 * WCF
 * SQLite
 * Sockets (for legacy TCP Client-Server communication, and for UDP Multicasting)

## Installation

The installers must be generated: After building the two setup projects, two installation files (one .msi and one .exe) are created for both applications. Installation with the .exe file is recommended because it also installs the dependencies if they are missing. Administrator rights are required to run and install the server application.

<br/>
<br/>

Author: Dániel Hunkó