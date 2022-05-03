#!/usr/bin/env python
# coding: utf-8

# In[3]:


from sklearn.svm import SVR
from sklearn.preprocessing import StandardScaler

import numpy as np
import pandas as pd


# In[5]:


# Import data
df = pd.read_csv('movies2_merged.csv', sep='|')
df = df[['Budget', 'Collection', 'CastSize', 'CrewSize', 'DirectorPopularity', 'AvgCastPopularity', 'Revenue']].dropna()

# Log numerical values
numericCols = df.select_dtypes(include=np.number).columns
logdf = df.copy()
logdf[numericCols] = np.log(logdf[numericCols]).replace([np.inf, -np.inf], np.nan).dropna()

# Standardize data
scX = StandardScaler()
scY = StandardScaler()

X = scX.fit_transform(logdf[['Budget', 'Collection', 'CastSize', 'CrewSize', 'DirectorPopularity', 'AvgCastPopularity']])
y = scY.fit_transform(logdf[['Revenue']])

# In[9]:


svr = SVR(kernel='rbf')
svr.fit(X, y.ravel())

while(True):
    print('working')
