# 🧠 AI Text Analyzer

An AI-powered text analysis service built with .NET 10 and Python. Analyzes sentiment, extracts keywords, and identifies named entities from text.

## 🏗️ Architecture
```
[Client]
    ↓ POST /api/analysis/analyze
[.NET 10 Web API]
    ↓ MediatR → AnalyzeTextQueryHandler
    ↓ Task.WhenAll (parallel)
    ↓                    ↓
[Gemini API]      [Python FastAPI]
(Sentiment)       (Keywords + NER)
    ↓                    ↓
         [AnalysisResultDto]
                ↓
          [JSON Response]
```

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend API | .NET 10, C# |
| AI/LLM | Google Gemini API |
| NLP | Python 3.12, spaCy |
| Python API | FastAPI, Uvicorn |
| Architecture | Clean Architecture, CQRS, MediatR |
| Containerization | Docker, Docker Compose |

## 📁 Project Structure
```
ai-text-analyzer/
├── src/
│   ├── TextAnalyzer.API/          # HTTP endpoints, middleware
│   ├── TextAnalyzer.Application/  # CQRS handlers, use cases
│   ├── TextAnalyzer.Domain/       # Entities, interfaces
│   └── TextAnalyzer.Infrastructure/ # Gemini & Python integrations
├── python-nlp-service/            # FastAPI + spaCy service
└── docker-compose.yml
```

## 🚀 Getting Started

### Prerequisites
- Docker & Docker Compose
- Google Gemini API Key → [Get it here](https://aistudio.google.com)

### Run

1. Clone the repository
```bash
git clone https://github.com/YOUR_USERNAME/ai-text-analyzer.git
cd ai-text-analyzer
```

2. Create `.env` file in root
```bash
GEMINI_API_KEY=your_api_key_here
```

3. Start services
```bash
docker-compose up --build
```

4. Open Swagger UI
```
http://localhost:5000/swagger
```

## 📡 API Usage

### Analyze Text

**POST** `/api/analysis/analyze`

Request:
```json
{
  "text": "Apple CEO Tim Cook announced new products in New York.",
  "language": "en"
}
```

Response:
```json
{
  "originalText": "Apple CEO Tim Cook announced new products in New York.",
  "sentiment": "Positive",
  "sentimentScore": 0.85,
  "keywords": ["apple ceo", "new products", "new york"],
  "entities": [
    { "text": "Apple", "type": "ORG" },
    { "text": "Tim Cook", "type": "PERSON" },
    { "text": "New York", "type": "GPE" }
  ],
  "analyzedAt": "2026-03-03T10:00:00Z"
}
```

## 🧠 Key Concepts Learned

- **Clean Architecture** — Layered architecture with dependency inversion
- **CQRS** — Command Query Responsibility Segregation with MediatR
- **NLP** — Sentiment analysis, Named Entity Recognition, keyword extraction
- **Prompt Engineering** — Structured JSON output from LLMs
- **Docker** — Multi-stage builds, layer caching, container networking
- **Async/Await** — Parallel service calls with Task.WhenAll