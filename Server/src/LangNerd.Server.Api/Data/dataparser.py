#This program is used to repair words.json when words with different definitions are duplicated. 
# It removes duplicate keys and adds all the definitions to a single word.
import json
import os 

base_dir = os.path.dirname(os.path.abspath(__file__))
input_path = os.path.join(base_dir,"words.json")
output_path = os.path.join(base_dir,"wrods_merged.json")
with open(input_path, "r", encoding="utf-8") as f:
    words = json.load(f)


merged_words = {}

for entry in words:
    word = entry["word"].strip()
    definition = entry["definition"].strip()

    if word in merged_words:
        existing_defs = merged_words[word].split(";")
        if definition not in existing_defs:
            merged_words[word] += "; " + definition
    else:
        merged_words[word] = definition

merged_list = [{"word": w, "definition": d} for w, d in merged_words.items()]

with open(output_path, "w", encoding="utf-8") as f:
    json.dump(merged_list, f, ensure_ascii=False, indent=4)
