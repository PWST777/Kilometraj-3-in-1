# Kilometraj-3-in-1
Proiect INFOEDUCATIE 2026 | Kilometraj 3 in 1

VERSIUNEA v1.3a

! Proiectul GitHub conțione doar codul sursă !

PROIECTUL ÎNTREG POATE FI GĂSIT AICI: https://drive.google.com/file/d/1Ljq1QJsonTWuJcBCxtEOkccg3dyqYaKu/view?usp=sharing

(Proiect Unity 2022.3.62f) 

Documentație: "Documentație.pdf" 

## Descriere
Kilometraj 3-în-1 este o aplicație android ce poate fi folosită ca post de kilometraj. Numele 3-în-1 vine de la abilitatea aplicației de a funcționa prin 3 moduri diferite:

• GPS - Folosește GPS-ul dispozitivului pentru a calcula viteza

• BLE - Folosește un senzor Bluetooth atașat de o roată de bicicletă, trotinetă etc. pentru a simți cum se învârte

• OBD-II - Folosește un cititor ELM327 ce funcționează prin bluetooth, atașat în mufa OBD dintr-o mașină pentru a obține viteză, turație, etc.

Funcția principală a acestei aplicație este abilitatea de a crea design-uri personalizate ce arată ca kilometraje de mașină, motocicletă sau kilometraje simple cum ar fi cele de bicicletă sau trotinetă electrică.
## Technologiile de dezvoltare
Aplicația a fost realizată în engine-ul Unity folosind limbajul C#. Unity a fost ales datorită flexibilității sale în dezvoltarea interfețelor și a posibilității de integrare rapidă a sistemelor externe.

Sistemul GPS a fost implementat utilizând funcțiile „Input.location” oferite de Unity. Pentru funcționalitatea BLE a fost utilizată librăria „Bluetooth LE for iOS, tvOS and Android”, aceasta oferind suport pentru comunicarea cu dispozitive Bluetooth Low Energy.

Sistemul OBD-II a fost realizat în Java folosind Android Studio, iar codul rezultat a fost integrat în proiectul Unity. Alegerea Android Studio a fost făcută datorită suportului extins pentru dezvoltarea aplicațiilor Android și compatibilității bune cu Java.

Elementele grafice și imaginile utilizate în aplicație au fost create în Adobe Photoshop, program ales pentru posibilitățile avansate de editare și creare a efectelor vizuale.
## Technologiile aplicației
Au fost alese sistemele GPS, BLE și OBD-II deoarece reprezintă cele mai populare și eficiente metode pentru monitorizarea vitezei și a datelor de deplasare:
⦁	GPS - Total universal, poate fi folosit absolut oriunde și nu îți trebuie nimic altceva decât telefonul tău;
⦁	BLE - Cel mai bun pentru biciclete, funcționează pe orice fel de bicicletă normală, electrică sau cu motor;
⦁	OBD-II - Cel mai bun pentru mașini, orice mașină produsă din anul 1994 sau mai recentă are o mufă OBD-II, iar acest mod poate obține și alte valori, cum ar fi turația motorului, temperatura, etc.
⦁	Accelerometru - Total universal, folosit la fel ca sistemul de GPS, nu necesită nimic altceva decât telefonul tău;


