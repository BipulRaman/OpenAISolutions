import os
import openai
import requests
from PIL import Image
from io import BytesIO

openai.api_type = "azure"
openai.api_base = ""
openai.api_version = ""
openai.api_key = ""

response = openai.Image.create(prompt="Indian Education System", size="1024x1024", n=1)
image_url = response["data"][0]["url"]

# Save the image to a file
response = requests.get(image_url)
img = Image.open(BytesIO(response.content))
img.save("D:\\output.png")