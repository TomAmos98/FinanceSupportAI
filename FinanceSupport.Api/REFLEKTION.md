# Reflektion – FinanceSupportAI

## Lösningen

Jag har utvecklat FinanceSupportAI som en MVP för kundservice inom finans. Målet var att minska mängden återkommande frågor som behöver hanteras manuellt och samtidigt ha en tydlig väg till mänsklig kundservice när systemet inte kan ge ett säkert svar.

Lösningen består av ett ASP.NET Core API i .NET 9, ett FAQ-baserat kunskapsunderlag, AI-integration samt ett system för eskalering och supportärenden.

Jag har även byggt en automation i n8n. När backend inte hittar relevant information skapas ett supportärende och frågan skickas automatiskt vidare till n8n. Där använder en lokal Llama 3.2 3B-modell via Ollama AI för att klassificera frågan som Betalning, Konto, Lån eller Övrigt.

## Val av arkitektur och förändringar från Inlämning 1

I Inlämning 1 rekommenderade jag RAG tillsammans med GPT-5 mini. Under utvecklingen valde jag att förenkla detta för MVP:n.

I stället för en fullständig RAG-lösning med embeddings och vektordatabas använder backend ett mindre, kontrollerat FAQ-underlag genom KnowledgeService. Systemet söker först efter relevant information där. Om information hittas används AI för att formulera svaret utifrån underlaget. Om relevant information saknas ska modellen inte gissa. Frågan eskaleras då till kundservice.

Jag gjorde denna förändring eftersom en fullständig vektordatabas hade ökat komplexiteten utan att vara nödvändig för att demonstrera kärnfunktionen i MVP:n. Principen från Inlämning 1 finns fortfarande kvar: AI:n ska basera sitt svar på företagets godkända information i stället för att enbart använda modellens generella kunskap.

GPT-5 mini var rekommendationen för kundchatbotten i Inlämning 1. I den byggda lösningen kompletterade jag detta med Llama 3.2 3B via Ollama för n8n-automationen. Den lokala modellen passar bra för den begränsade klassificeringsuppgiften och innebär ingen kostnad per klassificeringsanrop.

## AI och automation

AI används på två konkreta sätt i lösningen.

I API:t används AI för att formulera ett tydligt kundservicesvar utifrån information som först har hittats i FAQ-underlaget.

När systemet inte hittar relevant information skapar backend ett supportärende och anropar automatiskt n8n via en webhook. I n8n används Ollama och Llama 3.2 3B för att klassificera det eskalerade ärendet som Betalning, Konto, Lån eller Övrigt.

Det innebär att automationen är direkt kopplad till backend och kundserviceflödet. Kunden behöver inte manuellt starta klassificeringen.

## Vad som fungerade

Uppdelningen mellan KnowledgeService, AiService, TicketService och N8nService gjorde lösningen enklare att förstå och testa. Det fungerade också bra att använda ett kontrollerat FAQ-underlag och eskalera frågor när information saknas.

n8n fungerade bra för att visualisera automationen och koppla AI-klassificeringen till det eskalerade kundserviceflödet.

En viktig styrka blev fallback-lösningen. AI:n behöver inte försöka besvara varje fråga. När kunskapsunderlaget inte räcker kan en människa ta över.

## Vad som inte fungerade

En utmaning var att få den lokala Ollama-modellen att returnera det format som n8n Text Classifier förväntade sig. Modellen klassificerade även vissa tydliga betalningsfrågor felaktigt som Övrigt.

Det löstes genom tydligare systeminstruktioner, mer specifika kategoriregler, Auto-Fixing och Sampling Temperature 0.

Det visade att AI-output inte kan behandlas som deterministisk vanlig programkod. Modellen måste testas med flera formuleringar och systemet behöver kunna hantera oväntade svar.

## När AI var rätt verktyg

AI var användbart när uppgiften innehöll språk och variation. Att formulera ett naturligt kundservicesvar utifrån ett kontrollerat underlag är ett exempel. Klassificeringen i n8n är ett annat, eftersom kunder kan uttrycka samma typ av problem på många olika sätt.

AI var däremot inte rätt verktyg för deterministiska delar av systemet. Att skapa ett supportärende, kontrollera om ett FAQ-resultat finns, anropa en webhook och avgöra när ett ärende ska eskaleras är bättre att implementera med vanlig C#-logik.

Det är en viktig slutsats från projektet: AI bör användas där modellens språkförståelse ger ett konkret värde, inte för logik som vanlig kod kan utföra mer förutsägbart.

## Säkerhet

Eftersom lösningen är avsedd för finans är säkerhet särskilt viktig. Jag har därför analyserat lösningen utifrån OWASP LLM Top 10.

Bland de viktigaste riskerna finns prompt injection, Sensitive Information Disclosure, Misinformation och Excessive Agency. I MVP:n begränsas riskerna bland annat genom att riktiga kunduppgifter inte används, AI:n inte får genomföra ekonomiska åtgärder och okända frågor eskaleras till en människa.

API-nyckeln lagras inte i Git-repot utan hanteras separat med .NET User Secrets.

## Vad jag skulle göra annorlunda

Om jag började om projektet skulle jag tidigare definiera tydliga testfall för både AI-svar och klassificering. Problemen med Text Classifier visade att det inte räcker att testa en enda formulering och anta att AI-modellen alltid beter sig likadant.

Jag skulle också planera backend- och n8n-integrationen tidigare i utvecklingen så att hela flödet testades som en sammanhängande kedja från början.

## Vidareutveckling

Vid fortsatt utveckling skulle FAQ-underlaget kunna ersättas eller kompletteras med en fullständig RAG-lösning med embeddings och vektordatabas för att hantera större dokumentmängder.

Supportärenden skulle lagras permanent i en databas i stället för i minnet. Autentisering, rate limiting, loggning, persondatafiltrering och mer omfattande säkerhetstester skulle också behövas innan lösningen används med riktiga kunder.

n8n-flödet skulle dessutom kunna utvecklas vidare så att kategorin sparas på supportärendet och används för automatisk prioritering och routing till rätt del av kundservice.