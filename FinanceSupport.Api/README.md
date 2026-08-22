# FinanceSupportAI

FinanceSupportAI är en AI-baserad kundservicelösning utvecklad som en MVP för ett företag inom finans och ekonomi.

Systemet besvarar vanliga kundfrågor utifrån ett kontrollerat FAQ-underlag. Om relevant information saknas skapas ett supportärende och frågan skickas automatiskt vidare till ett n8n-workflow för AI-baserad klassificering.

## Teknik

- C# / ASP.NET Core
- .NET 9
- OpenAI SDK
- n8n
- Ollama
- Llama 3.2 3B
- REST API

## Funktioner

- Tar emot kundfrågor via `POST /api/chat`
- Söker efter relevant information i FAQ-underlaget
- Använder AI för att formulera svar utifrån hittad information
- Eskalerar frågor som inte kan besvaras
- Skapar supportärenden
- Supportärenden kan hämtas via `GET /api/tickets`
- Backend skickar automatiskt eskalerade frågor till n8n via webhook
- n8n använder AI för att klassificera ärendet
- Ollama med Llama 3.2 3B klassificerar frågor som:
    - Betalning
    - Konto
    - Lån
    - Övrigt

## Flöde

Känd fråga:

Kundfråga → .NET API → KnowledgeService → FAQ-underlag → AiService → svar

Okänd fråga:

Kundfråga → .NET API → TicketService → supportärende → N8nService → n8n webhook → Ollama → klassificering → kundservice

## Projektstruktur

- `Controllers/` – API-endpoints för chat och supportärenden
- `Services/KnowledgeService.cs` – söker i FAQ-underlaget
- `Services/AiService.cs` – AI-generering av kundservicesvar
- `Services/TicketService.cs` – skapar och hanterar supportärenden
- `Services/N8nService.cs` – skickar eskalerade frågor till n8n
- `Models/` – modeller för requests, responses, FAQ och supportärenden
- `Data/faq.json` – kontrollerat FAQ-underlag
- `n8n/FinanceSupport-Escalated-Ticket-Automation.json` – exporterat n8n-workflow
- `SECURITY_ANALYSIS.md` – säkerhetsanalys utifrån OWASP LLM Top 10
- `REFLEKTION.md` – kritisk reflektion

## Starta API:t

Projektet kräver .NET 9 SDK.

Från repots rot:

dotnet run --project FinanceSupport.Api/FinanceSupport.Api.csproj

API:t körs lokalt på:

http://localhost:5214

## OpenAI API-nyckel

API-nyckeln ska inte lagras i Git-repot.

Projektet använder .NET User Secrets:

dotnet user-secrets set "OpenAI:ApiKey" "DIN_API_NYCKEL" --project FinanceSupport.Api/FinanceSupport.Api.csproj

## Starta Ollama

Installera Ollama och hämta modellen:

ollama pull llama3.2:3b

Kontrollera att modellen finns:

ollama list

Ollamas lokala API används på:

http://127.0.0.1:11434

## Starta n8n

Starta n8n:

n8n

Öppna:

http://localhost:5678

Importera workflowet:

n8n/FinanceSupport-Escalated-Ticket-Automation.json

Konfigurera Ollama-credentialen i n8n med:

http://127.0.0.1:11434

Välj modellen:

llama3.2:3b

Publicera workflowet så att production-webhooken är aktiv.

Backend använder webhook-adressen som finns under `N8n:WebhookUrl` i `appsettings.Development.json`.

## Testa lösningen

Starta:

1. Ollama
2. n8n och det publicerade workflowet
3. FinanceSupport.Api

Testanrop finns i:

`FinanceSupport.Api.http`

En känd FAQ-fråga ska ge ett AI-formulerat svar.

En okänd fråga ska:

1. ge `escalated: true`
2. skapa ett supportärende
3. automatiskt skickas till n8n
4. klassificeras av Ollama

Supportärenden kan kontrolleras med:

GET http://localhost:5214/api/tickets

## Designval

I Inlämning 1 rekommenderades en fullständig RAG-lösning tillsammans med GPT-5 mini.

För MVP:n förenklades retrieval-delen till ett kontrollerat FAQ-underlag via `KnowledgeService`. Det gör det möjligt att demonstrera samma grundprincip – att AI:n baserar svaret på godkänd företagsinformation – utan att införa embeddings och vektordatabas i den första versionen.

Llama 3.2 3B via Ollama används för den avgränsade klassificeringsuppgiften i n8n.

## Säkerhet

`SECURITY_ANALYSIS.md` innehåller en analys av lösningen mappad mot OWASP LLM Top 10.

AI:n har inte tillgång till riktiga kundkonton och får inte genomföra transaktioner eller fatta ekonomiska beslut. Okända frågor eskaleras till mänsklig kundservice.

## Begränsningar

Detta är en MVP och använder inte riktiga kunduppgifter.

Supportärenden lagras i minnet och försvinner när API:t startas om.

FAQ-underlaget är begränsat och är inte en fullständig RAG-lösning.

n8n-klassificeringen kategoriserar ärendet men kategorin sparas ännu inte tillbaka permanent på supportärendet.

Autentisering, permanent databas, rate limiting och mer omfattande säkerhetskontroller skulle behövas innan lösningen används i produktion.