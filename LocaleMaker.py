import json
import os

locale_folder = "Locale"
output_file = "Locale_output.txt"

all_entries = []

lang_code = "en-US"
with open("Locale.json", "r", encoding="utf-8") as f:
    data = json.load(f)

entry_lines = [f'["{lang_code}"] = new()']
entry_lines.append("{")
for key, value in data.items():
    entry_lines.append(f'    ["{key}"] = "{value}",')
entry_lines.append("},")
all_entries.append("\n".join(entry_lines))

for filename in os.listdir(locale_folder):
    if filename.endswith(".json"):
        lang_code = os.path.splitext(filename)[0]
        path = os.path.join(locale_folder, filename)
        with open(path, "r", encoding="utf-8") as f:
            data = json.load(f)
        
        entry_lines = [f'["{lang_code}"] = new()']
        entry_lines.append("{")
        for key, value in data.items():
            entry_lines.append(f'    ["{key}"] = "{value}",')
        entry_lines.append("},")
        all_entries.append("\n".join(entry_lines))

with open(output_file, "w", encoding="utf-8") as f:
    f.write("\n".join(all_entries))

print(f"Processed {len(all_entries)} JSON files → {output_file}")
