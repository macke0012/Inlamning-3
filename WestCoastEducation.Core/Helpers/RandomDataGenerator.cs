using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Helpers;

/// <summary>
/// Skapar slumpade lärare, elever och kurser med realistisk testdata.
/// Användning:
///     var generator = new RandomDataGenerator();
///     Student student = generator.CreateStudent();
///     List&lt;Teacher&gt; teachers = generator.CreateTeachers(10);
/// Skicka in ett seed (t.ex. new RandomDataGenerator(42)) för att få samma data varje gång.
/// </summary>
public class RandomDataGenerator
{
    private readonly Random _random;

    public RandomDataGenerator(int? seed = null)
    {
        _random = seed is null ? new Random() : new Random(seed.Value);
    }

    // =====================================================================
    //  Publika metoder
    // =====================================================================

    public Student CreateStudent()
    {
        var p = CreatePersonData(minBirthYear: 1975, maxBirthYear: 2007, emailDomains: StudentEmailDomains);
        return new Student
        {
            FirstName = p.FirstName,
            LastName = p.LastName,
            Ssn = p.Ssn,
            PhoneNumber = p.Phone,
            Email = p.Email,
            Address = p.Address
        };
    }

    public Teacher CreateTeacher()
    {
        var p = CreatePersonData(minBirthYear: 1960, maxBirthYear: 1996, emailDomains: StaffEmailDomains);
        return new Teacher
        {
            FirstName = p.FirstName,
            LastName = p.LastName,
            Ssn = p.Ssn,
            PhoneNumber = p.Phone,
            Email = p.Email,
            Address = p.Address,
            CompetenciesList = CreateCompetencies()
        };
    }

    public RemoteCourse CreateRemoteCourse()
    {
        var area = Pick(SubjectAreas);
        var (start, end) = CreateCourseDates();
        var platform = Pick(System.Enum.GetValues<Platform>());

        return new RemoteCourse
        {
            CourseNumber = CreateCourseNumber(area),
            Title = Pick(area.Titles),
            StartDate = start,
            EndDate = end,
            Platform = platform,
            MeetingLink = CreateMeetingLink(platform)
        };
    }

    public ClassroomCourse CreateClassroomCourse()
    {
        var area = Pick(SubjectAreas);
        var (start, end) = CreateCourseDates();
        var campus = Pick(Campuses);

        return new ClassroomCourse
        {
            CourseNumber = CreateCourseNumber(area),
            Title = Pick(area.Titles),
            StartDate = start,
            EndDate = end,
            LectureHall = $"{Pick(LectureHallTypes)} {campus.Building}{_random.Next(1, 4)}{_random.Next(1, 30):00}",
            Location = new Address
            {
                Street = campus.Street,
                ZipCode = campus.ZipCode,
                Town = campus.Town,
                Country = Country.Sweden
            }
        };
    }

    /// <summary>Slumpar fram antingen en distans- eller klassrumskurs.</summary>
    public BaseCourse CreateCourse() =>
        _random.Next(2) == 0 ? CreateRemoteCourse() : CreateClassroomCourse();

    public List<Student> CreateStudents(int count) => CreateMany(count, CreateStudent);
    public List<Teacher> CreateTeachers(int count) => CreateMany(count, CreateTeacher);
    public List<RemoteCourse> CreateRemoteCourses(int count) => CreateMany(count, CreateRemoteCourse);
    public List<ClassroomCourse> CreateClassroomCourses(int count) => CreateMany(count, CreateClassroomCourse);
    public List<BaseCourse> CreateCourses(int count) => CreateMany(count, CreateCourse);

    // =====================================================================
    //  Hjälpmetoder
    // =====================================================================

    private List<T> CreateMany<T>(int count, Func<T> create)
    {
        var list = new List<T>(count);
        for (int i = 0; i < count; i++) list.Add(create());
        return list;
    }

    private T Pick<T>(IReadOnlyList<T> items) => items[_random.Next(items.Count)];

    private record PersonData(string FirstName, string LastName, string Ssn, string Phone, string Email, Address Address);

    private PersonData CreatePersonData(int minBirthYear, int maxBirthYear, IReadOnlyList<string> emailDomains)
    {
        bool isFemale = _random.Next(2) == 0;
        string firstName = Pick(isFemale ? FemaleFirstNames : MaleFirstNames);
        string lastName = Pick(LastNames);

        // Ibland ett dubbelt efternamn, t.ex. "Lind Ekström"
        if (_random.Next(12) == 0)
        {
            string second = Pick(LastNames);
            if (second != lastName) lastName = $"{lastName} {second}";
        }

        return new PersonData(
            firstName,
            lastName,
            CreateSsn(minBirthYear, maxBirthYear, isFemale),
            CreatePhoneNumber(),
            CreateEmail(firstName, lastName, emailDomains),
            CreateAddress());
    }

    /// <summary>
    /// Personnummer i formatet ÅÅMMDDNNNK (10 siffror, som i resten av appen).
    /// Näst sista siffran är jämn för kvinnor och udda för män, och sista siffran
    /// är en korrekt kontrollsiffra (Luhn-algoritmen).
    /// </summary>
    private string CreateSsn(int minBirthYear, int maxBirthYear, bool isFemale)
    {
        var first = new DateOnly(minBirthYear, 1, 1);
        var last = new DateOnly(maxBirthYear, 12, 31);
        var birthDate = first.AddDays(_random.Next(last.DayNumber - first.DayNumber + 1));

        int genderDigit = _random.Next(0, 5) * 2 + (isFemale ? 0 : 1); // jämn = kvinna, udda = man
        string digits = $"{birthDate:yyMMdd}{_random.Next(0, 100):00}{genderDigit}";

        return digits + LuhnCheckDigit(digits);
    }

    private static int LuhnCheckDigit(string digits)
    {
        int sum = 0;
        for (int i = 0; i < digits.Length; i++)
        {
            int d = (digits[i] - '0') * (i % 2 == 0 ? 2 : 1);
            sum += d / 10 + d % 10;
        }
        return (10 - sum % 10) % 10;
    }

    private string CreatePhoneNumber()
    {
        string prefix = Pick(MobilePrefixes);
        return prefix + _random.Next(0, 10_000_000).ToString("0000000");
    }

    private string CreateEmail(string firstName, string lastName, IReadOnlyList<string> domains)
    {
        string first = ToEmailPart(firstName);
        string last = ToEmailPart(lastName);

        string local = _random.Next(5) switch
        {
            0 => $"{first}{last}",
            1 => $"{first[0]}.{last}",
            2 => $"{first}.{last}{_random.Next(1, 100)}",
            _ => $"{first}.{last}"
        };
        return $"{local}@{Pick(domains)}";
    }

    /// <summary>Gör om t.ex. "Jović Åberg" till "jovicaberg" så att det fungerar i en e-postadress.</summary>
    private static string ToEmailPart(string text)
    {
        // Delar upp bokstäver med accent i bokstav + accent (é -> e + ´) så att accenten kan tas bort
        string decomposed = text.ToLowerInvariant()
            .Replace("ø", "o").Replace("æ", "ae").Replace("đ", "d")
            .Normalize(System.Text.NormalizationForm.FormD);

        var result = new System.Text.StringBuilder();
        foreach (char c in decomposed)
        {
            if (c is >= 'a' and <= 'z' or >= '0' and <= '9') result.Append(c);
        }
        return result.ToString();
    }

    private Address CreateAddress()
    {
        // 9 av 10 bor i Sverige, resten i ett annat nordiskt land
        if (_random.Next(10) != 0)
        {
            var town = Pick(SwedishTowns);
            return new Address
            {
                Street = CreateStreet(Pick(SwedishStreets)),
                ZipCode = $"{town.ZipPrefix} {_random.Next(10, 100)}",
                Town = town.Name,
                Country = Country.Sweden
            };
        }

        var foreign = Pick(NordicTowns);
        return new Address
        {
            Street = CreateStreet(Pick(foreign.Streets)),
            ZipCode = foreign.ZipPrefix + _random.Next(0, 100).ToString("00"),
            Town = foreign.Name,
            Country = foreign.Country
        };
    }

    private string CreateStreet(string streetName)
    {
        string street = $"{streetName} {_random.Next(1, 120)}";
        // Ibland en bokstav efter numret, t.ex. "Storgatan 12B"
        if (_random.Next(6) == 0) street += (char)('A' + _random.Next(0, 4));
        return street;
    }

    private List<Competencies> CreateCompetencies()
    {
        var area = Pick(SubjectAreas);
        int count = _random.Next(2, Math.Min(5, area.Competencies.Length) + 1);

        var result = area.Competencies.OrderBy(_ => _random.Next()).Take(count).ToList();

        // Ibland en kompetens från ett annat område också
        if (_random.Next(3) == 0)
        {
            var extra = Pick(Pick(SubjectAreas).Competencies);
            if (!result.Contains(extra)) result.Add(extra);
        }

        // Ibland en "mjuk" kompetens
        if (_random.Next(3) == 0)
        {
            var soft = Pick(SoftSkills);
            if (!result.Contains(soft)) result.Add(soft);
        }

        return result;
    }

    private string CreateCourseNumber(SubjectArea area) =>
        $"{area.CodePrefix}{_random.Next(1, 4)}{_random.Next(0, 100):00}";

    private (DateOnly Start, DateOnly End) CreateCourseDates()
    {
        // Startar en måndag någon gång mellan två månader bakåt och tio månader framåt
        var today = DateOnly.FromDateTime(DateTime.Today);
        var start = today.AddDays(_random.Next(-60, 300));
        while (start.DayOfWeek != DayOfWeek.Monday) start = start.AddDays(1);

        int weeks = Pick(CourseLengthsInWeeks);
        var end = start.AddDays(weeks * 7 - 3); // slutar på en fredag
        return (start, end);
    }

    private string CreateMeetingLink(Platform platform)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        const string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string RandomText(string chars, int length) =>
            new string(Enumerable.Range(0, length).Select(_ => chars[_random.Next(chars.Length)]).ToArray());

        return platform switch
        {
            Platform.Zoom => $"https://zoom.us/j/{_random.NextInt64(10_000_000_000, 99_999_999_999)}",
            Platform.Teams => $"https://teams.microsoft.com/l/meetup-join/19%3ameeting_{RandomText(alphanumeric, 32)}%40thread.v2/0",
            Platform.GoogleMeet => $"https://meet.google.com/{RandomText(letters, 3)}-{RandomText(letters, 4)}-{RandomText(letters, 3)}",
            Platform.Discord => $"https://discord.gg/{RandomText(alphanumeric, 8)}",
            _ => "https://example.com/meeting"
        };
    }

    // =====================================================================
    //  Data
    // =====================================================================

    private static readonly string[] FemaleFirstNames =
    [
        "Agnes", "Alice", "Alicia", "Alma", "Alva", "Amanda", "Amelia", "Andrea", "Angelica", "Anna",
        "Annie", "Astrid", "Bianca", "Birgitta", "Camilla", "Caroline", "Cecilia", "Clara", "Daniela", "Ebba",
        "Edith", "Elin", "Elina", "Elisabeth", "Ella", "Ellen", "Ellie", "Elsa", "Elvira", "Emelie",
        "Emilia", "Emma", "Engla", "Erika", "Evelina", "Fanny", "Felicia", "Filippa", "Frida", "Freja",
        "Hanna", "Hedda", "Helena", "Hilda", "Ida", "Ingrid", "Irma", "Isabelle", "Jasmine", "Jenny",
        "Johanna", "Josefin", "Julia", "Karin", "Klara", "Kristina", "Lea", "Leia", "Lena", "Linda",
        "Linnea", "Lisa", "Liv", "Louise", "Lova", "Lovisa", "Lydia", "Madeleine", "Maja", "Malin",
        "Maria", "Matilda", "Meja", "Mila", "Minna", "Moa", "My", "Nathalie", "Nellie", "Nora",
        "Olivia", "Patricia", "Rebecka", "Ronja", "Sara", "Saga", "Selma", "Signe", "Siri", "Sofia",
        "Sofie", "Stella", "Stina", "Svea", "Tilda", "Tindra", "Therese", "Tove", "Ulrika", "Vera",
        "Victoria", "Viktoria", "Vilma", "Wilma", "Yasmin", "Ylva", "Amira", "Leyla", "Noor", "Sana",
        "Aisha", "Fatima", "Hiba", "Lina", "Maryam", "Nadia", "Yara", "Zara", "Mira", "Ines"
    ];

    private static readonly string[] MaleFirstNames =
    [
        "Adam", "Albin", "Alexander", "Alfred", "Ali", "Alvin", "Anders", "Andreas", "Anton", "Arvid",
        "August", "Axel", "Benjamin", "Bo", "Carl", "Casper", "Charlie", "Christian", "Daniel", "David",
        "Edvin", "Elias", "Elliot", "Emil", "Erik", "Felix", "Filip", "Folke", "Fredrik", "Gabriel",
        "Gustav", "Harald", "Henrik", "Henry", "Hjalmar", "Hugo", "Isak", "Ivar", "Jack", "Jakob",
        "Jesper", "Joakim", "Johan", "Johannes", "Jonas", "Jonathan", "Josef", "Kalle", "Karl", "Kevin",
        "Kristoffer", "Lars", "Leo", "Leon", "Liam", "Linus", "Loke", "Love", "Lucas", "Ludvig",
        "Magnus", "Malte", "Marcus", "Martin", "Mattias", "Max", "Melker", "Mikael", "Milo", "Mohammed",
        "Nils", "Noah", "Noel", "Olle", "Oliver", "Oscar", "Otto", "Patrik", "Per", "Peter",
        "Rasmus", "Robin", "Samuel", "Sebastian", "Simon", "Sixten", "Stefan", "Sture", "Theo", "Theodor",
        "Thomas", "Tim", "Tobias", "Valter", "Vidar", "Viggo", "Viktor", "Vincent", "William", "Wilmer",
        "Ahmed", "Amir", "Hassan", "Ibrahim", "Karim", "Omar", "Yousef", "Zakaria", "Emir", "Luka",
        "Matteo", "Mateo", "Adrian", "Nikola", "Petar", "Dario", "Marko", "Aleksandar", "Ivan", "Milan"
    ];

    private static readonly string[] LastNames =
    [
        "Andersson", "Johansson", "Karlsson", "Nilsson", "Eriksson", "Larsson", "Olsson", "Persson", "Svensson", "Gustafsson",
        "Pettersson", "Jonsson", "Jansson", "Hansson", "Bengtsson", "Jönsson", "Lindberg", "Jakobsson", "Magnusson", "Olofsson",
        "Lindström", "Lindqvist", "Lindgren", "Axelsson", "Berg", "Bergström", "Lundberg", "Lind", "Lundgren", "Lundqvist",
        "Mattsson", "Berglund", "Fredriksson", "Sandberg", "Henriksson", "Forsberg", "Sjöberg", "Wallin", "Ali", "Engström",
        "Mohamed", "Eklund", "Danielsson", "Lundin", "Håkansson", "Björk", "Bergman", "Gunnarsson", "Holm", "Wikström",
        "Samuelsson", "Isaksson", "Fransson", "Bergqvist", "Nyström", "Holmberg", "Arvidsson", "Löfgren", "Söderberg", "Nyberg",
        "Blomqvist", "Claesson", "Nordström", "Mårtensson", "Lundström", "Viklund", "Björklund", "Eliasson", "Pålsson", "Berggren",
        "Sandström", "Lund", "Nordin", "Ström", "Åberg", "Hermansson", "Ekström", "Falk", "Holmgren", "Dahlberg",
        "Hellström", "Hedlund", "Sundberg", "Sjögren", "Ek", "Blom", "Abrahamsson", "Martinsson", "Öberg", "Andreasson",
        "Strömberg", "Månsson", "Thorén", "Åkesson", "Dahl", "Lindholm", "Norberg", "Holmqvist", "Hedström", "Strand",
        "Ekberg", "Lindahl", "Nordberg", "Sundström", "Hedberg", "Lindell", "Wallgren", "Carlsson", "Skoglund", "Ekman",
        "Malmberg", "Rosén", "Sundqvist", "Ahmed", "Hassan", "Hussein", "Yilmaz", "Kaya", "Nguyen", "Kovač",
        "Jović", "Petrović", "Popović", "Novak", "Horvat", "Nowak", "Kowalski", "Virtanen", "Korhonen", "Nieminen",
        "Hansen", "Johansen", "Olsen", "Larsen", "Jensen", "Nielsen", "Pedersen", "Christensen", "Rasmussen", "Sørensen",
        "Rossi", "García", "Martínez", "Silva", "Müller", "Schmidt", "Smith", "Brown", "Wilson", "Taylor"
    ];

    private static readonly string[] SwedishStreets =
    [
        "Storgatan", "Kungsgatan", "Drottninggatan", "Vasagatan", "Linnégatan", "Skolgatan", "Kyrkogatan", "Hamngatan", "Parkgatan", "Torggatan",
        "Järnvägsgatan", "Stationsgatan", "Nygatan", "Södra Vägen", "Norra Vägen", "Östra Hamngatan", "Västra Hamngatan", "Avenyn", "Aschebergsgatan", "Karl Johansgatan",
        "Övre Husargatan", "Nedre Husargatan", "Prinsgatan", "Haga Nygata", "Sprängkullsgatan", "Första Långgatan", "Andra Långgatan", "Tredje Långgatan", "Fjärde Långgatan", "Masthuggstorget",
        "Slottsskogsgatan", "Mölndalsvägen", "Sankt Sigfridsgatan", "Kastellgatan", "Bangatan", "Ekebergsgatan", "Skånegatan", "Friggagatan", "Redbergsvägen", "Danska Vägen",
        "Björkvägen", "Ekvägen", "Granvägen", "Tallvägen", "Lönnvägen", "Aspvägen", "Lindvägen", "Almvägen", "Rönnvägen", "Hasselvägen",
        "Sjövägen", "Havsvägen", "Strandvägen", "Sjöviksvägen", "Hamnvägen", "Fyrvägen", "Klippvägen", "Båtsmansgatan", "Fiskaregatan", "Sjömansgatan",
        "Blåbärsvägen", "Lingonvägen", "Hallonvägen", "Smultronvägen", "Vitsippsvägen", "Blåklintsvägen", "Prästkragevägen", "Liljevägen", "Rosenvägen", "Tulpanvägen",
        "Ängsvägen", "Skogsvägen", "Bergsvägen", "Dalvägen", "Kullavägen", "Backvägen", "Gärdesvägen", "Hagvägen", "Åkervägen", "Mossvägen",
        "Industrigatan", "Verkstadsgatan", "Fabriksgatan", "Smedjegatan", "Hantverkargatan", "Bryggaregatan", "Möllegatan", "Kvarngatan", "Sågverksgatan", "Tegelbruksvägen",
        "Solvägen", "Månvägen", "Stjärnvägen", "Norrskensvägen", "Vårvägen", "Sommarvägen", "Höstvägen", "Vintervägen", "Midsommarvägen", "Valborgsvägen",
        "Odinsgatan", "Torsgatan", "Freyagatan", "Balders Väg", "Lokes Väg", "Heimdalsvägen", "Brages Väg", "Idunsvägen", "Njords Väg", "Ägirs Väg",
        "Kapellgatan", "Klostergatan", "Prästgatan", "Biskopsgatan", "Kaptensgatan", "Majorsgatan", "Översteg", "Rådhusgatan", "Borgaregatan", "Köpmansgatan"
    ];

    private record Town(string Name, string ZipPrefix);

    private static readonly Town[] SwedishTowns =
    [
        // Västkusten (flest, så att de dyker upp oftast)
        new("Göteborg", "411"), new("Göteborg", "412"), new("Göteborg", "413"), new("Göteborg", "414"), new("Göteborg", "415"),
        new("Göteborg", "416"), new("Göteborg", "417"), new("Göteborg", "418"), new("Göteborg", "421"), new("Göteborg", "422"),
        new("Västra Frölunda", "426"), new("Hisings Backa", "422"), new("Angered", "424"), new("Torslanda", "423"), new("Askim", "436"),
        new("Mölndal", "431"), new("Partille", "433"), new("Kungsbacka", "434"), new("Lerum", "443"), new("Kungälv", "442"),
        new("Alingsås", "441"), new("Stenungsund", "444"), new("Ale", "449"), new("Uddevalla", "451"), new("Lysekil", "453"),
        new("Strömstad", "452"), new("Trollhättan", "461"), new("Vänersborg", "462"), new("Borås", "503"), new("Borås", "504"),
        new("Varberg", "432"), new("Falkenberg", "311"), new("Halmstad", "302"), new("Halmstad", "305"), new("Laholm", "312"),
        new("Kungshamn", "456"), new("Tjörn", "471"), new("Orust", "473"), new("Marstrand", "442"), new("Öckerö", "475"),
        // Övriga Sverige
        new("Stockholm", "111"), new("Stockholm", "113"), new("Stockholm", "116"), new("Solna", "171"), new("Uppsala", "753"),
        new("Malmö", "211"), new("Malmö", "214"), new("Lund", "222"), new("Helsingborg", "252"), new("Jönköping", "553"),
        new("Linköping", "582"), new("Norrköping", "602"), new("Örebro", "702"), new("Västerås", "722"), new("Karlstad", "652"),
        new("Växjö", "352"), new("Kalmar", "392"), new("Gävle", "803"), new("Sundsvall", "852"), new("Umeå", "903"),
        new("Luleå", "972"), new("Östersund", "831"), new("Falun", "791"), new("Skövde", "541"), new("Lidköping", "531")
    ];

    private record NordicTown(string Name, string ZipPrefix, Country Country, string[] Streets);

    private static readonly NordicTown[] NordicTowns =
    [
        new("Oslo", "01", Country.Norway, ["Karl Johans gate", "Bogstadveien", "Grensen", "Thorvald Meyers gate", "Markveien"]),
        new("Bergen", "50", Country.Norway, ["Torgallmenningen", "Strandgaten", "Bryggen", "Nygårdsgaten"]),
        new("Fredrikstad", "16", Country.Norway, ["Nygaardsgata", "Storgata", "Glommengata"]),
        new("Halden", "17", Country.Norway, ["Storgata", "Torggata", "Jernbanegata"]),
        new("København", "10", Country.Denmark, ["Strøget", "Nørrebrogade", "Vesterbrogade", "Amagerbrogade", "Østerbrogade"]),
        new("Aarhus", "80", Country.Denmark, ["Søndergade", "Frederiksgade", "Jægergårdsgade"]),
        new("Aalborg", "90", Country.Denmark, ["Bispensgade", "Boulevarden", "Jomfru Ane Gade"]),
        new("Frederikshavn", "99", Country.Denmark, ["Danmarksgade", "Havnegade", "Søndergade"]),
        new("Helsingfors", "001", Country.Finland, ["Mannerheimintie", "Aleksanterinkatu", "Bulevardi", "Esplanadi"]),
        new("Åbo", "201", Country.Finland, ["Eerikinkatu", "Yliopistonkatu", "Aurakatu"]),
        new("Mariehamn", "221", Country.Finland, ["Torggatan", "Storagatan", "Norra Esplanadgatan"])
    ];

    private static readonly string[] MobilePrefixes = ["070", "072", "073", "076", "079"];

    // Reserverade exempeldomäner, så att ingen riktig person får mejl av misstag
    private static readonly string[] StudentEmailDomains = ["example.com", "example.se", "example.org", "example.net"];
    private static readonly string[] StaffEmailDomains = ["westcoasteducation.example"];

    private record Campus(string Town, string Street, string ZipCode, char Building);

    private static readonly Campus[] Campuses =
    [
        new("Göteborg", "Lindholmsplatsen 1", "417 56", 'A'),
        new("Göteborg", "Lindholmsplatsen 1", "417 56", 'B'),
        new("Göteborg", "Kungsportsavenyen 21", "411 36", 'C'),
        new("Göteborg", "Gibraltargatan 82", "412 79", 'D'),
        new("Göteborg", "Hisingsgatan 30", "417 03", 'E'),
        new("Mölndal", "Kråketorpsgatan 20", "431 53", 'M'),
        new("Borås", "Allégatan 1", "503 32", 'H'),
        new("Trollhättan", "Gustava Melins gata 2", "461 32", 'T'),
        new("Halmstad", "Kristian IV:s väg 3", "301 18", 'K'),
        new("Varberg", "Västra Vallgatan 30", "432 41", 'V'),
        new("Uddevalla", "Kasernvägen 9", "451 50", 'U'),
        new("Kungsbacka", "Storgatan 41", "434 30", 'S')
    ];

    private static readonly string[] LectureHallTypes = ["Sal", "Sal", "Sal", "Datorsal", "Labb", "Grupprum", "Föreläsningssal", "Studio"];

    private static readonly int[] CourseLengthsInWeeks = [3, 4, 5, 6, 8, 8, 10, 10, 12, 15, 20];

    private static readonly Competencies[] SoftSkills =
    [
        Competencies.ProblemSolving, Competencies.Teamwork, Competencies.TechnicalCommunication,
        Competencies.TechnicalEnglish, Competencies.Presentation, Competencies.SelfDirectedLearning,
        Competencies.TimeManagement, Competencies.ProfessionalEthics, Competencies.CustomerService
    ];

    private record SubjectArea(string CodePrefix, string[] Titles, Competencies[] Competencies);

    private static readonly SubjectArea[] SubjectAreas =
    [
        new("PRG",
            ["Programmering grund", "Programmering fortsättning", "Algoritmer och datastrukturer", "Felsökning och debugging",
             "Clean Code i praktiken", "Designmönster", "Asynkron programmering", "Funktionell programmering"],
            [Competencies.ProgrammingFundamentals, Competencies.ObjectOrientedProgramming, Competencies.FunctionalProgramming,
             Competencies.DataStructures, Competencies.Algorithms, Competencies.Debugging, Competencies.CleanCode,
             Competencies.DesignPatterns, Competencies.AsynchronousProgramming]),
        new("NET",
            ["C# grund", "C# fortsättning", "Objektorienterad programmering med C#", "ASP.NET Core Web API",
             "Desktopapplikationer med Avalonia", "Entity Framework Core", ".NET för systemutvecklare"],
            [Competencies.CSharp, Competencies.ObjectOrientedProgramming, Competencies.AspNetCore, Competencies.OrmFrameworks,
             Competencies.DesktopApplications, Competencies.RestApiDesign, Competencies.UnitTesting]),
        new("WEB",
            ["Webbutveckling grund", "HTML och CSS", "JavaScript grund", "TypeScript", "React", "Responsiv design",
             "Tillgänglighet på webben", "Webbprestanda", "Fullstack med Node.js"],
            [Competencies.Html, Competencies.Css, Competencies.JavaScript, Competencies.TypeScript, Competencies.FrontendFrameworks,
             Competencies.ResponsiveDesign, Competencies.WebAccessibility, Competencies.WebPerformance, Competencies.NodeJs]),
        new("JAV",
            ["Java grund", "Java fortsättning", "Kotlin för Android", "Androidutveckling", "Spring Boot"],
            [Competencies.Java, Competencies.Kotlin, Competencies.AndroidDevelopment, Competencies.ObjectOrientedProgramming,
             Competencies.BackendDevelopment, Competencies.UnitTesting]),
        new("APP",
            ["Apputveckling för iOS", "Swift grund", "Plattformsoberoende apputveckling", "Mobil UX"],
            [Competencies.Swift, Competencies.IosDevelopment, Competencies.CrossPlatformMobile, Competencies.UiDesign,
             Competencies.AndroidDevelopment]),
        new("DBS",
            ["Databaser grund", "SQL i praktiken", "Databasdesign och normalisering", "NoSQL med MongoDB",
             "Datamodellering", "Databasadministration"],
            [Competencies.Sql, Competencies.RelationalDatabases, Competencies.DatabaseDesign, Competencies.Normalization,
             Competencies.NoSqlDatabases, Competencies.DataModelling, Competencies.OrmFrameworks]),
        new("DAT",
            ["Python för dataanalys", "Datavisualisering", "Business Intelligence", "ETL och datapipelines",
             "Big Data", "Statistik för dataanalytiker"],
            [Competencies.Python, Competencies.DataAnalysis, Competencies.DataVisualization, Competencies.BusinessIntelligence,
             Competencies.EtlPipelines, Competencies.BigData, Competencies.Sql]),
        new("AIM",
            ["Maskininlärning grund", "Deep Learning", "Natural Language Processing", "Datorseende",
             "Prompt Engineering", "AI-integration i applikationer", "Ansvarsfull AI"],
            [Competencies.MachineLearning, Competencies.DeepLearning, Competencies.NaturalLanguageProcessing,
             Competencies.ComputerVision, Competencies.PromptEngineering, Competencies.AiIntegration,
             Competencies.ResponsibleAi, Competencies.Python]),
        new("NAT",
            ["Nätverksteknik grund", "TCP/IP och subnetting", "Routing och switching", "Trådlösa nätverk",
             "VPN och fjärråtkomst", "Felsökning i nätverk", "Nätverksövervakning"],
            [Competencies.NetworkFundamentals, Competencies.TcpIp, Competencies.Subnetting, Competencies.Routing,
             Competencies.Switching, Competencies.Vlans, Competencies.WirelessNetworking, Competencies.Dns, Competencies.Dhcp,
             Competencies.Vpn, Competencies.NetworkTroubleshooting, Competencies.NetworkMonitoring]),
        new("SYS",
            ["Windows Server", "Linuxadministration", "Active Directory och Entra ID", "Virtualisering",
             "Backup och återställning", "Microsoft 365-administration", "IT-support", "Serverhårdvara"],
            [Competencies.OperatingSystems, Competencies.WindowsServer, Competencies.LinuxAdministration,
             Competencies.ActiveDirectory, Competencies.EntraId, Competencies.Virtualization, Competencies.BackupAndRecovery,
             Competencies.PatchManagement, Competencies.Microsoft365Administration, Competencies.ItSupport,
             Competencies.ServerHardware, Competencies.PowerShell]),
        new("MOL",
            ["Molntjänster grund", "Azure för utvecklare", "AWS grund", "Google Cloud", "Containrar med Docker",
             "Kubernetes", "Infrastructure as Code", "CI/CD-pipelines", "Serverless", "DevOps"],
            [Competencies.CloudFundamentals, Competencies.Azure, Competencies.Aws, Competencies.GoogleCloud,
             Competencies.Containers, Competencies.Kubernetes, Competencies.InfrastructureAsCode, Competencies.CiCd,
             Competencies.Serverless, Competencies.Observability, Competencies.SiteReliabilityEngineering, Competencies.Bash]),
        new("SEC",
            ["IT-säkerhet grund", "Säker kodning och OWASP", "Kryptografi", "Penetrationstestning", "Incidenthantering",
             "Digital forensik", "Hotmodellering", "GDPR och NIS2", "ISO 27001 i praktiken"],
            [Competencies.SecurityFundamentals, Competencies.SecureCoding, Competencies.Owasp, Competencies.Cryptography,
             Competencies.IdentityAndAccessManagement, Competencies.Firewalls, Competencies.PenetrationTesting,
             Competencies.VulnerabilityManagement, Competencies.IncidentResponse, Competencies.DigitalForensics,
             Competencies.SecurityOperations, Competencies.ThreatModelling, Competencies.RiskManagement,
             Competencies.Gdpr, Competencies.Nis2, Competencies.Iso27001]),
        new("TST",
            ["Testautomatisering", "Enhetstestning", "Testdriven utveckling", "Kvalitetssäkring av mjukvara", "Kodgranskning"],
            [Competencies.UnitTesting, Competencies.IntegrationTesting, Competencies.TestDrivenDevelopment,
             Competencies.TestAutomation, Competencies.CodeReview, Competencies.VersionControl]),
        new("ARK",
            ["Mjukvaruarkitektur", "Mikrotjänster", "Systemdesign", "Kravanalys", "UML och modellering",
             "Teknisk dokumentation", "API-design"],
            [Competencies.SoftwareArchitecture, Competencies.Microservices, Competencies.SystemDesign,
             Competencies.RequirementsAnalysis, Competencies.Uml, Competencies.Documentation, Competencies.RestApiDesign,
             Competencies.GraphQl]),
        new("EMB",
            ["Elektronik grund", "Inbyggda system", "Mikrokontroller med C", "Internet of Things", "Robotik"],
            [Competencies.ElectronicsBasics, Competencies.EmbeddedSystems, Competencies.Microcontrollers,
             Competencies.InternetOfThings, Competencies.Robotics, Competencies.C, Competencies.Cpp]),
        new("UXD",
            ["UX-design", "UI-design", "Prototyping i Figma", "Användarundersökningar", "Grafisk design",
             "Spelutveckling"],
            [Competencies.UxDesign, Competencies.UiDesign, Competencies.Prototyping, Competencies.UserResearch,
             Competencies.GraphicDesign, Competencies.GameDevelopment]),
        new("PRJ",
            ["Agila metoder", "Scrum i praktiken", "Projektledning inom IT", "ITIL", "Produktledning",
             "Entreprenörskap inom IT", "IT-juridik och licenser", "Hållbar IT"],
            [Competencies.AgileMethods, Competencies.Scrum, Competencies.Kanban, Competencies.ProjectManagement,
             Competencies.Itil, Competencies.ProductManagement, Competencies.Entrepreneurship,
             Competencies.ItLawAndLicensing, Competencies.Sustainability]),
        new("LAN",
            ["Rust", "Go för backend", "C++ grund", "PHP och webbservrar", "Skriptning med Bash och PowerShell",
             "Reguljära uttryck"],
            [Competencies.Rust, Competencies.Go, Competencies.Cpp, Competencies.Php, Competencies.Scripting,
             Competencies.Bash, Competencies.PowerShell, Competencies.RegularExpressions, Competencies.Concurrency])
    ];
}
