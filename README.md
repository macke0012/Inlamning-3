Okej,

Jag skriver det här nu när programmet är i princip klart. Kommentarer är både på engelska och svenska då mitt huvud inte kan bestämma sig. Jag hoppas även att all text i sjävla UI är på svenska då det var det jag siktade på. 

När det kommer till redigering och att lägga till objekt, kändes det nästan fel då både Add och Edit View modellerna ser nästan identiska ut, och jag är 100% säker på att man borde ha en basklass för detta, men då behöver jag ta hand om generiska typer och casta överallt. Vet inte någon bättre lösning.

Jag tog mig även friheten av att lägga till en delegat som jag har lagt i ./WestCoastEducationClient/Helpers/Navigation.cs för att navigera tillbaka till Huvudmenyn av de objekten man tittar på. Så om man sparar en lärare går man tillbaka till lärarvyn etc.
Den har jag lagt i DI Containern så att när en klass ber om den får den metoden som MainWindowViewModel har.

Jag använde mig även av Toasts (De kallas snackbars i koden) som jag hittade i biblioteket jag laddade ner för att få spinners etc.

Validering finns när man redigerar och skapar personer (Lärare och Studenter) men inte med kurser.

Det finns en Helper i Core som heter RandomDataGenerator.cs och denna är **HELT AI-GENERERAD**. Den gör ingenting med programmet förutom att skapa mock data, då jag är för lat för att skapa det själv. Den använder sig av stora listor med slumpmässiga namn, gator, och nummer för att skapa lärare, kurser och studenter.

Som sagt, känns lite dumt då det känns som att all kod är så lik, men tillräckligt annorlunda för att skapa en basklass för att lägga till eller redigera ett objekt. Men Personerna delar en basklass då det enda som skiljer på det är kompetenserna.
**Kompetenser enumen och Land enumen är också AI genererade.**