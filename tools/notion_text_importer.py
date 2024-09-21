from notion2pandas import Notion2PandasClient
import json
import ast

with open('tools/secrets.json', 'r') as notionFile:
    notion_data = json.load(notionFile)
    token = notion_data.get('notion_api_token')
    database_texts_id = notion_data.get(
        'gioco_jam').get('id_database_text')
    database_dialogs_id = notion_data.get(
        'gioco_jam').get('id_database_dialog')

n2p = Notion2PandasClient(auth=token)
dfText = n2p.from_notion_DB_to_dataframe(database_texts_id)

transformed_data = {row['key']: {"it": row['it'], "en": row['en']} for _, row in dfText.iterrows()}
json_result = json.dumps(transformed_data, indent=4)


with open('Assets/DialogueSystem/Resources/texts.json', 'w') as f:
    f.write(json_result)

dfdialog = n2p.from_notion_DB_to_dataframe(database_dialogs_id)

dfdialog['testi'] = dfdialog['testi'].apply(ast.literal_eval)
transformed_data = {row['key']: {"testi": row['testi']} for _, row in dfdialog.iterrows()}
json_result = json.dumps(transformed_data, indent=4)

with open('Assets/DialogueSystem/Resources/dialogues.json', 'w') as f:
    f.write(json_result)