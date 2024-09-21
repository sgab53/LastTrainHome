from notion2pandas import Notion2PandasClient
import json



with open('secrets.json', 'r') as notionFile:
    notion_data = json.load(notionFile)
    token = notion_data.get('notion_api_token')
    database_id = notion_data.get(
        'gioco_jam').get('id_database_text')

n2p = Notion2PandasClient(auth=token)
df = n2p.from_notion_DB_to_dataframe(database_id)

df.to_csv('texts.csv',columns=['key', 'it', 'en'], index=False)