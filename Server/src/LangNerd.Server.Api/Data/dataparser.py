import json

# Wczytaj dane z pliku JSON
with open("/Users/michu/Documents/LangNerd/Server/src/LangNerd.Server.Api/Data/words.json", "r", encoding="utf-8") as f:
    words = json.load(f)

# Słownik do przechowywania połączonych słów
merged_words = {}

for entry in words:
    word = entry["word"].strip()
    definition = entry["definition"].strip()

    if word in merged_words:
        # Dodaj nową definicję, jeśli jej jeszcze nie ma
        existing_defs = merged_words[word].split(";")
        if definition not in existing_defs:
            merged_words[word] += "; " + definition
    else:
        merged_words[word] = definition

# Zamień z powrotem na listę słowników
merged_list = [{"word": w, "definition": d} for w, d in merged_words.items()]

# Zapisz wynik do nowego pliku JSON
with open("words_merged.json", "w", encoding="utf-8") as f:
    json.dump(merged_list, f, ensure_ascii=False, indent=4)

print("Gotowe! Powtarzające się słowa zostały scalone.")
