# Generated by Django 3.1.3 on 2020-11-15 13:33

from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Address',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('address_line', models.CharField(max_length=50)),
                ('postal_code', models.CharField(max_length=5)),
                ('country', models.CharField(max_length=50)),
                ('city', models.CharField(max_length=50)),
                ('fax_number', models.CharField(max_length=13)),
                ('phone_number', models.CharField(max_length=13)),
            ],
        ),
    ]