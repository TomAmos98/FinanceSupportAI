# Reflektion – FinanceSupportAI

## Lösningen

Jag har utvecklat FinanceSupportAI som en MVP för kundservice inom finans. Målet var att minska mängden återkommande frågor som behöver hanteras manuellt och samtidigt ha en tydlig väg till mänsklig kundservice när systemet inte kan ge ett säkert svar.

Lösningen består av ett ASP.NET Core API i .NET 9, ett FAQ-baserat kunskapsunderlag, AI-integration samt ett system för eskalering och supportärenden.

Jag har även byggt en automation i n8n. Eskalerade frågor kan skickas till ett workflow där en lokal Llama 3.2 3B-modell via Ollama klassificerar frågan som Betalning, Konto, Lån eller Övrigt.

## Val av arkitektur

Jag valde att först söka efter relevant information i ett kontrollerat FAQ-underlag innan AI-modellen används. AI:n får därför ett begränsat underlag att formulera sitt svar från i stället för att fritt försöka besvara alla frågor.

Om relevant information saknas skapas i stället ett supportärende. Detta minskar risken för att AI:n hittar på information och gör det möjligt för en människa att ta över.

För MVP:n valde jag en relativt enkel arkitektur i stället för en fullständig RAG-lösning med embeddings och vektordatabas. Lösningen är enklare att utveckla, testa och demonstrera, samtidigt som den visar principen för hur AI kan användas tillsammans med ett kontrollerat kunskapsunderlag.

## AI och automation

AI används på två sätt i lösningen. I API:t används AI för att formulera ett tydligt kundservicesvar utifrån FAQ-information. I n8n används en lokal AI-modell för att klassificera supportfrågor.

Jag valde Ollama för automationen eftersom modellen kan köras lokalt utan kostnad per API-anrop. Det gjorde också att jag kunde testa AI-automationen utan att vara beroende av en extern betaltjänst.

## Problem och lärdomar

En utmaning var att få den lokala Ollama-modellen att returnera det format som n8n Text Classifier förväntade sig. Modellen klassificerade även vissa betalningsfrågor felaktigt som Övrigt.

Det löstes genom tydligare systeminstruktioner, mer specifika kategoriregler, Auto-Fixing och Sampling Temperature 0. Detta visade hur viktigt det är att testa AI-output och inte förutsätta att modellen alltid följer instruktionerna korrekt.

En annan lärdom var vikten av fallback. När AI eller kunskapsunderlaget inte räcker bör systemet inte gissa, utan lämna över till mänsklig kundservice.

## Säkerhet

Eftersom lösningen är avsedd för finans är säkerhet särskilt viktig. Jag har därför analyserat lösningen utifrån OWASP LLM Top 10.

Bland de viktigaste riskerna finns prompt injection, känslig informationsläckage, misinformation och excessive agency. I MVP:n begränsas riskerna bland annat genom att riktiga kunduppgifter inte används, AI:n inte får genomföra ekonomiska åtgärder och okända frågor eskaleras till en människa.

API-nyckeln lagras inte i Git-repot utan hanteras med .NET User Secrets.

## Vidareutveckling

Vid fortsatt utveckling skulle FAQ-underlaget kunna ersättas eller kompletteras med en RAG-lösning och en vektordatabas för att hantera större dokumentmängder.

Supportärenden skulle även kunna lagras permanent i en databas i stället för i minnet. Autentisering, rate limiting, loggning och mer omfattande säkerhetstester skulle också behövas innan lösningen används med riktiga kunder.

Jag skulle även koppla n8n-automationen direkt till ärendehanteringen så att kategorisering, prioritering och vidare routing sker automatiskt i produktionsflödet.