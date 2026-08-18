from collections import defaultdict
import json5
import os
import re
import shutil
import struct
from pathlib import Path
import argparse
from typing import Any, cast

parser = argparse.ArgumentParser(description="Example script")
parser.add_argument("-t", "--test", action="store_true", help="Run test mode")
args = parser.parse_args()
testing: bool = args.test

LOCALE_PATTERN = re.compile(r"^[Locale_|]*[a-z]{2}-[A-Za-z]{2,4}\.json$")
ENGLISH_LANG = "en-US"

script_dir = Path(__file__).resolve().parent


def write7BitEncodedInt(file, value):
    while value > 0x7F:
        file.write(bytes([(value & 0x7F) | 0x80]))
        value >>= 7
    file.write(bytes([value]))


def writeString(file, string):
    encoded = string.encode("utf-8")
    write7BitEncodedInt(file, len(encoded))
    file.write(encoded)


def write_loc_file(file_path, data):
    with open(file_path, "wb") as file:
        file.write(struct.pack("<H", 0x0001))

        writeString(file, data[0])
        writeString(file, data[1])
        writeString(file, data[2])

        file.write(struct.pack("<i", len(data[3])))
        for key, value in data[3].items():
            writeString(file, key)
            writeString(file, value)

        file.write(struct.pack("<i", len(data[4])))
        for key, value in data[4].items():
            writeString(file, key)
            writeString(file, value)

    print(f"Saved '{file_path}'")


LANG_MAP = {
    "en-US": ["English", "English"],
    "de-DE": ["German", "Deutsch"],
    "es-ES": ["Spanish", "Español"],
    "fr-FR": ["French", "Français"],
    "it-IT": ["Italian", "Italian"],
    "ja-JP": ["Japanese", "日本語"],
    "ko-KR": ["Korean", "한국어"],
    "pl-PL": ["Polish", "Polski"],
    "pt-BR": ["Portuguese", "Português brasileiro"],
    "ru-RU": ["Russian", "Русский"],
    "zh-HANS": ["ChineseSimplified", "汉语"],
    "zh-HANT": ["ChineseTraditional", "繁體中文"],
}


def create_loc(file_name, json_data, lang_code):
    output_filename = os.path.join(f"{file_name}_{lang_code}.loc")

    [lang_name, lang_loc_name] = LANG_MAP.get(lang_code)

    loc_data = [lang_name, lang_code, lang_loc_name, json_data, {}]

    write_loc_file(output_filename, loc_data)


def make_loc():
    grouped_data = defaultdict(dict)

    modName = Path("..").resolve().name
    print(f"Creating locales for {modName}")

    for directory in [script_dir, Path.cwd()]:
        for path in directory.iterdir():
            if path.is_file() and LOCALE_PATTERN.match(path.name):
                if testing:
                    print(f"Processing {path}")
                lang_code = path.name.removeprefix("Locale_").removesuffix(".json")
                with path.open("r", encoding="utf-8") as json_file:
                    json_data = cast(dict[str, str], json5.load(json_file))

                for id, text in json_data.items():
                    valid = False

                    if id.startswith("Option"):
                        valid, locale = format_options_text(id, modName)
                        if valid:
                            grouped_data[lang_code][locale] = text

                    if not valid:
                        # regex_key = regex_r(id)
                        grouped_data[lang_code][f"{modName}.{id}"] = text

    if ENGLISH_LANG not in grouped_data:
        raise RuntimeError(f"English locale '{ENGLISH_LANG}' not found.")

    english = grouped_data[ENGLISH_LANG]

    for lang_code, locale in grouped_data.items():
        if lang_code == ENGLISH_LANG:
            continue

        extra = set(locale) - set(english)
        if extra:
            raise RuntimeError(
                f"{lang_code} contains keys not present in {ENGLISH_LANG}:\n"
                + "\n".join(sorted(extra))
            )

        missing = set(english) - set(locale)
        for key in missing:
            locale[key] = english[key]

        if missing:
            print(
                f"{lang_code}: copied {len(missing)} missing entries from {ENGLISH_LANG}"
            )

    if testing:
        # print(grouped_data)
        return

    base_output_dir = f"..\\Resources\\Localization"

    folder_path = Path(base_output_dir)
    folder_path.mkdir(parents=True, exist_ok=True)

    for lang_code, aggregated_data in grouped_data.items():
        file_path = folder_path / modName
        create_loc(file_path, aggregated_data, lang_code)

    (folder_path / ".gitignore").write_text("*\n!.gitignore")


def regex_r(text):
    pattern = r"\[(.*?)\]"
    for line in text.splitlines():
        if "[" in line and "]" in line:
            matches = re.findall(pattern, line)
            return matches[0]
        else:
            if "." in line:
                return text.split(".")[0]
            return text


def format_options_text(text: str, modName: str) -> tuple[bool, str]:
    # Options.TAB[modName.modName.Mod.LogTab]
    if text.startswith("Option."):
        pattern = r"Option\.(.+)\.(.+)"
        matches = re.match(pattern, text)
        if matches:
            groups = matches.groups()
            if groups[1] == "Label":
                return (
                    True,
                    f"Options.OPTION[{modName}.{modName}.Mod.Setting.{groups[0]}]",
                )
            if groups[1] == "Description" or groups[1] == "Desc":
                return (
                    True,
                    f"Options.OPTION_DESCRIPTION[{modName}.{modName}.Mod.Setting.{groups[0]}]",
                )
    elif text.startswith("OptionsEnum."):
        pattern = r"OptionsEnum\.(.+)\.(.+)"
        matches = re.match(pattern, text)
        if matches:
            groups = matches.groups()
            return (
                True,
                f"Options.{modName}.{modName}.Mod.{groups[1].upper()}[{groups[1]}]",
            )
    else:
        pattern = r"Option(.+)\.(.+)"
        matches = re.match(pattern, text)
        if matches:
            groups = matches.groups()
            prefix = ""
            if groups[0] == "Group" or groups[0] == "Tab":
                prefix = f"{groups[0].upper()}"

            if prefix != "":
                return (
                    True,
                    f"Options.{prefix}[{modName}.{modName}.Mod.{groups[1]}]",
                )
    return False, ""


if not testing:
    base_output_dir = f"..\\Resources\\Localization"
    if os.path.exists(base_output_dir):
        shutil.rmtree(base_output_dir)
    os.makedirs(base_output_dir, exist_ok=True)
make_loc()
