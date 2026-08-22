# FinanceSupportAI

FinanceSupportAI är en AI-baserad kundservicelösning utvecklad som en MVP för ett företag inom finans och ekonomi.

Systemet kan besvara vanliga kundfrågor utifrån ett begränsat FAQ-underlag. Om systemet inte hittar relevant information eskaleras frågan till kundservice genom att ett supportärende skapas.

Projektet innehåller även ett n8n-workflow där en lokal AI-modell via Ollama klassificerar supportfrågor.

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
- Söker efter relevant information i ett FAQ-underlag
- Använder AI för att formulera svar
- Eskalerar frågor som inte kan besvaras
- Skapar supportärenden
- Supportärenden kan hämtas via `GET /api/tickets`
- n8n automatiserar klassificering av eskalerade frågor
- Lokal Ollama-modell klassificerar frågor som:
    - Betalning
    - Konto
    - Lån
    - Övrigt

## Projektstruktur

- `Controllers/` – API-endpoints för chat och supportärenden
- `Services/` – kunskapssökning, AI och ticket-hantering
- `Models/` – modeller för requests, responses, FAQ och supportärenden
- `Data/faq.json` – FAQ-underlag
- `n8n/` – exporterat n8n-workflow
- `SECURITY_ANALYSIS.md` – säkerhetsanalys utifrån OWASP LLM Top 10

## Starta API:t

Projektet kräver .NET 9 SDK.

Kör projektet från Rider eller från projektmappen:

dotnet run --project FinanceSupport.Api/FinanceSupport.Api.csproj

API:t körs lokalt på:

http://localhost:5214

## API-nyckel

OpenAI API-nyckeln ska inte lagras i Git-repot.

Projektet använder .NET User Secrets:

dotnet user-secrets set "OpenAI:ApiKey" "DIN_API_NYCKEL" --project FinanceSupport.Api/FinanceSupport.Api.csproj

## Starta Ollama

Projektets n8n-automation använder Ollama lokalt.

Modellen som används är:

llama3.2:3b

Kontrollera installerade modeller med:

ollama list

Ollamas lokala API används på:

http://127.0.0.1:11434

## Starta n8n

Starta n8n lokalt:

n8n

Öppna därefter n8n i webbläsaren på port 5678 och importera workflowet:

n8n/FinanceSupport-Escalated-Ticket-Automation.json

Workflowet använder:

Webhook → Text Classifier → Ollama → kategori → Merge → kundservice-status

## Säkerhet

Projektet innehåller en separat säkerhetsanalys i `SECURITY_ANALYSIS.md`.

Analysen behandlar OWASP LLM Top 10 och risker som bland annat prompt injection, informationsläckage, felaktig AI-output, excessive agency och resursförbrukning.

## Begränsningar

Detta är en MVP och använder inte riktiga kunduppgifter. Supportärenden lagras endast i minnet och försvinner när API:t startas om.

AI:n får inte genomföra ekonomiska transaktioner eller fatta ekonomiska beslut.