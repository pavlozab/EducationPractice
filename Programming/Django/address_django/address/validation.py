from django.core.exceptions import ValidationError 
import re


class Validation:
    def numberValidation(value: str):
        if len(value) == 13 and value[0:4] == '+380' and value[4:].isdigit():
            return value
        else: 
            raise ValidationError("The lenght of value should be 13. Contain only digits. ")

    def symbolValidation(value: str):
        if re.search(r'[^A-Za-z .]', value) == None:
            return value
        else:
            raise ValidationError("Value must contain only letters")
    
