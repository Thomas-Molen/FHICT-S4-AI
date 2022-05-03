FROM python:3.8

ADD requirements.txt /

RUN pip install -r /requirements.txt

ADD revenue-predictor.py /

ADD movies2_merged.csv /

ENV PYTHONUNBUFFERED=1

CMD [ "python", "./revenue-predictor.py" ]