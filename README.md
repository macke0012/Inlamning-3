Okej,

Jag skriver det här nu när programmet är i princip klart. Kommentarerna är både på engelska och svenska då mitt huvud inte kan bestämma sig. All text i själva UI:t ska vara på svenska, då det var det jag siktade på.

När det kommer till redigering och att lägga till objekt kändes det nästan fel, då både Add- och Edit-viewmodellerna ser nästan identiska ut. Jag är 100 % säker på att man borde ha en basklass för detta, men då behöver jag hantera generiska typer och casta överallt, och jag vet inte någon bättre lösning. Personerna (lärare och studenter) delar dock en basklass för redigering, eftersom det enda som skiljer dem åt är kompetenserna.

Jag tog mig friheten att lägga till en delegat i `./WestCoastEducation.Client/Helpers/Navigation.cs` för att navigera tillbaka till listvyn för det objekt man arbetar med. Så om man sparar en lärare går man tillbaka till lärarvyn, osv. Delegaten ligger i DI-containern, så när en klass ber om den får den `NavigateBack`-metoden från `MainWindowViewModel`.

Jag använde mig även av toasts (de kallas snackbars i koden) från biblioteket PleasantUI, och har gjort egna popup-rutor för bekräftelser och laddningsindikatorer.

Validering finns när man skapar och redigerar personer (lärare och studenter), men inte för kurser.

Som vi kom överens om använde jag MongoDB och deras NuGet-drivrutin istället för RestDb.io. Jag började titta på MongoDB:s realtidsuppdateringar (change streams), men valde att inte använda dem, eftersom vi ska använda Neon i nästa inlämning. Just nu hämtar jag därför bara om datan när något uppdateras.

Jag vet också att det inte är optimalt att hämta all data på en gång och att jag borde använda paginering, men "den dagen, den sorgen".

## Användar guide
Jag har försökt att göra programmet så användarvänligt som möjligt. Man kan högerklicka på posterna i tabellerna för att redigera eller radera. Man kan även välja flera poster för att bulk-radera med Cmd/Ctrl + Click eller Shift + Click. 

## AI-genererat

- `WestCoastEducation.Core/Helpers/RandomDataGenerator.cs` är **helt AI-genererad**. Den gör ingenting i programmet förutom att skapa mockdata, då jag var för lat för att skapa det själv. Den använder stora listor med slumpmässiga namn, gator och nummer för att skapa lärare, kurser och studenter.
- Enumerationerna för **kompetenser** (`Competencies`) och **länder** (`Country`) är också AI-genererade.
