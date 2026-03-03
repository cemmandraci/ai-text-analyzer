from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import spacy

app = FastAPI(title="NLP Service", version="1.0.0")

nlp = spacy.load("en_core_web_sm")

class AnalyzeRequest(BaseModel):
    text: str

class NamedEntity(BaseModel):
    text: str
    type: str

class AnalyzeResponse(BaseModel):
    keywords: list[str]
    entities: list[NamedEntity]


@app.get("/health")
def health():
    return {"status": "healthy"}

@app.post("/analyze", response_model=AnalyzeResponse)
def analyze(request: AnalyzeRequest):
    if not request.text or not request.text.strip():
        raise HTTPException(status_code=400, detail="Text cannot be empty.")
    
    doc = nlp(request.text)

    keywords = list(set([
        chunk.text.lower()
        for chunk in doc.noun_chunks
        if len(chunk.text) > 2
    ]))

    seen = set()
    entities = []
    for ent in doc.ents:
        key = (ent.text, ent.label_)
        if key not in seen:
            seen.add(key)
            entities.append(NamedEntity(text=ent.text, type=ent.label_))

    return AnalyzeResponse(keywords=keywords, entities=entities)