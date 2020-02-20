#!/usr/bin/env python
import csv
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from scipy.stats import linregress
from textwrap import wrap


# file stuff? probably unnecessary
# import os
# import shutil

dataFolder = 'data/processed/'
def read_csv(fname):
    with open(dataFolder + fname + '.csv', newline='', encoding='mac_roman') as csvfile:
        print('# Now reading ' + fname)
        csvreader = csv.reader(csvfile, delimiter=',', quotechar='|')
        dataframe = pd.read_csv(dataFolder + fname + '.csv')
        print(dataframe.head(10))
        #for row in spamreader:
        #    print(', '.join(row))
        return dataframe
        #return csvreader
        

#allAfterMR =  read_csv("all_MR_immersion_AFTER");
#allDuringMR =  read_csv("all_MR_immersion_DURING");
#allAfterVR =  read_csv("all_VR_immersion_AFTER");
#allDuringVR =  read_csv("all_VR_immersion_During");
#bestThirdDistAfterMR =  read_csv("best_third_distance_MR_immersion_AFTER");
#bestThirdDistDuringMR =  read_csv("best_third_distance_MR_immersion_DURING");
#bestThirdDistAfterVR =  read_csv("best_third_distance_VR_immersion_AFTER");
#bestThirdDistDuringVR =  read_csv("best_third_distance_VR_immersion_DURING");
#bestThirdLearnerAfterMR =  read_csv("best_third_learner_MR_immersion_AFTER");
#bestThirdLearnDuringMR =  read_csv("best_third_learner_MR_immersion_DURING");
#bestThirdLearnerAfterVR =  read_csv("best_third_learner_VR_immersion_AFTER");
#bestThirdLearnDuringVR =  read_csv("best_third_learner_VR_immersion_DURING");
#demographics =  read_csv("demographics");
resultDataThrowing = read_csv("result_data_throwing");
resultDataThrowingBestLearn = read_csv("result_data_throwing_best_learner");
resultDataThrowingBestLearnDist = read_csv("result_data_throwing_best_learner_distance");
resultDataThrowingDist = read_csv("result_data_throwing_best_third_distance");
#worstResultDataThrowingLearn =  read_csv("result_data_throwing_worst_learner");
#worstResultDataThrowingDist =  read_csv("result_data_throwing_worst_third_distance");
#worstThirdDistAfterMR =  read_csv("worst_third_distance_MR_immersion_AFTER");
#worstThirdDistDuringMR =  read_csv("worst_third_distance_MR_immersion_DURING");
#worstThirdDistAfterVR =  read_csv("worst_third_distance_VR_immersion_AFTER");
#worstThirdDistDuringVR =  read_csv("worst_third_distance_VR_immersion_DURING");
#worstThirdLearnAfterMR =  read_csv("worst_third_learner_MR_immersion_AFTER");
#worstThirdLearnDuringMR =  read_csv("worst_third_learner_MR_immersion_DURING");
#worstThirdLearnerAfterVR =  read_csv("worst_third_learner_VR_immersion_AFTER");
#worstThirdLearnDuringVR =  read_csv("worst_third_learner_VR_immersion_DURING");

def getValuesList(list,x):
    l = []
    for i in range(1,8):
        l.append(round(list.iloc[x][i],2))
    return l

# compare worst to best 
#labels = bestThirdDistAfterMR.columns.values[1:]
mrDat = resultDataThrowing
vrDat = allDuringVR

labels = ['\n'.join(wrap(l, 15)) for l in mrDat.columns.values[1:]] 
mr = getValuesList(mrDat, 0) #[bestThirdDistAfterMR.iloc[0][1], bestThirdDistAfterMR.iloc[0][2], bestThirdDistAfterMR.iloc[0][3], bestThirdDistAfterMR.iloc[0][4], bestThirdDistAfterMR.iloc[0][5], bestThirdDistAfterMR.iloc[0][6]]
mrError = getValuesList(mrDat, 1)
vr = getValuesList(vrDat, 0) #[bestThirdDistAfterVR.iloc[0][1], bestThirdDistAfterVR.iloc[0][2], bestThirdDistAfterVR.iloc[0][3], bestThirdDistAfterVR.iloc[0][4], bestThirdDistAfterVR.iloc[0][5], bestThirdDistAfterVR.iloc[0][6]]
vrError = getValuesList(vrDat, 1)
x = np.arange(len(labels))  # the label locations
width = 0.35  # the width of the bars

#print("PRINT")
#print(labels)
#print(bestAfterMR)
#print(bestAfterVR)

fig, ax = plt.subplots()


# Add some text for labels, title and custom x-axis tick labels, etc.
#ax.set_ylabel('Scores')

rects1 = ax.bar(x - width/2, mr, width, yerr=mrError, label='All MR', capsize=3.0)

ax.spines['top'].set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['bottom'].set_visible(False)
ax.spines['left'].set_visible(False)

ax.set_title('All Throw Values average')
ax.set_xticks(x)
ax.set_xticklabels(labels, fontsize=15)
ax.legend()

def autolabel(rects):
    #Attach a text label above each bar in *rects*, displaying its height
    for rect in rects:
        height = rect.get_height()
        ax.annotate('{}'.format(height),
                    xy=(rect.get_x() + rect.get_width() / 2, height),
                    xytext=(0, 3),  # 3 points vertical offset
                    textcoords="offset points",
                    ha='center', va='bottom',
                    fontsize=13)

autolabel(rects1)

fig.tight_layout()

fig.set_size_inches(13.5, 8.5)
plt.savefig("data/figures/test.png")
#
